using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for getting a single stock quote based on its symbol
    /// </summary>
    public class SingleStockQuoteRequest
    {
        /// <summary>
        /// Symbol of the stock
        /// </summary>
        [Required(ErrorMessage = "Stock symbol can't be null or empty")]
        public string Symbol { get; set; }
    }
}
