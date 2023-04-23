using InvestmentManager.ApplicationCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvestmentManager.Infrastructure.AppDbContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<StockPosition> StockPositions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StockPosition>().ToTable("StockPositions");
            builder.Entity<Transaction>().ToTable("Transactions");

            // Table relations
            builder.Entity<Transaction>(entity =>
            {
                entity.HasOne<StockPosition>()
                .WithMany()
                .HasForeignKey(e => e.PositionId);
            });

            builder.Entity<StockPosition>(entity =>
            {
                entity.HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId);
            });
        }
    }
}


