<template>
  <h1>transactions</h1>
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
        <td>{{ transaction.dateAndTimeOfTransaction }}</td>
        <td>{{ buildDescription(transaction) }}</td>
        <td>{{ transaction.cost }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script setup lang="ts">
import { Transaction } from '../../types/transactions';
import { TransactionType } from '../../enums';

interface Props {
  filteredTransactions: Transaction[];
}

const props = defineProps<Props>();

const buildDescription = (transaction: Transaction): string => {
  const { quantity, symbol, price } = transaction;

  const transactionType =
    transaction.transactionType === TransactionType.Buy ? 'Bought' : 'Sold';

  return `${transactionType} ${quantity} ${symbol} @ ${price}`;
};
</script>
