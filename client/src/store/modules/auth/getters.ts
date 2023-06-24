import { AuthState } from ".";

export default {
    getToken(state: AuthState): string | null {
        return state.token;
      },
      isAuthenticated(state: AuthState): boolean {
        return !!state.token;
      },
      getUserId(state: AuthState): string | null {
        return state.userId;
      },
}