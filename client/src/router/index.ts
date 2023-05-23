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
      meta: {
        requiresAuth: false,
      },
    },
    {
      path: '/login',
      component: () => import('../views/AuthLogin.vue'),
      meta: {
        requiresAuth: false,
      },
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
      meta: {
        requiresAuth: true,
      },
    },
    {
      path: '/transactions',
      component: () => import('../views/Transactions.vue'),
      meta: {
        requiresAuth: true,
      },
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
