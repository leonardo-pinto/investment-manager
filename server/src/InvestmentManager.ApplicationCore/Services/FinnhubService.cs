

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
            double stockPriceQuote = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            // if the stock symbol is invalid, the api returns "c" as 0 (int type)
            if (stockPriceQuote == 0)
            {
                throw new ArgumentException($"{stockSymbol} is an invalid stock symbol");
            }

            return stockPriceQuote;
            //catch (InvalidOperationException ex)
            //{
            //    // do something with repository exceptions
            //    throw new Exception("Catch from single stock quote");
            //}
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
