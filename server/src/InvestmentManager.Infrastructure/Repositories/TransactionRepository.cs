using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public Task AddTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<List<Transaction>> GetTransactionByStockPositionId(Guid positionId)
        {
            throw new NotImplementedException();
        }
    }
}
