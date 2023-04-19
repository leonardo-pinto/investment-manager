using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.ApplicationCore.Services
{
    public class BrApiService : IBrApiService
    {
        private readonly IBrApiRepository _brApiRepository;

        public BrApiService(IBrApiRepository brApiRepository)
        {
            _brApiRepository = brApiRepository;
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
                throw new BrApiException("Unable to retrieve stock price quote.", ex);
            }
        }
    }
}
