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
        private readonly IStockQuoteService _stockQuoteService;

        public StockQuotesController(IStockQuoteService stockQuoteService)
        {
            _stockQuoteService = stockQuoteService;
        }

        /// <summary>
        /// Get updated stock quote for a given trading country
        /// </summary>
        [HttpGet]
        async public Task<ActionResult<StockQuotesResponse>> GetStockQuotes([FromQuery, BindRequired] string symbols, [FromQuery, BindRequired] string tradingCountry)
        {
            var stockQuotes = await _stockQuoteService.GetStockPriceQuote(symbols, tradingCountry);

            return Ok(new StockQuotesResponse()
            {
                StockQuotes = stockQuotes,
                UpdatedAt = DateTimeOffset.Now
            });
        }
    }
}
