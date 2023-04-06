using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using Moq;
using System.Diagnostics;

namespace InvestmentManager.UnitTests.Services
{
    public class TransactionServiceTest : IDisposable
    {
        private readonly MockRepository _mockRepository;
        private readonly ITransactionService _transactionService;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly IFixture _fixture;

        public TransactionServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _transactionRepositoryMock = new Mock<ITransactionRepository>(MockBehavior.Strict);
            _transactionService = new TransactionService(_transactionRepositoryMock.Object);
            _fixture = new Fixture();
        }

        #region CreateTransaction

        [Fact]
        public async Task CreateTransaction_NullAddTransactionRequest_ToBeArgumentNullException()
        {
            AddTransactionRequest? addTransactionRequest = null;

            Func<Task> action = async () =>
            {
                await _transactionService.CreateTransaction(addTransactionRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'addTransactionRequest')");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task CreateTransaction_QuantityIsLessThanMinimum_ToBeArgumentException(int quantity)
        {
            AddTransactionRequest? addTransactionnRequest = _fixture.Build<AddTransactionRequest>()
                .With(temp => temp.Quantity, quantity)
                .Create();

            Func<Task> action = async () =>
            {
                await _transactionService.CreateTransaction(addTransactionnRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Quantity must be greater than 0");
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.001)]
        [InlineData(-1.0)]
        public async Task CreateTransaction_PriceIsLessThanMinimum_ToBeArgumentException(double? price)
        {
            AddTransactionRequest? addTransactionnRequest = _fixture.Build<AddTransactionRequest>()
                .With(temp => temp.Price, price)
                .Create();

            Func<Task> action = async () =>
            {
                await _transactionService.CreateTransaction(addTransactionnRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Price must be greater than 0.01");
        }

        [Fact]
        public async Task CreateTransaction_ValidData_ToBeSuccessful()
        {
            AddTransactionRequest addTransactionRequest = _fixture.Build<AddTransactionRequest>().Create();

            Transaction transactionExpected = addTransactionRequest.ToTransaction();

            _transactionRepositoryMock
                .Setup(temp => temp.AddTransaction(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            TransactionResponse transactionResponseExpected = transactionExpected.ToTransactionResponse();

            TransactionResponse transactionResponse = await _transactionService.CreateTransaction(addTransactionRequest);

            transactionResponse.Should().BeEquivalentTo(transactionResponseExpected);
        }

        #endregion

        #region GetTransactionHistory

        [Fact]
        public async Task GetTransactionHistory_NonMatchingPositionId_ToBeEmpty()
        {
            Guid positionId = _fixture.Create<Guid>();

            List<Transaction> transactions = new List<Transaction>();

            _transactionRepositoryMock
                .Setup(temp => temp.GetTransactionByStockPositionId(It.IsAny<Guid>()))
                .ReturnsAsync(transactions);

            List<TransactionResponse>? transactionResponse = await _transactionService.GetTransactionHistory(positionId);

            transactionResponse.Should().BeEmpty();
        }

        [Fact]
        public async Task GetTransactionHistory_ValidData_ToBeSuccessful()
        {
            Guid positionId = _fixture.Create<Guid>();

            List<Transaction> transactions = new List<Transaction>()
            { 
                _fixture.Build<Transaction>()
                .With(temp => temp.PositionId, positionId)
                .Create(),

                _fixture.Build<Transaction>()
                .With(temp => temp.PositionId, positionId)
                .Create(),

                _fixture.Build<Transaction>()
                .With(temp => temp.PositionId, positionId)
                .Create(),
            };

            List<TransactionResponse> transactionHistoryExpected = transactions.Select(temp => temp.ToTransactionResponse()).ToList();

            _transactionRepositoryMock
                .Setup(temp => temp.GetTransactionByStockPositionId(It.IsAny<Guid>()))
                .ReturnsAsync(transactions);

            List<TransactionResponse> transactionHistory = await _transactionService.GetTransactionHistory(positionId);

            transactionHistory.Should().BeEquivalentTo(transactionHistoryExpected);
        }

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }
        #endregion
    }
}