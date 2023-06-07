export interface StockQuotesList {
  stockQuotes: StockQuote[];
  updatedAt: string;
}

export interface StockQuote {
  symbol: string;
  price: number;
}
