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

table {
  border-radius: 15px;
  border-collapse: collapse;
  margin-top: 25px;
  min-width: 400px;
  overflow: hidden;
  width: 100%;
}

thead tr {
  background-color: #ff6000;
  color: #ffffff;
  text-align: left;
}

table td {
  font-size: 0.8rem;
}

table th,
table td {
  padding: 10px 12px;
}

table tbody tr {
  border-bottom: 1px solid #dddddd;
}

table tbody tr:nth-of-type(even) {
  background-color: #f3f3f3;
}

.form-control {
  margin: 0.5rem;
}

h2 {
  text-align: center;
}

form {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  margin: 0 auto;
  padding: 0 1rem;
}

label {
  display: block;
  font-weight: bold;
  margin-bottom: 0.5rem;
}

select {
  padding: 0.25rem;
}

input {
  border: 1px solid #454545;
  border-radius: 10px;
  display: block;
  font: inherit;
  margin-bottom: 0.5rem;
  padding: 0.5rem;
  width: 15rem;
}

input:focus {
  border-color: #ff6000;
  outline: none;
}

.flex {
  display: flex;
}

.w-50 {
  width: 15%;
}

.w-70 {
  width: 70%;
}
</style>
