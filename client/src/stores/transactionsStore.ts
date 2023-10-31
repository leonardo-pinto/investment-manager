import { defineStore } from 'pinia';
import { Transaction } from '../types/transactions';
import { getAllTransactions } from '../api/transactions.api';

interface State {
  transactions: Transaction[];
}

export const useTransactionsStore = defineStore('transactions', {
  state: (): State => ({
    transactions: [],
  }),
  actions: {
    async getTransactions() {
      try {
        const transactions: Transaction[] = await getAllTransactions();
        this.transactions = transactions;
      } catch (error) {
        throw error;
      }
    },
  },
});
