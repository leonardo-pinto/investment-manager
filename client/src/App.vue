<template>
  <TheHeader />
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
</style>
