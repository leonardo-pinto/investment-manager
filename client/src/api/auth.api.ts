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
  return (
    await httpClient.post<AuthResponse>(
      `${AUTH_ROUTE}/register`,
      authRegisterRequest
    )
  ).data;
};

const loginUser = async (
  authLoginRequest: AuthLoginRequest
): Promise<AuthResponse> => {
  return (
    await httpClient.post<AuthResponse>(`${AUTH_ROUTE}/login`, authLoginRequest)
  ).data;
};

export { registerUser, loginUser };
