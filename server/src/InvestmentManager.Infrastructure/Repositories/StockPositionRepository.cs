using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class StockPositionRepository : IStockPositionRepository
    {
        private readonly ApplicationDbContext _db;

        public StockPositionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateStockPosition(StockPosition stockPosition)
        {
            _db.StockPositions.Add(stockPosition);
            await _db.SaveChangesAsync();
        }

        public async Task<List<StockPosition>> GetAllStockPositions()
        {
            return await _db.StockPositions.ToListAsync();
        }

        public async Task<StockPosition?> GetSingleStockPosition(Guid positionId)
        {
            return await _db.StockPositions.FirstOrDefaultAsync(e => e.PositionId == positionId);
        }

        public async Task<bool> StockSymbolAlreadyExists(string symbol)
        {
            StockPosition? stockPosition = await _db.StockPositions.FirstOrDefaultAsync(e => e.Symbol == symbol);

            return stockPosition == null ? false : true;
        }

        public async Task UpdateStockPosition(StockPosition stockPosition)
        {
            StockPosition? matchingStockPositon =
                await _db.StockPositions.FirstOrDefaultAsync(e => e.PositionId == stockPosition.PositionId);

            if (matchingStockPositon != null)
            {
                matchingStockPositon.Quantity = stockPosition.Quantity;
                matchingStockPositon.AveragePrice = stockPosition.AveragePrice;
                matchingStockPositon.Cost = stockPosition.Cost;
                matchingStockPositon.CurrentPrice = stockPosition.CurrentPrice;

                await _db.SaveChangesAsync();
            }
        }
    }
}
