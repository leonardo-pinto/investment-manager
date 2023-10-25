import { InjectionKey } from 'vue';
import { createStore, useStore as baseUseStore, Store } from 'vuex';
import stockPositions, { StockPositionState } from './modules/stockPositions';
import transactions, { TransactionState } from './modules/transactions';

export interface State {
  stockPositions: StockPositionState;
  transactions: TransactionState;
}

export const key: InjectionKey<Store<State>> = Symbol();

export const store = createStore({
  modules: { stockPositions, transactions },
  strict: true,
});

export function useStore() {
  return baseUseStore(key);
}
