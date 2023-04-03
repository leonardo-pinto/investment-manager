namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for creating a new stock position
    /// </summary>
    public class AddStockPositionRequest
    {
        /// <summary>
        /// Unique symbol of the stock position
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The number of shares of this stock position
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The average price of the stock position
        /// </summary>
        public double AveragePrice { get; set; }

        /// <summary>
        /// If the position will be added in the transaction database
        /// </summary>
        public bool AddAsTransaction { get; set; }
    }
}
