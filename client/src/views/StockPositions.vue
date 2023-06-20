<template>
  <BaseCard cardWidth="80%">
    <BaseButton @click="handleCreateStockPosition"
      >REGISTER NEW POSITION</BaseButton
    >
    <label for="tradingCountry">Trading Country: </label>
    <select
      name="tradingCountry"
      id="tradingCountry"
      v-model="selectedTradingCountry"
    >
      <option :value="TradingCountry.US">US</option>
      <option :value="TradingCountry.BR">BR</option>
    </select>
    <BaseButton @click="refreshStockQuotes">Refresh Quotes</BaseButton>

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

    <div></div>
    <div v-if="isLoading"></div>
    <h2 v-if="!filteredStockPositions.stockPositions.length">
      There are no stock positions for {{ selectedTradingCountry }}
    </h2>
    <div v-else>
      <p>Last update: {{ formatDate(filteredStockPositions.updatedAt) }}</p>
      <!-- :key is used here to force StockPositionsTable update -->
      <!-- Since the props change is not tracked -->
      <StockPositionsTable
        :key="filteredStockPositions.updatedAt"
        :filteredStockPositions="filteredStockPositions.stockPositions"
        :currency="currency"
        @openUpdateStock="openUpdateStock"
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
import { TradingCountry, TransactionType } from '../enums';
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

const showCreateStockPosition = ref(false);

const handleCreateStockPosition = () => {
  showCreateStockPosition.value = !showCreateStockPosition.value;
};

const selectedStockPosition: Ref<StockPosition | null> = ref(null);
const selectedTransactionType: Ref<TransactionType> = ref(TransactionType.Buy);
const showUpdateStockPosition = ref(false);

const handleUpdateStockPosition = () => {
  showUpdateStockPosition.value = !showUpdateStockPosition.value;
};

interface openUpdateStockPayload {
  positionId: string;
  transactionType: TransactionType;
}

const openUpdateStock = (payload: openUpdateStockPayload): void => {
  handleUpdateStockPosition();
  selectedStockPosition.value =
    filteredStockPositions.value.stockPositions.find(
      (s) => s.positionId === payload.positionId
    ) ?? null;

  selectedTransactionType.value = payload.transactionType;
};

const getStockPositionsAndStockQuotes = async () => {
  const loader = $loading.show();
  try {
    isLoading.value = true;
    await store.dispatch('stockPositions/getAllStockPositions');
  } catch (error) {
    console.log(`ERROR ::: ${error}`);
  } finally {
    isLoading.value = false;
    loader.hide();
  }
};

getStockPositionsAndStockQuotes();

const refreshStockQuotes = () => {
  const loader = $loading.show();
  try {
    isLoading.value = true;
    store.dispatch('stockPositions/getStockPositionQuotes');
  } catch (error) {
    console.log(`ERROR ::: ${error}`);
  } finally {
    isLoading.value = false;
    loader.hide();
  }
};
</script>
