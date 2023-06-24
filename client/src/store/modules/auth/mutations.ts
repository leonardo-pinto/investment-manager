import { AuthState } from ".";
import { AuthResponse } from "../../../types/auth";

export default {
    authUser(state: AuthState, payload: AuthResponse) {
        state.userId = payload.id;
        state.username = payload.username;
        state.token = payload.accessToken;
      },
      logout(state: AuthState) {
        state.userId = null;
        state.username = null;
        state.token = null;
      },
}