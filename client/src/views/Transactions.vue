<template>
  <TransactionsFilter @changeFilters="setFilters" />
  <v-card class="mx-auto w-95 mt-5 mb-5">
    <v-expansion-panels>
      <v-expansion-panel>
        <v-expansion-panel-title>Transactions</v-expansion-panel-title>
        <v-expansion-panel-text>
          <v-card v-if="isLoading"
            ><v-progress-circular indeterminate :size="40"></v-progress-circular
          ></v-card>
          <v-card
            v-else-if="apiResponseError"
            class="error-api-response-message"
          >
            {{ apiResponseError }}
          </v-card>
          <h3 v-else-if="!sortedTransactions.length" class="text-left">
            There are no transactions for the selected filter.
          </h3>
          <TransactionsTable
            v-else
            :transactions="sortedTransactions"
            :currency="currency"
          ></TransactionsTable>
        </v-expansion-panel-text>
      </v-expansion-panel>
    </v-expansion-panels>
  </v-card>
</template>

<script setup lang="ts">
import { computed, ref, reactive } from 'vue';
import TransactionsTable from '../components/transactions/TransactionsTable.vue';
import TransactionsFilter from '../components/transactions/TransactionsFilter.vue';
import { Transaction } from '../types/transactions';
import { useLoading } from 'vue-loading-overlay';
import { TradingCountry, TransactionType } from '../enums';
import { usePositionsStore } from '../stores/positionsStore';
import { useTransactionsStore } from '../stores/transactionsStore';

const positionsStore = usePositionsStore();
const transactionsStore = useTransactionsStore();
const isLoading = ref(false);
const $loading = useLoading({
  color: '#ff6000',
});

const currency = computed<string>(() => {
  return selectedFilters.tradingCountry == TradingCountry.US ? '$' : 'R$';
});

const selectedFilters = reactive({
  tradingCountry: positionsStore.currentCountry,
  transactionType: '',
  symbol: '',
  startDate: '',
  endDate: '',
});

function setFilters(filters: any) {
  selectedFilters.tradingCountry = filters.tradingCountry;
  positionsStore.setTradingCountry(selectedFilters.tradingCountry);
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
  const transactions: Transaction[] = transactionsStore.transactions;
  return transactions.filter(
    ({ tradingCountry, transactionType, symbol, dateAndTimeOfTransaction }) => {
      const formattedDate = dateAndTimeOfTransaction.split('T')[0];
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

async function getTransactions() {
  const loader = $loading.show();
  try {
    isLoading.value = true;
    await transactionsStore.getTransactions();
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
