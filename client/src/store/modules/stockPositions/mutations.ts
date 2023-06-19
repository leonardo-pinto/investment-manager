import { StockPositionState } from ".";
import { TradingCountry } from "../../../enums";
import { StockPosition } from "../../../types/stockPosition";
import { StockQuotesList } from "../../../types/stockQuotes";

export default {
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
}