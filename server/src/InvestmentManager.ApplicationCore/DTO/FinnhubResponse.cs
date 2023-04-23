using System.Text.Json.Serialization;

namespace InvestmentManager.ApplicationCore.DTO
{
    /// <summary>
    /// DTO for the response of Finnhub stock quote endpoint
    /// US stocks
    /// </summary>
    public class FinnhubResponse
    {
        [JsonPropertyName("c")]
        public double CurrentPrice { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
