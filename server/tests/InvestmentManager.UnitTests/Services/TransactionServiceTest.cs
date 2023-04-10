using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using InvestmentManager.ApplicationCore.Mapper;
using Moq;
using InvestmentManager.UnitTests.Helpers;

namespace InvestmentManager.UnitTests.Services
{
    public class TransactionServiceTest 
    {
        private readonly ITransactionService _sut;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly IFixture _fixture;

        public TransactionServiceTest()
        {
            MapperConfiguration? autoMapperConfig = new (cfg => cfg.AddProfile(new MappingProfile()));
            IMapper mapper = new Mapper(autoMapperConfig);

            _transactionRepositoryMock = new Mock<ITransactionRepository>(MockBehavior.Strict);
            _sut = new TransactionService(
                _transactionRepositoryMock.Object, mapper);
            _fixture = new Fixture();
        }

        #region CreateTransaction

        [Fact]
        async public Task CreateTransaction_ToBeSuccessful()
        {
            // Arrange
            AddTransactionRequest addTransactionRequest = _fixture.Build<AddTransactionRequest>().Create();
            double expectedCost = MockHelper.ExpectedCost(addTransactionRequest.Quantity, addTransactionRequest.Price);

            _transactionRepositoryMock
                .Setup(m => m.CreateTransaction(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            // Act
            TransactionResponse transactionResponse = await _sut.CreateTransaction(addTransactionRequest);

            // Assert
            transactionResponse.Symbol.Should().Be(addTransactionRequest.Symbol);
            transactionResponse.Quantity.Should().Be(addTransactionRequest.Quantity);
            transactionResponse.Price.Should().Be(addTransactionRequest.Price);
            transactionResponse.Cost.Should().Be(expectedCost);
            transactionResponse.DateAndTimeOfTransaction.Should().Be(addTransactionRequest.DateAndTimeOfTransaction);
            transactionResponse.TransactionType.Should().Be(addTransactionRequest.TransactionType.ToString());

            _transactionRepositoryMock.Verify(m => m.CreateTransaction(It.IsAny<Transaction>()), Times.Once);
    }
        #endregion

        #region GetAllTransactions

        [Fact]
        public async Task GetAllTransactions_ToBeSuccessful()
        {
            // Arrange
            List<Transaction> transactionsListMock = new()
            {
                _fixture.Build<Transaction>().Create(),
                _fixture.Build<Transaction>().Create(),
                _fixture.Build<Transaction>().Create(),
            };

            _transactionRepositoryMock
                .Setup(m => m.GetAllTransactions())
                .ReturnsAsync(transactionsListMock);

            // Act
            List<TransactionResponse> transactionListResponse = await _sut.GetAllTransactions();

            // Arrange
            transactionListResponse.Should().HaveSameCount(transactionsListMock);
            transactionListResponse[0].Quantity.Should().Be(transactionsListMock[0].Quantity);
            transactionListResponse[1].Quantity.Should().Be(transactionsListMock[1].Quantity);
            transactionListResponse[2].Quantity.Should().Be(transactionsListMock[2].Quantity);

            _transactionRepositoryMock
               .Verify(m => m.GetAllTransactions(), Times.Once);
        }
        #endregion
    }
}