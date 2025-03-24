import { createRouter, createWebHistory } from "vue-router";
import DashboardLayout from "@/layouts/DashBoardLayout.vue";
import Roles from "@/models/Roles";
import { authGuard } from "./authGuard";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      redirect: "/dashboard",
    },
    {
      path: "/dashboard",
      component: DashboardLayout,
      children: [
        {
          path: "",
          name: "Dashboard",
          component: () => import("../views/DashboardView.vue"),
          meta: { title: "Ana Sayfa" },
        },
        {
          path: "sirket",
          name: "Sirket",
          component: () => import("../views/SirketView.vue"),
          meta: {
            title: "Sirket Yönetimi",
            requiredRole: [
              Roles.SirketYonetici.value,
              Roles.SirketYardimci.value,
              Roles.Admin.value,
            ],
          },
          beforeEnter: authGuard,
        },
        {
          path: "modal",
          name: "Modal",
          component: () => import("../components/modals/DepartmanCreateModal.vue"),
          meta: {
            title: "Modal",
          },
        },
        {
          path: "personel",
          name: "Personel",
          component: () => import("../views/PersonelView.vue"),
          meta: {
            title: "Personel Yönetimi",
            requiredRole: [
              Roles.SirketYonetici.value,
              Roles.SirketYardimci.value,
              Roles.Admin.value,
              Roles.Calisan.value,
            ],
          },
          beforeEnter: authGuard,
        },
        {
          path: "izin",
          name: "Izin",
          component: () => import("../views/IzinView.vue"),
          meta: { title: "İzin Yönetimi" },
        },
        {
          path: "izin/talep",
          name: "IzinTalep",
          component: () => import("../views/IzinTalepView.vue"),
          meta: { title: "İzin Talebi" },
        },
        {
          path: "maas",
          name: "Maas",
          component: () => import("../views/MaasView.vue"),
          meta: { title: "Maaş Yönetimi" },
        },
        {
          path: "takvim",
          name: "Takvim",
          component: () => import("../views/TakvimView.vue"),
          meta: { title: "Takvim" },
          beforeEnter: authGuard,
        },
        {
          path: "ayarlar",
          name: "Ayarlar",
          component: () => import("../views/AyarlarView.vue"),
          meta: { title: "Ayarlar" },
          beforeEnter: authGuard,
        },
        {
          path: "/unauthorized",
          name: "Unauthorized",
          component: () => import("../views/Auth/UnauthorizedView.vue"),
        },
      ],
    },
    {
      path: "/login",
      name: "Login",
      component: () => import("../views/Auth/LoginView.vue"),
      meta: { title: "Giriş", public: true },
    },
    {
      path: "/register",
      name: "register",
      component: () => import("../views/Auth/RegisterView.vue"),
      meta: { title: "Kayıt ol", public: true },
    },
    {
      path: "/:pathMatch(.*)*",
      name: "NotFound",
      component: () => import("../views/NotFoundView.vue"),
      meta: { title: "Sayfa Bulunamadı", public: true },
    },
  ],
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition;
    } else {
      return { top: 0 };
    }
  },
});

// // Navigation guards
// router.beforeEach((to, from, next) => {
//   document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;

//   // Add authentication logic here if needed
//   // For now, we'll just allow all routes
//   next();
// });

export default router;
