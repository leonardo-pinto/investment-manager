import { StockPosition } from '../../types/stockPosition';

function calculateValue(quantity: number, price: number): string {
  return price ? (quantity * price).toFixed(2) : '';
}

function calculateGainPercentage(price: number, avgPrice: number): string {
  return price ? ((price / avgPrice - 1) * 100).toFixed(2) : '';
}

function calculateGainMonetary(
  qty: number,
  price: number,
  avgPrice: number
): string {
  return price ? (qty * price - qty * avgPrice).toFixed(2) : '';
}

function calculateMarketValueSum(stockPositions: StockPosition[]): number {
  return stockPositions.reduce(
    (acc, curr) => acc + Number(calculateValue(curr.quantity, curr.price)),
    0
  );
}

function calculatePositionWeight(
  stockPosition: StockPosition,
  stockPositions: StockPosition[]
): string {
  const positionWeight =
    (100 *
      Number(calculateValue(stockPosition.quantity, stockPosition.price))) /
    calculateMarketValueSum(stockPositions);
  return String(positionWeight.toFixed(2));
}

export {
  calculateValue,
  calculateGainPercentage,
  calculateGainMonetary,
  calculatePositionWeight,
};
