using InvestmentManager.ApplicationCore.Domain.Entities;
using AutoFixture;
using Xunit.Sdk;
using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using System.Diagnostics;

namespace InvestmentManager.UnitTests.Entities
{
    public class StockPositionTest
    {
        private readonly IFixture _fixture;

        public StockPositionTest()
        {
            _fixture = new Fixture();
        }

        #region UpdateAveragePrice

        [Theory]
        [InlineData(100, 10.0, 100, 30.0)]
        [InlineData(164, 94.92, 373, 175.93)]
        [InlineData(6456, 1999.92, 346246, 682.27)]
        public void UpdateAveragePrice_ReturnCorrectValues(
            int quantity,
            double price,
            int newQuantity,
            double newPrice)
        {
            StockPosition stockPosition = _fixture
                .Build<StockPosition>()
                .With(temp => temp.Quantity, quantity)
                .With(temp => temp.AveragePrice, price)
                .Create();

            double updatedAveragePrice = stockPosition.UpdateAveragePrice(newQuantity, newPrice);

            double averagePriceExpected =
                ((quantity * price) + (newQuantity * newPrice)) / (quantity + newQuantity);

            updatedAveragePrice.Should().Be(averagePriceExpected);
        }

        #endregion

        #region ToStockPositionResponse
        [Fact]
        public void ToStockPositionResponse_ToBeSuccessful()
        {
            StockPosition stockPosition = _fixture
                .Build<StockPosition>()
                .Create();

            double price = _fixture.Create<double>();

            StockPositionResponse stockPositionResponse = stockPosition.ToStockPositionResponse(price);

            double marketValueExpected = stockPosition.Quantity * price;
            double percentualGainExpected = ((price / stockPosition.AveragePrice) - 1);
            double monetaryGainExpected = ((stockPosition.Quantity * price) - stockPosition.Cost);

            stockPositionResponse.PositionId.Should().Be(stockPosition.PositionId);
            stockPositionResponse.Symbol.Should().Be(stockPosition.Symbol);
            stockPositionResponse.Quantity.Should().Be(stockPosition.Quantity);
            stockPositionResponse.AveragePrice.Should().Be(stockPosition.AveragePrice);
            stockPositionResponse.Price.Should().Be(price);
            stockPositionResponse.MarketValue.Should().Be(marketValueExpected);
            stockPositionResponse.PercentualGain.Should().Be(percentualGainExpected);
            stockPositionResponse.MonetaryGain.Should().Be(monetaryGainExpected);
        }

        #endregion
    }
}
