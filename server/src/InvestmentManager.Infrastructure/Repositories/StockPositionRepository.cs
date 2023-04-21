using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.Enums;
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

        public async Task<StockPosition?> GetSingleStockPosition(Guid positionId)
        {
            return await _db.StockPositions.FirstOrDefaultAsync(e => e.PositionId == positionId);
        }

        public async Task UpdateStockPosition(StockPosition stockPosition)
        {
            StockPosition? matchingStockPositon =
                await _db.StockPositions.FirstOrDefaultAsync(e => e.PositionId == stockPosition.PositionId);

            if (matchingStockPositon != null)
            {
                matchingStockPositon.Quantity = stockPosition.Quantity;
                matchingStockPositon.AveragePrice = stockPosition.AveragePrice;

                await _db.SaveChangesAsync();
            }
        }
        public async Task<bool> DeleteStockPosition(Guid positionId)
        {
            _db.StockPositions.RemoveRange(_db.StockPositions.Where(e => e.PositionId == positionId));
            int rowsDeleted = await _db.SaveChangesAsync();
            return rowsDeleted > 0;
        }

        public async Task<bool> StockSymbolAlreadyExists(string symbol, string userId)
        {
            StockPosition? stockPosition = await _db.StockPositions
                .FirstOrDefaultAsync(e => e.Symbol == symbol && e.UserId == userId);

            return stockPosition != null;
        }

        public async Task<IEnumerable<StockPosition>> GetAllStockPositionsByUserIdAndTradingCountry(string userId, string tradingCountry)
        {
            return await _db.StockPositions
                .Where(e => e.UserId == userId && e.TradingCountry == tradingCountry).ToListAsync();
        }
    }
}
