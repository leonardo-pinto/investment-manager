<template>
  <v-row class="mt-3">
    <SelectTradingCountry></SelectTradingCountry>
    <v-select
      label="Select transaction type"
      :items="transactionTypeOptions"
      item-title="name"
      v-model="filters.transactionType"
      class="mx-2"
    >
    </v-select>
    <v-text-field v-model="filters.symbol" label="Symbol" density="comfortable">
    </v-text-field>
  </v-row>
  <v-row class="w-30">
    <v-text-field
      class="mr-2"
      label="Start Date"
      type="date"
      v-model="filters.startDate"
    >
    </v-text-field>
    <v-text-field label="End Date" type="date" v-model="filters.endDate">
    </v-text-field>
  </v-row>
  <v-row class="mb-1">
    <VBtnSecondary size="large" @click="resetFilters" class="mr-2">
      Reset Filter
    </VBtnSecondary>
    <VBtnPrimary @click="setFilters" size="large"> Apply Filter </VBtnPrimary>
  </v-row>
</template>
<script setup lang="ts">
import { reactive } from 'vue';
import { usePositionsStore } from '../../stores/positionsStore';
import { TransactionType } from '../../enums';
import SelectTradingCountry from '../../common/components/SelectTradingCountry.vue';

const positionsStore = usePositionsStore();
const filters = reactive({
  tradingCountry: positionsStore.currentCountry,
  transactionType: '',
  symbol: '',
  startDate: '',
  endDate: '',
});

const emit = defineEmits(['changeFilters']);

function resetFilters() {
  filters.transactionType = '';
  filters.symbol = '';
  filters.startDate = '';
  filters.endDate = '';
  emit('changeFilters', filters);
}

function setFilters() {
  emit('changeFilters', filters);
}

const transactionTypeOptions = [
  { name: 'All', value: '' },
  { name: 'Buys', value: TransactionType.Buy },
  { name: 'Sells', value: TransactionType.Sell },
];
</script>
