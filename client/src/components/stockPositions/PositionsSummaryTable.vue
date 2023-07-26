<template>
  <table>
    <thead>
      <tr>
        <th id="table-title" colspan="5">Positions Summary</th>
      </tr>
      <tr>
        <th></th>
        <th v-for="(type, index) in Object.keys(summaryData)" :key="index">
          {{ type }}
        </th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <th class="vertical-header">Cost</th>
        <td v-for="(type, index) in Object.keys(summaryData)" :key="index">
          {{ props.currency }}
          {{ summaryData[type].cost }}
        </td>
      </tr>
      <tr>
        <th class="vertical-header">Market value</th>
        <td v-for="(type, index) in Object.keys(summaryData)" :key="index">
          {{ props.currency }}
          {{ summaryData[type].marketValue }}
        </td>
      </tr>
      <tr>
        <th class="vertical-header">Monetary gain</th>
        <td
          v-for="(type, index) in Object.keys(summaryData)"
          :key="index"
          :class="checkProfit(summaryData[type].monetaryGain)"
        >
          {{ props.currency }}
          {{ summaryData[type].monetaryGain }}
        </td>
      </tr>
      <tr>
        <th class="vertical-header">Percentage gain</th>
        <td
          v-for="(type, index) in Object.keys(summaryData)"
          :key="index"
          :class="checkProfit(summaryData[type].percentageGain)"
        >
          {{ summaryData[type].percentageGain }}%
        </td>
      </tr>
    </tbody>
  </table>
</template>
<script setup lang="ts">
import { PositionType, TradingCountry } from '../../enums';
import { StockPosition } from '../../types/stockPosition';
import {
  calculateMarketValueSum,
  calculateCostSum,
  validPositionTypes,
} from '../../common/helpers';

interface Props {
  positions: StockPosition[];
  currency: string;
  tradingCountry: TradingCountry;
}

type summaryDataProps = {
  cost: string;
  marketValue: string;
  monetaryGain: string;
  percentageGain: string;
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

function calculatePercentageGain(data: summaryDataProps): string {
  return Number(data.cost) != 0
    ? ((Number(data.marketValue) / Number(data.cost) - 1) * 100).toFixed(2)
    : '0.00';
}

function calculateMonetaryGain(data: summaryDataProps): string {
  return (Number(data.marketValue) - Number(data.cost)).toFixed(2);
}

function checkProfit(value: string): string {
  return Number(value) > 0 ? 'positive' : 'negative';
}
</script>

<style scoped>
table {
  margin-bottom: 2rem;
}

#table-title {
  border-bottom: 1px solid #dddddd;
  text-align: center;
}

thead tr,
td {
  text-align: center;
}

table th,
table td {
  padding: 6px 8px;
}

.vertical-header {
  font-size: 0.9rem;
  width: 15%;
}

.positive {
  color: #228b22;
}

.negative {
  color: #ff0000;
}
</style>
