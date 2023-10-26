<template>
  <v-card
    title="Register to Investment Manager"
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
              :append-inner-icon="pswdVisible ? 'mdi-eye-off' : 'mdi-eye'"
              :type="pswdVisible ? 'text' : 'password'"
              density="compact"
              prepend-inner-icon="mdi-lock-outline"
              @click:append-inner="pswdVisible = !pswdVisible"
              :rules="passwordRules"
            ></v-text-field>
          </v-row>
          <v-row>
            <v-text-field
              label="Password Confirmation"
              v-model.trim="passwordConfirmation"
              :append-inner-icon="pswdConfVisible ? 'mdi-eye-off' : 'mdi-eye'"
              :type="pswdConfVisible ? 'text' : 'password'"
              density="compact"
              prepend-inner-icon="mdi-lock-outline"
              @click:append-inner="pswdConfVisible = !pswdConfVisible"
              :rules="passwordConfirmationRules"
            ></v-text-field>
          </v-row>
          <v-row class="d-flex justify-center">
            <v-btn
              type="submit"
              class="w-50 mt-4"
              color="#06C"
              :loading="loading"
            >
              Register
            </v-btn>
          </v-row>
          <v-row>
            <v-card-text class="register">
              Are you a member already?
              <router-link to="/login"> Log in here</router-link>
            </v-card-text>
          </v-row>
        </v-col>
      </v-container>
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { AuthRegisterRequest } from '../types/auth';
import {
  requiredField,
  passwordPattern,
} from '../common/helpers/validationRules';
import { useAuthStore } from '../stores/authStore';

const authStore = useAuthStore();
const userName = ref<string>('');
const password = ref<string>('');
const passwordConfirmation = ref<string>('');
const form = ref();
const pswdVisible = ref(false);
const pswdConfVisible = ref(false);
const loading = ref(false);
const apiResponseError = ref('');

const userNameRules = [(value: string) => requiredField(value, 'Username')];
const passwordRules = [
  (value: string) => requiredField(value, 'Password'),
  (value: string) => passwordPattern(value),
];
const passwordConfirmationRules = [
  (value: string) => value === password.value || 'Passwords must match',
];

async function submitForm() {
  const { valid } = await form.value?.validate();
  if (valid) {
    const authCredentials: AuthRegisterRequest = {
      userName: userName.value,
      password: password.value,
      confirmPassword: passwordConfirmation.value,
    };
    try {
      loading.value = true;
      await authStore.register(authCredentials);
    } catch (error) {
      apiResponseError.value = (error as any).response?.data?.error;
    } finally {
      loading.value = false;
    }
  }
}
</script>
