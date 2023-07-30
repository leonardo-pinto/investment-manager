using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InvestmentManager.ApplicationCore.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<IFinnhubService> _logger;

        public FinnhubService(IFinnhubRepository finnhubRepository, ILogger<FinnhubService> logger)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        async public Task<List<StockQuoteResult>> GetMultipleStockPriceQuote(string[] stockSymbols)
        {
            List<StockQuoteResult> stockQuoteResult = new();
            try
            {
                foreach (string stockSymbol in stockSymbols)
                {
                    double stockPriceQuote = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
                    stockQuoteResult.Add(new StockQuoteResult()
                    {
                        Symbol = stockSymbol,
                        Price = stockPriceQuote
                    });
                    // Due to the Finnhub call/min limit, it is necessary to add an interval
                    // between API calls to ensure that the request will be successfull
                    Thread.Sleep(50);
                }
                return stockQuoteResult;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Error to connect with Finnhub - Error: {@Ex}", JsonSerializer.Serialize(ex));
                throw new FinnhubException("Unable to retrieve stock price quote.", ex);
            }
        }

        public async Task<bool> IsStockSymbolValid(string stockSymbol)
        {
            try
            {
                double stockQuote = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
                return stockQuote != 0;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Error to connect with Finnhub - Error: {@Ex}", JsonSerializer.Serialize(ex));
                throw new FinnhubException("Unable to retrieve stock price quote.", ex);
            }
        }
    }
}
