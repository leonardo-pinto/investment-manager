import { AuthResponse } from '../../types/auth';

function setAuthToLocalStorage(authResponse: AuthResponse): void {
  const { accessToken, username } = authResponse;
  localStorage.setItem('token', accessToken);
  localStorage.setItem('username', username);
}

function removeAuthFromLocalStorage() {
  localStorage.removeItem('token');
  localStorage.removeItem('username');
}

export { setAuthToLocalStorage, removeAuthFromLocalStorage };
