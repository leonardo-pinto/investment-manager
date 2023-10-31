namespace InvestmentManager.ApplicationCore.Interfaces
{
    public interface IStockQuoteRepository
    {
        /// <summary>
        /// Get a stock current price quote
        /// </summary>
        /// <param name="stockSymbol">Stock symbol to search</param>
        /// <returns>Current price of the stock</returns>
        Task<double> GetStockPriceQuote(string stockSymbol);
    }
}
