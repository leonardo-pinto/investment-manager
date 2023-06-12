import { Module } from 'vuex';
import {
  CreateStockPositionRequest,
  StockPosition,
  StockPositionsByCountry,
  UpdateStockPositionRequest,
} from '../../../types/stockPosition';
import {
  createStockPosition,
  getAllStockPositions,
  updateStockPosition,
} from '../../../api/stockPositions.api';
import { TradingCountry } from '../../../enums';
import { getUpdateStockQuotes } from '../../../api/stockQuotes.api';
import { StockQuotesList } from '../../../types/stockQuotes';

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
  mutations: {
    setSelectedTradingCountry(
      state: StockPositionState,
      payload: TradingCountry
    ) {
      state.selectedCountry = payload;
    },
    createStockPosition(state: StockPositionState, payload: StockPosition) {
      state[state.selectedCountry].stockPositions.push(payload);
    },
    getAllStockPositions(state: StockPositionState, payload: StockPosition[]) {
      state[state.selectedCountry].stockPositions = payload;
    },
    updateStockQuotes(state: StockPositionState, payload: StockQuotesList) {
      const { stockQuotes, updatedAt } = payload;

      stockQuotes.forEach(({ symbol, price }) => {
        const index = state[state.selectedCountry].stockPositions.findIndex(
          (s) => s.symbol == symbol
        );

        if (index !== -1) {
          state[state.selectedCountry].stockPositions[index].price = price;
          state[state.selectedCountry].stockPositions = [
            ...state[state.selectedCountry].stockPositions,
          ];
        }
      });

      state[state.selectedCountry].updatedAt = updatedAt;
    },
    updateStockPosition(state: StockPositionState, payload: StockPosition) {
      const index = state[state.selectedCountry].stockPositions.findIndex(
        (s) => s.positionId === payload.positionId
      );

      if (index !== -1) {
        if (payload.quantity == 0) {
          state[state.selectedCountry].stockPositions.splice(index, 1);
        } else {
          state[state.selectedCountry].stockPositions[index] = payload;
        }

        state[state.selectedCountry].stockPositions = [
          ...state[state.selectedCountry].stockPositions,
        ];
      }
    },
  },
  actions: {
    setSelectedTradingCountry(context, payload: TradingCountry) {
      context.commit('setSelectedTradingCountry', payload);
    },
    async getAllStockPositions({ commit, getters, rootGetters }) {
      const selectedCountry: TradingCountry = getters['getSelectedCountry'];
      const userId: string = rootGetters['auth/getUserId'];

      try {
        const stockPositions: StockPosition[] = await getAllStockPositions(
          userId,
          selectedCountry
        );

        commit('getAllStockPositions', stockPositions);
      } catch (error) {
        throw error;
      }
    },
    async updatedStockPositionsQuote({ commit, getters }) {
      const symbols: string = getters['getStockSymbolsByCountry'];
      const selectedCountry: TradingCountry = getters['getSelectedCountry'];
      if (!symbols.length) return;

      try {
        const stockQuotesList: StockQuotesList = await getUpdateStockQuotes(
          symbols,
          selectedCountry.toString()
        );

        commit('updateStockQuotes', stockQuotesList);
      } catch (error) {
        throw error;
      }
    },
    async createStockPosition(context, payload: CreateStockPositionRequest) {
      try {
        const stockPosition: StockPosition = await createStockPosition(payload);

        context.commit('createStockPosition', stockPosition);
      } catch (error) {
        throw error;
      }
    },
    async updateStockPosition({ commit }, payload: UpdateStockPositionRequest) {
      try {
        const updatedStockPosition: StockPosition = await updateStockPosition(
          payload
        );

        commit('updateStockPosition', updatedStockPosition);
      } catch (error) {
        throw error;
      }
    },
  },
  getters: {
    getStockPositions(state: StockPositionState): StockPositionState {
      return state;
    },
    getSelectedCountry(state: StockPositionState): TradingCountry {
      return state.selectedCountry;
    },
    getStockSymbolsByCountry(state: StockPositionState): string[] {
      return state[state.selectedCountry].stockPositions.map((e) => e.symbol);
    },
  },
};

export default store;
