
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    /// <summary>
    /// Represents data access logic for managing Transaction entity
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Add a transaction to the data base
        /// </summary>
        /// <param name="transaction">Transaction object to add</param>
        /// <returns>Returns the transaction object after adding to the database</returns>
        Task<Transaction> AddTransaction(Transaction transaction);


        /// <summary>
        /// Get all the transaction for a given stock position
        /// </summary>
        /// <param name="positionId">Position id</param>
        /// <returns>Returns a list containing all transactions</returns>
        Task<List<TransactionResponse>?> GetTransactionByStockPositionId(string positionId);
    }
}
