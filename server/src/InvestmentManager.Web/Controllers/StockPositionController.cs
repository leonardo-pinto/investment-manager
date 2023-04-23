using AutoMapper;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InvestmentManager.Web.Controllers
{
    [Route("stock-position")]
    [Authorize]
    [ApiController]
    public class StockPositionController : ControllerBase
    {
        private readonly IStockPositionService _stockPositionService;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public StockPositionController(IStockPositionService stockPositionService, ITransactionService transactionService, IMapper mapper)
        {
            _stockPositionService = stockPositionService;
            _transactionService = transactionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all stock positions traded in a country for a given user id
        /// </summary>
        [HttpGet]
        [Route("user-id/{userId}/trading-country/{tradingCountry}")]
        async public Task<ActionResult<StockPositionsResponse>> GetAllStockPositionsByUserIdAndTradingCountry(
            string userId, string tradingCountry)
        {
            IEnumerable<StockPositionResponse> stockPositionResponse = await _stockPositionService.GetAllStockPositionsByUserIdAndTradingCountry(userId, tradingCountry);

            return Ok(new StockPositionsResponse() { StockPositions = stockPositionResponse });
        }

        /// <summary>
        /// Get a stock position for a given stock position id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<StockPositionResponse>> GetSingleStockPosition(Guid id)
        {
            StockPositionResponse? stockPositionResponse = await _stockPositionService.GetSingleStockPosition(id);

            if (stockPositionResponse == null)
            {
                return NotFound(new ErrorResponse() { Error = "Stock position not found" });
            }

            return Ok(stockPositionResponse);
        }

        /// <summary>
        /// Creates a new stock position
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<StockPositionResponse>> CreateStockPosition(AddStockPositionRequest addStockPositionRequest)
        {
            try
            {
                StockPositionResponse? stockPositionResponse = await _stockPositionService.CreateStockPosition(addStockPositionRequest);

                if (stockPositionResponse == null)
                {
                    return BadRequest(new ErrorResponse() { Error = "Invalid stock symbol" });
                }

                var addTransactionRequest = _mapper.Map<AddTransactionRequest>(addStockPositionRequest);
                addTransactionRequest.PositionId = stockPositionResponse.PositionId;

                await _transactionService.CreateTransaction(addTransactionRequest);

                return CreatedAtAction(
                    nameof(GetSingleStockPosition),
                    new { id = stockPositionResponse?.PositionId },
                    stockPositionResponse
               );
            }
            catch (RepeatedStockSymbolException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updated a stock position
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<StockPositionResponse>> UpdateStockPosition(UpdateStockPositionRequest updateStockPositionRequest)
        {
            try
            {
                StockPositionResponse? stockPositionResponse = await _stockPositionService.UpdateStockPosition(updateStockPositionRequest);

                if (stockPositionResponse == null)
                {
                    return NotFound(new ErrorResponse() { Error = "Stock position not found" });
                }

                await _transactionService
                    .CreateTransaction(_mapper.Map<AddTransactionRequest>(updateStockPositionRequest));

                return Ok(stockPositionResponse);
            }
            catch (InvalidStockQuantityException ex)
            {
                return BadRequest(new ErrorResponse() { Error = ex.Message });
            }
            catch (InvalidTransactionTypeException ex)
            {
                return BadRequest(new ErrorResponse() { Error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a stock position
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteStockPosition(Guid id)
        {
            try
            {
                bool deletionSucceeded = await _stockPositionService.DeleteStockPosition(id);

                if (deletionSucceeded)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound(new ErrorResponse() { Error = "Stock position to be deleted was not found" });
                }
            }
            catch (InvalidStockQuantityException ex)
            {
                return BadRequest(new ErrorResponse() { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ErrorResponse() { Error = ex.Message });
            }
        }
    }
}
