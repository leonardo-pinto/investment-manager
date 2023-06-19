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
        <th>Gain (%)</th>
        <th>Gain ($)</th>
        <th>Position Weight (%)</th>
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
        <td>{{ stockPosition.price?.toFixed(2) }}</td>
        <td>{{ stockPosition.averagePrice.toFixed(2) }}</td>
        <td>
          {{
            calculateValue(stockPosition.quantity, stockPosition.averagePrice)
          }}
        </td>
        <td>
          {{ calculateValue(stockPosition.quantity, stockPosition.price) }}
        </td>
        <td>
          {{
            calculateGainPercentage(
              stockPosition.price,
              stockPosition.averagePrice
            )
          }}
        </td>
        <td>
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
            calculatePositionWeight(stockPosition, props.filteredStockPositions)
          }}
        </td>
        <td>
          <BaseButton
            @click="
              openUpdateStock(stockPosition.positionId, TransactionType.Buy)
            "
            mode="outline"
            >Buy</BaseButton
          >
          <BaseButton
            @click="
              openUpdateStock(stockPosition.positionId, TransactionType.Sell)
            "
            mode="outline"
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
} from '../../common/helpers/stockMetrics';

interface Props {
  filteredStockPositions: StockPosition[];
}

const props = defineProps<Props>();

const emit = defineEmits(['openUpdateStock']);

function openUpdateStock(positionId: string, transactionType: TransactionType) {
  emit('openUpdateStock', { positionId, transactionType });
}
</script>
