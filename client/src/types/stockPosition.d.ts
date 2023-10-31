import { PositionType, TradingCountry, TransactionType } from '../enums';

export interface StockPosition {
  positionId: string;
  symbol: string;
  quantity: number;
  averagePrice: number;
  price: number;
  type: PositionType;
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
  quantity: number;
  averagePrice: number;
  dateAndTimeOfStockPosition: string;
  tradingCountry: TradingCountry;
  type: PositionType;
}

export interface UpdateStockPositionRequest {
  positionId: string;
  symbol: string;
  quantity: number;
  price: number;
  transactionType: TransactionType;
  tradingCountry: TradingCountry;
}

export interface PositionTableData {
  positionId: string;
  symbol: string;
  quantity: number;
  price: string;
  averagePrice: string;
  cost: string;
  marketValue: string;
  percentualGain: string;
  monetaryGain: string;
  positionWeight: string;
}
