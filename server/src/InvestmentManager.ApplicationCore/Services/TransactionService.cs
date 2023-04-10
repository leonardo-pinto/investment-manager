using AutoMapper;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.ApplicationCore.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IMapper mapper
        )
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<TransactionResponse> CreateTransaction(AddTransactionRequest addTransactionRequest)
        {
            Transaction transaction = _mapper.Map<Transaction>(addTransactionRequest);
            await _transactionRepository.CreateTransaction(transaction);

            TransactionResponse transactionResponse = _mapper.Map<TransactionResponse>(transaction);

            return transactionResponse;
        }

        public async Task<List<TransactionResponse>> GetAllTransactions()
        {
            List<Transaction> transactions = await _transactionRepository.GetAllTransactions();

            List<TransactionResponse> transactionsResponse = transactions.Select(_mapper.Map<TransactionResponse>).ToList();

            return transactionsResponse;
        }

    }
}
