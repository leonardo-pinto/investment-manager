import { defineStore } from 'pinia';
import { TradingCountry } from '../enums';
import {
  CreateStockPositionRequest,
  StockPosition,
  StockPositionsByCountry,
  UpdateStockPositionRequest,
} from '../types/stockPosition';
import {
  createStockPosition,
  getAllStockPositions,
  updateStockPosition,
} from '../api/stockPositions.api';
import { getUpdateStockQuotes } from '../api/stockQuotes.api';
import { StockQuotesList } from '../types/stockQuotes';

interface State {
  currentCountry: TradingCountry;
  BR: StockPositionsByCountry;
  US: StockPositionsByCountry;
}

export const usePositionsStore = defineStore('positions', {
  state: (): State => ({
    BR: {
      stockPositions: [],
      updatedAt: '',
    },
    US: {
      stockPositions: [],
      updatedAt: '',
    },
    currentCountry: TradingCountry.US,
  }),
  getters: {
    symbolsByCountry(state) {
      return state[state.currentCountry].stockPositions.map((p) => p.symbol);
    },
    getCurrency(state) {
      return state.currentCountry === TradingCountry.US ? '$' : 'R$';
    },
  },
  actions: {
    setTradingCountry(tradingCountry: TradingCountry) {
      this.currentCountry = tradingCountry;
    },
    async getAllStockPositionsByCountry() {
      try {
        const stockPositions: StockPosition[] = await getAllStockPositions(
          this.currentCountry
        );
        this[this.currentCountry].stockPositions = stockPositions;
        await this.getStockPositionQuotes();
      } catch (error) {
        throw error;
      }
    },
    async getStockPositionQuotes() {
      const symbols = this.symbolsByCountry;
      if (!symbols.length) return;

      try {
        const stockQuotesList: StockQuotesList = await getUpdateStockQuotes(
          symbols.join(),
          this.currentCountry.toString()
        );

        this.updatePositionQuotes(stockQuotesList);
      } catch (error) {
        throw error;
      }
    },
    updatePositionQuotes(payload: StockQuotesList) {
      const { stockQuotes, updatedAt } = payload;
      stockQuotes.forEach(({ symbol, price }) => {
        const index = this[this.currentCountry].stockPositions.findIndex(
          (s) => s.symbol == symbol
        );

        if (index !== -1) {
          this[this.currentCountry].stockPositions[index].price = price;
          this[this.currentCountry].stockPositions = [
            ...this[this.currentCountry].stockPositions,
          ];
        }
      });
      this[this.currentCountry].updatedAt = updatedAt;
    },
    async createPosition(payload: CreateStockPositionRequest) {
      try {
        const stockPosition: StockPosition = await createStockPosition(payload);
        this[this.currentCountry].stockPositions.push(stockPosition);
      } catch (error) {
        throw error;
      }
    },
    async updatePosition(payload: UpdateStockPositionRequest) {
      try {
        const response: StockPosition = await updateStockPosition(payload);
        this.setUpdatedPosition(response);

        await this.getStockPositionQuotes();
      } catch (error) {
        throw error;
      }
    },
    setUpdatedPosition(payload: StockPosition) {
      const index = this[this.currentCountry].stockPositions.findIndex(
        (s) => s.positionId === payload.positionId
      );

      if (index !== -1) {
        if (payload.quantity == 0) {
          this[this.currentCountry].stockPositions.splice(index, 1);
        } else {
          this[this.currentCountry].stockPositions[index] = payload;
        }
        this[this.currentCountry].stockPositions = [
          ...this[this.currentCountry].stockPositions,
        ];
      }
    },
  },
});
