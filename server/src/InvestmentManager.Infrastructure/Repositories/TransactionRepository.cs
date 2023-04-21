using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _db;

        public TransactionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateTransaction(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsByUserId(string userId)
        {
            return await _db.Transactions.Where(e => e.UserId == userId).ToListAsync();
        }
    }
}
