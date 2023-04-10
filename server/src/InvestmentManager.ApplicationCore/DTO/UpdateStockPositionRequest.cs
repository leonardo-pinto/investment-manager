using InvestmentManager.ApplicationCore.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for updating a stock position
    /// </summary>
    public class UpdateStockPositionRequest
    {
        /// <summary>
        /// Quantity of the stock
        /// </summary>
        [Required(ErrorMessage = "Quantity can't be null or empty")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        /// <summary>
        /// Price of the stock
        /// </summary>
        [Required(ErrorMessage = "Price can't be null or empty")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.01")]
        public double Price { get; set; }

        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// The date and time of the stock position
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateAndTimeOfStockPosition { get; set; }
    }
}
