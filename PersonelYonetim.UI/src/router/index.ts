import { createRouter, createWebHistory } from 'vue-router'
import DashboardLayout from '@/layouts/DashBoardLayout.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/dashboard'
    },
    {
      path: '/dashboard',
      component: DashboardLayout,
      children: [
        {
          path: '',
          name: 'Dashboard',
          component: () => import('../views/DashboardView.vue'),
          meta: { title: 'Ana Sayfa' }
        },
        {
          path: 'personel',
          name: 'Personel',
          component: () => import('../views/PersonelView.vue'),
          meta: { title: 'Personel Yönetimi' }
        },
        {
          path: 'izin',
          name: 'Izin',
          component: () => import('../views/IzinView.vue'),
          meta: { title: 'İzin Yönetimi' }
        },
        {
          path: 'izin/talep',
          name: 'IzinTalep',
          component: () => import('../views/IzinTalepView.vue'),
          meta: { title: 'İzin Talebi' }
        },
        {
          path: 'maas',
          name: 'Maas',
          component: () => import('../views/MaasView.vue'),
          meta: { title: 'Maaş Yönetimi' }
        },
        {
          path: 'takvim',
          name: 'Takvim',
          component: () => import('../views/TakvimView.vue'),
          meta: { title: 'Takvim' }
        },
        {
          path: 'ayarlar',
          name: 'Ayarlar',
          component: () => import('../views/AyarlarView.vue'),
          meta: { title: 'Ayarlar' }
        }
      ]
    },
    {
      path: '/login',
      name: 'Login',
      component: () => import('../views/LoginView.vue'),
      meta: { title: 'Giriş', public: true }
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'NotFound',
      component: () => import('../views/NotFoundView.vue'),
      meta: { title: 'Sayfa Bulunamadı', public: true }
    }
  ],
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition
    } else {
      return { top: 0 }
    }
  }
})

// Navigation guards
router.beforeEach((to, from, next) => {
  // Set document title
  document.title = `${to.meta.title || 'Personel Yönetim'} | Personel Yönetim Sistemi`
  
  // Add authentication logic here if needed
  // For now, we'll just allow all routes
  next()
})

export default router
