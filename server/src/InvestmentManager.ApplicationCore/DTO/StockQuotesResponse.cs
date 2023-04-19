namespace InvestmentManager.ApplicationCore.DTO
{
    public class StockQuotesResponse
    {
        public List<StockQuoteResult> StockQuotes { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public class StockQuoteResult
    {
        public string Symbol { get; set; }
        public double Price { get; set; }
    }
}
