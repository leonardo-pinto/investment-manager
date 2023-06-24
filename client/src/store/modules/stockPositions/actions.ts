import { ActionContext } from 'vuex';
import {
  createStockPosition,
  getAllStockPositions,
  updateStockPosition,
} from '../../../api/stockPositions.api';
import { TradingCountry } from '../../../enums';
import {
  CreateStockPositionRequest,
  StockPosition,
  UpdateStockPositionRequest,
} from '../../../types/stockPosition';
import { StockQuotesList } from '../../../types/stockQuotes';
import { StockPositionState } from '.';
import { getUpdateStockQuotes } from '../../../api/stockQuotes.api';

export default {
  setSelectedTradingCountry(
    { commit }: ActionContext<StockPositionState, unknown>,
    payload: TradingCountry
  ) {
    commit('setSelectedTradingCountry', payload);
  },
  async getAllStockPositions({
    commit,
    getters,
    rootGetters,
    dispatch,
  }: ActionContext<StockPositionState, unknown>) {
    const selectedCountry: TradingCountry = getters['getSelectedCountry'];
    const userId: string = rootGetters['auth/getUserId'];

    try {
      const stockPositions: StockPosition[] = await getAllStockPositions(
        userId,
        selectedCountry
      );
      commit('getAllStockPositions', stockPositions);
      await dispatch('getStockPositionQuotes');
    } catch (error) {
      throw error;
    }
  },
  async getStockPositionQuotes({
    commit,
    getters,
  }: ActionContext<StockPositionState, unknown>) {
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
  async createStockPosition(
    { commit }: ActionContext<StockPositionState, unknown>,
    payload: CreateStockPositionRequest
  ) {
    try {
      const stockPosition: StockPosition = await createStockPosition(payload);
      commit('createStockPosition', stockPosition);
    } catch (error) {
      throw error;
    }
  },
  async updateStockPosition(
    { commit, dispatch }: ActionContext<StockPositionState, unknown>,
    payload: UpdateStockPositionRequest
  ) {
    try {
      const updatedStockPosition: StockPosition = await updateStockPosition(
        payload
      );
      commit('updateStockPosition', updatedStockPosition);
      dispatch('getStockPositionQuotes');
    } catch (error) {
      throw error;
    }
  },
};
