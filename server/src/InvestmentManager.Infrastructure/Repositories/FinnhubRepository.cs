using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        Task<double> IFinnhubRepository.GetStockPriceQuote(string stockSymbol)
        {

            // if the response is null, throw exception
            // if the response contains a key error, throw exception
            // if the response does not contains the "c" key, throw exception
            // returns the "c" key-value
            throw new NotImplementedException();
        }
    }
}
