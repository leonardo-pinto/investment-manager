using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for getting the transaction history of a given stock position
    /// </summary>
    public class TransactionHistoryRequest
    {
        /// <summary>
        /// Position id for the given stock
        /// </summary>
        [Required(ErrorMessage = "Position id can't be null or empty")]
        public string PositionId { get; set; }
    }
}
