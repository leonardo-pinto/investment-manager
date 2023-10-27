<template>
  <v-row justify="start">
    <v-dialog v-model="visible" width="512">
      <template v-slot:activator="{ props }">
        <VBtnPrimary class="my-5 ml-2" v-bind="props" size="large">
          New Position
        </VBtnPrimary>
      </template>
      <v-card>
        <v-card-title class="text-h5 text-center mt-3">
          New Position
        </v-card-title>
        <v-form
          class="w-100"
          ref="form"
          validate-on="blur lazy"
          @submit.prevent="submitForm"
        >
          <v-container class="w-100">
            <v-col>
              <v-row>
                <v-text-field
                  v-model.trim="symbol"
                  label="Symbol"
                  :rules="symbolRules"
                >
                </v-text-field>
              </v-row>
              <v-row>
                <v-text-field
                  v-model.number="quantity"
                  type="number"
                  label="Quantity"
                  :rules="quantityRules"
                >
                </v-text-field>
              </v-row>
              <v-row>
                <v-text-field
                  v-model.number="price"
                  type="number"
                  label="Price"
                  :prefix="positionsStore.getCurrency"
                  :rules="priceRules"
                >
                </v-text-field>
              </v-row>
              <v-row>
                <v-select
                  v-model="type"
                  :items="validPositionTypes[positionsStore.currentCountry]"
                  label="Type"
                  :rules="typeRules"
                >
                </v-select>
              </v-row>
              <v-row
                v-if="apiResponseError"
                class="error-api-response-message mb-2"
              >
                {{ apiResponseError }}
              </v-row>
            </v-col>
          </v-container>
          <v-row class="d-flex justify-center mb-4">
            <VBtnSecondary size="large" @click="handleClose" class="mr-3">
              Close
            </VBtnSecondary>
            <VBtnPrimary type="submit" :loading="loading" size="large">
              Save
            </VBtnPrimary>
          </v-row>
        </v-form>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import {
  requiredField,
  validatePositive,
  validateInteger,
  validPositionTypes,
} from '../../common/helpers';
import { CreateStockPositionRequest } from '../../types/stockPosition';
import { usePositionsStore } from '../../stores/positionsStore';

const positionsStore = usePositionsStore();
const visible = ref(false);
const symbol = ref<string>('');
const quantity = ref<number>();
const price = ref<number>();
const type = ref();
const form = ref();
const loading = ref<boolean>(false);
const apiResponseError = ref('');

const symbolRules = [(value: string) => requiredField(value, 'Symbol')];
const quantityRules = [
  (value: number) => requiredField(value, 'Quantity'),
  (value: number) => validatePositive(value, 'Quantity'),
  (value: number) => validateInteger(value, 'Quantity'),
];
const priceRules = [
  (value: number) => requiredField(value, 'Price'),
  (value: number) => validatePositive(value, 'Price'),
];
const typeRules = [(value: string) => requiredField(value, 'Type')];

function handleClose() {
  visible.value = false;
  form.value.reset();
}

async function submitForm() {
  const { valid } = await form.value?.validate();
  if (valid) {
    const createStockPositionRequest: CreateStockPositionRequest = {
      symbol: symbol.value,
      quantity: quantity.value!,
      averagePrice: price.value!,
      dateAndTimeOfStockPosition: new Date().toISOString(),
      tradingCountry: positionsStore.currentCountry,
      type: type.value,
    };
    try {
      loading.value = true;
      await positionsStore.createPosition(createStockPositionRequest);
      await positionsStore.getStockPositionQuotes();
      handleClose();
    } catch (error) {
      apiResponseError.value = (error as any).response?.data?.error;
    } finally {
      loading.value = false;
    }
  }
}
</script>
