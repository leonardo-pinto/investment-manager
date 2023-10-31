import { StockQuotesList } from '../types/stockQuotes';
import httpClient from './httpClient';

const STOCK_QUOTE_ROUTE = '/stock-quote';

const getUpdateStockQuotes = async (
  symbols: string,
  tradingCountry: string
): Promise<StockQuotesList> => {
  return (
    await httpClient.get<StockQuotesList>(
      `${STOCK_QUOTE_ROUTE}/?symbols=${symbols}&tradingCountry=${tradingCountry}`
    )
  ).data;
};

export { getUpdateStockQuotes };
