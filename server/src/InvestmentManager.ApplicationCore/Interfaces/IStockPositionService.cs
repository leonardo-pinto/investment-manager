using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents business logic for manipulating StockPosition entity
    /// </summary>
    public interface IStockPositionService
    {
        /// <summary>
        /// Creates a new stock position
        /// </summary>
        /// <param name="addStockPositionRequest">AddStockPosition request type</param>
        /// <returns>Returns the position details</returns>
        Task<StockPositionResponse?> CreateStockPosition(AddStockPositionRequest addStockPositionRequest);

        /// <summary>
        /// Updates a stock position
        /// </summary>
        /// <param name="updateStockPositionRequest">UpdateStockPositionRequest type</param>
        /// <returns>Returns the position updated details</returns>
        Task<StockPositionResponse?> UpdateStockPosition(UpdateStockPositionRequest updateStockPositionRequest, Guid positionId);

        /// <summary>
        /// Get all stock positions of a given user
        /// </summary>
        /// <returns>Returns a list StockPositionResponse type</returns>
        Task<List<StockPositionResponse>> GetAllStockPositions();

        /// <summary>
        /// Get a single stock position of a given user
        /// </summary>
        /// <param name="positionId">Position id of the stock</param>
        /// <returns>Returns a StockPositionResponse type</returns>
        Task<StockPositionResponse?> GetSingleStockPosition(Guid positionId);

        /// <summary>
        /// Given a list of stock symbols and a dictionary containing a price for each symbol,
        /// it updates all the prices
        /// </summary>
        /// <param name="stockPriceDict">Dictionary of stock symbol (key) and price (value)</param>
        /// <param name="stockPositions">List of stock symbols</param>
        /// <returns>List of stock position with updated prices</returns>
        List<StockPosition> UpdateStockPriceListBySymbol(
            Dictionary<string, double> stockPriceDict, List<StockPosition> stockPositions);
    }
}
