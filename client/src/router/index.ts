import {
  createRouter,
  createWebHistory,
  RouteLocationNormalized,
  NavigationGuardNext,
} from 'vue-router';
import { store } from '../store';
import checkAuth from '../middlewares/checkAuth';

declare module 'vue-router' {
  interface RouteMeta {
    requiresAuth: boolean;
  }
}

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      component: () => import('../views/AuthLogin.vue'),
    },
    {
      path: '/login',
      component: () => import('../views/AuthLogin.vue'),
    },
    {
      path: '/register',
      component: () => import('../views/AuthRegister.vue'),
      meta: {
        requiresAuth: false,
      },
    },
    {
      path: '/stock-positions',
      component: () => import('../views/StockPositions.vue'),
    },
    {
      path: '/transactions',
      component: () => import('../views/Transactions.vue'),
    },
  ],
});

router.beforeEach(
  (to: RouteLocationNormalized, _from, next: NavigationGuardNext) => {
    checkAuth(
      next,
      to.meta.requiresAuth,
      store.getters['auth/isAuthenticated']
    );
  }
);

export default router;
