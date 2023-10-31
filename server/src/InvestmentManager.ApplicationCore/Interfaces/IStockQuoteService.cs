using InvestmentManager.ApplicationCore.DTO;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents a service to get stock quotes
    /// </summary>
    public interface IStockQuoteService
    {
        /// <summary>
        /// Get multiple stocks current price quote
        /// </summary>
        /// <param name="stockSymbols">String containing stock symbols to search for price quote</param>
        /// <returns>List of StockQuoteResult that contains stock symbol and latest price quote</returns>
        Task<List<StockQuoteResult>> GetStockPriceQuote(string stockSymbols, string tradingCountry);

        /// <summary>
        /// Check if the stock symbol is valid
        /// </summary>
        /// <param name="stockSymbol">Stock symbol</param>
        /// <returns>True if symbol is valid, false if it is invalid</returns>
        Task<bool> IsStockSymbolValid(string stockSymbol, string tradingCountry);
    }
}
