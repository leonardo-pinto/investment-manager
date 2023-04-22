using AutoMapper;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace InvestmentManager.ApplicationCore.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IMapper mapper,
            IMemoryCache memoryCache
        )
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<TransactionResponse> CreateTransaction(AddTransactionRequest addTransactionRequest)
        {
            Transaction transaction = _mapper.Map<Transaction>(addTransactionRequest);
            await _transactionRepository.CreateTransaction(transaction);

            _memoryCache.Remove($"transactionsUser{transaction.UserId}");

            TransactionResponse transactionResponse = _mapper.Map<TransactionResponse>(transaction);

            return transactionResponse;
        }

        public async Task<IEnumerable<TransactionResponse>> GetAllTransactionsByUserId(string userId)
        {
            string cacheKey = $"transactionsUser{userId}";
            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<Transaction> transactions))
            {
                transactions = await _transactionRepository.GetAllTransactionsByUserId(userId);

                _memoryCache.Set($"transactionsUser{userId}", transactions);
            }
            return transactions.Select(_mapper.Map<TransactionResponse>).ToList();
        }
    }
}
