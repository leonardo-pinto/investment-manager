import { TradingCountry } from '../enums';
import {
  CreateStockPositionRequest,
  StockPositionListResponse,
  StockPositionResponse,
  UpdateStockPositionRequest,
} from '../types/stockPosition';
import httpClient from './httpClient';

const STOCK_POSITION_ROUTE = '/stock-position';

const createStockPosition = async (
  createStockPosition: CreateStockPositionRequest
): Promise<StockPositionResponse> => {
  const res = await httpClient.post<StockPositionResponse>(
    STOCK_POSITION_ROUTE,
    createStockPosition
  );
  return res.data;
};

const getAllStockPositions = async (
  userId: string,
  tradingCountry: TradingCountry
): Promise<StockPositionListResponse> => {
  const res = await httpClient.get<StockPositionListResponse>(
    `${STOCK_POSITION_ROUTE}/user-id/${userId}/trading-country/${tradingCountry}`
  );
  return res.data;
};

const getStockPositionById = async (
  positionId: string
): Promise<StockPositionResponse> => {
  const res = await httpClient.get<StockPositionResponse>(
    `${STOCK_POSITION_ROUTE}/${positionId}`
  );
  return res.data;
};

const deleteStockPosition = async (positionId: string): Promise<void> => {
  await httpClient.delete<StockPositionResponse>(
    `${STOCK_POSITION_ROUTE}/${positionId}`
  );
};

const updateStockPosition = async (
  updateStockPosition: UpdateStockPositionRequest
): Promise<StockPositionResponse> => {
  const res = await httpClient.put<StockPositionResponse>(
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
