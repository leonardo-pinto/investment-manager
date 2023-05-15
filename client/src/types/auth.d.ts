export interface AuthLoginRequest {
  userName: string;
  password: string;
}

export interface AuthRegisterRequest {
  userName: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponse {
  id: string;
  userName: string;
  accessToken: string;
}
