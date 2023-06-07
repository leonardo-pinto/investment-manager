import { Transaction } from '../types/transactions';
import httpClient from './httpClient';

const TRANSACTIONS_ROUTE = '/transactions';

const getAllTransactions = async (
  userId: string
): Promise<Transaction[]> => {
  const res = await httpClient.get<Transaction[]>(
    `${TRANSACTIONS_ROUTE}/user-id/${userId}`
  );
  return res.data;
};

export { getAllTransactions };
