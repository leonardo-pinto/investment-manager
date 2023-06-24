<template>
  <BaseCard width="40%">
    <h2>Register</h2>
    <form @submit.prevent="submitForm" class="w-50">
      <div class="form-control" :class="{ invalid: errors.username }">
        <label for="username">Username</label>
        <input
          type="text"
          id="username"
          v-model.trim="registerData.username"
          @blur="validateEmptyField(registerData.username, 'username')"
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
          v-model.trim="registerData.password"
          @blur="validatePassword(registerData.password)"
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
      <div
        class="form-control"
        :class="{ invalid: errors.passwordConfirmation }"
      >
        <label for="passwordConfirmation">Confirmation</label>
        <input
          :type="passwordConfType"
          id="passwordConfirmation"
          v-model="registerData.passwordConfirmation"
          @blur="
            validatePasswordConfirmation(
              registerData.password,
              registerData.passwordConfirmation
            )
          "
        />
        <font-awesome-icon
          v-if="passwordConfType == 'password'"
          id="eye-icon"
          icon="fa-solid fa-eye-slash"
          @click="togglePasswordConfType"
        />
        <font-awesome-icon
          v-else
          id="eye-icon"
          icon="fa-solid fa-eye"
          @click="togglePasswordConfType"
        />
        <p v-if="errors.passwordConfirmation" class="error-message">
          {{ errors.passwordConfirmation }}
        </p>
      </div>
      <div v-if="apiResponseError" class="error-api-response-message">
        {{ apiResponseError }}
      </div>
      <BaseButton class="register-btn">Register</BaseButton>
    </form>
    <div class="register flex">
      <p>
        Are you a member already?
        <router-link to="/login"> Login here.</router-link>
      </p>
    </div>
  </BaseCard>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useStore } from '../store/index.ts';
import { useRouter } from 'vue-router';
import { AuthRegisterRequest } from '../types/auth';
import useFormValidation from '../common/composables/useFormValidation.ts';
import { useLoading } from 'vue-loading-overlay';

const store = useStore();
const router = useRouter();
const {
  errors,
  isFormValid,
  validateEmptyField,
  validatePassword,
  validatePasswordConfirmation,
} = useFormValidation();

const registerData = ref({
  username: '',
  password: '',
  passwordConfirmation: '',
});

const passwordType = ref<string>('password');
const passwordConfType = ref<string>('password');

function togglePasswordType() {
  if (passwordType.value == 'password') {
    passwordType.value = 'text';
  } else {
    passwordType.value = 'password';
  }
};

function togglePasswordConfType() {
  if (passwordConfType.value == 'password') {
    passwordConfType.value = 'text';
  } else {
    passwordConfType.value = 'password';
  }
};

const $loading = useLoading({
  color: '#ff6000',
});

function validateAllFormFields() {
  validateEmptyField(registerData.value.username, 'username');
  validatePassword(registerData.value.password);
  validatePasswordConfirmation(
    registerData.value.password,
    registerData.value.passwordConfirmation
  );
};

const apiResponseError = ref('');

async function submitForm() {
  validateAllFormFields();
  if (!isFormValid(registerData.value)) {
    return;
  }

  const authCredentials: AuthRegisterRequest = {
    userName: registerData.value.username,
    password: registerData.value.password,
    confirmPassword: registerData.value.passwordConfirmation,
  };

  const loader = $loading.show();
  try {
    await store.dispatch('auth/register', authCredentials);
    router.replace('/stock-positions');
  } catch (error) {
    apiResponseError.value = (error as any).response?.data?.error;
  } finally {
    loader.hide();
  }
};
</script>

<style scoped>
p {
  text-align: center;
}

.register-btn {
  width: 10rem;
  margin-top: 1.25rem;
}

.register {
  margin-top: 2rem;
  justify-content: center;
  align-items: center;
}

.register a {
  text-decoration: none;
  color: #ff6000;
}
</style>
