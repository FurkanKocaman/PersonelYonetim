<script setup lang="ts">
import { ref, onMounted, watch, nextTick } from "vue";
import { useRouter, useRoute } from "vue-router";
import SidebarMenu from "@/components/dashboard/SidebarMenu.vue";
import TopBar from "../components/dashboard/TopBar.vue";
import type { MenuItem } from "@/types/menu";
import { useUserStore } from "@/stores/user";
import type { UserModel } from "@/models/UserModel";

// Router
const router = useRouter();
const route = useRoute();

// Kenar çubuğu durumu
const sidebarOpen = ref(true);

// Yükleniyor durumu
const isLoading = ref(false);

// Menü öğeleri
const menuItems = ref<MenuItem[]>([
  {
    name: "Ana Sayfa",
    icon: "home",
    active: false,
    path: "/dashboard",
  },
  {
    name: "Sirket",
    icon: "building",
    active: false,
    path: "/dashboard/sirket",
  },
  {
    name: "Personel",
    icon: "users",
    active: false,
    path: "/dashboard/personel",
  },
  {
    name: "İzin Yönetimi",
    icon: "calendar-alt",
    active: false,
    path: "/dashboard/izin",
  },
  {
    name: "Maaş Yönetimi",
    icon: "money-bill-wave",
    active: false,
    path: "/dashboard/maas",
  },
  {
    name: "Takvim",
    icon: "calendar-alt",
    active: false,
    path: "/dashboard/takvim",
  },
  {
    name: "Ayarlar",
    icon: "cog",
    active: false,
    path: "/dashboard/ayarlar",
  },
]);

const userStore = useUserStore();

const user: UserModel = userStore.user;

const toggleSidebar = () => {
  sidebarOpen.value = !sidebarOpen.value;
};

const handleMenuClick = (item: MenuItem) => {
  if (route.path !== item.path) {
    isLoading.value = true;
    router.push(item.path);
  }
};

// Mevcut rotaya göre aktif menü öğesini güncelle
const updateActiveMenuItem = () => {
  // Tüm öğeleri sıfırla
  menuItems.value.forEach((item) => {
    item.active = false;
  });

  // En spesifik eşleşen rotayı bul
  let bestMatch: MenuItem | null = null;
  let bestMatchLength = 0;

  menuItems.value.forEach((item) => {
    if (route.path.startsWith(item.path) && item.path.length > bestMatchLength) {
      bestMatch = item;
      bestMatchLength = item.path.length;
    }
  });

  // En iyi eşleşmeyi aktif olarak ayarla
  if (bestMatch) {
    (bestMatch as MenuItem).active = true;
  }
  // Eşleşme bulunamazsa ve dashboard kökündeysek, dashboard öğesini etkinleştir
  else if (route.path === "/dashboard") {
    const dashboardItem = menuItems.value.find((item) => item.path === "/dashboard");
    if (dashboardItem) {
      (dashboardItem as MenuItem).active = true;
    }
  }
};

// Aktif menü öğesini güncellemek için rota değişikliklerini izle
watch(
  () => route.path,
  () => {
    updateActiveMenuItem();
  },
  { immediate: true }
);

// Bileşen bağlandığında başlat
onMounted(() => {
  userStore.getUser();
  updateActiveMenuItem();

  // Gezinme olay dinleyicilerini ekle
  router.beforeEach(() => {
    isLoading.value = true;
    return true;
  });

  router.afterEach(async () => {
    // Yükleme göstergesini gizlemeden önce DOM'un güncellenmesini bekle
    await nextTick();
    setTimeout(() => {
      isLoading.value = false;
    }, 300);
  });
});
</script>

<template>
  <div class="flex h-screen bg-gray-100 dark:bg-neutral-900">
    <!-- Kenar Çubuğu -->
    <SidebarMenu
      :menuItems="menuItems"
      :user="user"
      :sidebarOpen="sidebarOpen"
      @menu-click="handleMenuClick"
      @toggle-sidebar="toggleSidebar"
    />

    <!-- Ana İçerik -->
    <div
      class="flex-1 flex flex-col transition-all duration-300"
      :class="{
        'lg:ml-50': sidebarOpen,
        'lg:ml-20': !sidebarOpen,
      }"
    >
      <!-- Üst Çubuk -->
      <TopBar :sidebarOpen="sidebarOpen" :header="route.path" @toggle-sidebar="toggleSidebar" />

      <!-- İçerik Alanı -->
      <div
        class="flex-1 overflow-x-hidden overflow-y-auto bg-gray-100 dark:bg-neutral-900 relative"
      >
        <!-- Yükleme Göstergesi -->
        <div
          v-if="isLoading"
          class="absolute inset-0 flex flex-col items-center justify-center bg-white bg-opacity-80 dark:bg-neutral-900 dark:bg-opacity-80 z-50"
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
</template>

<style scoped>
/* Sayfa değişimleri için geçişler */
.page-enter-active,
.page-leave-active {
  transition: opacity 0.2s ease;
}

.page-enter-from,
.page-leave-to {
  opacity: 0;
}
</style>
