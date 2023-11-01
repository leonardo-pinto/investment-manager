<template>
  <v-card class="mx-auto w-95 mt-5">
    <ExpansionPanelsWrapper title="Filters">
      <v-row class="mt-3 w-20">
        <SelectTradingCountry></SelectTradingCountry>
      </v-row>
    </ExpansionPanelsWrapper>
  </v-card>
  <v-card
    class="mx-auto w-95 mt-5"
    v-if="filteredStockPositions.stockPositions.length"
  >
    <ExpansionPanelsWrapper title="Summary">
      <PositionsSummaryTable
        :positions="filteredStockPositions.stockPositions"
        :trading-country="selectedTradingCountry"
        :key="filteredStockPositions.updatedAt"
      />
    </ExpansionPanelsWrapper>
  </v-card>
  <v-card class="mx-auto w-95 mt-5 mb-5">
    <ExpansionPanelsWrapper title="Positions">
      <CreateStockPosition></CreateStockPosition>
      <v-card v-if="isLoading" variant="flat">
        <v-progress-circular indeterminate :size="40"></v-progress-circular>
      </v-card>
      <v-card v-else-if="apiResponseError" class="error-api-response-message">
        {{ apiResponseError }}
      </v-card>
      <v-card
        class="mt-5"
        variant="flat"
        v-else-if="!filteredStockPositions.stockPositions.length"
      >
        <h3>There are no positions for the selected trading country</h3>
        <p>Create a new position to get started!</p>
      </v-card>
      <v-card v-else variant="flat">
        <v-card variant="flat" class="d-flex align-center justify-start mb-4">
          <p>Last updated {{ formatDate(quoteUpdatedAt) }}</p>
          <v-btn class="ml-2" variant="text" @click="getStockPositionQuotes">
            <v-icon start icon="mdi-refresh"></v-icon>
            Refresh
          </v-btn>
        </v-card>
        <!-- :key is used here to force StockPositionsTable update -->
        <!-- Since the props change is not tracked -->
        <span v-for="({ data, title }, index) in mapPositionType" :key="index">
          <ExpansionPanelsWrapper
            v-if="data.value.length"
            :title="title"
            :key="`expansion-panel-${index}`"
          >
            <PositionsTable
              :filteredPositions="data.value"
              :key="`${data.value[0].positionId}-${filteredStockPositions.updatedAt}`"
              :tradingCountry="positionsStore.currentCountry"
            >
            </PositionsTable>
          </ExpansionPanelsWrapper>
        </span>
      </v-card>
    </ExpansionPanelsWrapper>
  </v-card>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import PositionsTable from '../components/stockPositions/PositionsTable.vue';
import PositionsSummaryTable from '../components/stockPositions/PositionsSummaryTable.vue';
import CreateStockPosition from '../components/stockPositions/CreateStockPosition.vue';
import ExpansionPanelsWrapper from '../common/components/ExpansionPanelsWrapper.vue';
import SelectTradingCountry from '../common/components/SelectTradingCountry.vue';
import { PositionType, TradingCountry } from '../enums';
import { StockPosition, StockPositionsByCountry } from '../types/stockPosition';
import { formatDate } from '../common/helpers';
import { usePositionsStore } from '../stores/positionsStore';

const positionsStore = usePositionsStore();
const isLoading = ref(false);

const selectedTradingCountry = computed<TradingCountry>(
  () => positionsStore.currentCountry
);

watch(selectedTradingCountry, (value) => {
  positionsStore.setTradingCountry(value);
  getStockPositionsAndStockQuotes();
});

const filteredStockPositions = computed<StockPositionsByCountry>(() => {
  return positionsStore[positionsStore.currentCountry];
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
  return positionsStore[positionsStore.currentCountry].updatedAt;
});

const mapPositionType = [
  {
    title: PositionType.Stocks,
    data: stocks,
  },
  {
    title: PositionType.REITs,
    data: reits,
  },
  {
    title: PositionType.Bonds,
    data: bonds,
  },
];

const apiResponseError = ref('');

async function getStockPositionsAndStockQuotes() {
  try {
    isLoading.value = true;
    await positionsStore.getAllStockPositionsByCountry();
  } catch (_error) {
    apiResponseError.value =
      'An error occurred while consulting stock positions. Please try again.';
  } finally {
    isLoading.value = false;
  }
}

getStockPositionsAndStockQuotes();

async function getStockPositionQuotes() {
  try {
    isLoading.value = true;
    await positionsStore.getStockPositionQuotes();
  } catch (_error) {
    apiResponseError.value =
      'An error occurred while consulting stock quotes. Please try again.';
  } finally {
    isLoading.value = false;
  }
}
</script>
