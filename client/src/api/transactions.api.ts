import { Transaction, Transactions } from '../types/transactions';
import httpClient from './httpClient';

const TRANSACTIONS_ROUTE = '/transactions';

const getAllTransactions = async (userId: string): Promise<Transaction[]> => {
  return (
    await httpClient.get<Transactions>(
      `${TRANSACTIONS_ROUTE}/user-id/${userId}`
    )
  ).data.transactions;
};

export { getAllTransactions };
