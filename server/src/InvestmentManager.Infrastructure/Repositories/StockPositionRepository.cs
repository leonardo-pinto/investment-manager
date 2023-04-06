using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class StockPositionRepository : IStockPositionRepository
    {

        public async Task CreateStockPosition(StockPosition stockPosition)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StockPosition>> GetAllStockPositions()
        {
            throw new NotImplementedException();
        }

        public async Task<StockPosition?> GetSingleStockPosition(Guid positionId)
        {
            throw new NotImplementedException();
        }

        public async Task<StockPosition> UpdateStockPosition(StockPosition stockPosition)
        {
            throw new NotImplementedException();
        }
    }
}
