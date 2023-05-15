import { StockQuotesListResponse } from '@/types/stockQuotes';
import httpClient from './httpClient';

const STOCK_QUOTE_ROUTE = '/stock-quote';

const getUsStockQuotes = async (): Promise<StockQuotesListResponse> => {
  const res = await httpClient.get<StockQuotesListResponse>(
    `${STOCK_QUOTE_ROUTE}/us`
  );

  return res.data;
};

const getBrStockQuotes = async (): Promise<StockQuotesListResponse> => {
  const res = await httpClient.get<StockQuotesListResponse>(
    `${STOCK_QUOTE_ROUTE}/br`
  );

  return res.data;
};

export { getUsStockQuotes, getBrStockQuotes };
