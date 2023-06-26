<template>
  <header>
    <nav>
      <router-link to="/"
        ><img id="logo" src="../assets/logo.png"
      /></router-link>
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
          <BaseButton link to="/avg-price-calculator"
            >Average Price Calculator</BaseButton
          >
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

function logout() {
  store.dispatch('auth/logout');
  router.replace('/login');
}
</script>

<style scoped>
header {
  height: 8rem;
  padding: 1rem 2.5rem;
}

header nav {
  align-items: center;
  display: flex;
  justify-content: space-between;
}

header ul {
  align-items: center;
  display: flex;
  justify-content: center;
  list-style: none;
}

header a {
  text-decoration: none;
}

li {
  margin: 0 0.5rem;
}

a {
  margin: 0;
  padding: 0;
}

#logo {
  height: 5rem;
}
</style>
