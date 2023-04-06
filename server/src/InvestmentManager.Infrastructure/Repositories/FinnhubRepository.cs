using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        Task<Dictionary<string, object>> IFinnhubRepository.GetStockPriceQuote(string stockSymbol)
        {
            throw new NotImplementedException();
        }
    }
}
