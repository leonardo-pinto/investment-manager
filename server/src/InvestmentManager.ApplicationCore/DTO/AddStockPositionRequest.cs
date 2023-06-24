using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.Enums;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Stock symbol can't be null or empty")]
        public required string Symbol { get; set; }

        /// <summary>
        /// Unique user id
        /// </summary>
        [Required(ErrorMessage = "User id can't be null or empty")]
        public required string UserId { get; set; }

        /// <summary>
        /// The number of shares of this stock position
        /// </summary>
        [Required(ErrorMessage = "Quantity can't be null or empty")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public required int Quantity { get; set; }

        /// <summary>
        /// The average price of the stock position
        /// </summary>
        [Required(ErrorMessage = "Average price can't be null or empty")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Average price must be greater than 0.01")]
        public required double AveragePrice { get; set; }

        /// <summary>
        /// The country in which the stock is negotiated
        /// </summary>
        [Required(ErrorMessage = "Trading country can't be null or empty")]
        [EnumDataType(typeof(TradingCountry))]
        public required TradingCountry TradingCountry { get; set; }
    }
}
