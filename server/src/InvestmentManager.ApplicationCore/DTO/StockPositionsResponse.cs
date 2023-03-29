namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO class that represents the return of multiple stock positions
    /// </summary>
    public class StockPositionsResponse
    {
       /// <summary>
       /// List of stock position response
       /// </summary>
       public List<StockPositionResponse>? StockPositions { get; set; }
    }
}
