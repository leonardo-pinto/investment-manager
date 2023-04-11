using AutoFixture;
using InvestmentManager.ApplicationCore.Domain.Entities;

namespace InvestmentManager.UnitTests.TestHelpers
{
    static class MockHelper
    {
        private static readonly IFixture _fixture = new Fixture();

        static public Dictionary<string, double> GenerateStockPriceDict(List<StockPosition> input)
        {
            List<string> stockSymbols = input.Select(e => e.Symbol).ToList();
            Dictionary<string, double> dict = new();
            foreach (string symbol in stockSymbols)
            {
                dict.Add(symbol, _fixture.Create<double>());
            }
            return dict;
        }
    }
}
