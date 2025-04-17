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
  const roleClaims: string[] = Array.isArray(to.meta.roleClaims) ? to.meta.roleClaims : [];

  if (!user?.id || user.id === "") {
    document.title = "Giriş Yap | Personel Yönetim Sistemi";
    return next({ name: "login" });
  }
  const set = new Set(user.roleClaims);
  if (roleClaims.length > 0 && !roleClaims.some((item) => set.has(item))) {
    document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
    return next({ name: "Unauthorized" });
  }

  document.title = `${to.meta.title || "Personel Yönetim"} | Personel Yönetim Sistemi`;
  return next();
}
