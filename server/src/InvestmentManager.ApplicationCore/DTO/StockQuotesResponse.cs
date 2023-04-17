namespace InvestmentManager.ApplicationCore.DTO
{
    public class StockQuotesResponse
    {
        public Dictionary<string, double> StockQuotes { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
