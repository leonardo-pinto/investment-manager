import { TransactionState } from '.';
import { Transaction } from '../../../types/transactions';

export default {
  async getTransactions(state: TransactionState, payload: Transaction[]) {
    state.transactions = payload;
  },
};
