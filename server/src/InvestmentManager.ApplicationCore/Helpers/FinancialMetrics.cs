namespace InvestmentManager.ApplicationCore.Helpers
{
    static public class FinancialMetrics
    {
        static public double CalculateMonetaryGain(int quantity, double currentPrice, double averagePrice)
        {
            double cost = quantity * averagePrice;
            double result = ((quantity * currentPrice) - cost);
            return Math.Round(result, 2);
        }

        static public double CalculatePercentualGain(double currentPrice, double averagePrice)
        {
            double result = ((currentPrice / averagePrice) - 1) * 100; ;
            return Math.Round(result, 2);
        }
        static public double CalculateMarketValue(int quantity, double currentPrice)
        {
            double result = quantity * currentPrice;
            return Math.Round(result, 2);
        }
        static public double CalculateCost(int quantity, double averagePrice)
        {
            double result = quantity * averagePrice;
            return Math.Round(result, 2);
        }
    }
}
