<template>
  <table>
    <thead>
      <tr>
        <th>Symbol</th>
        <th>Quantity</th>
        <th>Price</th>
        <th>Average Price</th>
        <th>Cost</th>
        <th>Market Value</th>
        <th>Percentual Gain</th>
        <th>Monetary Gain</th>
        <th>Position Weight</th>
        <th>Actions</th>
      </tr>
    </thead>
    <tbody>
      <tr
        v-for="(stockPosition, index) in props.filteredStockPositions"
        :key="index"
      >
        <td>{{ stockPosition.symbol }}</td>
        <td>{{ stockPosition.quantity }}</td>
        <td>
          {{ props.currency }}
          {{ stockPosition.price?.toFixed(2) }}
        </td>
        <td>
          {{ props.currency }}
          {{ stockPosition.averagePrice.toFixed(2) }}
        </td>
        <td>
          {{ props.currency }}
          {{
            calculateValue(stockPosition.quantity, stockPosition.averagePrice)
          }}
        </td>
        <td>
          {{ props.currency }}
          {{ calculateValue(stockPosition.quantity, stockPosition.price) }}
        </td>
        <td :style="{ color: checkProfit(stockPosition) }">
          {{
            calculateGainPercentage(
              stockPosition.price,
              stockPosition.averagePrice
            )
          }}%
        </td>
        <td :style="{ color: checkProfit(stockPosition) }">
          {{ props.currency }}
          {{
            calculateGainMonetary(
              stockPosition.quantity,
              stockPosition.price,
              stockPosition.averagePrice
            )
          }}
        </td>
        <td>
          {{
            calculatePositionWeight(
              stockPosition,
              props.filteredStockPositions
            )
          }}%
        </td>
        <td>
          <BaseButton
            @click="
              openUpdateStock(stockPosition.positionId, TransactionType.Buy)
            "
            mode="outline"
            class="action-buttons"
            >Buy</BaseButton
          >
          <BaseButton
            @click="
              openUpdateStock(stockPosition.positionId, TransactionType.Sell)
            "
            mode="outline"
            class="action-buttons"
            >Sell</BaseButton
          >
        </td>
      </tr>
    </tbody>
  </table>
</template>

<script setup lang="ts">
import { TransactionType } from '../../enums';
import { StockPosition } from '../../types/stockPosition';
import {
  calculateValue,
  calculateGainPercentage,
  calculateGainMonetary,
  calculatePositionWeight,
} from '../../common/helpers';

interface Props {
  filteredStockPositions: StockPosition[];
  currency: string;
}

const props = defineProps<Props>();

const emit = defineEmits(['openUpdateStock']);

function openUpdateStock(positionId: string, transactionType: TransactionType) {
  emit('openUpdateStock', { positionId, transactionType });
}

function checkProfit(stockPosition: StockPosition): string {
  const { price, averagePrice } = stockPosition;
  return price > averagePrice ? '#228B22' : '#FF0000';
}
</script>

<style scoped>
.action-buttons {
  margin-right: 0.4rem;
  padding: 0.25rem 1rem;
  font-weight: 200;
}
</style>
