using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.Domain.Entities
{
    /// <summary>
    /// Position domain model class
    /// </summary>
    public class StockPosition
    {
        /// <summary>
        /// Unique Id of the stock position
        /// </summary>
        [Key]
        public Guid PositionId { get; set; }


        /// <summary>
        /// Unique user id
        /// </summary>
        public required string UserId { get; set; }

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

        /// <summary>
        /// Country in which the stock is traded
        /// </summary>
        public required string TradingCountry { get; set; }

        /// <summary>
        /// Type of the stock. e.g., stock, reit or bond
        /// </summary>
        public required  string Type { get; set; }



        /// <summary>
        /// Calculates an updated average price
        /// </summary>
        /// <param name="increasedQuantity">Number of new stocks</param>
        /// <param name="newPrice">Price of new stocks</param>
        /// <returns>Upated average price based on stock previous quantity and price</returns>

        public double UpdateAveragePrice(int increasedQuantity, double newPrice)
        {
            int totalQuantity = increasedQuantity + Quantity;
            double averagePrice = ((Quantity * AveragePrice) + (increasedQuantity * newPrice)) / totalQuantity;
            return Math.Round(averagePrice, 2);
        }
    }
}
