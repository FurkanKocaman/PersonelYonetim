import { createRouter, createWebHistory } from "vue-router";
import { authGuard } from "./authGuard";
import RoleClaims from "@/models/RoleClaims";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/home",
      component: () => import("@/views/HomeView.vue"),
      meta: { title: "Ana Sayfa", public: true },
    },
    {
      path: "/",
      redirect: "/dashboard",
    },
    {
      path: "/dashboard",
      component: () => import("@/layouts/DashBoardLayout.vue"),
      beforeEnter: authGuard,
      children: [
        {
          path: "",
          name: "Dashboard",
          component: () => import("@/views/dashboard/DashboardView.vue"),
          meta: { title: "Ana Sayfa" },
        },
        {
          path: "duyurular",
          name: "Duyurular",
          component: () => import("@/views/duyurular/DuyurularView.vue"),
          meta: { title: "Duyurular" },
        },
        {
          path: "sirket",
          name: "Sirket",
          component: () => import("@/views/sirket/SirketView.vue"),
          meta: {
            title: "Sirket Yönetimi",
            roleClaims: [RoleClaims.viewKurumsalYapi],
          },
        },
        {
          path: "personel",
          name: "Personel",
          component: () => import("@/views/personel/PersonelView.vue"),
          meta: {
            title: "Personel Yönetimi",
            roleClaims: [RoleClaims.viewPersonel],
          },
        },
        // İzin Yönetimi Ana Sayfası
        {
          path: "izin",
          name: "Izin",
          component: () => import("@/views/izin/IzinView.vue"),
          meta: { title: "İzin Yönetimi" },
          redirect: "/dashboard/izin/izinler",
          children: [
            {
              path: "izinler",
              name: "Izinler",
              component: () => import("@/views/izin/IzinTalepTableView.vue"),
              meta: { title: "İzin Listesi" },
            },
            {
              path: "izin-kurallar",
              name: "IzinKurallar",
              component: () => import("@/views/izin/IzinKuralTableView.vue"),
              meta: { title: "İzin Kuralları" },
            },
            {
              path: "kural-create",
              name: "KurallarCreate",
              component: () => import("@/views/izin/IzinKuralCreateView.vue"),
              meta: { title: "İzin Kuralları" },
            },
          ],
        },

        {
          path: "bordro",
          name: "Bordro",
          component: () => import("@/layouts/BordroLayout.vue"),
          meta: { title: "Bordro" },
          redirect: "/dashboard/bordro/calisanlar",
          children: [
            {
              path: "",
              name: "",
              component: () => import("@/views/bordro/BordroView.vue"),
              meta: { title: "Bordro" },
            },

            {
              path: "calisanlar",
              name: "Calisanlar",
              component: () => import("@/views/bordro/CalisanlarView.vue"),
              meta: { title: "Calisanlar" },
            },
          ],
        },
        {
          path: "takvim",
          name: "Takvim",
          component: () => import("@/views/takvim/TakvimView.vue"),
          meta: { title: "Takvim" },
        },
        {
          path: "ayarlar",
          name: "Ayarlar",
          component: () => import("@/views/ayarlar/AyarlarView.vue"),
          meta: { title: "Ayarlar" },
        },
        {
          path: "profile",
          name: "Profile",
          component: () => import("@/layouts/ProfileLayout.vue"),
          meta: { title: "Profil", public: false },
          beforeEnter: authGuard,
          children: [
            {
              path: "",
              name: "ProfileDefault",
              component: () => import("@/views/profile/ProfileView.vue"),
              meta: { title: "Profilim" },
            },
            {
              path: "kisisel-bilgiler",
              name: "ProfilKisiselBilgiler",
              component: () => import("@/views/profile/KisiselBilgilerView.vue"),
              meta: { title: "Kişisel Bilgiler" },
            },
            {
              path: "kariyerim",
              name: "ProfilKariyer",
              component: () => import("@/views/profile/KariyerView.vue"),
              meta: { title: "Kariyerim" },
            },
            {
              path: "izinlerim",
              name: "ProfilIzinler",
              component: () => import("@/views/profile/IzinView.vue"),
              meta: { title: "İzinlerim" },
            },
            {
              path: "odemelerim",
              name: "ProfilOdemeler",
              component: () => import("@/views/profile/OdemelerView.vue"),
              meta: { title: "Odemelerim" },
            },
            {
              path: "mesailerim",
              name: "ProfilMesailer",
              component: () => import("@/views/profile/MesailerView.vue"),
              meta: { title: "Mesailerim" },
            },
            {
              path: "diger",
              name: "ProfilDiger",
              component: () => import("@/views/profile/DigerView.vue"),
              meta: { title: "Diger" },
            },
          ],
        },

        {
          path: "/unauthorized",
          name: "Unauthorized",
          component: () => import("@/views/Auth/UnauthorizedView.vue"),
        },
      ],
    },

    {
      path: "/login",
      name: "login",
      component: () => import("@/views/Auth/LoginView.vue"),
      meta: { title: "Giriş", public: true },
    },
    {
      path: "/register",
      name: "register",
      component: () => import("@/views/Auth/RegisterView.vue"),
      meta: { title: "Kayıt ol", public: true },
    },
    {
      path: "/forgot-password",
      name: "ForgotPassword",
      component: () => import("@/views/Auth/ForgotPasswordView.vue"),
      meta: { title: "Şifremi Unuttum", public: true },
    },
    {
      path: "/reset-password",
      name: "ResetPassword",
      component: () => import("@/views/Auth/ResetPasswordView.vue"),
      meta: { title: "Yeni Şifre", public: true },
    },
    {
      path: "/:pathMatch(.*)*",
      name: "NotFound",
      component: () => import("@/views/NotFoundView.vue"),
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

export default router;
