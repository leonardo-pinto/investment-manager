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
  return (
    await httpClient.get<StockPositions>(
      `${STOCK_POSITION_ROUTE}/user-id/${userId}/trading-country/${tradingCountry}`
    )
  ).data.stockPositions;
};

const getStockPositionById = async (
  positionId: string
): Promise<StockPosition> => {
  return (
    await httpClient.get<StockPosition>(`${STOCK_POSITION_ROUTE}/${positionId}`)
  ).data;
};

const updateStockPosition = async (
  updateStockPosition: UpdateStockPositionRequest
): Promise<StockPosition> => {
  return (
    await httpClient.put<StockPosition>(
      STOCK_POSITION_ROUTE,
      updateStockPosition
    )
  ).data;
};

export {
  createStockPosition,
  getAllStockPositions,
  getStockPositionById,
  updateStockPosition,
};
