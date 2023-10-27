<template>
  <v-data-table
    v-model:items-per-page="itemsPerPage"
    :headers="headers"
    :items="processedPositions"
    class="mb-3"
  >
    <template v-slot:item.percentualGain="{ value }">
      <td :style="{ color: getResultColor(value) }">{{ value }}</td>
    </template>
    <template v-slot:item.monetaryGain="{ value }">
      <td :style="{ color: getResultColor(value) }">{{ value }}</td>
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
</template>
<script setup lang="ts">
import type { VDataTable } from 'vuetify/lib/labs/components.mjs';
import { StockPosition } from '../../types/stockPosition';
import {
  mapPositionToPositionTableData,
  getResultColor,
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

const processedPositions = props.filteredPositions.map((p) => {
  return mapPositionToPositionTableData(
    p,
    props.filteredPositions,
    props.tradingCountry
  );
});
</script>
