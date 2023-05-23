<template>
  <BaseCard cardWidth="40%">
    <h2>Register</h2>
    <h1 v-if="isLoading">LOADING. . .</h1>
    <form v-else @submit.prevent="submitForm">
      <div
        class="form-control"
        :class="{ invalid: errors.username }"
        id="username-container"
      >
        <label for="username">Username</label>
        <input
          type="text"
          id="username"
          v-model="registerData.username"
          @blur="validateEmptyField(registerData.username, 'username')"
        />
        <p v-if="errors.username" class="error-message">
          {{ errors.username }}
        </p>
      </div>
      <div>
        <div class="form-control" id="passwords-row">
          <div :class="{ invalid: errors.password }" id="password-container">
            <label for="password">Password</label>
            <input
              type="password"
              id="password"
              v-model="registerData.password"
              @blur="validatePassword(registerData.password)"
            />
          </div>
          <div
            :class="{ invalid: errors.passwordConfirmation }"
            id="password-confirmation-container"
          >
            <label for="passwordConfirmation">Confirmation</label>
            <input
              type="password"
              id="passwordConfirmation"
              v-model="registerData.passwordConfirmation"
              @blur="
                validatePasswordConfirmation(
                  registerData.password,
                  registerData.passwordConfirmation
                )
              "
            />
          </div>
        </div>
        <p v-if="errors.password" class="error-message">
          {{ errors.password }}
        </p>
        <p v-if="errors.passwordConfirmation" class="error-message">
          {{ errors.passwordConfirmation }}
        </p>
      </div>
      <BaseButton class="register-btn">Register</BaseButton>
      <div class="register">
        <p>Are you a member already?</p>
        <router-link to="/login">Login here.</router-link>
      </div>
    </form>
  </BaseCard>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useStore } from '../store/index.ts';
import { useRouter } from 'vue-router';
import { AuthRegisterRequest } from '../types/auth';
import useFormValidation from '../hooks/useFormValidation';

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

const isLoading = ref(false);

const validateAllFormFields = (): void => {
  validateEmptyField(registerData.value.username, 'username');
  validatePassword(registerData.value.password);
  validatePasswordConfirmation(
    registerData.value.password,
    registerData.value.passwordConfirmation
  );
};

const submitForm = async () => {
  validateAllFormFields();
  if (!isFormValid(registerData.value)) {
    return;
  }

  const authCredentials: AuthRegisterRequest = {
    userName: registerData.value.username,
    password: registerData.value.password,
    confirmPassword: registerData.value.passwordConfirmation,
  };

  isLoading.value = true;
  try {
    await store.dispatch('auth/register', authCredentials);
    router.replace('/stock-positions');
  } catch (error) {
    console.log('error -> ' + error);
    console.log((error as any)?.response?.data?.error);
  }
  isLoading.value = false;
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
  width: 60%;
}

.form-control {
  margin: 0.5rem 0;
}

#passwords-row {
  display: flex;
  justify-content: space-between;
  width: 100%;
}

#username-container,
#username-container input,
#password-container input,
#password-confirmation-container input {
  width: 100%;
}

#password-container,
#password-confirmation-container {
  width: 50%;
}

#password-confirmation-container {
  margin-left: 1.5rem;
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

.register-btn {
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

.invalid label,
.error-message {
  color: red;
}

.error-message {
  font-size: 0.8rem;
}

.invalid input {
  border: 1px solid red;
}
</style>
