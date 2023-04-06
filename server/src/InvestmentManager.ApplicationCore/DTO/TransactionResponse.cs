using InvestmentManager.ApplicationCore.Enums;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for returning a transaction response
    /// </summary>
    public class TransactionResponse
    {
        /// <summary>
        /// Unique symbol of the stock
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The number of shares of the transaction
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The price of the transaction
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The cost of the transaction
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// The date and time of the transaction
        /// </summary>
        public DateTime DateAndTimeOfTransaction { get; set; }

        /// <summary>
        /// The type of the transaction (buy or sell)
        /// </summary>
        public string? TransactionType { get; set; }
    }
}
