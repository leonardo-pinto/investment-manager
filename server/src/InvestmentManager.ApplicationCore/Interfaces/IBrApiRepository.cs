using InvestmentManager.ApplicationCore.DTO;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents a repository that makes HTTP requests to BrApi
    /// https://brapi.dev/docs
    /// Access Brazilian stocks information
    /// </summary>
    public interface IBrApiRepository
    {
        /// <summary>
        /// Gets the stock price of stock symbols
        /// </summary>
        /// <param name="concatenatedStockSymbols">Stock symbols string concatenated and separated by comma (e.g.: "PETR4,VALE3,BBDC4")</param>
        /// <returns>BrApiResponse type containing results and errors</returns>
        Task<BrApiResponse> GetStocksPriceQuote(string concatenatedStockSymbols);
    }
}
