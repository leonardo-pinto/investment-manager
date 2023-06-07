import { InjectionKey } from 'vue';
import { createStore, useStore as baseUseStore, Store } from 'vuex';
import auth, { AuthState } from './modules/auth';
import stockPositions, { StockPositionState } from './modules/stockPositions';
import transactions, { TransactionState } from './modules/transactions';

export interface State {
  auth: AuthState;
  stockPositions: StockPositionState;
  transactions: TransactionState;
}

export const key: InjectionKey<Store<State>> = Symbol();

export const store = createStore({
  modules: { auth, stockPositions, transactions },
  strict: true,
});

export function useStore() {
  return baseUseStore(key);
}
