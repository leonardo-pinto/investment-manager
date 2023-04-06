namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO class that represents the return of a single stock position
    /// </summary>
    public class StockPositionResponse
    {
        /// <summary>
        /// Unique stock position id
        /// </summary>
        public Guid PositionId { get; set; }

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
        /// The cost of the stock position (Quantity * AveragePrice)
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// Current stock price based on latest quote
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Current stock market value (Quantity * Price)
        /// </summary>
        public double MarketValue { get; set; }

        /// <summary>
        /// Current percentual gain ((Price/AveragePrice) - 1)
        /// </summary>
        public double PercentualGain { get; set; }

        /// <summary>
        /// Current monetary gain (MarketValue - Cost)
        /// </summary>
        public double MonetaryGain { get; set; }
    }
}
