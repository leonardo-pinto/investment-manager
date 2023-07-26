<template>
  <BaseCard width="80%">
    <TransactionsFilter @changeFilters="setFilters" />
    <div v-if="isLoading"></div>
    <div v-else-if="apiResponseError" class="error-api-response-message">
      {{ apiResponseError }}
    </div>
    <h2 v-else-if="!sortedTransactions.length">
      There are no transactions for the selected filter.
    </h2>
    <TransactionsTable
      v-else
      :transactions="sortedTransactions"
      :currency="currency"
    ></TransactionsTable>
  </BaseCard>
</template>

<script setup lang="ts">
import { computed, ref, reactive } from 'vue';
import { useStore } from '../store';
import TransactionsTable from '../components/transactions/TransactionsTable.vue';
import TransactionsFilter from '../components/transactions/TransactionsFilter.vue';
import { Transaction } from '../types/transactions';
import { useLoading } from 'vue-loading-overlay';
import { TradingCountry, TransactionType } from '../enums';

const store = useStore();
const isLoading = ref(false);
const $loading = useLoading({
  color: '#ff6000',
});

const currency = computed<string>(() => {
  return selectedFilters.tradingCountry == TradingCountry.US ? '$' : 'R$';
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
  store.dispatch(
    'stockPositions/setSelectedTradingCountry',
    selectedFilters.tradingCountry
  );
  selectedFilters.transactionType = filters.transactionType;
  selectedFilters.symbol = filters.symbol;
  selectedFilters.startDate = filters.startDate;
  selectedFilters.endDate = filters.endDate;
}

function filterByTradingCountry(value: TradingCountry): boolean {
  if (
    selectedFilters.tradingCountry &&
    selectedFilters.tradingCountry !== value
  ) {
    return false;
  }
  return true;
}

function filterByTransactionType(value: TransactionType): boolean {
  if (
    selectedFilters.transactionType &&
    selectedFilters.transactionType !== value
  ) {
    return false;
  }
  return true;
}

function filterBySymbol(value: string): boolean {
  if (
    selectedFilters.symbol &&
    selectedFilters.symbol.toUpperCase() !== value
  ) {
    return false;
  }
  return true;
}

function filterByDate(value: string): boolean {
  if (
    selectedFilters.startDate &&
    selectedFilters.endDate &&
    (selectedFilters.startDate > value || selectedFilters.endDate < value)
  ) {
    return false;
  }
  return true;
}

const filteredTransactions = computed(() => {
  const transactions: Transaction[] =
    store.getters['transactions/getTransactions'];
  return transactions.filter(
    ({ tradingCountry, transactionType, symbol, dateAndTimeOfTransaction }) => {
      const formattedDate = dateAndTimeOfTransaction.split('T')[0];
      console.log(transactions);
      if (!filterByTradingCountry(tradingCountry)) {
        return false;
      }
      if (!filterByTransactionType(transactionType)) {
        return false;
      }
      if (!filterBySymbol(symbol)) {
        return false;
      }

      if (!filterByDate(formattedDate)) {
        return false;
      }

      return true;
    }
  );
});

const sortedTransactions = computed(() => {
  return filteredTransactions.value.sort((a, b) =>
    a.dateAndTimeOfTransaction > b.dateAndTimeOfTransaction
      ? -1
      : b.dateAndTimeOfTransaction > a.dateAndTimeOfTransaction
      ? 1
      : 0
  );
});

const apiResponseError = ref('');

function getTransactions() {
  const loader = $loading.show();
  try {
    isLoading.value = true;
    store.dispatch('transactions/getTransactions');
  } catch (_error) {
    apiResponseError.value =
      'An error occurred while consulting stock positions. Please try again.';
  } finally {
    isLoading.value = false;
    loader.hide();
  }
}

getTransactions();
</script>
