using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentManager.Web.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{positionId}")]
        async public Task<IActionResult> GetAllTransactionsByPositionId(Guid positionId)
        {
            List<TransactionResponse> transactionHistory = await _transactionService.GetTransactionHistory(positionId);

            return Ok(transactionHistory);
        }
    }
}
