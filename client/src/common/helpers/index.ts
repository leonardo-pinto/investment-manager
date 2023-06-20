import { AuthResponse } from '../../types/auth';
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
  if (!stockPosition.price) {
    return '';
  }

  const positionWeight =
    (100 *
      Number(calculateValue(stockPosition.quantity, stockPosition.price))) /
    calculateMarketValueSum(stockPositions);
  return String(positionWeight.toFixed(2));
}

function calculateAveragePrice(
  firstQuantity: number,
  firstPrice: number,
  secondQuantity: number,
  secondPrice: number
): string {
  // since for this method we wont have null price values
  // it is easier to calculate inside the function than use the
  // calculateValue method
  return (
    (firstQuantity * firstPrice + secondQuantity * secondPrice) /
    (firstQuantity + secondQuantity)
  ).toFixed(2);
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
  const { id, accessToken, username } = authResponse;
  localStorage.setItem('userId', id);
  localStorage.setItem('token', accessToken);
  localStorage.setItem('username', username);
}

function removeAuthFromLocalStorage() {
  localStorage.removeItem('userId');
  localStorage.removeItem('token');
  localStorage.removeItem('username');
}

export {
  calculateValue,
  calculateGainPercentage,
  calculateGainMonetary,
  calculatePositionWeight,
  calculateAveragePrice,
  formatDate,
  setAuthToLocalStorage,
  removeAuthFromLocalStorage,
};
