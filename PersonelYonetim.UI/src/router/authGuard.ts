import type { PersonelItem } from "@/models/PersonelModels";
import { useUserStore } from "@/stores/user";
import type { NavigationGuardNext, RouteLocationNormalized } from "vue-router";

export async function authGuard(
  to: RouteLocationNormalized,
  from: RouteLocationNormalized,
  next: NavigationGuardNext
) {
  await useUserStore().getUser();
  const userStore = useUserStore();

  await userStore.getUser();

  const user: PersonelItem = userStore.user;
  const requiredRoles: number[] = Array.isArray(to.meta.requiredRole) ? to.meta.requiredRole : [];

  if (!user?.id || user.id === "") {
    document.title = "Giriş Yap | Personel Yönetim Sistemi";
    return next({ name: "login" });
  }
  if (requiredRoles.length > 0 && !requiredRoles.includes(user.role)) {
    document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
    return next({ name: "Unauthorized" });
  }

  document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
  return next();
}
