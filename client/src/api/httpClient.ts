import axios from 'axios';
import router from '../router';
import { removeAuthFromLocalStorage } from '../common/helpers';

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

const unauthErrorInterceptor = (error: any) => {
  if (
    error?.response?.status === 401 &&
    router.currentRoute.value.meta.requiresAuth
  ) {
    removeAuthFromLocalStorage();
    router.push('/register');
  }
  return Promise.reject(error);
};

httpClient.interceptors.request.use(authInterceptor);
httpClient.interceptors.response.use(
  responseInterceptor,
  unauthErrorInterceptor
);

export default httpClient;
