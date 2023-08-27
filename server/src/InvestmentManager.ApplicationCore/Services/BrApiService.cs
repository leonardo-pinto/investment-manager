using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InvestmentManager.ApplicationCore.Services
{
    public class BrApiService : IBrApiService
    {
        private readonly IBrApiRepository _brApiRepository;
        private readonly ILogger<BrApiService> _logger;
        private readonly IMemoryCache _memoryCache;

        public BrApiService(IBrApiRepository brApiRepository, ILogger<BrApiService> logger, IMemoryCache memoryCache)
        {
            _brApiRepository = brApiRepository;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<List<StockQuoteResult>> GetStocksPriceQuote(string concatenatedStockSymbols)
        {
            string cacheKey = $"stockSymbols-{concatenatedStockSymbols}";
            if (!_memoryCache.TryGetValue(cacheKey, out BrApiResponse stockPriceResult))
            {
                stockPriceResult = await _brApiRepository.GetStocksPriceQuote(concatenatedStockSymbols);

                _memoryCache.Set(
                    cacheKey, stockPriceResult, TimeSpan.FromMinutes(5));
            }


            try
            {
                List<StockQuoteResult> stockQuoteResult = new();
                if (stockPriceResult.Results != null && stockPriceResult.Results.Length > 0)
                {
                    foreach (Result result in stockPriceResult.Results)
                    {
                        var element = new StockQuoteResult()
                        {
                            Symbol = result.Symbol,
                            Price = result.RegularMarketPrice
                        };
                        stockQuoteResult.Add(element);
                    }
                }
                return stockQuoteResult;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Error to connect with BrApi - Error: {@Ex}", JsonSerializer.Serialize(ex));
                throw new BrApiException("Unable to retrieve stock price quote.", ex);
            }
        }

        public async Task<bool> IsStockSymbolValid(string concatenatedStockSymbols)
        {
            try
            {
                BrApiResponse response = await _brApiRepository.GetStocksPriceQuote(concatenatedStockSymbols);
                return response.Error == null;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Error to connect with BrApi - Error: {@Ex}", JsonSerializer.Serialize(ex));
                throw new BrApiException("Unable to retrieve stock price quote.", ex);
            }
        }
    }
}
