import { TransactionType } from '../enums';

export interface Transaction {
  symbol: string;
  quantity: number;
  price: number;
  dateAndTimeOfTransaction: string;
  transactionType: string;
  tradingCountry: TransactionType;
}

export interface Transactions {
  transactions: Transaction[];
}
