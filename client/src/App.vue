<template>
  <TheHeader />
  <Notifications position="center right" classes="notify vue-notification" />
  <router-view></router-view>
</template>

<script setup lang="ts">
import { useAuthStore } from './stores/authStore';
import TheHeader from './layout/TheHeader.vue';
const authStore = useAuthStore();
authStore.autoLogin();
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap');

* {
  box-sizing: border-box;
}

html {
  font-family: 'Roboto', sans-serif;
}

body {
  margin: 0;
  background-color: #eeeeee;
}

.error-api-response-message {
  color: red;
  font-size: 1.1rem;
}

.notify {
  font-size: 1.1rem;
  margin: 0;
}

.w-20 {
  width: 20%;
}

.w-70 {
  width: 70%;
}

.w-95 {
  width: 95%;
}

.register a {
  text-decoration: none;
  color: #06c;
}
</style>
