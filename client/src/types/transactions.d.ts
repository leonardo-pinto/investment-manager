import { TradingCountry, TransactionType } from '../enums';

export interface Transaction {
  symbol: string;
  quantity: number;
  price: number;
  dateAndTimeOfTransaction: string;
  transactionType: TransactionType;
  tradingCountry: TradingCountry;
}

export interface Transactions {
  transactions: Transaction[];
}
