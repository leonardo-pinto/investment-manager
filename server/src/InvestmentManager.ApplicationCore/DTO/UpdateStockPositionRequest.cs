using InvestmentManager.ApplicationCore.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for updating a stock position
    /// </summary>
    public class UpdateStockPositionRequest
    {
        /// <summary>
        /// Position id of the stock to be updated
        /// </summary>
        [Required(ErrorMessage = "PositionId can't be null or empty")]
        public Guid PositionId { get; set; }

        /// <summary>
        /// Stock position symbol
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Stock position symbol
        /// </summary>
        [Required(ErrorMessage = "Symbol can't be null or empty")]
        public required string Symbol { get; set; }

        /// <summary>
        /// Quantity of the stock
        /// </summary>
        [Required(ErrorMessage = "Quantity can't be null or empty")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public required int Quantity { get; set; }

        /// <summary>
        /// Price of the stock
        /// </summary>
        [Required(ErrorMessage = "Price can't be null or empty")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.01")]
        public required double Price { get; set; }

        /// <summary>
        /// Type of the transaction. Buy = 0, Sell = 1
        /// </summary>
        [Required(ErrorMessage = "Transaction type can't be null or empty")]
        [EnumDataType(typeof(TransactionType))]
        public required TransactionType TransactionType { get; set; }

        /// <summary>
        /// The country in which the stock is negotiated
        /// </summary>
        [Required(ErrorMessage = "Trading country can't be null or empty")]
        [EnumDataType(typeof(TradingCountry))]
        public required TradingCountry TradingCountry { get; set; }
    }
}
