<template>
  <v-data-table
    v-model:sort-by="sortBy"
    v-model:items-per-page="itemsPerPage"
    :headers="headers"
    :items="processedTransactions"
  >
    <template v-slot:item.amount="{ value }">
      <td :style="{ color: getColor(value) }">{{ value }}</td>
    </template>
  </v-data-table>
</template>

<script setup lang="ts">
import { VDataTable } from 'vuetify/lib/labs/VDataTable/index.mjs';
import { Transaction } from '../../types/transactions';
import { formatDate } from '../../common/helpers';
import { TransactionType } from '../../enums';
import { ref } from 'vue';

// https://stackoverflow.com/questions/75991355/import-datatableheader-typescript-type-of-vuetify3-v-data-table
// followed this solution to fix the headers type
type UnwrapReadonlyArrayType<A> = A extends Readonly<Array<infer I>>
  ? UnwrapReadonlyArrayType<I>
  : A;
type DT = InstanceType<typeof VDataTable>;
type ReadonlyDataTableHeader = UnwrapReadonlyArrayType<DT['headers']>;
type ReadonlySortItem = UnwrapReadonlyArrayType<DT['sortBy']>;

interface Props {
  transactions: Transaction[];
  currency: string;
}

const props = defineProps<Props>();

const itemsPerPage = ref(50);
const sortBy: ReadonlySortItem[] = [{ key: 'dateAndTime', order: 'desc' }];

const headers: ReadonlyDataTableHeader[] = [
  {
    title: 'Time/Date',
    align: 'start',
    key: 'dateAndTime',
  },
  { title: 'Description', align: 'start', key: 'description', sortable: false },
  { title: 'Amount', align: 'start', key: 'amount' },
];

function getColor(value: string) {
  return Number(value.substring(1)) > 0 ? 'green' : 'red';
}

function buildDescription(transaction: Transaction): string {
  const { quantity, symbol, price } = transaction;

  const transactionType =
    transaction.transactionType === TransactionType.Buy ? 'Bought' : 'Sold';

  return `${transactionType} ${quantity} ${symbol} @ ${
    props.currency
  } ${price.toFixed(2)}`;
}

function calculateAmount(transaction: Transaction): string {
  const { quantity, price, transactionType } = transaction;
  const amount = (quantity * price).toFixed(2);

  return transactionType == TransactionType.Buy ? `-${amount}` : amount;
}

type ProcessedTransactions = {
  dateAndTime: string;
  description: string;
  amount: string;
};

const processedTransactions = props.transactions.map((t) => {
  return {
    dateAndTime: formatDate(t.dateAndTimeOfTransaction),
    description: buildDescription(t),
    amount: `${props.currency}${calculateAmount(t)}`,
  } as ProcessedTransactions;
});
</script>
