// import AuthService from "@/services/AuthService";
import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "Home",
      component: () => import("@/views/HomeView.vue"),
    },
    {
      path: "/dashboard",
      name: "Dashboard",
      component: () => import("@/views/DashboardView.vue"),
      // Auth guard can be added here when authentication is implemented
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
    {
      path: "/register",
      name: "Register",
      component: () => import("@/views/LoginView.vue"), // You can create a separate RegisterView later
    },
  ],
});

export default router;
