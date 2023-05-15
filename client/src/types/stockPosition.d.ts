export enum TradingCountry {
  US = 'US',
  BR = 'BR',
}

export enum TransactionType {
  Buy = 'Buy',
  Sell = 'Sell',
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
  userId: string;
  symbol: string;
  quantity: number;
  price: number;
  transactionType: TransactionType;
  dateAndTimeOfStockPosition: string;
}

export interface StockPositionResponse {
  positionId: string;
  symbol: string;
  quantity: number;
  averagePrice: number;
}

export interface StockPositionListResponse {
  stockPositions: StockPositionResponse[];
}

export interface StockPositionData extends StockPositionResponse {
  price: number;
  cost: number;
  marketValue: number;
  dayGainPercentage: number;
  dayGainMonetary: number;
  gainPercentage: number;
  gainMonetary: number;
}
