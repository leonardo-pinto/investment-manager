import { Module } from 'vuex';
import {
  AuthLoginRequest,
  AuthRegisterRequest,
  AuthResponse,
} from '../../../types/auth';
import { loginUser, registerUser } from '../../../api/auth.api';

export interface AuthState {
  userId: string | null;
  username: string | null;
  token: string | null;
}

const store: Module<AuthState, unknown> = {
  namespaced: true,
  state() {
    return {
      userId: null,
      username: null,
      token: null,
    };
  },
  mutations: {
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
  },
  actions: {
    async register(context, payload: AuthRegisterRequest) {
      try {
        const authResponse: AuthResponse = await registerUser(payload);

        localStorage.setItem('userId', authResponse.id);
        localStorage.setItem('token', authResponse.accessToken);
        localStorage.setItem('username', authResponse.username);

        context.commit('authUser', authResponse);
      } catch (error) {
        throw error;
      }
    },
    async login(context, payload: AuthLoginRequest) {
      try {
        const authResponse: AuthResponse = await loginUser(payload);
        localStorage.setItem('userId', authResponse.id);
        localStorage.setItem('token', authResponse.accessToken);
        localStorage.setItem('username', authResponse.username);

        context.commit('authUser', authResponse);
      } catch (error) {
        throw error;
      }
    },
    logout(context) {
      localStorage.removeItem('userId');
      localStorage.removeItem('token');
      localStorage.removeItem('username');

      context.commit('logout');
    },
    autoLogin(context) {
      const userId = localStorage.getItem('userId');
      const token = localStorage.getItem('token');
      const username = localStorage.getItem('username');

      if (userId && token && username) {
        const authUser: AuthResponse = {
          id: userId,
          accessToken: token,
          username,
        };
        context.commit('authUser', authUser);
      }
    },
  },
  getters: {
    getToken(state: AuthState): string | null {
      return state.token;
    },
    isAuthenticated(state: AuthState): boolean {
      return !!state.token;
    },
    getUserId(state: AuthState): string | null {
      return state.userId;
    },
  },
};

export default store;
