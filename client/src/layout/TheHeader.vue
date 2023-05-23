<template>
  <header>
    <nav>
      <h1><router-link to="/">Investment Manager</router-link></h1>
      <ul v-if="!isAuth">
        <li>
          <BaseButton link to="/register">Register</BaseButton>
        </li>
        <li>
          <BaseButton mode="outline" link to="/login">Login</BaseButton>
        </li>
      </ul>
      <ul v-else>
        <li>
          <BaseButton link to="/stock-positions">Stock Positions</BaseButton>
        </li>
        <li>
          <BaseButton link to="/transactions">Transactions</BaseButton>
        </li>
        <li>
          <BaseButton @click="logout" mode="outline">Logout</BaseButton>
        </li>
      </ul>
    </nav>
  </header>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useStore } from '../store';
import { useRouter } from 'vue-router';

const store = useStore();
const router = useRouter();
const isAuth = computed<boolean>(() => store.getters['auth/isAuthenticated']);

const logout = () => {
  store.dispatch('auth/logout');
  router.replace('/');
};
</script>

<style scoped>
header {
  align-items: center;
  background-color: #fff;
  display: flex;
  height: 8rem;
  justify-content: center;
  width: 100%;
}

header nav {
  align-items: center;
  display: flex;
  justify-content: space-between;
  width: 90%;
}

header ul {
  align-items: center;
  display: flex;
  justify-content: center;
  list-style: none;
}

header a {
  padding: 0.75rem 1.25rem;
  text-decoration: none;
}

li {
  margin: 0 0.5rem;
}
</style>
