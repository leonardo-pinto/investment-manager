using InvestmentManager.ApplicationCore.DTO;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents a service that makes HTTP requests to finnhub.io
    /// https://finnhub.io/docs/api/introduction
    /// </summary>
    public interface IFinnhubService
    {
        /// <summary>
        /// Get multiple stocks current price quote
        /// </summary>
        /// <param name="stockSymbols">Array of stock symbols to search for price quote</param>
        /// <returns>List of StockQuoteResult that contains stock symbol and latest price quote</returns>
        Task<List<StockQuoteResult>> GetMultipleStockPriceQuote(string[] stockSymbols);

        /// <summary>
        /// Check if the stock symbol is valid
        /// </summary>
        /// <param name="stockSymbol">Stock symbol</param>
        /// <returns>True if symbol is valid, false if it is invalid</returns>
        Task<bool> IsStockSymbolValid(string stockSymbol);
    }
}
