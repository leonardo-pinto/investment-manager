
using InvestmentManager.ApplicationCore.Domain.Entities;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents data access logic for managing Transaction entity
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Create a transaction to the data base
        /// </summary>
        /// <param name="transaction">Transaction object to add</param>
        Task CreateTransaction(Transaction transaction);


        /// <summary>
        /// Get all the transactions of the given user id in the data base
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Returns a list containing all transactions</returns>
        Task<List<Transaction>> GetAllTransactionsByUserId(string userId);
    }
}
