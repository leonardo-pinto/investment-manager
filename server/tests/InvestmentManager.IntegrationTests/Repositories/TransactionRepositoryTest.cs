using Microsoft.EntityFrameworkCore;
using InvestmentManager.Infrastructure.AppDbContext;
using InvestmentManager.Infrastructure.Repositories;
using AutoFixture;
using InvestmentManager.ApplicationCore.Domain.Entities;
using FluentAssertions;

namespace InvestmentManager.IntegrationTests.Repositories
{
    public class TransactionRepositoryTest
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly IFixture _fixture;

        public TransactionRepositoryTest()
        {
            _fixture = new Fixture();
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: _fixture.Create<string>())
                .Options;
        }

        #region CreateTransaction

        [Fact]
        public async Task CreateTransaction_ToBeSuccessful()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var transactionRepository = new TransactionRepository(db);

            var transaction = _fixture.Build<Transaction>().Create();

            // Act
            await transactionRepository.CreateTransaction(transaction);

            // Assert
            var resultFromDb = await db.Transactions.ToListAsync();

            resultFromDb.Should().HaveCount(1);
            resultFromDb[0].Should().BeEquivalentTo(transaction);
        }

        #endregion

        #region GetAllTransactionsById

        [Fact]
        public async Task GetAllTransactionsById_WhenTransactionsExists_ToBeSuccessful()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var transactionRepository = new TransactionRepository(db);

            var transaction = _fixture.Build<Transaction>().Create();
            db.Add(transaction);
            await db.SaveChangesAsync();

            // Act
            var result = await transactionRepository.GetAllTransactionsByUserId(transaction.UserId);

            // Assert
            result.Should().HaveCount(1);
            result.ElementAt(0).Should().BeEquivalentTo(transaction);
        }

        [Fact]
        public async Task GetAllTransactionsById_WhenThereIsNotTransactions_ToBeEmpty()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var transactionRepository = new TransactionRepository(db);

            string userId = _fixture.Create<string>();

            // Act
            var result = await transactionRepository.GetAllTransactionsByUserId(userId);

            // Assert
            result.Should().HaveCount(0);
            result.Should().BeEmpty();
        }

        #endregion
    }
}
