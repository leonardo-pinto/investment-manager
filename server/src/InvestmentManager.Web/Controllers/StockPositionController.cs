using AutoMapper;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InvestmentManager.Web.Controllers
{
    [Route("api/stock-position")]
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

        [HttpGet]
        [Route("user-id/{userId}/trading-country/{tradingCountry}")]
        async public Task<IActionResult> GetAllStockPositionsByUserIdAndTradingCountry(
            string userId, string tradingCountry)
        {
            List<StockPositionResponse> stockPositionResponse = await _stockPositionService.GetAllStockPositionsByUserIdAndTradingCountry(userId, tradingCountry);

            return Ok(new StockPositionResponseList() { StockPositions = stockPositionResponse });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetSingleStockPosition(Guid id)
        {
            StockPositionResponse? stockPositionResponse = await _stockPositionService.GetSingleStockPosition(id);

            if (stockPositionResponse == null)
            {
                return NotFound(new ErrorResponse() { Error = "Stock position not found" });
            }

            return Ok(stockPositionResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStockPosition(AddStockPositionRequest addStockPositionRequest)
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

        [HttpPut]
        public async Task<IActionResult> UpdateStockPosition(UpdateStockPositionRequest updateStockPositionRequest)
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

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStockPosition(Guid id)
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
        }
    }
}
