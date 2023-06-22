<template>
  <TheHeader />
  <Notifications position="center right" classes="notify vue-notification" />
  <router-view></router-view>
</template>

<script setup lang="ts">
import { useStore } from './store';
import TheHeader from './layout/TheHeader.vue';

const store = useStore();
store.dispatch('auth/autoLogin');
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
}

.invalid label,
.error-message,
.error-api-response-message {
  color: red;
}

.error-message {
  font-size: 0.8rem;
}

.error-api-response-message {
  font-size: 1.1rem;
}

.invalid input {
  border: 1px solid red;
}

.notify {
  font-size: 1.1rem;
  margin: 0;
}

#eye-icon {
  float: right;
  position: relative;
  margin-top: -35px;
  margin-right: 10px;
}
</style>
