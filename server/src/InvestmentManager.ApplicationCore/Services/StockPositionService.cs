﻿using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Helpers;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Enums;

namespace InvestmentManager.ApplicationCore.Services
{
    public class StockPositionService : IStockPositionService
    {
        private readonly ITransactionService _transactionService;
        private readonly IFinnhubService _finnhubService;
        private readonly IStockPositionRepository _stockPositionRepository;

        public StockPositionService(
            ITransactionService transactionService,
            IFinnhubService finnhubService,
            IStockPositionRepository stockPositionRepository)
        {
            _transactionService = transactionService;
            _finnhubService = finnhubService;
            _stockPositionRepository = stockPositionRepository;

        }

        async public Task<StockPositionResponse> CreateStockPosition(AddStockPositionRequest addStockPositionRequest)
        {
            if (addStockPositionRequest == null)
            {
                throw new ArgumentNullException(nameof(addStockPositionRequest));
            }

            ValidationHelper.ModelValidation(addStockPositionRequest);

         
            // add exception handling
            double stockPrice = await _finnhubService.GetStockPriceQuote(addStockPositionRequest.Symbol);

            if (stockPrice == 0)
            {
                throw new ArgumentException();
            }

            StockPosition stockPosition = addStockPositionRequest.ToStockPosition();
            stockPosition.PositionId = Guid.NewGuid();

            await _stockPositionRepository.CreateStockPosition(stockPosition);

            AddTransactionRequest addTransactionRequest
                = addStockPositionRequest.ToAddTransactionRequest(stockPosition.PositionId, TransactionType.Buy);

            // add exception handling
            // delete position if transaction gave error???
            await _transactionService.CreateTransaction(addTransactionRequest);

            return stockPosition.ToStockPositionResponse(stockPrice);
        }

        async public Task<List<StockPositionResponse>?> GetAllStockPositions()
        {
            List<StockPositionResponse> stockPositionsResponse = new();
            List<StockPosition> stockPositions = await _stockPositionRepository.GetAllStockPositions();

            if(!stockPositions.Any())
            {
                return stockPositionsResponse;
            }

            List<string> stockSymbols = stockPositions.Select(temp => temp.Symbol).ToList();

            Dictionary<string, double> stockPriceDict = await _finnhubService.GetMultipleStockPriceQuote(stockSymbols);

            foreach (KeyValuePair<string, double> entry in stockPriceDict)
            {
                // get index of stockPosition with the given stockSymbol
                int stockPositionSymbolIndex = stockPositions
                    .FindIndex(stockPosition => stockPosition.Symbol == entry.Key);

                stockPositionsResponse
                    .Add(stockPositions[stockPositionSymbolIndex]
                    .ToStockPositionResponse(entry.Value));
            }

            return stockPositionsResponse;
        }

        async public Task<StockPositionResponse?> GetSingleStockPosition(Guid? positionId)
        {
            if (positionId == null)
            {
                throw new ArgumentNullException(nameof(positionId));
            }
          
            StockPosition? stockPosition = await _stockPositionRepository.GetSingleStockPosition(positionId.Value);

            if (stockPosition == null)
            {
                return null;
            }

            double stockPrice = await _finnhubService.GetStockPriceQuote(stockPosition.Symbol);
            // if it exists, get update price
            // check if stockPrice is 0???

            return stockPosition.ToStockPositionResponse(stockPrice);
        }

        public Task<StockPositionResponse?> UpdateStockPosition(UpdateStockPositionRequest updateStockPositionRequest)
        {
            throw new NotImplementedException();
        }
    }
}
