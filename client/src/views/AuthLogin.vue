<template>
  <BaseCard cardWidth="40%">
    <h2>Login</h2>
    <form @submit.prevent="submitForm">
      <div class="form-control" :class="{ invalid: errors.username }">
        <label for="username">Username</label>
        <input
          type="text"
          id="username"
          v-model.trim="loginData.username"
          @blur="validateEmptyField(loginData.username, 'username')"
        />
        <p v-if="errors.username" class="error-message">
          {{ errors.username }}
        </p>
      </div>
      <div class="form-control" :class="{ invalid: errors.password }">
        <label for="password">Password</label>
        <input
          type="password"
          id="password"
          v-model.trim="loginData.password"
          @blur="validateEmptyField(loginData.password, 'password')"
        />
        <p v-if="errors.password" class="error-message">
          {{ errors.password }}
        </p>
      </div>
      <div v-if="apiResponseError" class="error-api-response-message">
        {{ apiResponseError }}
      </div>
      <BaseButton class="login-btn">Login</BaseButton>
      <div class="register">
        <p>Not a member yet?</p>
        <router-link to="/register">Register here.</router-link>
      </div>
    </form>
  </BaseCard>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useStore } from '../store';
import { useRouter } from 'vue-router';
import { AuthLoginRequest } from '../types/auth';
import useFormValidation from '../common/composables/useFormValidation';
import { useLoading } from 'vue-loading-overlay';

const store = useStore();
const router = useRouter();
const { errors, validateEmptyField, isFormValid } = useFormValidation();

const loginData = ref({
  username: '',
  password: '',
});

const $loading = useLoading({
  color: '#ff6000',
});

const validateAllFormFields = (): void => {
  validateEmptyField(loginData.value.username, 'username');
  validateEmptyField(loginData.value.password, 'password');
};

const apiResponseError = ref('');

const submitForm = async () => {
  validateAllFormFields();
  if (!isFormValid(loginData.value)) {
    return;
  }

  const authCredentials: AuthLoginRequest = {
    userName: loginData.value.username,
    password: loginData.value.password,
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
};
</script>

<style scoped>
h2 {
  text-align: center;
}

form {
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
  margin: 0 auto;
  padding: 1rem;
  width: 70%;
}

.form-control {
  margin: 0.5rem 0;
}

label {
  font-weight: bold;
  display: block;
  margin-bottom: 0.5rem;
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

.login-btn {
  width: 10rem;
  margin-top: 1.25rem;
}

p {
  display: inline;
  padding: 0 0.5rem;
}

.register {
  margin-top: 2rem;
}

.register a {
  text-decoration: none;
  color: #ff6000;
}
</style>
