import { StockQuotesList } from '../types/stockQuotes';
import httpClient from './httpClient';

const STOCK_QUOTE_ROUTE = '/stock-quote';

const getUpdateStockQuotes = async (
  symbols: string,
  tradingCountry: string
): Promise<StockQuotesList> => {
  const res = await httpClient.get<StockQuotesList>(
    `${STOCK_QUOTE_ROUTE}/${tradingCountry}?symbols=${symbols}`
  );

  return res.data;
};

export { getUpdateStockQuotes };
