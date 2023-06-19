<template>
  <BaseCard cardWidth="70%">
    <h2>Average Price Calculator</h2>
    <div>
      <form @submit.prevent="doAverageCalculation">
        <fieldset>
          First transaction
          <label for="firstQuantity">Quantity</label>
          <input
            type="number"
            min="1"
            id="firstQuantity"
            v-model="firstQuantity"
          />

          <label for="firstPrice">Price</label>
          <input
            type="number"
            min="0.01"
            step=".01"
            id="firstPrice"
            v-model="firstPrice"
          />
        </fieldset>

        <fieldset>
          Second transaction
          <label for="secondQuantity">Quantity</label>
          <input
            type="number"
            min="1"
            id="secondQuantity"
            v-model="secondQuantity"
          />

          <label for="secondPrice">Price</label>
          <input
            type="number"
            min="0.01"
            step=".01"
            id="secondPrice"
            v-model="secondPrice"
          />
        </fieldset>
        <BaseButton>Calculate</BaseButton>
      </form>
    </div>
    <BaseCard cardWidth="70%">
      <h3>Stock Average Price</h3>
      <div v-if="averagePrice.length">
        <p>Average price: ${{ averagePrice }}</p>
        <p>Total Quantity: {{ totalQuantity }}</p>
      </div>
      <p>
        Stock average is calculated by dividing the total cost of the stock by
        the total quantity of stocks.
      </p>
    </BaseCard>
  </BaseCard>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { calculateAveragePrice } from '../common/helpers';

const firstQuantity = ref<number>(0);
const firstPrice = ref<number>(0);
const secondQuantity = ref<number>(0);
const secondPrice = ref<number>(0);
const averagePrice = ref<string>('');

function doAverageCalculation(): void {
  averagePrice.value = calculateAveragePrice(
    firstQuantity.value,
    firstPrice.value,
    secondQuantity.value,
    secondPrice.value
  );
}

const totalQuantity = computed<number>(
  () => firstQuantity.value + secondQuantity.value
);
</script>

<style scoped>
h2 {
  text-align: center;
}
</style>
