<template>
  <BaseDialog
    :show="props.show"
    :title="title"
    width="30%"
    @close="handleClose"
  >
    <form>
      <div class="form-control">
        <label for="symbol">Stock Symbol</label>
        <input
          type="text"
          id="symbol"
          disabled
          :value="props.stockPosition?.symbol"
        />
      </div>

      <div class="form-control" :class="{ invalid: errors.quantity }">
        <label for="quantity">Quantity</label>
        <input
          type="number"
          id="quantity"
          min="1"
          v-model="stockPositionData.quantity"
          @blur="validatePositiveValue(stockPositionData.quantity, 'quantity')"
        />
        <p v-if="errors.quantity" class="error-message">
          {{ errors.quantity }}
        </p>
      </div>
      <div class="form-control" :class="{ invalid: errors.price }">
        <label for="price">Price</label>
        <input
          type="number"
          id="price"
          min="0.01"
          step=".01"
          v-model="stockPositionData.price"
          @blur="validatePositiveValue(stockPositionData.price, 'price')"
        />
        <p v-if="errors.price" class="error-message">
          {{ errors.price }}
        </p>
      </div>

      <div v-if="apiResponseError" class="error-api-response-message">
        {{ apiResponseError }}
      </div>
    </form>
    <template #actions>
      <BaseButton @click="submitForm">{{
        props.transactionType.toString()
      }}</BaseButton>
    </template>
  </BaseDialog>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import {
  StockPosition,
  UpdateStockPositionRequest,
} from '../../types/stockPosition';
import { TransactionType } from '../../enums';
import { useStore } from '../../store';
import useFormValidation from '../../common/composables/useFormValidation';
import { useLoading } from 'vue-loading-overlay';

const store = useStore();

interface Props {
  show: boolean;
  stockPosition: StockPosition | null;
  transactionType: TransactionType;
}

const props = defineProps<Props>();
const emit = defineEmits(['close']);

const $loading = useLoading({
  color: '#ff6000',
});

const { errors, validatePositiveValue, isFormValid, clearFormErrors } =
  useFormValidation();

const stockPositionData = ref({
  quantity: 1,
  price: 0.01,
});

const title = computed<string>(() =>
  props.transactionType == TransactionType.Buy
    ? 'Buy stock position'
    : 'Sell stock position'
);

function validateAllFormFields(): void {
  validatePositiveValue(stockPositionData.value.quantity, 'quantity');
  validatePositiveValue(stockPositionData.value.price, 'price');
}

function clearFields() {
  stockPositionData.value.quantity = 1;
  stockPositionData.value.price = 0.01;
  apiResponseError.value = '';
}

function handleClose(): void {
  clearFields();
  clearFormErrors();
  emit('close');
}

const apiResponseError = ref('');

async function submitForm () {
  validateAllFormFields();
  if (!isFormValid(stockPositionData)) {
    return;
  }

  const updateStock: UpdateStockPositionRequest = {
    positionId: props.stockPosition?.positionId ?? '',
    userId: store.getters['auth/getUserId'],
    symbol: props.stockPosition?.symbol ?? '',
    quantity: stockPositionData.value.quantity,
    price: stockPositionData.value.price,
    transactionType: props.transactionType,
    tradingCountry: store.getters['stockPositions/getSelectedCountry'],
  };

  const loader = $loading.show();
  try {
    await store.dispatch('stockPositions/updateStockPosition', updateStock);
    handleClose();
  } catch (error) {
    apiResponseError.value = (error as any).response?.data?.error;
  } finally {
    loader.hide();
  }
};
</script>

<style scoped>
input {
  width: 100%;
}
</style>
