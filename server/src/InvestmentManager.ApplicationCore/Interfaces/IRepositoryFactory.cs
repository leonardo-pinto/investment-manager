namespace InvestmentManager.ApplicationCore.Interfaces
{
    public interface IRepositoryFactory
    {
        IStockQuoteRepository CreateRepository(string tradingCountry);
    }
}
