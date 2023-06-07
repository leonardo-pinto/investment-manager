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
            calculateCostOrMarketValue(
              stockPosition.quantity,
              stockPosition.averagePrice
            )
          }}
        </td>
        <td>
          {{
            calculateCostOrMarketValue(
              stockPosition.quantity,
              stockPosition.price
            )
          }}
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
          {{ calculatePositionWeight(stockPosition) }}
        </td>
        <td>
          <BaseButton
            @click="openUpdateStock(stockPosition.positionId, TransactionType.Buy)"
            mode="outline"
            >Buy</BaseButton
          >
          <BaseButton
            @click="openUpdateStock(stockPosition.positionId, TransactionType.Sell)"
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

interface Props {
  filteredStockPositions: StockPosition[];
}

const props = defineProps<Props>();

const emit = defineEmits(['openUpdateStock']);

function openUpdateStock(positionId: string, transactionType: TransactionType) {
  emit('openUpdateStock', { positionId, transactionType });
}

function calculateCostOrMarketValue(quantity: number, value: number): string {
  return value ? (quantity * value).toFixed(2) : '';
}

function calculateGainPercentage(price: number, avgPrice: number): string {
  return price ? ((price / avgPrice - 1) * 100).toFixed(2) : '';
}

function calculateGainMonetary(
  qty: number,
  price: number,
  avgPrice: number
): string {
  return price ? (qty * price - qty * avgPrice).toFixed(2) : '';
}

function calculateMktValueSum(): number {
  return props.filteredStockPositions.reduce(
    (acc, curr) =>
      acc + Number(calculateCostOrMarketValue(curr.quantity, curr.price)),
    0
  );
}

const mktValueSum: number = calculateMktValueSum();

function calculatePositionWeight(stockPosition: StockPosition): string {
  const positionWeight =
    (100 *
      Number(
        calculateCostOrMarketValue(stockPosition.quantity, stockPosition.price)
      )) /
    mktValueSum;
  return String(positionWeight.toFixed(2));
}
</script>
