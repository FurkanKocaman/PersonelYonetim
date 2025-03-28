import type { UserModel } from "@/models/UserModel";
import { useUserStore } from "@/stores/user";
import type { NavigationGuardNext, RouteLocationNormalized } from "vue-router";

export async function authGuard(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
  next();

  /*
  await useUserStore().getUser();
  const user: UserModel = useUserStore().user;

  const requiredRoles: number[] = Array.isArray(to.meta.requiredRole) ? to.meta.requiredRole : [];

  if (requiredRoles.length > 0 && !requiredRoles.includes(user.role)) {
    document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
    next({ name: "Unauthorized" });
  } else {
    document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
    next();
  }
  */
}
