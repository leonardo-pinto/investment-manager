import { Module } from 'vuex';
import { Transaction } from '../../../types/transactions';
import mutations from './mutations.ts';
import actions from './actions.ts';
import getters from './getters.ts';

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
  mutations,
  actions,
  getters,
};

export default store;
