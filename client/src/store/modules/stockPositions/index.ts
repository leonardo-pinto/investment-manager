import { Module } from 'vuex';
import { StockPositionsByCountry } from '../../../types/stockPosition';
import { TradingCountry } from '../../../enums';
import mutations from './mutations.ts';
import actions from './actions.ts';
import getters from './getters.ts';

export interface StockPositionState {
  BR: StockPositionsByCountry;
  US: StockPositionsByCountry;
  selectedCountry: TradingCountry;
}

const store: Module<StockPositionState, unknown> = {
  namespaced: true,
  state() {
    return {
      BR: {
        stockPositions: [],
        updatedAt: '',
      },
      US: {
        stockPositions: [],
        updatedAt: '',
      },
      selectedCountry: TradingCountry.US,
    };
  },
  mutations,
  actions,
  getters,
};

export default store;
