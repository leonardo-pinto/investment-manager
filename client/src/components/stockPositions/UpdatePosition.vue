<template>
  <v-dialog width="512" v-model="visible">
    <template v-slot:activator="{ props }">
      <v-btn v-bind:color="getColor()" class="mr-2" size="small" v-bind="props">
        {{ $props.transactionType }}
      </v-btn>
    </template>
    <v-card>
      <v-card-title class="text-h5 text-center mt-3">
        {{ props.transactionType }} Position
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
                v-model="props.position!.symbol"
                label="Symbol"
                readonly
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
            <v-row v-if="apiResponseError" class="error-api-response-message">
              {{ apiResponseError }}
            </v-row>
          </v-col>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-row class="mb-5">
            <VBtnSecondary size="large" @click="handleClose">
              Close
            </VBtnSecondary>
            <VBtnPrimary type="submit" :loading="loading" size="large">
              {{ props.transactionType }}
            </VBtnPrimary>
          </v-row>
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import {
  StockPosition,
  UpdateStockPositionRequest,
} from '../../types/stockPosition';
import {
  requiredField,
  validateInteger,
  validatePositive,
} from '../../common/helpers/validationRules';
import { usePositionsStore } from '../../stores/positionsStore';
import { TransactionType } from '../../enums';

interface Props {
  position: StockPosition | null;
  transactionType: string;
}
const props = defineProps<Props>();
const positionsStore = usePositionsStore();
const visible = ref<boolean>(false);
const form = ref();
const quantity = ref<number>();
const price = ref<number>();
const quantityRules = [
  (value: number) => requiredField(value, 'Quantity'),
  (value: number) => validatePositive(value, 'Quantity'),
  (value: number) => validateInteger(value, 'Quantity'),
];
const priceRules = [
  (value: number) => requiredField(value, 'Price'),
  (value: number) => validatePositive(value, 'Price'),
];
function handleClose() {
  visible.value = false;
}
const loading = ref<boolean>(false);
const apiResponseError = ref('');

async function submitForm() {
  const { valid } = await form.value?.validate();

  if (valid) {
    const updatePosition: UpdateStockPositionRequest = {
      positionId: props.position?.positionId!,
      symbol: props.position?.symbol!,
      quantity: quantity.value!,
      price: price.value!,
      transactionType:
        TransactionType[props.transactionType as keyof typeof TransactionType],
      tradingCountry: positionsStore.currentCountry,
    };

    loading.value = true;
    try {
      await positionsStore.updatePosition(updatePosition);
      handleClose();
    } catch (error) {
      apiResponseError.value = (error as any).response?.data?.error;
    } finally {
      loading.value = false;
    }
  }
}

function getColor() {
  return props.transactionType == TransactionType.Buy ? '#4CAF50' : '#F44336';
}
</script>
