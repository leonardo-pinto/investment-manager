using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Http;

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

            HttpRequestMessage httpRequestMessage = new ()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}")
            };

            httpRequestMessage.Headers.Add(
                "X-Finnhub-Token", _configuration.GetSection("FinnhubAccessToken").Value);

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

            Dictionary<string, double>? responseDict = JsonSerializer.Deserialize<Dictionary<string, double>>(responseBody);

            if (responseDict == null)
            {
                throw new InvalidOperationException("No response from server");
            }

            if (responseDict.ContainsKey("error"))
            {
                throw new InvalidOperationException(Convert.ToString(responseDict["error"]));
            }

            // returns value related to the current price
            return responseDict["c"];
        }
    }
}
