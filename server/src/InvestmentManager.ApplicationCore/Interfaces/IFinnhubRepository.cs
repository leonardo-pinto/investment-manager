
namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents a repository that makes HTTP requests to finnhub.io
    /// https://finnhub.io/docs/api/introduction
    /// </summary>
    public interface IFinnhubRepository
    {
        /// <summary>
        /// Get a stock current price quote
        /// </summary>
        /// <param name="stockSymbol">Stock symbol to search</param>
        /// <returns>Current price of the stock</returns>
        Task<double> GetStockPriceQuote(string stockSymbol);
    }
}
