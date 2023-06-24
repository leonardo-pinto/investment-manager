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
        public required string Symbol { get; set; }

        /// <summary>
        /// The number of shares of the transaction
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// The price of the transaction
        /// </summary>
        public required double Price { get; set; }

        /// <summary>
        /// The date and time of the transaction
        /// </summary>
        public required DateTimeOffset DateAndTimeOfTransaction { get; set; }

        /// <summary>
        /// The type of the transaction (buy or sell)
        /// </summary>
        public required string TransactionType { get; set; }

        /// <summary>
        /// Trading country of the transaction
        /// </summary>
        public required string TradingCountry { get; set; }
    }

    public class TransactionsResponse
    { 
        public required IEnumerable<TransactionResponse> Transactions { get; set; }
    }

}
