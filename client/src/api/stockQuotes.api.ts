import { StockQuotesList } from '../types/stockQuotes';
import httpClient from './httpClient';

const STOCK_QUOTE_ROUTE = '/stock-quote';

// const getUsStockQuotes = async (
//   symbols: string
// ): Promise<StockQuotesListResponse> => {
//   const res = await httpClient.get<StockQuotesListResponse>(
//     `${STOCK_QUOTE_ROUTE}/us/symbols=${symbols}`
//   );

//   return res.data;
// };

const getUpdateStockQuotes = async (
  symbols: string,
  tradingCountry: string
): Promise<StockQuotesList> => {
  const res = await httpClient.get<StockQuotesList>(
    `${STOCK_QUOTE_ROUTE}/${tradingCountry}?symbols=${symbols}`
  );

  return res.data;
};

// const getBrStockQuotes = async (
//   symbols: string
// ): Promise<StockQuotesListResponse> => {
//   const res = await httpClient.get<StockQuotesListResponse>(
//     `${STOCK_QUOTE_ROUTE}/br/`
//   );

//   return res.data;
// };

export { getUpdateStockQuotes };
