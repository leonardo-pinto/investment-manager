import { ActionContext } from 'vuex';
import {
  AuthLoginRequest,
  AuthRegisterRequest,
  AuthResponse,
} from '../../../types/auth';
import { AuthState } from '.';
import { loginUser, registerUser } from '../../../api/auth.api';

export default {
  async register(
    { commit }: ActionContext<AuthState, unknown>,
    payload: AuthRegisterRequest
  ) {
    try {
      const authResponse: AuthResponse = await registerUser(payload);

      localStorage.setItem('userId', authResponse.id);
      localStorage.setItem('token', authResponse.accessToken);
      localStorage.setItem('username', authResponse.username);

      commit('authUser', authResponse);
    } catch (error) {
      throw error;
    }
  },
  async login(
    { commit }: ActionContext<AuthState, unknown>,
    payload: AuthLoginRequest
  ) {
    try {
      const authResponse: AuthResponse = await loginUser(payload);
      localStorage.setItem('userId', authResponse.id);
      localStorage.setItem('token', authResponse.accessToken);
      localStorage.setItem('username', authResponse.username);

      commit('authUser', authResponse);
    } catch (error) {
      throw error;
    }
  },
  logout({ commit }: ActionContext<AuthState, unknown>) {
    localStorage.removeItem('userId');
    localStorage.removeItem('token');
    localStorage.removeItem('username');

    commit('logout');
  },
  autoLogin({ commit }: ActionContext<AuthState, unknown>) {
    const userId = localStorage.getItem('userId');
    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');

    if (userId && token && username) {
      const authUser: AuthResponse = {
        id: userId,
        accessToken: token,
        username,
      };
      commit('authUser', authUser);
    }
  },
};
