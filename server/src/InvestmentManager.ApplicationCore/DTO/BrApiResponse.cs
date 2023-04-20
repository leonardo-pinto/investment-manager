using System.Text.Json.Serialization;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for the response of BrApi stock quote endpoint
    /// BR stocks
    /// </summary>
    public class BrApiResponse
    {
        [JsonPropertyName("results")]
        public Result[]? Results { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }


    public class Result
    {
        [JsonPropertyName("symbol")]
        public required string Symbol { get; set; }

        [JsonPropertyName("regularMarketPrice")]
        public required double RegularMarketPrice { get; set; }
    }
}
