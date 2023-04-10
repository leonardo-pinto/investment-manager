using FluentAssertions;
using InvestmentManager.ApplicationCore.Helpers;

namespace InvestmentManager.UnitTests.Helpers
{
    public class FinancialMetricsTest
    {
        #region CalculateMonetaryGain
        [Fact]
        public void CalculateMonetaryGain_ToBeSuccessful()
        {
            double result = FinancialMetrics.CalculateMonetaryGain(10, 100, 50);
            result.Should().Be(500);
        }

        #endregion

        #region CalculatePercentualGain
        [Theory]
        [InlineData(100, 50, 100)]
        [InlineData(10, 20, -50)]
        [InlineData(16.93, 73.99, -77.12)]
        public void CalculatePercentualGain_ToBeSuccessful(
            double currPrice, double avgPrice, double percentualGain
        )
        {
            double result = FinancialMetrics.CalculatePercentualGain(currPrice, avgPrice);

            result.Should().Be(percentualGain);
        }
        #endregion

        #region CalculateMarketValue
        [Theory]
        [InlineData(100, 50, 5000)]
        [InlineData(10, 20, 200)]
        [InlineData(178, 982.29, 174847.62)]
        public void CalculateMarketValue_ToBeSuccessful(
            int quantity, double currPrice, double mktValue
        )
        {
            double result = FinancialMetrics.CalculateMarketValue(quantity, currPrice);

            result.Should().Be(mktValue);
        }
        #endregion

        #region
        [Theory]
        [InlineData(100, 50, 5000)]
        [InlineData(10, 20, 200)]
        [InlineData(87, 73.99, 6437.13)]
        public void CalculateCost_ToBeSuccessful(
           int quantity, double currPrice, double cost
        )
        {
            double result = FinancialMetrics.CalculateCost(quantity, currPrice);

            result.Should().Be(cost);
        }
        #endregion
    }
}
