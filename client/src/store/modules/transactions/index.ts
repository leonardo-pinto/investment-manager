import { Module } from 'vuex';
import { Transaction } from '../../../types/transactions';
import { TradingCountry } from '../../../enums';
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
    async getTransactions() {},
  },
  actions: {
    async getTransactions({ commit, rootGetters }) {
      const userId: string = rootGetters['auth/getUserId'];

      try {
        const transactions: Transaction[] = await getAllTransactions(userId);

        commit('getTransactions', transactions);

        console.log(`transactions response:: ${transactions}`);
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
