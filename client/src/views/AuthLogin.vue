<template>
  <v-card
    title="Log in to Investment Manager"
    class="mx-auto mt-5 text-center"
    elevation="8"
    max-width="448"
    rounded="lg"
  >
    <v-form ref="form" validate-on="blur lazy" @submit.prevent="submitForm">
      <v-container>
        <v-col>
          <v-row>
            <v-text-field
              label="Username"
              v-model.trim="userName"
              prepend-inner-icon="mdi-account"
              density="compact"
              :rules="userNameRules"
            ></v-text-field>
          </v-row>
          <v-row>
            <v-text-field
              label="Password"
              v-model.trim="password"
              :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
              :type="visible ? 'text' : 'password'"
              density="compact"
              prepend-inner-icon="mdi-lock-outline"
              @click:append-inner="visible = !visible"
              :rules="passwordRules"
            ></v-text-field>
          </v-row>
          <v-row class="d-flex justify-center">
            <v-btn type="submit" class="w-50 mt-4" color="#06C"> Log In </v-btn>
          </v-row>
          <v-row>
            <v-card-text class="register">
              Not a member yet?
              <router-link to="/register">Register here</router-link>
            </v-card-text>
          </v-row>
        </v-col>
      </v-container>
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { AuthLoginRequest } from '../types/auth';
import { useStore } from '../store';
import { useRouter } from 'vue-router';
import { useLoading } from 'vue-loading-overlay';
import {
  requiredField,
  passwordPattern,
} from '../common/helpers/validationRules';

const store = useStore();
const router = useRouter();
const userName = ref<string>('');
const password = ref<string>('');
const form = ref();

const userNameRules = [(value: string) => requiredField(value, 'Username')];
const passwordRules = [
  (value: string) => requiredField(value, 'Password'),
  (value: string) => passwordPattern(value),
];

const $loading = useLoading({
  color: '#ff6000',
});

const visible = ref(false);

const apiResponseError = ref('');

async function submitForm() {
  const { valid } = await form.value?.validate();
  if (valid) {
    const authCredentials: AuthLoginRequest = {
      userName: userName.value,
      password: password.value,
    };
    const loader = $loading.show();
    try {
      await store.dispatch('auth/login', authCredentials);
      router.replace('/stock-positions');
    } catch (error) {
      apiResponseError.value = (error as any).response?.data?.error;
    } finally {
      loader.hide();
    }
  }
}
</script>

<style scoped>
.register a {
  text-decoration: none;
  color: #06c;
}
</style>
