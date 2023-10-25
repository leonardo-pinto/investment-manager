import { PositionType, TradingCountry } from '../../enums';
import { AuthResponse } from '../../types/auth';
import { StockPosition } from '../../types/stockPosition';

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

function formatDate(date: string | null): string {
  if (!date) {
    return '';
  } else {
    const formattedTime = new Date(date).toLocaleTimeString([], {
      hour12: false,
    });
    const formattedDate = new Date(date)
      .toLocaleDateString('en-US', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
      })
      .replace(/\//g, '/');

    return `${formattedDate} ${formattedTime}`;
  }
}

function setAuthToLocalStorage(authResponse: AuthResponse): void {
  const { accessToken, username } = authResponse;
  localStorage.setItem('token', accessToken);
  localStorage.setItem('username', username);
}

function removeAuthFromLocalStorage() {
  localStorage.removeItem('token');
  localStorage.removeItem('username');
}

const validPositionTypes: { [key: string]: string[] } = {
  [TradingCountry.BR]: [PositionType.Stocks, PositionType.REITs],
  [TradingCountry.US]: [
    PositionType.Stocks,
    PositionType.REITs,
    PositionType.Bonds,
  ],
};

export {
  calculateValue,
  calculateGainPercentage,
  calculateGainMonetary,
  calculatePositionWeight,
  calculateAveragePrice,
  calculateMarketValueSum,
  calculateCostSum,
  formatDate,
  setAuthToLocalStorage,
  removeAuthFromLocalStorage,
  validPositionTypes,
};
