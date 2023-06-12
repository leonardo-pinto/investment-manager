
using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.Enums;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.Infrastructure.AppDbContext;
using InvestmentManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InvestmentManager.IntegrationTests.Repositories
{
    public class StockPositionRepositoryTest
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly IFixture _fixture;

        public StockPositionRepositoryTest()
        {
            _fixture = new Fixture();
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: _fixture.Create<string>())
                .Options;
        }

        #region CreateStockPosition

        [Fact]
        public async Task CreateStockPosition_ToBeSuccessful()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var stockPositionRepository = new StockPositionRepository(db);

            var stockPosition = _fixture.Build<StockPosition>().Create();

            // Act
            await stockPositionRepository.CreateStockPosition(stockPosition);

            // Assert
            var resultFromDb = await db.StockPositions.ToListAsync();

            resultFromDb.Should().HaveCount(1);
            resultFromDb[0].Should().BeEquivalentTo(stockPosition);
        }

        #endregion

        #region GetSingleStockPosition

        [Fact]
        public async Task GetSingleStockPosition_WhenIdIsValid_ToBeSuccessful()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var stockPositionRepository = new StockPositionRepository(db);
            var stockPosition = _fixture.Build<StockPosition>().Create();
            db.StockPositions.Add(stockPosition);
            await db.SaveChangesAsync();

            // Act
            var result = await stockPositionRepository.GetSingleStockPosition(stockPosition.PositionId);

            // Assert
            result.Should().BeEquivalentTo(stockPosition);
        }


        [Fact]
        public async Task GetSingleStockPosition_WhenIdIsInvalid_ToBeNull()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var stockPositionRepository = new StockPositionRepository(db);

            // Act
            var result = await stockPositionRepository.GetSingleStockPosition(_fixture.Create<Guid>());

            // Assert
            result.Should().BeNull();
        }
        #endregion


        #region DeleteStockPosition

        [Fact]
        public async Task DeleteStockPosition_WhenSuccessful_ToBeTrue()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var stockPositionRepository = new StockPositionRepository(db);
            var stockPosition = _fixture.Build<StockPosition>().Create();
            db.StockPositions.Add(stockPosition);
            await db.SaveChangesAsync();

            // Act
            var result = await stockPositionRepository.DeleteStockPosition(stockPosition.PositionId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteStockPosition_WhenNotSuccessful_ToBeFalse()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var stockPositionRepository = new StockPositionRepository(db);

            // Act
            var result = await stockPositionRepository.DeleteStockPosition(_fixture.Create<Guid>());

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region StockSymbolAlreadyExists

        [Fact]
        public async Task StockSymbolAlreadyExists_WhenAlreadyExists_ToBeTrue()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var stockPositionRepository = new StockPositionRepository(db);
            var stockPosition = _fixture.Build<StockPosition>().With(e => e.Quantity, 10).Create();
            db.StockPositions.Add(stockPosition);
            await db.SaveChangesAsync();

            // Act
            var result = await stockPositionRepository.StockSymbolAlreadyExists(stockPosition.Symbol, stockPosition.UserId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task StockSymbolAlreadyExists_WhenDoestNotExist_ToBeFalse()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var stockPositionRepository = new StockPositionRepository(db);

            // Act
            var result = await stockPositionRepository.StockSymbolAlreadyExists(_fixture.Create<string>(), _fixture.Create<string>());

            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region GetAllStockPositionsByUserIdAndTradingCountry

        [Fact]
        public async Task GetAllStockPositionsByUserIdAndtradingCountry_WhenExists_ToBeSuccessful()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var stockPositionRepository = new StockPositionRepository(db);

            var userIdMock = _fixture.Create<string>();
            var tradingCountryMock = TradingCountry.BR.ToString();
            var stockPosition1 = _fixture
                .Build<StockPosition>()
                .With(e => e.UserId, userIdMock)
                .With(e => e.TradingCountry, tradingCountryMock)
                .Create();

            var stockPosition2 = _fixture
               .Build<StockPosition>()
               .With(e => e.UserId, userIdMock)
               .With(e => e.TradingCountry, tradingCountryMock)
               .Create();

            db.StockPositions.Add(stockPosition1);
            db.StockPositions.Add(stockPosition2);
            await db.SaveChangesAsync();

            // Act
            var result = await stockPositionRepository.GetAllStockPositionsByUserIdAndTradingCountry(userIdMock, tradingCountryMock);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllStockPositionsByUserIdAndtradingCountry_WhenDoesNotExists_ToBeEmpty()
        {
            // Arrange
            var db = new ApplicationDbContext(_dbContextOptions);
            var stockPositionRepository = new StockPositionRepository(db);

            // Act
            var result = await stockPositionRepository.GetAllStockPositionsByUserIdAndTradingCountry(_fixture.Create<string>(), _fixture.Create<string>());

            // Assert
            result.Should().BeEmpty();
        }
        #endregion
    }
}
