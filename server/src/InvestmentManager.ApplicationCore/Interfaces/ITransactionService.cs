using InvestmentManager.ApplicationCore.DTO;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents business logic for manipulating Transaction entity
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Add a new transaction
        /// </summary>
        /// <param name="addTransactionRequest">Transaction to add</param>
        /// <returns>Returns the transaction details</returns>
        Task<TransactionResponse> AddTransactionRequest(AddTransactionRequest addTransactionRequest);

        /// <summary>
        /// Get all the transactions for a given stock position
        /// </summary>
        /// <param name="positionId">Position id</param>
        /// <returns>Returns a list containing all transactions for the given position id</returns>
        Task<List<TransactionResponse>?> GetTransactionHistory(string positionId);
    }
}
