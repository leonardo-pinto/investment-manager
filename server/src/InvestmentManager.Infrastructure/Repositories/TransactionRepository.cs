using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public Task AddTransaction(Transaction transaction)
        {
            // add to db
            // save changes
            throw new NotImplementedException();
        }

        public Task CreateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<List<Transaction>> GetAllTransactions()
        {
            throw new NotImplementedException();
        }
    }
}
