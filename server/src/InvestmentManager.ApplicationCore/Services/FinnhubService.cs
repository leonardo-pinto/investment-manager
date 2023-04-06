

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

        public async Task<double> GetStockPriceQuote(string stockSymbol)
        {
            Dictionary<string, object> responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            // if the stock symbol is invalid, the api returns "c" as 0 (int type)
            if (responseDictionary["c"] is int @int)
            {
                if (@int == 0)
                {
                    throw new ArgumentException("Invalid stock symbol");
                }
            }

            double stockPrice = (double)responseDictionary["c"];
            return stockPrice;
            //catch (InvalidOperationException ex)
            //{
            //    // do something with repository exceptions
            //    throw new Exception("Catch from single stock quote");
            //}
        }

        public Task<Dictionary<string, double>> GetMultipleStockPriceQuote(List<string> stockSymbols)
        {
            throw new NotImplementedException();
        }

    }
}
