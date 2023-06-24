<template>
  <table>
    <thead>
      <tr>
        <th>Date/Time</th>
        <th>Description</th>
        <th>Amount</th>
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="(transaction, index) in props.filteredTransactions"
        :key="index"
      >
        <td>{{ formatDate(transaction.dateAndTimeOfTransaction) }}</td>
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
import { Transaction } from '../../types/transactions';
import { TransactionType } from '../../enums';
import { formatDate } from '../../common/helpers';

interface Props {
  filteredTransactions: Transaction[];
  currency: string;
}

const props = defineProps<Props>();

function buildDescription(transaction: Transaction): string {
  const { quantity, symbol, price } = transaction;

  const transactionType =
    transaction.transactionType === TransactionType.Buy ? 'Bought' : 'Sold';

  return `${transactionType} ${quantity} ${symbol} @ ${props.currency} ${price}`;
}

function calculateAmount(transaction: Transaction): string {
  const { quantity, price, transactionType } = transaction;
  const amount = (quantity * price).toString();

  return transactionType == TransactionType.Buy ? `-${amount}` : amount;
}

function isBuy(transaction: Transaction): string {
  return transaction.transactionType == TransactionType.Sell
    ? '#228B22'
    : '#FF0000';
}
</script>
