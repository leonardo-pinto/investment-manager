using InvestmentManager.ApplicationCore.Domain.Entities;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents data access logic for managing StockPosition entity
    /// </summary>
    public interface IStockPositionRepository
    {
        /// <summary>
        /// Create a stock position to the data base
        /// </summary>
        /// <param name="stockPosition">Stock position object to add</param>
        Task CreateStockPosition(StockPosition stockPosition);

        /// <summary>
        /// Updates a stock position object 
        /// </summary>
        /// <param name="stockPosition">Stock position to update</param>
        /// <returns>Returns the stock position object after updating</returns>
        Task<StockPosition> UpdateStockPosition(StockPosition stockPosition);

        /// <summary>
        /// Returns all stock position in the database
        /// </summary>
        /// <returns>Returns a list containing all stock positions</returns>
        Task<List<StockPosition>> GetAllStockPositions();

        /// <summary>
        /// Get a stock position based on its id
        /// </summary>
        /// <param name="positionId">Id of the given stock position</param>
        /// <returns>A stock position object or null</returns>
        Task<StockPosition?> GetSingleStockPosition(Guid positionId);
    }
}
