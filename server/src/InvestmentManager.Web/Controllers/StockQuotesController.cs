using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InvestmentManager.Web.Controllers
{
    [Route("stock-quote")]
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

        /// <summary>
        /// Get update price quote for brazilian stocks
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("br")]
        public async Task<ActionResult<StockQuotesResponse>> GetBrStockQuotes([FromQuery, BindRequired] string symbols)
        {
            var stockQuotes = await _brApiService.GetStocksPriceQuote(symbols);

            return Ok(new StockQuotesResponse()
            {
                StockQuotes = stockQuotes,
                UpdatedAt = DateTimeOffset.Now
            });
        }

        /// <summary>
        /// Get update price quote for us stocks
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("us")]
        public async Task<ActionResult<StockQuotesResponse>> GetUsStockQuotes([FromQuery, BindRequired] string symbols) 
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
