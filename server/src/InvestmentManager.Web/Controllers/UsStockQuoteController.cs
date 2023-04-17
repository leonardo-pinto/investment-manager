using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InvestmentManager.Web.Controllers
{
    [Route("api/stock-quote/us")]
    [Authorize]
    [ApiController]
    public class UsStockQuoteController : ControllerBase
    {
        private readonly IFinnhubService _finnhubService;

        public UsStockQuoteController(IFinnhubService finnhubService)
        {
            _finnhubService = finnhubService;
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetSingleUsStockQuote(string symbol)
        {
            double stockQuote = await _finnhubService.GetStockPriceQuote(symbol);
            return Ok(stockQuote);
        }

        [HttpGet("quotes-list")]
        public async Task<IActionResult> GetMultipleUsStockQuotes([FromQuery] string symbols) 
        {
            string[] symbolsArr = symbols.Split(",");
            Dictionary<string, double> stockQuotes = await _finnhubService.GetMultipleStockPriceQuote(symbolsArr);

            return Ok(new StockQuotesResponse()
            {
                StockQuotes = stockQuotes,
                UpdatedAt = DateTimeOffset.Now
            });
        }
    }
}
