using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        Task<double> IFinnhubRepository.GetStockPriceQuote(string stockSymbol)
        {
            throw new NotImplementedException();
        }
    }
}
