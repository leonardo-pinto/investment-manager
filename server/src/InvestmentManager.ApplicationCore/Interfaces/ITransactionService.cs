using InvestmentManager.ApplicationCore.DTO;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents business logic for manipulating Transaction entity
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Create a transaction for an existing stock position
        /// </summary>
        /// <param name="addTransaction">Transaction to add</param>
        /// <returns>Transaction details</returns>
        Task<TransactionResponse> CreateTransaction(AddTransactionRequest addTransactionRequest);

        /// <summary>
        /// Get all the transactions of a given user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Returns a list containing all transactions</returns>
        Task<List<TransactionResponse>> GetAllTransactionsByUserId(string userId);
    }
}
