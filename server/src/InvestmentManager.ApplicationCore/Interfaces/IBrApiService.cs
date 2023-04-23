using InvestmentManager.ApplicationCore.DTO;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents service that implements the business logic to interact with BrApi
    /// https://brapi.dev/docs
    /// Access Brazilian stocks information
    /// </summary>
    public interface IBrApiService
    {
        /// <summary>
        /// Checks if a stock symbol is valid
        /// </summary>
        /// <param name="stockSymbol">Stock symbol</param>
        /// <returns>Bool representing if stock symbol is valid</returns>
        Task<bool> IsStockSymbolValid(string stockSymbol);

        /// <summary>
        /// Get the quote prices of stock symbols
        /// </summary>
        /// <param name="concatenatedStockSymbols">Stock symbols string concatenated and separated by comma (e.g.: "PETR4,VALE3,BBDC4")</param>
        /// <returns>List of StockQuoteResult that contains stock symbol and latest price quote</returns>
        Task<List<StockQuoteResult>> GetStocksPriceQuote(string concatenatedStockSymbols);
    }
}
