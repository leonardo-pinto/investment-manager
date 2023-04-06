using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Enums;
using System.Diagnostics;

namespace InvestmentManager.UnitTests.Entities
{
    public class TransactionTest
    {
        private readonly IFixture _fixture;

        public TransactionTest()
        {
            _fixture = new Fixture();
        }

        #region ToTransactionResponse

        public void ToTransactionResponse_ToBeSuccessful()
        {
            Transaction transaction = _fixture.Build<Transaction>().Create();

            TransactionResponse transactionResponse = transaction.ToTransactionResponse();

            transactionResponse.Symbol.Should().Be(transaction.Symbol);
            transactionResponse.Quantity.Should().Be(transaction.Quantity);
            transactionResponse.Price.Should().Be(transaction.Price);
            transactionResponse.Cost.Should().Be(transaction.Cost);
            transactionResponse.DateAndTimeOfTransaction.Should().Be(transaction.DateAndTimeOfTransaction);
            transactionResponse.TransactionType.Should().Be(transaction.TransactionType);
        }
        #endregion
    }
}
