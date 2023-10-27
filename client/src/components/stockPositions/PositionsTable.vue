<template>
  <v-expansion-panels>
    <v-expansion-panel>
      <v-expansion-panel-title> {{ props.title }} </v-expansion-panel-title>
      <v-expansion-panel-text>
        <v-data-table
          v-model:items-per-page="itemsPerPage"
          :headers="headers"
          :items="processedPositions"
          class="mb-3"
        >
          <template v-slot:item.percentualGain="{ value }">
            <td :style="{ color: getColor(value) }">{{ value }}</td>
          </template>
          <template v-slot:item.monetaryGain="{ value }">
            <td :style="{ color: getColor(value) }">{{ value }}</td>
          </template>
          <template v-slot:item.actions="{ item }">
            <UpdatePosition
              v-for="(value, index) in Object.keys(TransactionType)"
              :key="`${index}-${value}`"
              :position="item"
              :transaction-type="value"
            ></UpdatePosition>
          </template>
        </v-data-table>
      </v-expansion-panel-text>
    </v-expansion-panel>
  </v-expansion-panels>
</template>
<script setup lang="ts">
import type { VDataTable } from 'vuetify/lib/labs/components.mjs';
import { StockPosition } from '../../types/stockPosition';
import {
  calculateGainMonetary,
  calculateGainPercentage,
  calculatePositionWeight,
  calculateValue,
} from '../../common/helpers';
import UpdatePosition from './UpdatePosition.vue';
import { TradingCountry, TransactionType } from '../../enums';
import { ref } from 'vue';
// https://stackoverflow.com/questions/75991355/import-datatableheader-typescript-type-of-vuetify3-v-data-table
// followed this solution to fix the headers type
type UnwrapReadonlyArrayType<A> = A extends Readonly<Array<infer I>>
  ? UnwrapReadonlyArrayType<I>
  : A;
type DT = InstanceType<typeof VDataTable>;
type ReadonlyDataTableHeader = UnwrapReadonlyArrayType<DT['headers']>;

interface Props {
  filteredPositions: StockPosition[];
  title: string;
  tradingCountry: TradingCountry;
}
const props = defineProps<Props>();

const itemsPerPage = ref(50);
const headers: ReadonlyDataTableHeader[] = [
  {
    title: 'Symbol',
    align: 'start',
    key: 'symbol',
  },
  { title: 'Quantity', align: 'start', key: 'quantity', width: '8%' },
  { title: 'Price', align: 'start', key: 'price', sortable: false },
  {
    title: 'Average Price',
    align: 'start',
    key: 'averagePrice',
    sortable: false,
  },
  { title: 'Cost', align: 'start', key: 'cost', sortable: false },
  {
    title: 'Market Value',
    align: 'start',
    key: 'marketValue',
    sortable: false,
  },
  { title: 'Gain (%)', align: 'start', key: 'percentualGain' },
  { title: 'Gain ($)', align: 'start', key: 'monetaryGain' },
  { title: 'Position Weight', align: 'start', key: 'positionWeight' },
  { title: 'Actions', key: 'actions', sortable: false, width: '15%' },
];

type ProcessedPosition = {
  positionId: string;
  symbol: string;
  quantity: number;
  price: string;
  averagePrice: string;
  cost: string;
  marketValue: string;
  percentualGain: string;
  monetaryGain: string;
  positionWeight: string;
};

const currencyFormatter = new Intl.NumberFormat('en-US', {
  style: 'currency',
  currency: props.tradingCountry === TradingCountry.US ? 'USD' : 'BRL',
  minimumFractionDigits: 2,
});

const processedPositions = props.filteredPositions.map((p) => {
  const position: ProcessedPosition = {
    positionId: p.positionId,
    symbol: p.symbol,
    quantity: p.quantity,
    price: currencyFormatter.format(p.price),
    averagePrice: currencyFormatter.format(p.averagePrice),
    cost: currencyFormatter.format(calculateValue(p.quantity, p.averagePrice)),
    marketValue: currencyFormatter.format(calculateValue(p.quantity, p.price)),
    percentualGain: currencyFormatter.format(
      calculateGainPercentage(p.price, p.averagePrice)
    ),
    monetaryGain: currencyFormatter.format(
      calculateGainMonetary(p.quantity, p.price, p.averagePrice)
    ),
    positionWeight: `${calculatePositionWeight(
      p,
      props.filteredPositions
    ).toFixed(2)}%`,
  };
  return position;
});

function getColor(value: string) {
  return Number(value.substring(1)) > 0 ? 'green' : 'red';
}
</script>
