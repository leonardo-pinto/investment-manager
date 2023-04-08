using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using InvestmentManager.ApplicationCore.Mapper;
using InvestmentManager.UnitTests.Helpers;
using Moq;

namespace InvestmentManager.UnitTests.Services
{
    public class TransactionServiceTest 
    {
        private readonly ITransactionService _sut;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly IFixture _fixture;

        public TransactionServiceTest()
        {
            MapperConfiguration? autoMapperConfig = new (cfg => cfg.AddProfile(new TransactionProfile()));
            IMapper mapper = new Mapper(autoMapperConfig);

            _transactionRepositoryMock = new Mock<ITransactionRepository>(MockBehavior.Strict);
            _sut = new TransactionService(
                _transactionRepositoryMock.Object, mapper);
            _fixture = new Fixture();
        }

        #region CreateTransaction

        [Fact]
        async public Task CreateTransaction_BeSuccessful()
        {
            // Arrange
            AddTransactionRequest addTransactionRequest = _fixture.Build<AddTransactionRequest>().Create();
            double expectedCost = addTransactionRequest.Price * addTransactionRequest.Quantity;

            _transactionRepositoryMock
                .Setup(m => m.AddTransaction(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            // Act
            TransactionResponse transactionResponse = await _sut.CreateTransaction(addTransactionRequest);

            // Assert
            transactionResponse.Symbol.Should().Be(addTransactionRequest.Symbol);
            transactionResponse.Quantity.Should().Be(addTransactionRequest.Quantity);
            transactionResponse.Cost.Should().Be(expectedCost);
            transactionResponse.DateAndTimeOfTransaction.Should().Be(addTransactionRequest.DateAndTimeOfTransaction);
            transactionResponse.Symbol.Should().Be(addTransactionRequest.Symbol);
            transactionResponse.TransactionType.Should().Be(addTransactionRequest.TransactionType.ToString());

            _transactionRepositoryMock.Verify(m => m.AddTransaction(It.IsAny<Transaction>()), Times.Once);
    }
        #endregion

        #region GetTransactionHistory

        [Fact]
        public async Task GetTransactionHistory_NonMatchingPositionId_ToBeEmpty()
        {
            // Arrange
            Guid positionId = _fixture.Create<Guid>();
            List<Transaction> transactionsListMock = new();

            _transactionRepositoryMock
                .Setup(temp => temp.GetTransactionByStockPositionId(It.IsAny<Guid>()))
                .ReturnsAsync(transactionsListMock);

            // Act
            List<TransactionResponse> transactionListResponse = await _sut.GetTransactionHistory(positionId);

            // Assert
            transactionListResponse.Should().BeEmpty();
            _transactionRepositoryMock
                .Verify(m => m.GetTransactionByStockPositionId(positionId), Times.Once);
        }

        [Fact]
        public async Task GetTransactionHistory_ValidData_ToBeSuccessful()
        {
            // Arrange
            Guid positionId = _fixture.Create<Guid>();

            List<Transaction> transactionsListMock = new()
            {
                _fixture.Build<Transaction>().With(e => e.PositionId, positionId).Create(),
                _fixture.Build<Transaction>().With(e => e.PositionId, positionId).Create(),
                _fixture.Build<Transaction>().With(e => e.PositionId, positionId).Create(),
            };

            _transactionRepositoryMock
                .Setup(m => m.GetTransactionByStockPositionId(It.IsAny<Guid>()))
                .ReturnsAsync(transactionsListMock);

            // Act
            List<TransactionResponse> transactionListResponse = await _sut.GetTransactionHistory(positionId);

            // Arrange
            transactionListResponse.Should().HaveSameCount(transactionsListMock);
            transactionListResponse[0].Quantity.Should().Be(transactionsListMock[0].Quantity);
            transactionListResponse[1].Quantity.Should().Be(transactionsListMock[1].Quantity);
            transactionListResponse[2].Quantity.Should().Be(transactionsListMock[2].Quantity);

            _transactionRepositoryMock
               .Verify(m => m.GetTransactionByStockPositionId(positionId), Times.Once);
        }

        #endregion
    }
}