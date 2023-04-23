using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Enums;
using InvestmentManager.ApplicationCore.Exceptions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace InvestmentManager.ApplicationCore.Services
{
    public class StockPositionService : IStockPositionService
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IBrApiService _brApiService;
        private readonly IStockPositionRepository _stockPositionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StockPositionService> _logger;
        private readonly IMemoryCache _memoryCache;

        public StockPositionService(
            IFinnhubService finnhubService,
            IBrApiService brApiService,
            IStockPositionRepository stockPositionRepository,
            IMapper mapper, ILogger<StockPositionService> logger,
            IMemoryCache memoryCache)
        {
            _finnhubService = finnhubService;
            _brApiService = brApiService;
            _stockPositionRepository = stockPositionRepository;
            _mapper = mapper;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<StockPositionResponse?> CreateStockPosition(AddStockPositionRequest addStockPositionRequest)
        {
            bool symbolAlreadyExists = await _stockPositionRepository.StockSymbolAlreadyExists(addStockPositionRequest.Symbol, addStockPositionRequest.UserId);

            if (symbolAlreadyExists)
            {
                _logger.LogError("Repeated stock symbol - StockSymbol: {@AddStockPositionRequest.Symbol}", addStockPositionRequest.Symbol);
                throw new RepeatedStockSymbolException("Stock symbol already registered. Please update the position instead of creating a new one.");
            }

            bool isStockSymbolValid;
            if (addStockPositionRequest.TradingCountry == TradingCountry.US)
            {
                isStockSymbolValid = await _finnhubService.IsStockSymbolValid(addStockPositionRequest.Symbol);
            } 
            else
            {
                isStockSymbolValid = await _brApiService.IsStockSymbolValid(addStockPositionRequest.Symbol);
            }

            if (!isStockSymbolValid)
            {
                _logger.LogError("Invalid stock symbol - StockSymbol: {@AddStockPositionRequest.Symbol}", addStockPositionRequest.Symbol);
                return null;
            }

            StockPosition stockPosition = _mapper.Map<StockPosition>(addStockPositionRequest);

            await _stockPositionRepository.CreateStockPosition(stockPosition);

            _memoryCache.Remove($"stocksUser{addStockPositionRequest.UserId}Country{addStockPositionRequest.TradingCountry}");

            StockPositionResponse stockPositionResponse = _mapper.Map<StockPositionResponse>(stockPosition);
            return stockPositionResponse;
        }

        public async Task<bool> DeleteStockPosition(Guid positionId)
        {
            StockPosition? stockPosition = await _stockPositionRepository.GetSingleStockPosition(positionId);

            if (stockPosition == null)
            {
                _logger.LogError("Invalid stock position id - StockPositionId: {@PositionId}", positionId);
                throw new ArgumentException("Stock position not found");
            }

            if (stockPosition?.Quantity != 0)
            {
                _logger.LogError("Invalid stock position quantity - StockPosition: {@StockPosition}", JsonSerializer.Serialize(stockPosition));
                throw new InvalidStockQuantityException("It is not possible to delete a stock position which quantity is not zero.");
            }

            bool result = await _stockPositionRepository.DeleteStockPosition(positionId);
            string cacheKey = $"stocksUser{stockPosition.UserId}Country{stockPosition.TradingCountry}";
            _memoryCache.Remove(cacheKey);
            
            return result;
        }

        async public Task<IEnumerable<StockPositionResponse>> GetAllStockPositionsByUserIdAndTradingCountry(string userId, string tradingCountry)
        {
            string cacheKey = $"stocksUser{userId}Country{tradingCountry.ToUpper()}";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<StockPosition> stockPositions))
            {
                stockPositions = await _stockPositionRepository.GetAllStockPositionsByUserIdAndTradingCountry(userId, tradingCountry);

                _memoryCache.Set(
                    cacheKey,
                    stockPositions,
                    TimeSpan.FromMinutes(10));
            }
            return stockPositions.Select(e => _mapper.Map<StockPositionResponse>(e)).ToList();
        }

        async public Task<StockPositionResponse?> GetSingleStockPosition(Guid positionId)
        {
            StockPosition? stockPosition = await _stockPositionRepository.GetSingleStockPosition(positionId);

            if (stockPosition == null)
            {
                _logger.LogError("Invalid stock position id - StockPositionId: {@PositionId}", positionId);
                return null;
            }

            return _mapper.Map<StockPositionResponse>(stockPosition);
        }

        async public Task<StockPositionResponse?> UpdateStockPosition(UpdateStockPositionRequest updateStockPositionRequest)
        {
            StockPosition? matchingStock = await _stockPositionRepository.GetSingleStockPosition(updateStockPositionRequest.PositionId);

            if (matchingStock == null)
            {
                _logger.LogError("Invalid stock position id - StockPositionId: {@updateStockPositionRequest.PositionId}", updateStockPositionRequest.PositionId);
                return null;
            }

            matchingStock = UpdateStockPropertiesByTransactionType(
                matchingStock, updateStockPositionRequest);

            await _stockPositionRepository.UpdateStockPosition(matchingStock);

            string cacheKey = $"stocksUser{matchingStock.UserId}Country{matchingStock.TradingCountry}";
            _memoryCache.Remove(cacheKey);

            return _mapper.Map<StockPositionResponse>(matchingStock);
        }

        public StockPosition UpdateStockPropertiesByTransactionType(
            StockPosition matchingStock,
            UpdateStockPositionRequest updateStockPositionRequest
        )
        {
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
                    _logger.LogError("Invalid stock position quantity - UpdateStockPositionRequest: {@UpdateStockPositionRequest}", JsonSerializer.Serialize(updateStockPositionRequest));
                    throw new InvalidStockQuantityException("The stock quantity to be sold is greater than the current stock position quantity.");
                }
                matchingStock.Quantity -= updateStockPositionRequest.Quantity;
            }
            return matchingStock;
        }
    }
}
