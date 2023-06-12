import { Module } from 'vuex';
import { Transaction } from '../../../types/transactions';
import { getAllTransactions } from '../../../api/transactions.api';

export interface TransactionState {
  transactions: Transaction[];
}

const store: Module<TransactionState, unknown> = {
  namespaced: true,
  state() {
    return {
      transactions: [],
    };
  },
  mutations: {
    async getTransactions(state: TransactionState, payload: Transaction[]) {
      state.transactions = payload;
    },
  },
  actions: {
    async getTransactions({ commit, rootGetters }) {
      const userId: string = rootGetters['auth/getUserId'];

      try {
        const transactions: Transaction[] = await getAllTransactions(userId);
        commit('getTransactions', transactions);
      } catch (error) {
        throw error;
      }
    },
  },
  getters: {
    getTransactions(state: TransactionState): Transaction[] {
      return state.transactions;
    },
  },
};

export default store;
