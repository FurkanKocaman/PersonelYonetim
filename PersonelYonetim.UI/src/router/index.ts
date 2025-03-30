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
      beforeEnter: authGuard, // Add auth guard to entire dashboard
      children: [
        {
          path: "",
          name: "Dashboard",
          component: () => import("@/views/dashboard/DashboardView.vue"),
          meta: { title: "Ana Sayfa" },
        },
        {
          path: "sirket",
          name: "Sirket",
          component: () => import("@/views/sirket/SirketView.vue"),
          meta: {
            title: "Sirket Yönetimi",
            requiredRole: [
              Roles.SirketYonetici.value,
              Roles.SirketYardimci.value,
              Roles.Admin.value,
            ],
          },
        },
        {
          path: "modal",
          name: "Modal",
          component: () => import("@/components/modals/DepartmanCreateModal.vue"),
          meta: {
            title: "Modal",
          },
        },
        {
          path: "personel",
          name: "Personel",
          component: () => import("@/views/personel/PersonelView.vue"),
          meta: {
            title: "Personel Yönetimi",
            requiredRole: [
              Roles.SirketYonetici.value,
              Roles.SirketYardimci.value,
              Roles.Admin.value,
              Roles.Calisan.value,
              Roles.DepartmanYonetici.value,
              Roles.DepartmanYardimci.value,
            ],
          },
        },
        // İzin Yönetimi Ana Sayfası
        {
          path: "izin",
          name: "Izin",
          component: () => import("@/views/izin/izin-view/IzinView.vue"),
          meta: { title: "İzin Yönetimi" },
          children: [
            {
              path: "izinler",
              name: "Izinler",
              component: () => import("@/components/izin/izin-components/IzinlerTable.vue"),
              meta: { title: "İzin Listesi" },
            },
            {
              path: "izin-kurallar",
              name: "IzinKurallar",
              component: () => import("@/components/izin/izin-components/IzinKural.vue"),
              meta: { title: "İzin Kuralları" },
            },
          ],
        },
        // İzin Talebi Sayfası
        {
          path: "izin/talep",
          name: "IzinTalep",
          component: () => import("@/views/izin/izin-view/IzinTalepView.vue"),
          meta: { title: "İzin Talebi" },
        },
        // İzin Kuralları Ana Sayfası
        {
          path: "izin/kurallar",
          name: "IzinKurallari",
          component: () => import("@/views/izin/izin-view/IzinKurallariView.vue"),
          meta: { title: "İzin Kuralları" },
          children: [
            {
              path: "",
              name: "IzinKurallariKurallar",
              component: () => import("@/views/izin/izin-view/IzinKurallariKurallarViewNew.vue"),
              meta: { title: "İzin Kuralları" },
            },
            {
              path: "raporlar",
              name: "IzinKurallariRaporlar",
              component: () => import("@/views/izin/izin-view/IzinKurallariRaporlarView.vue"),
              meta: { title: "İzin Raporları" },
            },
            {
              path: "orneksablonlar",
              name: "IzinKurallariOrnekSablonlar",
              component: () => import("@/views/izin/izin-view/IzinKurallariOrnekSablonlarView.vue"),
              meta: { title: "İzin Kuralları Şablonları" },
            },
          ],
        },
        // İzin Kuralı Oluşturma Sayfası
        {
          path: "izin/kurallar/olustur",
          name: "IzinKuralOlustur",
          component: () => import("@/views/izin/izin-view/IzinKuralCreateView.vue"),
          meta: { title: "İzin Kuralı Oluştur" },
        },
        {
          path: "maas",
          name: "Maas",
          component: () => import("@/views/maas/MaasView.vue"),
          meta: { title: "Maaş Yönetimi" },
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
          path: "/unauthorized",
          name: "Unauthorized",
          component: () => import("@/views/auth/UnauthorizedView.vue"),
        },
      ],
    },
    {
      path: "/login",
      name: "login",
      component: () => import("@/views/auth/LoginView.vue"),
      meta: { title: "Giriş", public: true },
    },
    {
      path: "/register",
      name: "register",
      component: () => import("@/views/auth/RegisterView.vue"),
      meta: { title: "Kayıt ol", public: true },
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
