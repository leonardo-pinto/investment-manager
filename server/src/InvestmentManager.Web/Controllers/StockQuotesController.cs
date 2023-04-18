using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InvestmentManager.Web.Controllers
{
    [Route("api/stock-quote")]
    [Authorize]
    [ApiController]
    public class StockQuotesController : ControllerBase
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IBrApiService _brApiService;

        public StockQuotesController(IFinnhubService finnhubService, IBrApiService brApiService)
        {
            _finnhubService = finnhubService;
            _brApiService = brApiService;
        }

        [HttpGet("br/quotes")]
        public async Task<IActionResult> GetMultipleBrStockQuotes([FromQuery] string symbols)
        {
            var stockQuotes = await _brApiService.GetStocksPriceQuote(symbols);

            return Ok(new StockQuotesResponse()
            {
                StockQuotes = stockQuotes,
                UpdatedAt = DateTimeOffset.Now
            });
        }

        [HttpGet("us/{symbol}")]
        public async Task<IActionResult> GetSingleUsStockQuote(string symbol)
        {
            double stockQuote = await _finnhubService.GetStockPriceQuote(symbol);
            return Ok(stockQuote);
        }

        [HttpGet("us/quotes-list")]
        public async Task<IActionResult> GetMultipleUsStockQuotes([FromQuery] string symbols) 
        {
            string[] symbolsArr = symbols.Split(",");
            var stockQuotes = await _finnhubService.GetMultipleStockPriceQuote(symbolsArr);

            return Ok(new StockQuotesResponse()
            {
                StockQuotes = stockQuotes,
                UpdatedAt = DateTimeOffset.Now
            });
        }
    }
}
