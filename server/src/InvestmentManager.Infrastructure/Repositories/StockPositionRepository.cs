using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class StockPositionRepository : IStockPositionRepository
    {

        public async Task CreateStockPosition(StockPosition stockPosition)
        {
            // add
            // save changes
            throw new NotImplementedException();
        }

        public async Task<List<StockPosition>> GetAllStockPositions()
        {
            // get all
            // return
            throw new NotImplementedException();
        }

        public async Task<StockPosition?> GetSingleStockPosition(Guid positionId)
        {
            // get specific by position id
            // return
            throw new NotImplementedException();
        }

        public async Task UpdateStockPosition(StockPosition stockPosition)
        {
            // retrieve first obj from db
            // update values with the input
            // update obj from db
            // save changes
            throw new NotImplementedException();
        }
    }
}
