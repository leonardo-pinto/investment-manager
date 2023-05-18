import { NavigationGuardNext } from 'vue-router';

export default function checkAuth(
  next: NavigationGuardNext,
  requiresAuth: boolean,
  isAuth: boolean
) {
  if (requiresAuth && !isAuth) {
    next('/login');
  } else {
    next();
  }
}
