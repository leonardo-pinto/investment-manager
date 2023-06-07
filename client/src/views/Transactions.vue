<template>
  Transactions view
  <select
    name="tradingCountry"
    id="tradingCountry"
    v-model="selectedTradingCountry"
  >
    <option :value="TradingCountry.US">US</option>
    <option :value="TradingCountry.BR">BR</option>
  </select>

  <select>
    Type:
  </select>

  <select>
    Symbols
  </select>

  <p>View Range:</p>
  <p>View dates:</p>

  <h1 v-if="isLoading">LOADING...</h1>
  <h2 v-else-if="!filteredTransactions.length">
    There are no transactions for {{ selectedTradingCountry }}
  </h2>
  <TransactionsTable
    :filteredTransactions="filteredTransactions"
  ></TransactionsTable>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import { TradingCountry } from '../enums';
import { useStore } from '../store';
import TransactionsTable from '../components/transactions/TransactionsTable.vue';

const store = useStore();
const isLoading = ref(false);

const selectedTradingCountry = ref<TradingCountry>(
  store.getters['stockPositions/getSelectedCountry']
);

watch(selectedTradingCountry, (value) => {
  store.dispatch('stockPositions/setSelectedTradingCountry', value);
  getTransactions();
});

const filteredTransactions = computed(() => {
  return store.getters['transactions/getTransactions'];
});

const getTransactions = () => {
  try {
    isLoading.value = true;
    store.dispatch('transactions/getTransactions');
  } catch (error) {
    console.log(`ERROR ::: ${error}`);
  } finally {
    isLoading.value = false;
  }
};

getTransactions();
</script>
