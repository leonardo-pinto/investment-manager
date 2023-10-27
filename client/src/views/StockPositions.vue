<template>
  <v-card class="mx-auto w-95 mt-5">
    <v-expansion-panels>
      <v-expansion-panel>
        <v-expansion-panel-title> Filters </v-expansion-panel-title>
        <v-expansion-panel-text>
          <v-select
            label="Select trading country"
            :items="countryOptions"
            item-title="name"
            item-value="value"
            v-model="selectedTradingCountry"
            class="w-20"
          >
            <template #item="{ item, props }">
              <v-list-item v-bind="props">
                <template #title>
                  <span class="d-flex align-center"
                    ><img
                      class="mr-2"
                      style="width: 32px"
                      :src="item.raw.icon"
                    />
                    {{ item.raw.name }}</span
                  >
                </template>
              </v-list-item>
            </template>
          </v-select>
        </v-expansion-panel-text>
      </v-expansion-panel>
    </v-expansion-panels>
  </v-card>
  <v-card
    class="mx-auto w-95 mt-5"
    v-if="filteredStockPositions.stockPositions.length"
  >
    <v-expansion-panels>
      <v-expansion-panel>
        <v-expansion-panel-title> Summary </v-expansion-panel-title>
        <v-expansion-panel-text>
          <PositionsSummaryTable
            :positions="filteredStockPositions.stockPositions"
            :trading-country="selectedTradingCountry"
            :key="filteredStockPositions.updatedAt"
          />
        </v-expansion-panel-text>
      </v-expansion-panel>
    </v-expansion-panels>
  </v-card>
  <v-card class="mx-auto w-95 mt-5 mb-5">
    <v-expansion-panels>
      <v-expansion-panel>
        <v-expansion-panel-title> Positions </v-expansion-panel-title>
        <v-expansion-panel-text>
          <CreateStockPosition></CreateStockPosition>
          <v-card v-if="isLoading">
            <v-progress-circular indeterminate :size="40"></v-progress-circular>
          </v-card>
          <v-card
            v-else-if="apiResponseError"
            class="error-api-response-message"
          >
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
          <v-card v-else variant="text">
            <v-card class="d-flex align-center justify-start mt-5">
              <p>Last updated {{ formatDate(quoteUpdatedAt) }}</p>
              <v-btn variant="text" @click="getStockPositionQuotes">
                <v-icon start icon="mdi-refresh"></v-icon>
                Refresh
              </v-btn>
            </v-card>
            <!-- :key is used here to force StockPositionsTable update -->
            <!-- Since the props change is not tracked -->
            <span
              v-for="({ data, title }, index) in mapPositionType"
              :key="index"
            >
              <PositionsTable
                v-if="data.value.length"
                :filteredPositions="data.value"
                :key="`${data.value[0].positionId}-${filteredStockPositions.updatedAt}`"
                :tradingCountry="positionsStore.currentCountry"
                :title="title"
              >
              </PositionsTable>
            </span>
          </v-card>
        </v-expansion-panel-text>
      </v-expansion-panel>
    </v-expansion-panels>
  </v-card>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import PositionsTable from '../components/stockPositions/PositionsTable.vue';
import PositionsSummaryTable from '../components/stockPositions/PositionsSummaryTable.vue';
import CreateStockPosition from '../components/stockPositions/CreateStockPosition.vue';
import { PositionType, TradingCountry } from '../enums';
import { StockPosition, StockPositionsByCountry } from '../types/stockPosition';
import { formatDate } from '../common/helpers';
import { usePositionsStore } from '../stores/positionsStore';

const positionsStore = usePositionsStore();

const isLoading = ref(false);

const countryOptions = [
  {
    name: 'United States',
    value: TradingCountry.US,
    icon: 'src/assets/us-flag-icon.png',
  },
  {
    name: 'Brazil',
    value: TradingCountry.BR,
    icon: 'src/assets/br-flag-icon.png',
  },
];

const selectedTradingCountry = ref<TradingCountry>(
  positionsStore.currentCountry
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
