import {
  AuthLoginRequest,
  AuthRegisterRequest,
  AuthResponse,
} from '../types/auth';
import httpClient from './httpClient';

const AUTH_ROUTE = '/auth';

const registerUser = async (
  authRegisterRequest: AuthRegisterRequest
): Promise<AuthResponse> => {
  const res = await httpClient.post<AuthResponse>(
    `${AUTH_ROUTE}/register`,
    authRegisterRequest
  );
  return res.data;
};

const loginUser = async (
  authLoginRequest: AuthLoginRequest
): Promise<AuthResponse> => {
  const res = await httpClient.post<AuthResponse>(
    `${AUTH_ROUTE}/login`,
    authLoginRequest
  );

  return res.data;
};

export { registerUser, loginUser };
