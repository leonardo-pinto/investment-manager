using AutoMapper;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Helpers;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentManager.Web.Controllers
{
    [Route("api/stock-position")]
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
        async public Task<IActionResult> GetSingleStockPosition(string id)
        {
            Guid positionId = Guid.Parse(id);

            StockPositionResponse? stockPositionResponse = await _stockPositionService.GetSingleStockPosition(positionId);

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
                new { positionId = stockPositionResponse?.PositionId },
                stockPositionResponse
           );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStockPosition(UpdateStockPositionRequest updateStockPositionRequest, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid positionId = Guid.Parse(id);
            StockPositionResponse? stockPositionResponse = await _stockPositionService.UpdateStockPosition(updateStockPositionRequest, positionId);

            if (stockPositionResponse == null)
            {
                return BadRequest("Invalid position id");
            }

            AddTransactionRequest addTransactionRequest = _mapper.Map<AddTransactionRequest>(updateStockPositionRequest);

            await _transactionService.CreateTransaction(addTransactionRequest);

            return Ok(stockPositionResponse);
        }
    }
}
