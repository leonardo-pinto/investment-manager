namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents a service that makes HTTP requests to finnhub.io
    /// https://finnhub.io/docs/api/introduction
    /// </summary>
    public interface IFinnhubService
    {
        /// <summary>
        /// Get a single stock current price quote
        /// </summary>
        /// <param name="stockSymbol">Stock symbol to search for price quote</param>
        /// <returns>Stock latest price quote</returns>
        Task<double> GetStockPriceQuote(string stockSymbol);

        /// <summary>
        /// Get multiple stocks current price quote
        /// </summary>
        /// <param name="stockSymbols">Array of stock symbols to search for price quote</param>
        /// <returns>Dictionary that contains stock symbol and latest price quote</returns>
        Task<Dictionary<string, double>> GetMultipleStockPriceQuote(string[] stockSymbols);
    }
}
