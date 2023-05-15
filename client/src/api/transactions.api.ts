import { TransactionsResponse } from '@/types/transactions';
import httpClient from './httpClient';

const TRANSACTIONS_ROUTE = '/transactions';

const getAllTransactions = async (
  userId: string
): Promise<TransactionsResponse> => {
  const res = await httpClient.get<TransactionsResponse>(
    `${TRANSACTIONS_ROUTE}/user-id/${userId}`
  );
  return res.data;
};

export { getAllTransactions };
