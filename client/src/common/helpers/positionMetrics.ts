import { PositionType, TradingCountry } from '../../enums';
import { PositionTableData, StockPosition } from '../../types/stockPosition';
import { currencyFormatter } from './';

function calculateValue(quantity: number, price: number) {
  return quantity * price;
}

function calculateGainPercentage(price: number, avgPrice: number) {
  return (price / avgPrice - 1) * 100;
}

function calculateGainMonetary(qty: number, price: number, avgPrice: number) {
  return qty * price - qty * avgPrice;
}

function calculateMarketValueSum(stockPositions: StockPosition[]) {
  return stockPositions.reduce(
    (acc, curr) => acc + Number(calculateValue(curr.quantity, curr.price)),
    0
  );
}

function calculateCostSum(stockPositions: StockPosition[]): number {
  return stockPositions.reduce(
    (acc, curr) =>
      acc + Number(calculateValue(curr.quantity, curr.averagePrice)),
    0
  );
}

function calculatePositionWeight(
  stockPosition: StockPosition,
  stockPositions: StockPosition[]
) {
  const positionWeight =
    (100 *
      Number(calculateValue(stockPosition.quantity, stockPosition.price))) /
    Number(calculateMarketValueSum(stockPositions));
  return positionWeight;
}

function calculateAveragePrice(
  firstQuantity: number,
  firstPrice: number,
  secondQuantity: number,
  secondPrice: number
) {
  // since for this method we wont have null price values
  // it is easier to calculate inside the function than use the
  // calculateValue method
  return (
    (firstQuantity * firstPrice + secondQuantity * secondPrice) /
    (firstQuantity + secondQuantity)
  );
}

const validPositionTypes: { [key: string]: string[] } = {
  [TradingCountry.BR]: [PositionType.Stocks, PositionType.REITs],
  [TradingCountry.US]: [
    PositionType.Stocks,
    PositionType.REITs,
    PositionType.Bonds,
  ],
};

function getResultColor(value: string | number) {
  return String(value).indexOf('-') ? 'green' : 'red';
}

function mapPositionToPositionTableData(
  p: StockPosition,
  allPositions: StockPosition[],
  tradingCountry: TradingCountry
) {
  const formatter = currencyFormatter(tradingCountry);

  return {
    positionId: p.positionId,
    symbol: p.symbol,
    quantity: p.quantity,
    price: formatter.format(p.price),
    averagePrice: formatter.format(p.averagePrice),
    cost: formatter.format(calculateValue(p.quantity, p.averagePrice)),
    marketValue: formatter.format(calculateValue(p.quantity, p.price)),
    percentualGain: formatter.format(
      calculateGainPercentage(p.price, p.averagePrice)
    ),
    monetaryGain: formatter.format(
      calculateGainMonetary(p.quantity, p.price, p.averagePrice)
    ),
    positionWeight: `${calculatePositionWeight(p, allPositions).toFixed(2)}%`,
  } as PositionTableData;
}

export {
  calculateValue,
  calculateGainPercentage,
  calculateGainMonetary,
  calculatePositionWeight,
  calculateAveragePrice,
  calculateMarketValueSum,
  calculateCostSum,
  getResultColor,
  validPositionTypes,
  mapPositionToPositionTableData,
};
