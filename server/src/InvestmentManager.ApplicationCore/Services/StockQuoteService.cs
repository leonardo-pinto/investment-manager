using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InvestmentManager.ApplicationCore.Services
{
    public class StockQuoteService : IStockQuoteService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ILogger<StockQuoteService> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly int _cacheTime = 30;

        public StockQuoteService(IRepositoryFactory repositoryFactory, ILogger<StockQuoteService> logger, IMemoryCache memoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        async public Task<List<StockQuoteResult>> GetStockPriceQuote(string stockSymbols, string tradingCountry)
        {
            string[] symbolsArr = stockSymbols.Split(",");
            List<StockQuoteResult> stockQuoteResult = new();
            IStockQuoteRepository repository = _repositoryFactory.CreateRepository(tradingCountry);
            try
            {
                foreach (string stockSymbol in symbolsArr)
                {
                    string cacheKey = $"stockSymbol-{stockSymbol}";
                    if (!_memoryCache.TryGetValue(cacheKey, out double stockPriceQuote))
                    {
                        stockPriceQuote = await repository.GetStockPriceQuote(stockSymbol);

                        _memoryCache.Set(
                            cacheKey,
                            stockPriceQuote,
                            TimeSpan.FromMinutes(_cacheTime));
                    }

                    stockQuoteResult.Add(new StockQuoteResult()
                    {
                        Symbol = stockSymbol,
                        Price = stockPriceQuote
                    });
                }
                return stockQuoteResult;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Error to connect with stock quote API - Error: {@Ex}", JsonSerializer.Serialize(ex));
                throw new InvalidOperationException("Unable to retrieve stock price quote.", ex);
            }
        }

        public async Task<bool> IsStockSymbolValid(string stockSymbol, string tradingCountry)
        {
            try
            {
                var repository = _repositoryFactory.CreateRepository(tradingCountry);
                double stockQuote = await repository.GetStockPriceQuote(stockSymbol);
                return stockQuote != 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to retrieve stock price quote.", ex);
            }
        }
    }
}
