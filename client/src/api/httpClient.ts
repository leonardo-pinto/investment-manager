import axios from 'axios';
import router from '../router';
import { notify } from '@kyvg/vue3-notification';
import { useAuthStore } from '../stores/authStore';

const httpClient = axios.create({
  baseURL: import.meta.env.VITE_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

const getAuthToken = () => localStorage.getItem('token');

const authInterceptor = (config: any) => {
  config.headers['Authorization'] = `Bearer ${getAuthToken()}`;
  return config;
};

const responseInterceptor = (response: any) => response;

const errorInterceptor = (error: any) => {
  if (!error.response) {
    notify({
      title: 'Connection error',
      type: 'error',
      text: 'Please check if the server is running.',
    });
  }

  if (
    error?.response?.status === 401 &&
    router.currentRoute.value.meta.requiresAuth
  ) {
    useAuthStore().logout();
    notify({
      title: 'Session expired',
      type: 'error',
      text: 'Please login again.',
    });
  }

  if (error?.response?.status === 500) {
    notify({
      title: 'Unexpected server error',
      type: 'error',
      text: 'Please try again.',
    });
  }
  return Promise.reject(error);
};

httpClient.interceptors.request.use(authInterceptor);
httpClient.interceptors.response.use(responseInterceptor, errorInterceptor);

export default httpClient;
