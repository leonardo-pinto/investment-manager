using InvestmentManager.ApplicationCore.DTO;
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

        public async Task<bool> IsStockSymbolValid(string concatenatedStockSymbols)
        {
            BrApiResponse response = await _brApiRepository.GetStocksPriceQuote(concatenatedStockSymbols);
            return response.Error == null;
        }
    }
}
