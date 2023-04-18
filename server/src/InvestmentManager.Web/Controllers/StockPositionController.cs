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

        [HttpGet("user-id/{userId}")]
        async public Task<IActionResult> GetAllStockPositionsByUserId(string userId)
        {
            List<StockPositionResponse> stockPositionResponse = await _stockPositionService.GetAllStockPositionsByUserId(userId);

            return Ok(stockPositionResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleStockPosition(Guid id)
        {
            StockPositionResponse? stockPositionResponse = await _stockPositionService.GetSingleStockPosition(id);

            if (stockPositionResponse == null)
            {
                return BadRequest("Invalid position id");
            }

            return Ok(stockPositionResponse);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStockPosition(AddStockPositionRequest addStockPositionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                StockPositionResponse? stockPositionResponse = await _stockPositionService.CreateStockPosition(addStockPositionRequest);

                if (stockPositionResponse == null)
                {
                    return BadRequest("Invalid stock symbol");
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
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                StockPositionResponse? stockPositionResponse = await _stockPositionService.UpdateStockPosition(updateStockPositionRequest);

                if (stockPositionResponse == null)
                {
                    return BadRequest("Invalid position id");
                }

                await _transactionService
                    .CreateTransaction(_mapper.Map<AddTransactionRequest>(updateStockPositionRequest));

                return Ok(stockPositionResponse);
            }
            catch (InvalidStockQuantityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidTransactionTypeException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
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
                    return BadRequest("There was an error while deleting the stock position.");
                }
            }
            catch (InvalidStockQuantityException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
