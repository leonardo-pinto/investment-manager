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
        public Guid PositionId { get; set; }

        /// <summary>
        /// Unique stock symbol of the transaction
        /// </summary>
        [Required(ErrorMessage = "Transaction symbol can't be null or empty")]
        public string Symbol { get; set; }

        /// <summary>
        /// The number of shares of this transaction
        /// </summary>
        [Required(ErrorMessage = "Transaction quantity can't be null or empty")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        /// <summary>
        /// The price of the transaction
        /// </summary>
        [Required(ErrorMessage = "Transaction price can't be null or empty")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.01")]
        public double Price { get; set; }

        /// <summary>
        /// The date and time of the transaction
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateAndTimeOfTransaction { get; set; }

        /// <summary>
        /// Type of the transaction (buy or sell)
        /// </summary>
        [Required(ErrorMessage = "Invalid transaction type")]
        public TransactionType TransactionType { get; set; }

        
    }
}
