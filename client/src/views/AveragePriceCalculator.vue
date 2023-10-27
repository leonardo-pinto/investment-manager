<template>
  <v-card class="mx-auto w-50 mt-5 mb-5">
    <v-card variant="flat">
      <v-card-item class="d-flex justify-center align-items mt-4 w-100">
        <v-form
          @submit.prevent="doAverageCalculation"
          ref="form"
          validate-on="blur lazy"
          class="d-flex"
        >
          <v-card variant="flat">
            <v-card-title class="text-center">First Position</v-card-title>
            <v-card-item>
              <v-text-field
                type="number"
                label="Quantity"
                v-model.number="firstQuantity"
                :rules="quantityRules"
              >
              </v-text-field>
              <v-text-field
                type="number"
                label="Price"
                prefix="$"
                v-model.number="firstPrice"
                :rules="priceRules"
              >
              </v-text-field>
            </v-card-item>
          </v-card>
          <v-card class="ml-4" variant="flat">
            <v-card-title class="text-center"> Second Position </v-card-title>
            <v-card-item>
              <v-text-field
                label="Quantity"
                type="number"
                v-model.number="secondQuantity"
                :rules="quantityRules"
              >
              </v-text-field>

              <v-text-field
                label="Price"
                type="number"
                prefix="$"
                v-model.number="secondPrice"
                :rules="priceRules"
              >
              </v-text-field>
            </v-card-item>
          </v-card>
        </v-form>
      </v-card-item>
      <v-card-actions class="d-flex justify-center mb-6">
        <VBtnSecondary @click="clearFields">Clear</VBtnSecondary>
        <VBtnPrimary @click="doAverageCalculation">Calculate</VBtnPrimary>
      </v-card-actions>
    </v-card>
    <v-card
      class="d-flex justify-center flex-column align-center mb-5"
      variant="flat"
      v-if="averagePrice"
    >
      <p class="font-weight-bold">Average price: ${{ averagePrice }}</p>
      <p class="font-weight-bold">Total Quantity: {{ totalQuantity }}</p>
      <p class="font-weight-bold">Total Amount: ${{ totalAmount }}</p>
    </v-card>
  </v-card>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { calculateAveragePrice } from '../common/helpers';
import {
  requiredField,
  validateInteger,
  validatePositive,
} from '../common/helpers/validationRules';
const form = ref();
const firstQuantity = ref<number | null>(null);
const firstPrice = ref<number | null>(null);
const secondQuantity = ref<number | null>(null);
const secondPrice = ref<number | null>(null);
const averagePrice = ref<number | null>(null);

const quantityRules = [
  (value: number) => requiredField(value, 'Quantity'),
  (value: number) => validatePositive(value, 'Quantity'),
  (value: number) => validateInteger(value, 'Quantity'),
];
const priceRules = [
  (value: number) => requiredField(value, 'Price'),
  (value: number) => validatePositive(value, 'Price'),
];

async function doAverageCalculation() {
  const { valid } = await form.value?.validate();

  if (valid) {
    averagePrice.value = +calculateAveragePrice(
      firstQuantity.value!,
      firstPrice.value!,
      secondQuantity.value!,
      secondPrice.value!
    ).toFixed(2);
  }
}

function clearFields() {
  firstQuantity.value = null;
  firstPrice.value = null;
  secondQuantity.value = null;
  secondPrice.value = null;
  averagePrice.value = null;
}

const totalQuantity = computed<number>(
  () => firstQuantity.value! + secondQuantity.value!
);
const totalAmount = computed<number>(
  () => firstPrice.value! + secondPrice.value!
);
</script>
