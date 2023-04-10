using InvestmentManager.ApplicationCore.DTO;
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
        /// The current stock price quote
        /// </summary>
        public double CurrentPrice { get; set; }

        /// <summary>
        /// The cost of the stock position (Quantity * AveragePrice)
        /// </summary>
        public double Cost { get; set; }

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
