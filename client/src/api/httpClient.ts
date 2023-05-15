import axios from 'axios';

const httpClient = axios.create({
  baseURL: import.meta.env.VITE_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  // add timeout?
});

const getAuthToken = () => localStorage.getItem('token');

const authInterceptor = (config: any) => {
  config.headers['Authorization'] = `Bearer ${getAuthToken()}`;
  return config;
};

httpClient.interceptors.request.use(authInterceptor);

export default httpClient;
