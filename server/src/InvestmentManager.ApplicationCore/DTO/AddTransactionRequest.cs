using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for creating a new transaction
    /// </summary>
    public class AddTransactionRequest
    {
        /// <summary>
        /// Position id of the stock position for the given transaction
        /// </summary>
        [Required(ErrorMessage = "Position ID can't be null or empty")]
        public required Guid PositionId { get; set; }

        /// <summary>
        /// Unique user id
        /// </summary>
        public required string UserId { get; set; }

        /// <summary>
        /// Unique stock symbol of the transaction
        /// </summary>
        [Required(ErrorMessage = "Transaction symbol can't be null or empty")]
        public required string Symbol { get; set; }

        /// <summary>
        /// The number of shares of this transaction
        /// </summary>
        [Required(ErrorMessage = "Transaction quantity can't be null or empty")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public required int Quantity { get; set; }

        /// <summary>
        /// The price of the transaction
        /// </summary>
        [Required(ErrorMessage = "Transaction price can't be null or empty")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.01")]
        public required double Price { get; set; }

        /// <summary>
        /// The date and time of the transaction
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public required DateTime DateAndTimeOfTransaction { get; set; }

        /// <summary>
        /// Type of the transaction (buy or sell)
        /// </summary>
        [Required(ErrorMessage = "Invalid transaction type")]
        public required TransactionType TransactionType { get; set; }

        /// <summary>
        /// The country in which the stock is negotiated
        /// </summary>
        [Required(ErrorMessage = "Trading country can't be null or empty")]
        public required TradingCountry TradingCountry { get; set; }


    }
}
