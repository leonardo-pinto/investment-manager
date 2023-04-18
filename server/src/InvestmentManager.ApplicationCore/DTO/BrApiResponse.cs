using System.Text.Json.Serialization;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for the response of BrApi stock quote endpoint
    /// </summary>
    public class BrApiResponse
    {
        [JsonPropertyName("results")]
        public Result[] Results { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }


    public class Result
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("regularMarketPrice")]
        public double RegularMarketPrice { get; set; }
    }
}
