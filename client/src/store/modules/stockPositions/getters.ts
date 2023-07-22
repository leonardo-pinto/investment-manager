import { StockPositionState } from '.';
import { TradingCountry } from '../../../enums';

export default {
  getStockPositions(state: StockPositionState): StockPositionState {
    return state;
  },
  getSelectedCountry(state: StockPositionState): TradingCountry {
    return state.selectedCountry;
  },
  getStockSymbolsByCountry(state: StockPositionState): string[] {
    return state[state.selectedCountry].stockPositions.map((e) => e.symbol);
  },
  getUpdateAtByCountry(state: StockPositionState): string {
    return state[state.selectedCountry].updatedAt;
  },
};
