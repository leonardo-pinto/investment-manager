using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Enums;
using System.Diagnostics;

namespace InvestmentManager.UnitTests.DTO
{
    public class UpdateStockPositionRequestTest
    {
        private readonly IFixture _fixture;

        public UpdateStockPositionRequestTest()
        {
            _fixture = new Fixture();
        }

        #region ToAddTransactionRequest

        [Fact]
        public void ToAddTransactionRequest_ToBeSucessful()
        {
            UpdateStockPositionRequest updateStockPositionRequest =
                _fixture.Build<UpdateStockPositionRequest>().Create();

            AddTransactionRequest addTransactionRequest = 
                updateStockPositionRequest.ToAddTransactionRequest(
                    updateStockPositionRequest.PositionId, updateStockPositionRequest.TransactionType);

            addTransactionRequest.PositionId.Should().Be(updateStockPositionRequest.PositionId);
            addTransactionRequest.Symbol.Should().Be(updateStockPositionRequest.Symbol);
            addTransactionRequest.Quantity.Should().Be(updateStockPositionRequest.Quantity);
            addTransactionRequest.Price.Should().Be(updateStockPositionRequest.Price);
            addTransactionRequest.DateAndTimeOfTransaction.Should().Be(updateStockPositionRequest.DateAndTimeOfStockPosition);
            addTransactionRequest.TransactionType.Should().Be(updateStockPositionRequest.TransactionType);
        }
        #endregion
    }
}
