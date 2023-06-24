import { ActionContext } from 'vuex';
import { TransactionState } from '.';
import { Transaction } from '../../../types/transactions';
import { getAllTransactions } from '../../../api/transactions.api';

export default {
  async getTransactions({
    commit,
    rootGetters,
  }: ActionContext<TransactionState, unknown>) {
    const userId: string = rootGetters['auth/getUserId'];

    try {
      const transactions: Transaction[] = await getAllTransactions(userId);
      commit('getTransactions', transactions);
    } catch (error) {
      throw error;
    }
  },
};
