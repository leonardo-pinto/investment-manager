<template>
  <table id="transactions-table">
    <thead>
      <tr>
        <th>Date/Time</th>
        <th>Description</th>
        <th>Amount</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="(transaction, index) in props.transactions" :key="index">
        <td id="date-column">
          {{ formatDate(transaction.dateAndTimeOfTransaction) }}
        </td>
        <td>{{ buildDescription(transaction) }}</td>
        <td :style="{ color: isBuy(transaction) }">
          {{ props.currency }}
          {{ calculateAmount(transaction) }}
        </td>
      </tr>
    </tbody>
  </table>
</template>

<script setup lang="ts">
// import { ref } from 'vue';
import { Transaction } from '../../types/transactions';
import { TransactionType } from '../../enums';
import { formatDate } from '../../common/helpers';

interface Props {
  transactions: Transaction[];
  currency: string;
}

const props = defineProps<Props>();

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

function isBuy(transaction: Transaction): string {
  return transaction.transactionType == TransactionType.Sell
    ? '#228B22'
    : '#FF0000';
}
</script>

<style scoped>
#transactions-table {
  margin-top: 25px;
}

#date-column {
  width: 30%;
}
</style>
