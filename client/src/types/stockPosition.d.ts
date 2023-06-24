import { TradingCountry, TransactionType } from '../enums';

export interface StockPosition {
  positionId: string;
  symbol: string;
  quantity: number;
  averagePrice: number;
  price: number;
}

export interface StockPositions {
  stockPositions: StockPosition[];
}

export interface StockPositionsByCountry {
  stockPositions: StockPosition[];
  updatedAt: string;
}

export interface CreateStockPositionRequest {
  symbol: string;
  userId: string;
  quantity: number;
  averagePrice: number;
  dateAndTimeOfStockPosition: string;
  tradingCountry: TradingCountry;
}

export interface UpdateStockPositionRequest {
  positionId: string;
  userId?: string;
  symbol: string;
  quantity: number;
  price: number;
  transactionType: TransactionType;
}
