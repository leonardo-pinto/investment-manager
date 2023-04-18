﻿using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using InvestmentManager.ApplicationCore.Mapper;
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

            _transactionRepositoryMock
                .Setup(m => m.CreateTransaction(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            // Act
            TransactionResponse transactionResponse = await _sut.CreateTransaction(addTransactionRequest);

            // Assert
            transactionResponse.Symbol.Should().Be(addTransactionRequest.Symbol);
            transactionResponse.Quantity.Should().Be(addTransactionRequest.Quantity);
            transactionResponse.Price.Should().Be(addTransactionRequest.Price);
            transactionResponse.DateAndTimeOfTransaction.Should().Be(addTransactionRequest.DateAndTimeOfTransaction);
            transactionResponse.TransactionType.Should().Be(addTransactionRequest.TransactionType.ToString());

            _transactionRepositoryMock.Verify(m => m.CreateTransaction(It.IsAny<Transaction>()), Times.Once);
    }
        #endregion

        #region GetAllTransactionsByUserId

        [Fact]
        public async Task GetAllTransactionsByUserId_ToBeSuccessful()
        {
            // Arrange
            string userId = _fixture.Create<string>();
            List<Transaction> transactionsListMock = new()
            {
                _fixture.Build<Transaction>().Create(),
                _fixture.Build<Transaction>().Create(),
                _fixture.Build<Transaction>().Create(),
            };

            _transactionRepositoryMock
                .Setup(m => m.GetAllTransactionsByUserId(It.IsAny<string>()))
                .ReturnsAsync(transactionsListMock);

            // Act
            List<TransactionResponse> transactionListResponse = await _sut.GetAllTransactionsByUserId(userId);

            // Arrange
            transactionListResponse.Should().HaveSameCount(transactionsListMock);
            transactionListResponse[0].Quantity.Should().Be(transactionsListMock[0].Quantity);
            transactionListResponse[1].Quantity.Should().Be(transactionsListMock[1].Quantity);
            transactionListResponse[2].Quantity.Should().Be(transactionsListMock[2].Quantity);

            _transactionRepositoryMock
               .Verify(m => m.GetAllTransactionsByUserId(userId), Times.Once);
        }
        #endregion
    }
}