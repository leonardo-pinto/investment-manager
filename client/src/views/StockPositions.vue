<template>
  <BaseCard cardWidth="80%">
    <BaseButton @click="handleCreateStockPositionDialog"
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
      :show="isCreateStockPositionDialogOpen"
      @close="handleCreateStockPositionDialog"
      :tradingCountry="selectedTradingCountry"
    />
    <UpdateStockPosition
      :show="isUpdateStockPositionDialogOpen"
      :stockPosition="selectedStockPosition"
      :transactionType="selectedTransactionType"
      @close="handleUpdateStockPositionDialog"
    />
    <h1 v-if="isLoading">LOADING . . .</h1>
    <h2 v-else-if="!filteredStockPositions.stockPositions.length">
      There are no stock positions for {{ selectedTradingCountry }}
    </h2>
    <div v-else>
      <p>Last update: {{ filteredStockPositions.updatedAt }}</p>
      <!-- :key is used here to force StockPositionsTable update -->
      <!-- Since the props change is not tracked -->
      <StockPositionsTable
        :key="filteredStockPositions.updatedAt"
        :filteredStockPositions="filteredStockPositions.stockPositions"
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

const store = useStore();
const isLoading = ref(false);

const selectedTradingCountry = ref<TradingCountry>(
  store.getters['stockPositions/getSelectedCountry']
);

watch(selectedTradingCountry, (value) => {
  store.dispatch('stockPositions/setSelectedTradingCountry', value);
  getStockPositionsAndStockQuotes();
});

const filteredStockPositions = computed<StockPositionsByCountry>(() => {
  return store.getters['stockPositions/getStockPositions'][
    selectedTradingCountry.value
  ];
});

const isCreateStockPositionDialogOpen = ref(false);

const handleCreateStockPositionDialog = () => {
  isCreateStockPositionDialogOpen.value =
    !isCreateStockPositionDialogOpen.value;
};

const selectedStockPosition: Ref<StockPosition | null> = ref(null);
const selectedTransactionType: Ref<TransactionType> = ref(TransactionType.Buy);
const isUpdateStockPositionDialogOpen = ref(false);

const handleUpdateStockPositionDialog = () => {
  isUpdateStockPositionDialogOpen.value =
    !isUpdateStockPositionDialogOpen.value;
};

const openUpdateStock = (payload: any): void => {
  // add type
  isUpdateStockPositionDialogOpen.value = true;

  selectedStockPosition.value =
    filteredStockPositions.value.stockPositions.find(
      (s) => s.positionId === payload.positionId
    ) ?? null;

  selectedTransactionType.value = payload.transactionType;
};

const getStockPositionsAndStockQuotes = async () => {
  try {
    isLoading.value = true;
    await store.dispatch('stockPositions/getAllStockPositions');
    if (filteredStockPositions.value.stockPositions.length) {
      await store.dispatch('stockPositions/updatedStockPositionsQuote');
    }
  } catch (error) {
    console.log(`ERROR ::: ${error}`);
  } finally {
    isLoading.value = false;
  }
};

getStockPositionsAndStockQuotes();

const refreshStockQuotes = () => {
  try {
    isLoading.value = true;
    store.dispatch('stockPositions/updatedStockPositionsQuote');
  } catch (error) {
    console.log(`ERROR ::: ${error}`);
  } finally {
    isLoading.value = false;
  }
};
</script>
