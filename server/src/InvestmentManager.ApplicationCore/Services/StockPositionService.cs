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
        private readonly IBrApiService _brApiService;
        private readonly IStockPositionRepository _stockPositionRepository;
        private readonly IMapper _mapper;

        public StockPositionService(
            IFinnhubService finnhubService,
            IBrApiService brApiService,
            IStockPositionRepository stockPositionRepository,
            IMapper mapper)
        {
            _finnhubService = finnhubService;
            _brApiService = brApiService;
            _stockPositionRepository = stockPositionRepository;
            _mapper = mapper;
        }

        public async Task<StockPositionResponse?> CreateStockPosition(AddStockPositionRequest addStockPositionRequest)
        {
            bool symbolAlreadyExists = await _stockPositionRepository.StockSymbolAlreadyExists(addStockPositionRequest.Symbol);

            if (symbolAlreadyExists)
            {
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
                return null;
            }

            StockPosition stockPosition = _mapper.Map<StockPosition>(addStockPositionRequest);

            await _stockPositionRepository.CreateStockPosition(stockPosition);

            StockPositionResponse stockPositionResponse = _mapper.Map<StockPositionResponse>(stockPosition);
            return stockPositionResponse;
        }

        public async Task<bool> DeleteStockPosition(Guid positionId)
        {
            StockPosition? stockPosition = await _stockPositionRepository.GetSingleStockPosition(positionId);

            if (stockPosition?.Quantity != 0)
            {
                throw new InvalidStockQuantityException("It is not possible to delete a stock position which quantity is not zero.");
            }

            return await _stockPositionRepository.DeleteStockPosition(positionId);
        }

        async public Task<List<StockPositionResponse>> GetAllStockPositionsByUserId(string userId)
        {
            List<StockPositionResponse> stockPositionsResponse = new();
            List<StockPosition> stockPositions = await _stockPositionRepository.GetAllStockPositionsByUserId(userId);

            if (!stockPositions.Any())
            {
                return stockPositionsResponse;
            }

            return stockPositions.Select(e => _mapper.Map<StockPositionResponse>(e)).ToList();
        }

        async public Task<StockPositionResponse?> GetSingleStockPosition(Guid positionId)
        {
            StockPosition? stockPosition = await _stockPositionRepository.GetSingleStockPosition(positionId);

            if (stockPosition == null)
            {
                return null;
            }

            return _mapper.Map<StockPositionResponse>(stockPosition);
        }

        async public Task<StockPositionResponse?> UpdateStockPosition(UpdateStockPositionRequest updateStockPositionRequest)
        {
            StockPosition? matchingStock = await _stockPositionRepository.GetSingleStockPosition(updateStockPositionRequest.PositionId);

            if (matchingStock == null)
            {
                return null;
            }

            matchingStock = UpdateStockPropertiesByTransactionType(
                matchingStock, updateStockPositionRequest);

            await _stockPositionRepository.UpdateStockPosition(matchingStock);

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
                    throw new InvalidStockQuantityException("The stock quantity to be sold is greater than the current stock position quantity.");
                }
                matchingStock.Quantity -= updateStockPositionRequest.Quantity;
            }
            return matchingStock;
        }
    }
}
