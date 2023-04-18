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
        Task<StockPositionResponse?> UpdateStockPosition(UpdateStockPositionRequest updateStockPositionRequest);

        /// <summary>
        /// Gets all stock position for the given user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>List of stock positions</returns>
        Task<List<StockPositionResponse>> GetAllStockPositionsByUserId(string userId);

        /// <summary>
        /// Get a single stock position of a given user
        /// </summary>
        /// <param name="positionId">Position id of the stock</param>
        /// <returns>Returns a StockPositionResponse type</returns>
        Task<StockPositionResponse?> GetSingleStockPosition(Guid positionId);

        /// <summary>
        /// Delete stock position of a given position id
        /// </summary>
        /// <param name="positionId">Position id</param>
        /// <returns>Boolean representing if the method was succeeded</returns>
        Task<bool> DeleteStockPosition(Guid positionId);
    }
}
