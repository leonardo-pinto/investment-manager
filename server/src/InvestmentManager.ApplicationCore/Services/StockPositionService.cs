using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Helpers;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Enums;
using InvestmentManager.ApplicationCore.Exceptions;
using AutoMapper;

namespace InvestmentManager.ApplicationCore.Services
{
    public class StockPositionService : IStockPositionService
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStockPositionRepository _stockPositionRepository;
        private readonly IMapper _mapper;

        public StockPositionService(
            IFinnhubService finnhubService,
            IStockPositionRepository stockPositionRepository,
            IMapper mapper)
        {
            _finnhubService = finnhubService;
            _stockPositionRepository = stockPositionRepository;
            _mapper = mapper;
        }

        async public Task<StockPositionResponse> CreateStockPosition(AddStockPositionRequest addStockPositionRequest)
        {
            // add logic if something went wrong in the creation

            // add exception handling
            double currentStockPrice = await _finnhubService.GetStockPriceQuote(addStockPositionRequest.Symbol);

            // return null if it is invalid symbol

            StockPosition stockPosition = _mapper.Map<StockPosition>(addStockPositionRequest);
            stockPosition.CurrentPrice = currentStockPrice;

            await _stockPositionRepository.CreateStockPosition(stockPosition);

            StockPositionResponse stockPositionResponse = _mapper.Map<StockPositionResponse>(stockPosition);
            return stockPositionResponse;
        }

        async public Task<List<StockPositionResponse>?> GetAllStockPositions()
        {
            List<StockPositionResponse> stockPositionsResponse = new();
            List<StockPosition> stockPositions = await _stockPositionRepository.GetAllStockPositions();

            if (!stockPositions.Any())
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

                StockPositionResponse stockPositionResponse = _mapper.Map<StockPositionResponse>(stockPositions[stockPositionSymbolIndex]);

                stockPositionResponse.CurrentPrice = entry.Value;
            }

            return stockPositionsResponse;
        }

        async public Task<StockPositionResponse?> GetSingleStockPosition(Guid positionId)
        {
            StockPosition? stockPosition = await _stockPositionRepository.GetSingleStockPosition(positionId);

            if (stockPosition == null)
            {
                return null;
            }

            double stockPrice = await _finnhubService.GetStockPriceQuote(stockPosition.Symbol);
            stockPosition.CurrentPrice = stockPrice;

            return _mapper.Map<StockPositionResponse>(stockPosition);
        }

        async public Task<StockPositionResponse?> UpdateStockPosition(UpdateStockPositionRequest updateStockPositionRequest)
        {
            StockPosition? matchingStockPosition = await _stockPositionRepository.GetSingleStockPosition(updateStockPositionRequest.PositionId);

            if (matchingStockPosition == null)
            {
                return null; 
            }

            if (updateStockPositionRequest.TransactionType == TransactionType.Buy)
            {
                matchingStockPosition.AveragePrice =
                    matchingStockPosition.UpdateAveragePrice(
                        updateStockPositionRequest.Quantity, updateStockPositionRequest.Price);
                matchingStockPosition.Quantity += updateStockPositionRequest.Quantity;
            }
            else
            {
                if (updateStockPositionRequest.Quantity > matchingStockPosition.Quantity)
                {
                    throw new InvalidStockQuantityException("The stock quantity to be sold is greater than the current stock position quantity.");
                }
                matchingStockPosition.Quantity -= updateStockPositionRequest.Quantity;
            }

            matchingStockPosition.Cost = matchingStockPosition.Quantity * matchingStockPosition.AveragePrice;

            await _stockPositionRepository.UpdateStockPosition(matchingStockPosition);
            double stockPrice = await _finnhubService.GetStockPriceQuote(matchingStockPosition.Symbol);
            matchingStockPosition.CurrentPrice = stockPrice;

            StockPositionResponse stockPositionResponse = _mapper.Map<StockPositionResponse>(matchingStockPosition);

            return stockPositionResponse;
        }
    }
}
