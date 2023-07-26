<template>
  <BaseCard width="90%">
    <div id="top-actions-wrapper">
      <div id="trading-country-wrapper" class="flex">
        <label for="tradingCountry">Trading Country: </label>
        <select
          name="tradingCountry"
          id="tradingCountry"
          v-model="selectedTradingCountry"
        >
          <option :value="TradingCountry.US">US</option>
          <option :value="TradingCountry.BR">BR</option>
        </select>
      </div>
      <BaseButton @click="handleCreateStockPosition"
        >New Stock Position</BaseButton
      >
    </div>
    <CreateStockPosition
      :show="showCreateStockPosition"
      @close="handleCreateStockPosition"
      :tradingCountry="selectedTradingCountry"
    />
    <UpdateStockPosition
      :show="showUpdateStockPosition"
      :stockPosition="selectedStockPosition"
      :transactionType="selectedTransactionType"
      @close="handleUpdateStockPosition"
    />
    <div v-if="isLoading"></div>
    <div v-else-if="apiResponseError" class="error-api-response-message">
      {{ apiResponseError }}
    </div>
    <div v-else-if="!filteredStockPositions.stockPositions.length">
      <h3>There are no stock positions for the selected trading country</h3>
      <p>Create a new stock position to get started!</p>
    </div>
    <div v-else>
      <div class="update-container">
        <p>Last update: {{ formatDate(quoteUpdatedAt) }}</p>
        <a href="#" @click.prevent="getStockPositionQuotes">Update Quotes</a>
      </div>
      <PositionsSummaryTable
        :positions="filteredStockPositions.stockPositions"
        :currency="currency"
        :trading-country="selectedTradingCountry"
        :key="filteredStockPositions.updatedAt"
      />
      <!-- :key is used here to force StockPositionsTable update -->
      <!-- Since the props change is not tracked -->
      <StockPositionsTable
        v-if="stocks.length"
        :title="PositionType.Stocks"
        :key="filteredStockPositions.updatedAt"
        :filteredStockPositions="stocks"
        :currency="currency"
        @openUpdateStock="openUpdateStock"
        class="stock-position-table"
      />
      <StockPositionsTable
        v-if="reits.length"
        :title="PositionType.REITs"
        :key="filteredStockPositions.updatedAt"
        :filteredStockPositions="reits"
        :currency="currency"
        @openUpdateStock="openUpdateStock"
        class="stock-position-table"
      />
      <StockPositionsTable
        v-if="bonds.length"
        :title="PositionType.Bonds"
        :key="filteredStockPositions.updatedAt"
        :filteredStockPositions="bonds"
        :currency="currency"
        @openUpdateStock="openUpdateStock"
        class="stock-position-table"
      />
    </div>
  </BaseCard>
</template>

<script setup lang="ts">
import { Ref, computed, ref, watch } from 'vue';
import { useStore } from '../store';
import StockPositionsTable from '../components/stockPositions/StockPositionsTable.vue';
import CreateStockPosition from '../components/stockPositions/CreateStockPosition.vue';
import UpdateStockPosition from '../components/stockPositions/UpdateStockPosition.vue';
import PositionsSummaryTable from '../components/stockPositions/PositionsSummaryTable.vue';
import { PositionType, TradingCountry, TransactionType } from '../enums';
import { StockPosition, StockPositionsByCountry } from '../types/stockPosition';
import { useLoading } from 'vue-loading-overlay';
import { formatDate } from '../common/helpers';

const store = useStore();

const isLoading = ref(false);
const $loading = useLoading({
  color: '#ff6000',
});

const selectedTradingCountry = ref<TradingCountry>(
  store.getters['stockPositions/getSelectedCountry']
);

watch(selectedTradingCountry, (value) => {
  store.dispatch('stockPositions/setSelectedTradingCountry', value);
  getStockPositionsAndStockQuotes();
});

const currency = computed<string>(() => {
  return selectedTradingCountry.value == TradingCountry.US ? '$' : 'R$';
});

const filteredStockPositions = computed<StockPositionsByCountry>(() => {
  return store.getters['stockPositions/getStockPositions'][
    selectedTradingCountry.value
  ];
});

const stocks = computed<StockPosition[]>(() => {
  return filteredStockPositions.value.stockPositions.filter(
    (e) => e.type == PositionType.Stocks
  );
});

const reits = computed<StockPosition[]>(() => {
  return filteredStockPositions.value.stockPositions.filter(
    (e) => e.type == PositionType.REITs
  );
});

const bonds = computed<StockPosition[]>(() => {
  return filteredStockPositions.value.stockPositions.filter(
    (e) => e.type == PositionType.Bonds
  );
});

const quoteUpdatedAt = computed<string>(() => {
  return store.getters['stockPositions/getUpdateAtByCountry'];
});

const showCreateStockPosition = ref(false);
const handleCreateStockPosition = () => {
  showCreateStockPosition.value = !showCreateStockPosition.value;
};

const selectedStockPosition: Ref<StockPosition | null> = ref(null);
const selectedTransactionType: Ref<TransactionType> = ref(TransactionType.Buy);
const showUpdateStockPosition = ref(false);

function handleUpdateStockPosition() {
  showUpdateStockPosition.value = !showUpdateStockPosition.value;
}

interface openUpdateStockPayload {
  positionId: string;
  transactionType: TransactionType;
}

function openUpdateStock(payload: openUpdateStockPayload) {
  handleUpdateStockPosition();
  selectedStockPosition.value =
    filteredStockPositions.value.stockPositions.find(
      (s) => s.positionId === payload.positionId
    ) ?? null;

  selectedTransactionType.value = payload.transactionType;
}

const apiResponseError = ref('');

async function getStockPositionsAndStockQuotes() {
  const loader = $loading.show();
  try {
    isLoading.value = true;
    await store.dispatch('stockPositions/getAllStockPositions');
  } catch (_error) {
    apiResponseError.value =
      'An error occurred while consulting stock positions. Please try again.';
  } finally {
    isLoading.value = false;
    loader.hide();
  }
}

getStockPositionsAndStockQuotes();

function getStockPositionQuotes() {
  const loader = $loading.show();
  try {
    isLoading.value = true;
    store.dispatch('stockPositions/getStockPositionQuotes');
  } catch (_error) {
    apiResponseError.value =
      'An error occurred while consulting stock quotes. Please try again.';
  } finally {
    isLoading.value = false;
    loader.hide();
  }
}
</script>

<style scoped>
#top-actions-wrapper {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

#trading-country-wrapper {
  justify-content: center;
  align-items: center;
}

#trading-country-wrapper label {
  margin: 0;
}

#trading-country-wrapper select {
  margin-left: 0.5rem;
}

.update-container {
  display: flex;
  justify-content: left;
  align-items: center;
  font-size: 0.9rem;
}

.update-container a {
  color: #ff6000;
  margin-left: 0.8rem;
}

.stock-position-table {
  margin-bottom: 2rem;
}
</style>
