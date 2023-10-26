import { Transaction, Transactions } from '../types/transactions';
import httpClient from './httpClient';

const TRANSACTIONS_ROUTE = '/transactions';

const getAllTransactions = async (): Promise<Transaction[]> => {
  return (await httpClient.get<Transactions>(`${TRANSACTIONS_ROUTE}`)).data
    .transactions;
};

export { getAllTransactions };
