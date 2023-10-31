using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class BrApiRepository : IStockQuoteRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public BrApiRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<double> GetStockPriceQuote(string stockSymbol)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            var token = _configuration["BrApiAccessToken"];
            var response = await httpClient.GetAsync($"https://brapi.dev/api/quote/{stockSymbol}?token={token}");
            var responseData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            BrApiResponse? brApiResponse = JsonSerializer.Deserialize<BrApiResponse>(responseData, options);

            if (brApiResponse is null)
            {
                throw new BrApiException("No response from BrApi");
            }

            if (brApiResponse.Error == true)
            {
                // when the stock symbol is invalid, the BrApi returns an error
                // in this case, we return a value of 0
                return 0;
            }

            // if there is no error, the response is successfull
            // since the free plan of the BrApi enables only one stock quote per request
            // the result will always be on the index 0 the results array
            return brApiResponse.Results[0].RegularMarketPrice;
        }
    }
}
