<template>
  <BaseDialog
    :show="props.show"
    width="50%"
    title="New stock position"
    @close="handleClose"
  >
    <form class="w-70">
      <div class="flex">
        <div class="form-control" :class="{ invalid: errors.symbol }">
          <label for="symbol">Stock Symbol</label>
          <input
            type="text"
            id="symbol"
            v-model.trim="stockPositionData.symbol"
            @blur="validateEmptyField(stockPositionData.symbol, 'symbol')"
          />
          <p v-if="errors.symbol" class="error-message">
            {{ errors.symbol }}
          </p>
        </div>
        <div class="form-control" :class="{ invalid: errors.quantity }">
          <label for="quantity">Quantity</label>
          <input
            type="number"
            id="quantity"
            min="1"
            v-model="stockPositionData.quantity"
            @blur="
              validatePositiveValue(stockPositionData.quantity, 'quantity')
            "
          />
          <p v-if="errors.quantity" class="error-message">
            {{ errors.quantity }}
          </p>
        </div>
      </div>
      <div class="flex">
        <div class="form-control" :class="{ invalid: errors.averagePrice }">
          <label for="averagePrice">Average Price</label>
          <input
            type="number"
            id="averagePrice"
            min="0.01"
            step=".01"
            v-model="stockPositionData.averagePrice"
            @blur="
              validatePositiveValue(
                stockPositionData.averagePrice,
                'averagePrice'
              )
            "
          />
          <p v-if="errors.averagePrice" class="error-message">
            {{ errors.averagePrice }}
          </p>
        </div>
        <div class="form-control">
          <label for="tradingCountry">Trading Country</label>
          <input
            type="text"
            id="tradingCountry"
            disabled
            :value="props.tradingCountry"
          />
        </div>
      </div>
    </form>
    <template #actions>
      <BaseButton @click="submitForm">Create</BaseButton>
    </template>
  </BaseDialog>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useStore } from '../../store';
import { CreateStockPositionRequest } from '../../types/stockPosition';
import { TradingCountry } from '../../enums';
import useFormValidation from '../../common/composables/useFormValidation';
import { useLoading } from 'vue-loading-overlay';

interface Props {
  show: boolean;
  tradingCountry: TradingCountry;
}

const store = useStore();
const props = defineProps<Props>();
const emit = defineEmits(['close']);
const $loading = useLoading({
  color: '#ff6000',
});

const { errors, validateEmptyField, validatePositiveValue, isFormValid } =
  useFormValidation();

const stockPositionData = ref({
  symbol: '',
  quantity: 1,
  averagePrice: 0.01,
});

function clearFields() {
  stockPositionData.value.symbol = '';
  stockPositionData.value.quantity = 1;
  stockPositionData.value.averagePrice = 0.01;
}

function handleClose(): void {
  clearFields();
  emit('close');
}

function validateAllFormFields() {
  validateEmptyField(stockPositionData.value.symbol, 'symbol');
  validatePositiveValue(stockPositionData.value.quantity, 'quantity');
  validatePositiveValue(stockPositionData.value.averagePrice, 'averagePrice');
};

async function submitForm() {
  validateAllFormFields();
  if (!isFormValid(stockPositionData.value)) {
    return;
  }

  const createStockPositionRequest: CreateStockPositionRequest = {
    symbol: stockPositionData.value.symbol,
    userId: store.getters['auth/getUserId'],
    quantity: stockPositionData.value.quantity,
    averagePrice: stockPositionData.value.averagePrice,
    dateAndTimeOfStockPosition: new Date().toISOString(),
    tradingCountry: props.tradingCountry as TradingCountry,
  };

  const loader = $loading.show();
  try {
    await store.dispatch(
      'stockPositions/createStockPosition',
      createStockPositionRequest
    );
    await store.dispatch('stockPositions/getStockPositionQuotes');
    handleClose();
  } catch (error) {
    errors['symbol'] = (error as any).response.data.error;
  } finally {
    loader.hide();
  }
};
</script>
