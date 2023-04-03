using InvestmentManager.ApplicationCore.Enums;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for updating a stock position
    /// </summary>
    public class UpdateStockPositionRequest
    {
        /// <summary>
        /// Id of the stock position
        /// </summary>
        public string PositionId { get; set; }

        /// <summary>
        /// Quantity of the stock
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Price of the stock
        /// </summary>
        public double Price { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}
