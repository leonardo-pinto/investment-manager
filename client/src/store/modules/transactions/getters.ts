import { TransactionState } from '.';
import { Transaction } from '../../../types/transactions';

export default {
  getTransactions(state: TransactionState): Transaction[] {
    return state.transactions;
  },
};
