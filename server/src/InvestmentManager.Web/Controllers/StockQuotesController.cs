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

        [HttpGet("br")]
        public async Task<IActionResult> GetBrStockQuotes([FromQuery] string symbols)
        {
            var stockQuotes = await _brApiService.GetStocksPriceQuote(symbols);

            return Ok(new StockQuotesResponse()
            {
                StockQuotes = stockQuotes,
                UpdatedAt = DateTimeOffset.Now
            });
        }

        [HttpGet("us")]
        public async Task<IActionResult> GetUsStockQuotes([FromQuery] string symbols) 
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
