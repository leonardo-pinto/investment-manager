using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using InvestmentManager.ApplicationCore.DTO;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<double> GetStockPriceQuote(string stockSymbol)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-Finnhub-Token", _configuration["FinnhubAccessToken"]);

            var response = await httpClient.GetAsync($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}");
            var responseData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            FinnhubResponse? finnhubResponse = JsonSerializer.Deserialize<FinnhubResponse>(responseData, options);

            if (finnhubResponse == null)
            {
                throw new InvalidOperationException("No response from server");
            }

            if (finnhubResponse.Error != null)
            {
                throw new InvalidOperationException(finnhubResponse.Error);
            }

            return finnhubResponse.CurrentPrice;
        }
    }
}
