import { Module } from 'vuex';
import mutations from './mutations.ts';
import actions from './actions.ts';
import getters from './getters.ts';

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
  mutations,
  actions,
  getters,
};

export default store;
