using AutoMapper;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        async public Task<IActionResult> GetAllStockPositions()
        {
            List<StockPositionResponse> stockPositionResponse = await _stockPositionService.GetAllStockPositions();

            return Ok(stockPositionResponse);
        }

        [HttpGet("{id}")]
        async public Task<IActionResult> GetSingleStockPosition(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

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
            if (!ModelState.IsValid) return BadRequest(ModelState);
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
    }
}
