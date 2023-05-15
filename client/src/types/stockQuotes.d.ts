export interface StockQuotesListResponse {
  stockQuotes: StockQuote[];
  updatedAt: string;
}

export interface StockQuote {
  symbol: string;
  price: number;
}
