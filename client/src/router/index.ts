import {
  createRouter,
  createWebHistory,
  RouteLocationNormalized,
  NavigationGuardNext,
} from 'vue-router';
import checkAuth from '../middlewares/checkAuth';
import { useAuthStore } from '../stores/authStore';

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
      redirect: '/stock-positions',
    },
    {
      path: '/login',
      component: () => import('../views/AuthLogin.vue'),
    },
    {
      path: '/register',
      component: () => import('../views/AuthRegister.vue'),
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
    {
      path: '/avg-price-calculator',
      component: () => import('../views/AveragePriceCalculator.vue'),
      meta: {
        requiresAuth: true,
      },
    },
  ],
});

router.beforeEach(
  (to: RouteLocationNormalized, _from, next: NavigationGuardNext) => {
    checkAuth(next, to.meta.requiresAuth, useAuthStore().isAuthenticated);
  }
);

export default router;
