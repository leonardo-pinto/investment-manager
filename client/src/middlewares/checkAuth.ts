import { NavigationGuardNext } from 'vue-router';

export default function checkAuth(
  next: NavigationGuardNext,
  requiresAuth: boolean,
  isAuth: boolean
) {
  if (requiresAuth && !isAuth) {
    next('/login');
  } else if (!requiresAuth && isAuth) {
    // for the case when user in logged in
    // and tries to manually go to login/register view
    next('/');
  } else {
    next();
  }
}
