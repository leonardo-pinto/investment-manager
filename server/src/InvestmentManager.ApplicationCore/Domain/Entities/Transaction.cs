using InvestmentManager.ApplicationCore.DTO;
using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.Domain.Entities
{
    /// <summary>
    /// Transaction domain model class
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Unique Id of the transaction
        /// </summary>
        [Key]
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Unique user id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Unique Id of the position from the transaction
        /// </summary>
        public Guid PositionId { get; set; }

        /// <summary>
        /// Unique Symbol of the transaction
        /// </summary>

        public string Symbol { get; set; }

        /// <summary>
        /// The number of shares of this transaction
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The price of the transaction
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The date and time of the transaction
        /// </summary>
        public DateTime DateAndTimeOfTransaction { get; set; }

        /// <summary>
        /// Type of the transaction (buy or sell)
        /// </summary>
        public string TransactionType { get; set; }
    }
}
