using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Enums;

namespace InvestmentManager.UnitTests.DTO
{
    public class AddTransactionRequestTest
    {
        private readonly IFixture _fixture;

        public AddTransactionRequestTest()
        {
            _fixture = new Fixture();
        }

        #region ToTransaction

        [Fact]
        public void ToTransaction_ToBeSuccessful()
        {
            AddTransactionRequest addTransactionRequest =
                _fixture.Build<AddTransactionRequest>().Create();

            Transaction transaction = addTransactionRequest.ToTransaction();

            double costExpected = addTransactionRequest.Quantity * addTransactionRequest.Price;

            transaction.PositionId.Should().Be(addTransactionRequest.PositionId);
            transaction.Symbol.Should().Be(addTransactionRequest.Symbol);
            transaction.Quantity.Should().Be(addTransactionRequest.Quantity);
            transaction.Price.Should().Be(addTransactionRequest.Price);
            transaction.Cost.Should().Be(costExpected);
            transaction.DateAndTimeOfTransaction.Should().Be(addTransactionRequest.DateAndTimeOfTransaction);
            transaction.TransactionType.Should().Be(addTransactionRequest.TransactionType.ToString());
        }

        #endregion
    }
}
