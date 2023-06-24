<template>
  <BaseCard width="40%">
    <h2>Login</h2>
    <form @submit.prevent="submitForm" class="w-70">
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
          :type="passwordType"
          id="password"
          v-model.trim="loginData.password"
          @blur="validateEmptyField(loginData.password, 'password')"
        />
        <font-awesome-icon
          v-if="passwordType == 'password'"
          id="eye-icon"
          icon="fa-solid fa-eye-slash"
          @click="togglePasswordType"
        />
        <font-awesome-icon
          v-else
          id="eye-icon"
          icon="fa-solid fa-eye"
          @click="togglePasswordType"
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

const passwordType = ref<string>('password');

const togglePasswordType = () => {
  if (passwordType.value == 'password') {
    passwordType.value = 'text';
  } else {
    passwordType.value = 'password';
  }
};

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
.login-btn {
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
