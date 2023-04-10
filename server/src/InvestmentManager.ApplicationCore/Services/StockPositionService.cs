using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
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

        async public Task<StockPositionResponse?> CreateStockPosition(AddStockPositionRequest addStockPositionRequest)
        {
            double currentStockPrice = await _finnhubService.GetStockPriceQuote(addStockPositionRequest.Symbol);

            if (currentStockPrice == 0) return null;

            StockPosition stockPosition = _mapper.Map<StockPosition>(addStockPositionRequest);
            stockPosition.CurrentPrice = currentStockPrice;

            await _stockPositionRepository.CreateStockPosition(stockPosition);

            StockPositionResponse stockPositionResponse = _mapper.Map<StockPositionResponse>(stockPosition);
            return stockPositionResponse;
        }

        async public Task<List<StockPositionResponse>> GetAllStockPositions()
        {
            List<StockPositionResponse> stockPositionsResponse = new();
            List<StockPosition> stockPositions = await _stockPositionRepository.GetAllStockPositions();

            if (!stockPositions.Any())
            {
                return stockPositionsResponse;
            }

            List<string> stockSymbols = stockPositions.Select(temp => temp.Symbol).ToList();

            Dictionary<string, double> stockPriceDict = await _finnhubService.GetMultipleStockPriceQuote(stockSymbols);

            stockPositions = UpdateStockPriceListBySymbol(stockPriceDict, stockPositions);

            return stockPositions.Select(e => _mapper.Map<StockPositionResponse>(e)).ToList();
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

        async public Task<StockPositionResponse?> UpdateStockPosition(UpdateStockPositionRequest updateStockPositionRequest, Guid positionId)
        {
            StockPosition? matchingStock = await _stockPositionRepository.GetSingleStockPosition(positionId);

            if (matchingStock == null) return null;
          
            if (updateStockPositionRequest.TransactionType == TransactionType.Buy)
            {
                matchingStock.AveragePrice =
                    matchingStock.UpdateAveragePrice(
                        updateStockPositionRequest.Quantity, updateStockPositionRequest.Price);
                matchingStock.Quantity += updateStockPositionRequest.Quantity;
            }
            else
            {
                if (updateStockPositionRequest.Quantity > matchingStock.Quantity)
                {
                    throw new InvalidStockQuantityException("The stock quantity to be sold is greater than the current stock position quantity.");
                }
                matchingStock.Quantity -= updateStockPositionRequest.Quantity;
            }

            matchingStock.Cost = matchingStock.Quantity * matchingStock.AveragePrice;
            matchingStock.CurrentPrice = await _finnhubService.GetStockPriceQuote(matchingStock.Symbol);
            await _stockPositionRepository.UpdateStockPosition(matchingStock);

            return _mapper.Map<StockPositionResponse>(matchingStock);
        }

        public List<StockPosition> UpdateStockPriceListBySymbol(Dictionary<string, double> stockPriceDict, List<StockPosition> stockPositions)
        {
            foreach (KeyValuePair<string, double> entry in stockPriceDict)
            {
                // get index of stockPosition with the given stockSymbol
                int index = stockPositions
                    .FindIndex(stockPosition => stockPosition.Symbol == entry.Key);
                stockPositions[index].CurrentPrice = entry.Value;
            }
            return stockPositions;
        }
    }
}
