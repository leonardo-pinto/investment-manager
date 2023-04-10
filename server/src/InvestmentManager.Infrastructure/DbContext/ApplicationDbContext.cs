using InvestmentManager.ApplicationCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestmentManager.Infrastructure.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<StockPosition> StockPositions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
