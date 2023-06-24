<template>
  <div class="flex filter-row">
    <label for="tradingCountry">Trading Country: </label>
    <select
      name="tradingCountry"
      id="tradingCountry"
      v-model="filters.tradingCountry"
    >
      <option :value="TradingCountry.US">US</option>
      <option :value="TradingCountry.BR">BR</option>
    </select>
  </div>
  <div class="flex filter-row">
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
  </div>
  <div class="flex filter-row">
    <label for="symbol">Symbol: </label>
    <input name="symbol" id="symbol" v-model="filters.symbol" />
  </div>

  <div class="flex filter-row">
    <label for="startDate">Date Range: </label>
    <input
      type="date"
      name="startDate"
      id="startDate"
      v-model="filters.startDate"
    />
    <label id="end-date-label" for="endDate">to:</label>
    <input type="date" name="endDate" id="endDate" v-model="filters.endDate" />
  </div>

  <BaseButton @click="setFilters">Filter</BaseButton>
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

<style scoped>
.filter-row {
  margin: 1rem 0;
  justify-content: left;
  align-items: center;
}

label {
  margin: 0 0.5rem;
  width: 15%;
}

input {
  border-radius: 5%;
  font-size: 0.8rem;
  margin-bottom: 0%;
  width: 20%;
}

select,
input {
  padding: 0.3rem;
}

select:focus {
  border-color: #ff6000;
  outline: none;
}

#end-date-label {
  width: 1rem;
}
</style>
