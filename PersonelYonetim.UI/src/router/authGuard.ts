import type { UserModel } from "@/models/UserModel";
import { useUserStore } from "@/stores/user";
import type { NavigationGuardNext, RouteLocationNormalized } from "vue-router";

export async function authGuard(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  // Check if token exists
  const token = localStorage.getItem("accessToken");
  if (!token && !to.meta.public) {
    // No token and route is not public, redirect to login
    return next({ name: "login", query: { redirect: to.fullPath } });
  }

  try {
    // Try to get user info
    await useUserStore().getUser();
    const user: UserModel = useUserStore().user;

    // Check role requirements if any
    const requiredRoles: number[] = Array.isArray(to.meta.requiredRole) ? to.meta.requiredRole : [];
    
    if (requiredRoles.length > 0 && !requiredRoles.includes(user.role)) {
      // User doesn't have required role
      document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
      return next({ name: "Unauthorized" });
    }

    // All checks passed
    document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
    return next();
  } catch (error) {
    // Error getting user info, probably invalid token
    console.error("Auth error:", error);
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    return next({ name: "login", query: { redirect: to.fullPath } });
  }
}
