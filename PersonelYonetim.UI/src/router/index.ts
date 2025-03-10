// import AuthService from "@/services/AuthService";
import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "Home",
      component: () => import("@/views/HomeView.vue"),
      //AuthGuard
      // beforeEnter: (to, from, next) => {
      //   if (!AuthService.isAuthenticated()) {
      //     next({ name: "Login" });
      //   } else {
      //     next();
      //   }
      // },
    },
    {
      path: "/login",
      name: "Login",
      component: () => import("@/views/LoginView.vue"),
    },
  ],
});

export default router;
