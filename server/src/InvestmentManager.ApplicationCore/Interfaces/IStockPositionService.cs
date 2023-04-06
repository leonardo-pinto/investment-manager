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
        /// Get all stock positions of a given user
        /// </summary>
        /// <returns>Returns a list StockPositionResponse type</returns>
        Task<List<StockPositionResponse>> GetAllStockPositions();

        /// <summary>
        /// Get a single stock position of a given user
        /// </summary>
        /// <param name="positionId">Position id of the stock</param>
        /// <returns>Returns a StockPositionResponse type</returns>
        Task<StockPositionResponse?> GetSingleStockPosition(Guid? positionId);
    }
}
