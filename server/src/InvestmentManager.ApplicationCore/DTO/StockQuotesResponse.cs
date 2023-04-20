namespace InvestmentManager.ApplicationCore.DTO
{
    public class StockQuotesResponse
    {
        public required List<StockQuoteResult> StockQuotes { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }
    }

    public class StockQuoteResult
    {
        public required string Symbol { get; set; }
        public required double Price { get; set; }
    }
}
