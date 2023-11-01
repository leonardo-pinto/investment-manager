<template>
  <v-table>
    <thead>
      <tr>
        <th></th>
        <th
          v-for="(type, index) in Object.keys(summaryData)"
          :key="index"
          class="text-left"
        >
          {{ type }}
        </th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <th class="vertical-header">Cost</th>
        <td
          v-for="(type, index) in Object.keys(summaryData)"
          :key="index"
          class="text-left"
        >
          {{ formatCurrency.format(summaryData[type].cost) }}
        </td>
      </tr>
      <tr>
        <th class="vertical-header">Market value</th>
        <td
          v-for="(type, index) in Object.keys(summaryData)"
          :key="index"
          class="text-left"
        >
          {{ formatCurrency.format(summaryData[type].marketValue) }}
        </td>
      </tr>
      <tr>
        <th class="vertical-header">Monetary gain</th>
        <td
          v-for="(type, index) in Object.keys(summaryData)"
          :key="index"
          :style="{
            color: getProfitColor(summaryData[type].monetaryGain.toString()),
          }"
          class="text-left"
        >
          {{ formatCurrency.format(summaryData[type].monetaryGain) }}
        </td>
      </tr>
      <tr>
        <th class="vertical-header">Percentage gain</th>
        <td
          v-for="(type, index) in Object.keys(summaryData)"
          :key="index"
          :style="{
            color: getProfitColor(summaryData[type].percentageGain.toString()),
          }"
          class="text-left"
        >
          {{ formatCurrency.format(summaryData[type].percentageGain) }}%
        </td>
      </tr>
    </tbody>
  </v-table>
</template>
<script setup lang="ts">
import { PositionType, TradingCountry } from '../../enums';
import { StockPosition } from '../../types/stockPosition';
import {
  calculateMarketValueSum,
  calculateCostSum,
  validPositionTypes,
  currencyFormatter,
  getProfitColor,
} from '../../common/helpers';

interface Props {
  positions: StockPosition[];
  tradingCountry: TradingCountry;
}

type summaryDataProps = {
  cost: number;
  marketValue: number;
  monetaryGain: number;
  percentageGain: number;
};

const props = defineProps<Props>();

let summaryData: { [key: string]: summaryDataProps } = {};

validPositionTypes[props.tradingCountry].forEach((type) => {
  summaryData[type] = {} as summaryDataProps;
});
summaryData['Total'] = {} as summaryDataProps;

Object.keys(summaryData).forEach((key) => {
  // If the key is Stocks, REITs or Bonds, values are calculated
  // filtering the positions by type first
  // if not (case of 'Total'), value is calculated without filtering by type
  if (Object.values(PositionType).includes(key as PositionType)) {
    summaryData[key].cost = calculateCostSum(
      filterPositionByType(key as PositionType)
    );
    summaryData[key].marketValue = calculateMarketValueSum(
      filterPositionByType(key as PositionType)
    );
  } else {
    summaryData[key].cost = calculateCostSum(props.positions);
    summaryData[key].marketValue = calculateMarketValueSum(props.positions);
  }
  summaryData[key].monetaryGain = calculateMonetaryGain(summaryData[key]);
  summaryData[key].percentageGain = calculatePercentageGain(summaryData[key]);
});

function filterPositionByType(type: PositionType): StockPosition[] {
  return props.positions.filter((position) => position.type == type);
}

function calculatePercentageGain(data: summaryDataProps) {
  return Number(data.cost) != 0
    ? (Number(data.marketValue) / Number(data.cost) - 1) * 100
    : 0;
}

function calculateMonetaryGain(data: summaryDataProps) {
  return Number(data.marketValue) - Number(data.cost);
}

const formatCurrency = currencyFormatter(props.tradingCountry);
</script>

<style scoped></style>
