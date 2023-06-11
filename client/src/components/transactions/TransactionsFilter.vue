<template>
  <label for="tradingCountry">Trading Country: </label>
  <select
    name="tradingCountry"
    id="tradingCountry"
    v-model="filters.tradingCountry"
  >
    <option :value="TradingCountry.US">US</option>
    <option :value="TradingCountry.BR">BR</option>
  </select>

  <label for="transactionType">Transaction Type: </label>
  <select
    name="transactionType"
    id="transactionType"
    v-model="filters.transactionType"
  >
    <option value="">All transaction types</option>
    <option :value="TransactionType.Buy">Buys</option>
    <option :value="TransactionType.Sell">Sells</option>
  </select>

  <label for="symbol">Symbol</label>
  <input name="symbol" id="symbol" v-model="filters.symbol" />

  <label>Date range</label>
  <input type="date" name="startDate" v-model="filters.startDate" />to
  <input type="date" name="endDate" v-model="filters.endDate" />

  <BaseButton @click="setFilters">View</BaseButton>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useStore } from '../../store';
import { TransactionType, TradingCountry } from '../../enums';

const store = useStore();

const filters = reactive({
  tradingCountry: store.getters['stockPositions/getSelectedCountry'],
  transactionType: '',
  symbol: '',
  startDate: '',
  endDate: '',
});

const emit = defineEmits(['changeFilters']);

function setFilters() {
  emit('changeFilters', filters);
}
</script>
