<template>
  <BaseDialog
    :show="props.show"
    width="80%"
    title="Register new stock position"
    @close="handleClose"
  >
    <form>
      <div class="form-row">
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
      <div class="form-row">
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
      <BaseButton @click="submitForm">REGISTER</BaseButton>
    </template>
  </BaseDialog>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useStore } from '../../store';
import { CreateStockPositionRequest } from '../../types/stockPosition';
import { TradingCountry } from '../../enums';
import useFormValidation from '../../common/composables/useFormValidation';

interface Props {
  show: boolean;
  tradingCountry: TradingCountry;
}

const store = useStore();
const props = defineProps<Props>();
const emit = defineEmits(['close']);

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

const validateAllFormFields = (): void => {
  validateEmptyField(stockPositionData.value.symbol, 'symbol');
  validatePositiveValue(stockPositionData.value.quantity, 'quantity');
  validatePositiveValue(stockPositionData.value.averagePrice, 'averagePrice');
};

const submitForm = async () => {
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

  try {
    await store.dispatch(
      'stockPositions/createStockPosition',
      createStockPositionRequest
    );
    await store.dispatch('stockPositions/updatedStockPositionsQuote');
    handleClose();
  } catch (error) {
    errors['symbol'] = (error as any).response.data.error;
  }
};
</script>

<style scoped>
form {
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
  margin: 0 auto;
  padding: 1rem;
  width: 70%;
}

.form-row {
  display: flex;
}

.form-control {
  margin: 0.5rem;
}

label {
  font-weight: bold;
  display: block;
  margin-bottom: 0.5rem;
}

input {
  border: 1px solid #454545;
  border-radius: 10px;
  display: block;
  font: inherit;
  margin-bottom: 0.5rem;
  padding: 0.5rem;
  width: 15rem;
}

input:focus {
  border-color: #ff6000;
  outline: none;
}

.invalid label,
.error-message {
  color: red;
}

.error-message {
  font-size: 0.8rem;
}

.invalid input {
  border: 1px solid red;
}
</style>
