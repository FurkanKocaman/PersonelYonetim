<script setup lang="ts">
import { ref, onMounted, nextTick } from "vue";
import { useRouter, useRoute } from "vue-router";
import SidebarMenu from "@/components/dashboard/SidebarMenu.vue";
import TopBar from "../components/dashboard/TopBar.vue";
import type { MenuItem } from "@/types/menu";
import { useUserStore } from "@/stores/user";
import RoleClaims from "@/models/RoleClaims";
import type { PersonelItem } from "@/models/PersonelModels";

const router = useRouter();
const route = useRoute();

const sidebarOpen = ref(true);
const isUserLoaded = ref(false);
const isLoading = ref(false);

const menuItems = ref<MenuItem[]>([
  {
    name: "Ana Sayfa",
    icon: "home",
    active: false,
    path: "/dashboard",
  },
  {
    name: "Şirket Yönetimi",
    icon: "building",
    active: false,
    path: "/dashboard/sirket",
    roleClaims: [RoleClaims.viewKurumsalYapi],
  },
  {
    name: "Personel",
    icon: "users",
    active: false,
    path: "/dashboard/personel",
    roleClaims: [RoleClaims.viewPersonel],
  },
  {
    name: "İzin Yönetimi",
    icon: "calendar-alt",
    active: false,
    path: "/dashboard/izin",
    roleClaims: [RoleClaims.approveIzinler],
  },
  {
    name: "Bordro",
    icon: "money-bill-wave",
    active: false,
    path: "/dashboard/bordro/calisanlar",
  },
  {
    name: "Takvim",
    icon: "calendar-alt",
    active: false,
    path: "/dashboard/takvim",
  },
  // {
  //   name: "Ayarlar",
  //   icon: "cog",
  //   active: false,
  //   path: "/dashboard/ayarlar",
  // },
  {
    name: "Profil",
    icon: "user",
    active: false,
    path: "/dashboard/profile",
  },
]);

const userStore = useUserStore();
const user = ref<PersonelItem | undefined>(undefined);

const toggleSidebar = () => {
  sidebarOpen.value = !sidebarOpen.value;
};

const handleMenuClick = (item: MenuItem) => {
  if (route.path !== item.path) {
    isLoading.value = true;
    router.push(item.path);
  }
};

onMounted(async () => {
  isLoading.value = true;
  await userStore.getUser();
  user.value = userStore.user;
  isUserLoaded.value = true;
  isLoading.value = false;

  router.beforeEach(() => {
    isLoading.value = true;
    return true;
  });

  router.afterEach(async () => {
    await nextTick();
    setTimeout(() => {
      isLoading.value = false;
    }, 300);
  });
});
</script>

<template>
  <div v-if="isUserLoaded" class="flex h-[100dvh] w-[100dvw] bg-gray-100 dark:bg-neutral-900">
    <SidebarMenu
      :menuItems="menuItems"
      :user="user!"
      :sidebarOpen="sidebarOpen"
      @menu-click="handleMenuClick"
      @toggle-sidebar="toggleSidebar"
    />

    <div
      class="w-full flex flex-col transition-all duration-300 bg-neutral-50 dark:bg-neutral-900"
      :class="sidebarOpen ? ' max-w-[100dvw] xl:max-w-[85dvw]' : 'xl:max-w-[95dvw]'"
    >
      <TopBar :sidebarOpen="sidebarOpen" :header="route.path" @toggle-sidebar="toggleSidebar" />

      <div class="flex-1 overflow-x-hidden overflow-y-auto relative">
        <div
          v-if="isLoading"
          class="absolute inset-0 flex flex-col items-center justify-center bg-opacity-80 dark:bg-opacity-80 z-50"
        >
          <div
            class="w-16 h-16 border-4 border-sky-600 border-t-transparent rounded-full animate-spin"
          ></div>
          <p class="mt-4 text-gray-700 dark:text-gray-300 font-medium">Yükleniyor...</p>
        </div>

        <!-- Ana İçerik -->
        <router-view v-slot="{ Component }">
          <component :is="Component" :key="route.fullPath" />
        </router-view>
      </div>
    </div>
  </div>
  <div
    v-else
    class="absolute inset-0 flex flex-col items-center justify-center bg-opacity-80 dark:bg-opacity-80 z-50"
  >
    <div
      class="w-16 h-16 border-4 border-sky-600 border-t-transparent rounded-full animate-spin"
    ></div>
    <p class="mt-4 text-gray-700 dark:text-gray-300 font-medium">Yükleniyor...</p>
  </div>
</template>

<style scoped>
.page-enter-active,
.page-leave-active {
  transition: opacity 0.2s ease;
}

.page-enter-from,
.page-leave-to {
  opacity: 0;
}
</style>
