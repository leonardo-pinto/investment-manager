using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Domain.Entities;

namespace InvestmentManager.UnitTests.Entities
{
    public class StockPositionTest
    {
        private readonly IFixture _fixture = new Fixture();

        #region UpdateAveragePrice

        [Fact]
        public void UpdateAveragePrice_ToBeSuccessful()
        {
            StockPosition stockPosition = _fixture
                .Build<StockPosition>()
                .With(e => e.Quantity, 10)
                .With(e => e.AveragePrice, 10)
                .Create();

            double result = stockPosition.UpdateAveragePrice(10, 30);

            result.Should().Be(20);
        }

        #endregion
    }
}
