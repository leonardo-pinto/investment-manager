<template>
  <table>
    <thead>
      <tr>
        <th id="table-title" colspan="10">{{ props.title }}</th>
      </tr>
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
      <tr v-for="(stockPosition, index) in sortedStockPositions" :key="index">
        <td>{{ stockPosition.symbol }}</td>
        <td>{{ stockPosition.quantity }}</td>
        <td>
          {{ currencyFormatter.format(stockPosition.price) }}
        </td>
        <td>
          {{ currencyFormatter.format(stockPosition.averagePrice) }}
        </td>
        <td>
          {{
            currencyFormatter.format(
              calculateValue(stockPosition.quantity, stockPosition.averagePrice)
            )
          }}
        </td>
        <td>
          {{
            currencyFormatter.format(
              calculateValue(stockPosition.quantity, stockPosition.price)
            )
          }}
        </td>
        <td :style="{ color: checkProfit(stockPosition) }">
          {{
            currencyFormatter.format(
              calculateGainPercentage(
                stockPosition.price,
                stockPosition.averagePrice
              )
            )
          }}%
        </td>
        <td :style="{ color: checkProfit(stockPosition) }">
          {{
            currencyFormatter.format(
              calculateGainMonetary(
                stockPosition.quantity,
                stockPosition.price,
                stockPosition.averagePrice
              )
            )
          }}
        </td>
        <td>
          {{
             currencyFormatter.format(calculatePositionWeight(
              stockPosition,
              props.filteredStockPositions
            ))
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
  title: string;
  filteredStockPositions: StockPosition[];
  currency: string;
}

const props = defineProps<Props>();

const emit = defineEmits(['openUpdateStock']);

const sortedStockPositions = [...props.filteredStockPositions].sort((a, b) =>
  a.symbol > b.symbol ? 1 : b.symbol > a.symbol ? -1 : 0
);

function openUpdateStock(positionId: string, transactionType: TransactionType) {
  emit('openUpdateStock', { positionId, transactionType });
}

function checkProfit(stockPosition: StockPosition): string {
  const { price, averagePrice } = stockPosition;
  return price > averagePrice ? '#228B22' : '#FF0000';
}

const currencyFormatter = new Intl.NumberFormat('en-US', {
  style: 'currency',
  currency: props.currency === '$' ? 'USD' : 'BRL',
  minimumFractionDigits: 2,
});
</script>

<style scoped>
#table-title {
  border-bottom: 1px solid #dddddd;
  text-align: center;
}

.action-buttons {
  margin-right: 0.4rem;
  padding: 0.25rem 1rem;
  font-weight: 200;
}
</style>
