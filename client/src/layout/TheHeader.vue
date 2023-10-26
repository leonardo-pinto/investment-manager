<template>
  <v-toolbar color="#FFFFFF">
    <router-link to="/"><img id="logo" src="../assets/logo.png" /></router-link>
    <v-spacer></v-spacer>
    <div v-if="isAuth">
      <v-btn class="text-cyan-darken-3" to="/stock-positions">
        Positions
      </v-btn>
      <v-btn class="mx-6 text-cyan-darken-3" to="/transactions">
        Transactions
      </v-btn>
      <v-btn class="mr-3 text-cyan-darken-3" to="/avg-price-calculator">
        Price Calculator
      </v-btn>
      <v-btn class="text-cyan-darken-3" @click="authStore.logout"
        >Log Out
      </v-btn>
    </div>
  </v-toolbar>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useAuthStore } from '../stores/authStore';

const authStore = useAuthStore();
const isAuth = computed<boolean>(() => authStore.isAuthenticated);
</script>

<style scoped>
header {
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
