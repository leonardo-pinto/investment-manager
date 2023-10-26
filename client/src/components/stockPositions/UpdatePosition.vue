<template>
  <v-row justify="center">
    <v-dialog width="512" v-model="visible">
      <template v-slot:activator="{ props }">
        <v-btn v-bind="props"> {{ $props.transactionType }} </v-btn>
      </template>
      <v-card>
        <v-card-title class="text-h5 text-center mt-3">
          <span>{{ props.transactionType }} Position</span>
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
                  :rules="priceRules"
                >
                </v-text-field>
              </v-row>
              <v-row v-if="apiResponseError">
                {{ apiResponseError }}
              </v-row>
            </v-col>
          </v-container>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              size="large"
              @click="handleClose"
              variant="outlined"
              color="#00838f"
            >
              Close
            </v-btn>
            <v-btn
              type="submit"
              :loading="loading"
              color="#00838f"
              variant="flat"
              size="large"
            >
              {{ props.transactionType }}
            </v-btn>
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>
  </v-row>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { TransactionType } from '../../enums';
import {
  StockPosition,
  UpdateStockPositionRequest,
} from '../../types/stockPosition';
import {
  validateInteger,
  validatePositive,
} from '../../common/helpers/validationRules';
import { usePositionsStore } from '../../stores/positionsStore';

interface Props {
  position: StockPosition | null;
  transactionType: TransactionType;
}
const props = defineProps<Props>();
const positionsStore = usePositionsStore();
const visible = ref<boolean>(false);
const form = ref();
const quantity = ref<number>();
const price = ref<number>();
const quantityRules = [
  (value: number) => validatePositive(value, 'Quantity'),
  (value: number) => validateInteger(value, 'Quantity'),
];
const priceRules = [(value: number) => validatePositive(value, 'Price')];
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
      transactionType: props.transactionType,
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
</script>
