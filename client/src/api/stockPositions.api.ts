import { TradingCountry } from '../enums';
import {
  CreateStockPositionRequest,
  StockPosition,
  StockPositions,
  UpdateStockPositionRequest,
} from '../types/stockPosition';
import httpClient from './httpClient';

const STOCK_POSITION_ROUTE = '/stock-position';

const createStockPosition = async (
  createStockPosition: CreateStockPositionRequest
): Promise<StockPosition> => {
  const res = await httpClient.post<StockPosition>(
    STOCK_POSITION_ROUTE,
    createStockPosition
  );

  return res.data;
};

const getAllStockPositions = async (
  userId: string,
  tradingCountry: TradingCountry
): Promise<StockPosition[]> => {
  const res = await httpClient.get<StockPositions>(
    `${STOCK_POSITION_ROUTE}/user-id/${userId}/trading-country/${tradingCountry}`
  );
  return res.data.stockPositions;
};

const getStockPositionById = async (
  positionId: string
): Promise<StockPosition> => {
  const res = await httpClient.get<StockPosition>(
    `${STOCK_POSITION_ROUTE}/${positionId}`
  );
  return res.data;
};

const deleteStockPosition = async (positionId: string): Promise<void> => {
  await httpClient.delete<StockPosition>(
    `${STOCK_POSITION_ROUTE}/${positionId}`
  );
};

const updateStockPosition = async (
  updateStockPosition: UpdateStockPositionRequest
): Promise<StockPosition> => {
  const res = await httpClient.put<StockPosition>(
    STOCK_POSITION_ROUTE,
    updateStockPosition
  );
  return res.data;
};

export {
  createStockPosition,
  getAllStockPositions,
  getStockPositionById,
  deleteStockPosition,
  updateStockPosition,
};
