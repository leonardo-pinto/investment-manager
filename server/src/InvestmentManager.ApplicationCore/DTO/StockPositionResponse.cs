﻿namespace InvestmentManager.ApplicationCore.DTO
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
        public required string Symbol { get; set; }

        /// <summary>
        /// The number of shares of this stock position
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// The average price of the stock position
        /// </summary>
        public required double AveragePrice { get; set; }
    }

    public class StockPositionsResponse
    {
        public IEnumerable<StockPositionResponse>? StockPositions { get; set; }
    }

}
