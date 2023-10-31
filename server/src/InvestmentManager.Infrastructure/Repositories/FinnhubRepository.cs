using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class FinnhubRepository : IStockQuoteRepository
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
                throw new FinnhubException("No response from Finnhub API");
            }

            if (finnhubResponse.Error != null)
            {
                throw new FinnhubException($"Finnhub API error: {finnhubResponse.Error}");
            }

            // finnhub api returns 0 if the stock symbol is invalid, instead of returning an error
            return finnhubResponse.CurrentPrice;
        }
    }
}
