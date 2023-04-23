using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InvestmentManager.ApplicationCore.Services
{
    public class BrApiService : IBrApiService
    {
        private readonly IBrApiRepository _brApiRepository;
        private readonly ILogger<BrApiService> _logger;

        public BrApiService(IBrApiRepository brApiRepository, ILogger<BrApiService> logger)
        {
            _brApiRepository = brApiRepository;
            _logger = logger;
        }

        public async Task<List<StockQuoteResult>> GetStocksPriceQuote(string concatenatedStockSymbols)
        {
            BrApiResponse response = await _brApiRepository.GetStocksPriceQuote(concatenatedStockSymbols);
            List<StockQuoteResult> stockQuoteResult = new ();
            try
            {
                if (response.Results != null && response.Results.Length > 0)
                {
                    foreach (Result result in response.Results)
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
