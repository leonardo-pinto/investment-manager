using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Enums;

namespace InvestmentManager.UnitTests.Helpers
{
    static class MockHelper
    {
        static public double ExpectedCost(int quantity, double price)
        {
            return quantity * price;
        }
    }
}
