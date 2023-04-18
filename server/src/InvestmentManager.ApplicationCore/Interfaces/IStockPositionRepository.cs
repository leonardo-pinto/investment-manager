using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.Enums;

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
        Task UpdateStockPosition(StockPosition stockPosition);

        /// <summary>
        /// Get all stock position of a given user id in the data base
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="tradingCountry">Trading country of stock positions</param>
        /// <returns>Returns a list containing all stock positions</returns>
        Task<List<StockPosition>> GetAllStockPositionsByUserIdAndTradingCountry(string userId, string tradingCountry);

        /// <summary>
        /// Get a stock position based on its id
        /// </summary>
        /// <param name="positionId">Id of the given stock position</param>
        /// <returns>A stock position object or null</returns>
        Task<StockPosition?> GetSingleStockPosition(Guid positionId);

        /// <summary>
        /// Check if a stock position with the given symbol already exists
        /// </summary>
        /// <param name="symbol">Stock symbol</param>
        /// <param name="userId">User id</param>
        /// <returns>Boolean value</returns>
        Task<bool> StockSymbolAlreadyExists(string symbol, string userId);

        /// <summary>
        /// Delete a stock position based on a given position id
        /// </summary>
        /// <param name="positionId">Position id</param>
        /// <returns>Boolean value representing if the method was succeeded</returns>
        Task<bool> DeleteStockPosition(Guid positionId);
    }
}
