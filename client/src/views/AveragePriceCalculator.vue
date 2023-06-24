<template>
  <BaseCard>
    <h2>Average Price Calculator</h2>
    <div class="flex content-wrapper">
      <form @submit.prevent="doAverageCalculation">
        <fieldset>
          <p>First transaction</p>
          <div class="flex">
            <div>
              <label for="firstQuantity">Quantity</label>
              <input
                type="number"
                min="1"
                id="firstQuantity"
                v-model="firstQuantity"
              />
            </div>
            <div>
              <label for="firstPrice">Price</label>
              <input
                type="number"
                min="0.01"
                step=".01"
                id="firstPrice"
                v-model="firstPrice"
              />
            </div>
          </div>
        </fieldset>
        <fieldset>
          <p>Second transaction</p>
          <div class="flex">
            <div>
              <label for="secondQuantity">Quantity</label>
              <input
                type="number"
                min="1"
                id="secondQuantity"
                v-model="secondQuantity"
              />
            </div>
            <div>
              <label for="secondPrice">Price</label>
              <input
                type="number"
                min="0.01"
                step=".01"
                id="secondPrice"
                v-model="secondPrice"
              />
            </div>
          </div>
        </fieldset>
        <BaseButton @click="doAverageCalculation">Calculate</BaseButton>
      </form>
      <BaseCard width="50%" class="avg-price-content">
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
    </div>
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
.content-wrapper {
  align-items: center;
  justify-content: center;
}

.avg-price-content {
  text-align: center;
}

.avg-price-content div p {
  font-weight: bold;
}

input {
  margin-right: 0.5rem;
  width: 80%;
}

fieldset {
  font-weight: bold;
  border: 0;
}

button {
  margin-top: 1rem;
}
</style>
