

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

        async public Task<double> GetStockPriceQuote(string stockSymbol)
        {
            try
            {
                double stockPriceQuote = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

                return stockPriceQuote;
            }
            catch (InvalidOperationException ex)
            {
                FinnhubException finnhubException = new FinnhubException("Unable to retrieve stock price quote.", ex);
                throw finnhubException;
            }
        }

        async public Task<Dictionary<string, double>> GetMultipleStockPriceQuote(List<string> stockSymbols)
        {
            Dictionary<string, double> stockSymbolQuoteDict = new();

            foreach (string stockSymbol in stockSymbols)
            {
                double stockPriceQuote = await GetStockPriceQuote(stockSymbol);
                stockSymbolQuoteDict.Add(stockSymbol, stockPriceQuote);
            }

            return stockSymbolQuoteDict;
        }

    }
}
