using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Enums;

namespace InvestmentManager.UnitTests.DTO
{
    public class AddStockPositionRequestTest
    {
        private readonly IFixture _fixture;

        public AddStockPositionRequestTest()
        {
            _fixture = new Fixture();
        }

        #region ToStockPosition

        [Fact]
        public void ToStockPosition_ToBeSuccessful()
        {
            AddStockPositionRequest addStockPositionRequest = 
                _fixture.Build<AddStockPositionRequest>().Create();

            StockPosition stockPosition = addStockPositionRequest.ToStockPosition();

            double costExpected = addStockPositionRequest.Quantity * addStockPositionRequest.AveragePrice;

            stockPosition.Symbol.Should().Be(addStockPositionRequest.Symbol);
            stockPosition.Quantity.Should().Be(addStockPositionRequest.Quantity);
            stockPosition.AveragePrice.Should().Be(addStockPositionRequest.AveragePrice);
            stockPosition.Cost.Should().Be(costExpected);
        }

        #endregion

        #region ToAddTransactionRequest

        [Fact]
        public void ToAddTransactionRequest_ToBeSuccessful()
        {
            Guid positionId = _fixture.Create<Guid>();
            TransactionType transactionType = _fixture.Create<TransactionType>();

            AddStockPositionRequest addStockPositionRequest =
                _fixture
                .Build<AddStockPositionRequest>()
                .Create();

            AddTransactionRequest addTransactionRequest =
                addStockPositionRequest.ToAddTransactionRequest(positionId, transactionType);

            double costExpected = addStockPositionRequest.Quantity * addStockPositionRequest.AveragePrice;

            addTransactionRequest.PositionId.Should().Be(positionId);
            addTransactionRequest.Symbol.Should().Be(addStockPositionRequest.Symbol);
            addTransactionRequest.Price.Should().Be(addStockPositionRequest.AveragePrice);
            addTransactionRequest.DateAndTimeOfTransaction.Should().Be(addStockPositionRequest.DateAndTimeOfStockPosition);
            addTransactionRequest.TransactionType.Should().Be(transactionType);

        }

        #endregion
    }
}
