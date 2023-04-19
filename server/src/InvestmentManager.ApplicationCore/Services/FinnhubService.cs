using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.ApplicationCore.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
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
                }
                return stockQuoteResult;
            }
            catch (InvalidOperationException ex)
            {
                throw new FinnhubException("Unable to retrieve stock price quote.", ex);
            }
        }

        public async Task<bool> IsStockSymbolValid(string stockSymbol)
        {
            double stockQuote = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
            return stockQuote != 0;
        }
    }
}
