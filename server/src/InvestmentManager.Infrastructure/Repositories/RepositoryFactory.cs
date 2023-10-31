using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryFactory(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public IStockQuoteRepository CreateRepository(string tradingCountry)
        {
            if (tradingCountry.ToUpper().Equals("US"))
            {
                return ActivatorUtilities.CreateInstance<FinnhubRepository>(_serviceProvider);

            }
            else if (tradingCountry.ToUpper().Equals("BR"))
            {
                return ActivatorUtilities.CreateInstance<BrApiRepository>(_serviceProvider);
            } 
            else
            {
                throw new ArgumentException("Invalid trading country");
            }
        }
    }
}
