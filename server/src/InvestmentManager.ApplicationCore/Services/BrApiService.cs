using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.ApplicationCore.Services
{
    public class BrApiService : IBrApiService
    {
        private readonly IBrApiRepository _brApiRepository;

        public BrApiService(IBrApiRepository brApiRepository)
        {
            _brApiRepository = brApiRepository;
        }

        public async Task<Dictionary<string, double>> GetStocksPriceQuote(string concatenatedStockSymbols)
        {
            BrApiResponse response = await _brApiRepository.GetStocksPriceQuote(concatenatedStockSymbols);
            var responseDict = new Dictionary<string, double>();

            if (response.Results != null && response.Results.Length > 0)
            {
                foreach (Result result in response.Results)
                {
                    responseDict.Add(result.Symbol, result.RegularMarketPrice);
                }
            }

            return responseDict;
        }

        public async Task<bool> IsStockSymbolValid(string concatenatedStockSymbols)
        {
            BrApiResponse response = await _brApiRepository.GetStocksPriceQuote(concatenatedStockSymbols);
            return response.Error == null;
        }
    }
}
