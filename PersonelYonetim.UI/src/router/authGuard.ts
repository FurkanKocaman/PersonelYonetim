import type { UserModel } from "@/models/UserModel";
import { useUserStore } from "@/stores/user";
import type { NavigationGuardNext, RouteLocationNormalized } from "vue-router";

export async function authGuard(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  await useUserStore().getUser();
  const user: UserModel = useUserStore().user;

  const requiredRoles: number[] = Array.isArray(to.meta.requiredRole) ? to.meta.requiredRole : [];
  console.log(typeof user.role);
  console.log(requiredRoles);
  console.log(requiredRoles.includes(user.role, 0));

  if (requiredRoles.length > 0 && !requiredRoles.includes(user.role)) {
    document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
    next({ name: "Unauthorized" });
  } else {
    document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
    next();
  }
}
