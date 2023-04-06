
namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents a repository that makes HTTP requests to finnhub.tio
    /// https://finnhub.io/docs/api/introduction
    /// </summary>
    public interface IFinnhubRepository
    {
        /// <summary>
        /// Check if the given stock symbol is valid
        /// </summary>
        /// <param name="stockSymbol">Stock symbol to validate</param>
        /// <returns>Returns true if the symbol is valid, otherwise returns false</returns>
        Task<bool> IsStockSymbolValid(string stockSymbol);


        /// <summary>
        /// Get a single stock current price
        /// </summary>
        /// <param name="stockSymbol">Stock symbol to search</param>
        /// <returns>Returns an integer corresponding to the stock latest price</returns>
        Task<int>? GetSingleStockQuote(string stockSymbol);

        /// <summary>
        /// Get multiple stocks current price
        /// </summary>
        /// <param name="stockSymbols">Array of stock symbols to search</param>
        /// <returns>Returns a dictionary that contains the symbol and the latest price of each stock</returns>
        Task<Dictionary<string, int>?> GetMultipleStockQuotes(string[] stockSymbols);    
    }
}
