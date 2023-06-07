export interface Transaction {
  symbol: string;
  quantity: number;
  price: number;
  cost: number;
  dateAndTimeOfTransaction: string;
  transactionType: string;
}
