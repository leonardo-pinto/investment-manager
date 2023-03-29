using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for getting multiple stock quotes based on their symbols
    /// </summary>
    public class MultipleStockQuoteRequest
    {
        /// <summary>
        /// List containing the stock symbols
        /// </summary>
        [Required(ErrorMessage = "Stock symbols list can't be null or empty")]
        public List<string> Symbols { get; set; }
    }
}
