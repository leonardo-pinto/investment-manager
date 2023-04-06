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
        public string Symbol { get; set; }

        /// <summary>
        /// The number of shares of this stock position
        /// </summary>
        [Required(ErrorMessage = "Quantity can't be null or empty")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        /// <summary>
        /// The average price of the stock position
        /// </summary>
        [Required(ErrorMessage = "Average price can't be null or empty")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Average price must be greater than 0.01")]
        public double AveragePrice { get; set; }

        /// <summary>
        /// The date and time of the stock position
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateAndTimeOfStockPosition { get; set; }

        /// <summary>
        /// Converts the current object of AddStockPositionRequest into a new object
        /// of StockPosition type
        /// </summary>
        /// <returns>A new object of StockPosition class</returns>
        public StockPosition ToStockPosition()
        {
            return new StockPosition()
            {
                Symbol = Symbol,
                Quantity = Quantity,
                AveragePrice = AveragePrice,
                Cost = Quantity * AveragePrice
            };
        }

        /// <summary>
        /// Converts the current object of AddStockPositionRequest
        /// into a new object of AddTransactionRequest type
        /// </summary>
        /// <returns></returns>

        public AddTransactionRequest ToAddTransactionRequest(Guid positionId, TransactionType transactionType)
        {
            return new AddTransactionRequest()
            { 
                PositionId = positionId,
                Symbol = Symbol,
                Quantity = Quantity,
                Price = AveragePrice,
                DateAndTimeOfTransaction = DateAndTimeOfStockPosition,
                TransactionType = transactionType
            };
        }
    }
}
