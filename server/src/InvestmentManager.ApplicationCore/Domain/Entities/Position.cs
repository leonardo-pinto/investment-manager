using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.Domain.Entities
{
    /// <summary>
    /// Position domain model class
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Unique Id of the position
        /// </summary>
        [Key]
        public Guid PositionId { get; set; }

        /// <summary>
        /// Unique Symbol of the position
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The number of shares of this position
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The average price of the position
        /// </summary>
        public double AveragePrice { get; set; }

        /// <summary>
        /// The cost of the position (Quantity * AveragePrice)
        /// </summary>
        public double Cost { get; set; }
    }
}
