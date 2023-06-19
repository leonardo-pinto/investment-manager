<template>
  Transactions view
  <h1 v-if="isLoading">LOADING...</h1>
  <h2 v-else-if="!filteredTransactions.length">There are no transactions</h2>
  <TransactionsFilter @changeFilters="setFilters" />
  <TransactionsTable
    :filteredTransactions="filteredTransactions"
  ></TransactionsTable>
</template>

<script setup lang="ts">
import { computed, ref, reactive } from 'vue';
import { useStore } from '../store';
import TransactionsTable from '../components/transactions/TransactionsTable.vue';
import TransactionsFilter from '../components/transactions/TransactionsFilter.vue';
import { Transaction } from '../types/transactions';
import { useLoading } from 'vue-loading-overlay';

const store = useStore();
const isLoading = ref(false);
const $loading = useLoading({
  color: '#ff6000',
});

const selectedFilters = reactive({
  tradingCountry: store.getters['stockPositions/getSelectedCountry'],
  transactionType: '',
  symbol: '',
  startDate: '',
  endDate: '',
});

function setFilters(filters: any) {
  selectedFilters.tradingCountry = filters.tradingCountry;
  selectedFilters.transactionType = filters.transactionType;
  selectedFilters.symbol = filters.symbol;
  selectedFilters.startDate = filters.startDate;
  selectedFilters.endDate = filters.endDate;
}

const filteredTransactions = computed(() => {
  const transactions: Transaction[] =
    store.getters['transactions/getTransactions'];
  return transactions.filter(
    ({ tradingCountry, transactionType, symbol, dateAndTimeOfTransaction }) => {
      const formattedDate = dateAndTimeOfTransaction.split('T')[0];

      if (
        selectedFilters.tradingCountry &&
        selectedFilters.tradingCountry !== tradingCountry
      ) {
        return false;
      }

      if (
        selectedFilters.transactionType &&
        selectedFilters.transactionType !== transactionType
      ) {
        return false;
      }

      if (
        selectedFilters.symbol &&
        selectedFilters.symbol.toUpperCase() !== symbol
      ) {
        return false;
      }

      if (
        selectedFilters.startDate &&
        selectedFilters.endDate &&
        (selectedFilters.startDate > formattedDate ||
          selectedFilters.endDate < formattedDate)
      ) {
        return false;
      }

      return true;
    }
  );
});

const getTransactions = () => {
  const loader = $loading.show();
  try {
    isLoading.value = true;
    store.dispatch('transactions/getTransactions');
  } catch (error) {
    console.log(`ERROR ::: ${error}`);
  } finally {
    isLoading.value = false;
    loader.hide();
  }
};

getTransactions();
</script>
