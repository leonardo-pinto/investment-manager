using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class BrApiRepository : IBrApiRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BrApiRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BrApiResponse> GetStocksPriceQuote(string concatenatedStockSymbols)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"https://brapi.dev/api/quote/{concatenatedStockSymbols}");
            var responseData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            BrApiResponse? brApiResponse = JsonSerializer.Deserialize<BrApiResponse>(responseData, options);

            if (brApiResponse == null)
            {
                throw new InvalidOperationException("No response from server");
            }

            return brApiResponse;
        }
    }
}
