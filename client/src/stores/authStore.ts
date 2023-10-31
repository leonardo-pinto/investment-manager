import { defineStore } from 'pinia';
import {
  AuthLoginRequest,
  AuthRegisterRequest,
  AuthResponse,
} from '../types/auth';
import { loginUser, registerUser } from '../api/auth.api';
import {
  removeAuthFromLocalStorage,
  setAuthToLocalStorage,
} from '../common/helpers';
import router from '../router';

interface State {
  username: string | null;
  token: string | null;
}

export const useAuthStore = defineStore('auth', {
  state: (): State => ({
    username: null,
    token: null,
  }),
  getters: {
    isAuthenticated(): boolean {
      return !!this.token;
    },
  },
  actions: {
    async register(payload: AuthRegisterRequest) {
      try {
        const authResponse: AuthResponse = await registerUser(payload);
        this.authUser(authResponse);
      } catch (error) {
        throw error;
      }
    },
    async login(payload: AuthLoginRequest) {
      try {
        const authResponse: AuthResponse = await loginUser(payload);
        this.authUser(authResponse);
      } catch (error) {
        throw error;
      }
    },
    authUser(payload: AuthResponse) {
      router.replace('/stock-positions');
      setAuthToLocalStorage(payload);
      this.username = payload.username;
      this.token = payload.accessToken;
    },
    logout() {
      router.replace('/login');
      removeAuthFromLocalStorage();
      this.username = null;
      this.token = null;
    },
    autoLogin() {
      const token = localStorage.getItem('token');
      const username = localStorage.getItem('username');

      if (token && username) {
        const payload: AuthResponse = {
          accessToken: token,
          username,
        };
        this.authUser(payload);
      }
    },
  },
});
