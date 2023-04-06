

using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.ApplicationCore.Services
{
    public class FinnhubService : IFinnhubService
    {
        Task<double> IFinnhubService.GetStockPriceQuote(string stockSymbol)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, double>> GetMultipleStockPriceQuote(List<string> stockSymbols)
        {
            throw new NotImplementedException();
        }

    }
}
