<template>
  <v-card
    title="Log in to Investment Manager"
    class="mx-auto mt-5 text-center"
    elevation="8"
    max-width="448"
    rounded="lg"
  >
    <form @submit.prevent="submitForm">
      <v-container>
        <v-col>
          <v-row>
            <v-text-field
              label="Username"
              v-model.trim="userName.value.value"
              prepend-inner-icon="mdi-account"
              density="compact"
              :error-messages="userName.errorMessage.value"
            ></v-text-field>
          </v-row>
          <v-row>
            <v-text-field
              label="Password"
              v-model.trim="password.value.value"
              :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
              :type="visible ? 'text' : 'password'"
              density="compact"
              prepend-inner-icon="mdi-lock-outline"
              @click:append-inner="visible = !visible"
              :error-messages="password.errorMessage.value"
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
    </form>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useField, useForm } from 'vee-validate';
import { useStore } from '../store';
import { useRouter } from 'vue-router';
import { AuthLoginRequest } from '../types/auth';
import { useLoading } from 'vue-loading-overlay';
import * as yup from 'yup';

const store = useStore();
const router = useRouter();

const validationSchema = yup.object().shape({
  userName: yup.string().required('Username is required'),
  password: yup
    .string()
    .matches(
      /^(?=.*[A-Z])(?=.*\W).{8,}$/,
      'Password must contain at least 8 characters including one non alphanumeric and one upper case character.'
    )
    .required('Password is required'),
});

const { handleSubmit } = useForm({
  validationSchema,
});

const userName = useField<string>('userName', validationSchema);
const password = useField<string>('password', validationSchema);

const $loading = useLoading({
  color: '#ff6000',
});

const visible = ref(false);

const apiResponseError = ref('');

const submitForm = handleSubmit(async () => {
  const authCredentials: AuthLoginRequest = {
    userName: userName.value.value,
    password: password.value.value,
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
});
</script>

<style scoped>
.register a {
  text-decoration: none;
  color: #06c;
}
</style>
