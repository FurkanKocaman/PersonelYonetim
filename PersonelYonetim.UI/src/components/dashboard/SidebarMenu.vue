<script setup lang="ts">
import type { UserModel } from "@/models/UserModel";
import type { MenuItem } from "@/types/menu";
import { defineProps, defineEmits, ref, onMounted, computed } from "vue";
import { useRoute } from "vue-router";

// Menü öğesi için tip tanımı

// Component prop'ları
const props = defineProps({
  menuItems: {
    type: Array as () => MenuItem[],
    required: true,
  },
  user: {
    type: Object as () => UserModel,
    required: true,
  },
  sidebarOpen: {
    type: Boolean,
    default: true,
  },
});

const filteredMenuItems = computed(() => {
  return props.menuItems.filter((item) => {
    if (!item.roles) return true;

    return item.roles.includes(props.user.role);
  });
});

// Komponent event'leri
const emit = defineEmits(["menu-click", "toggle-sidebar"]);

// Mevcut rota
const route = useRoute();

// Menü öğesi tıklama işlevi
const handleMenuClick = (item: MenuItem) => {
  emit("menu-click", item);
};

// Sidebar toggle işlevi
const toggleSidebar = () => {
  emit("toggle-sidebar");
};

const isMobile = ref(false);

onMounted(() => {
  checkScreenWidth();
  console.log(props.user);
  window.addEventListener("resize", checkScreenWidth);
});

const checkScreenWidth = () => {
  isMobile.value = window.innerWidth < 1024;
};

// Bir menü öğesinin aktif olup olmadığını mevcut rotaya göre kontrol et
const isMenuItemActive = (itemPath: string): boolean => {
  // Tam eşleşmeler için (örneğin dashboard ana sayfası)
  if (itemPath === route.path) {
    return true;
  }

  // İç içe rotalar için (örneğin /dashboard/personel)
  if (itemPath !== "/dashboard" && route.path.startsWith(itemPath)) {
    return true;
  }

  return false;
};
</script>

<template>
  <div>
    <!-- Mobil görünüm için overlay -->
    <div
      v-if="sidebarOpen && isMobile"
      class="lg:hidden inset-0 z-20 bg-black bg-opacity-50 sidebar-overlay"
      @click="toggleSidebar"
    ></div>

    <!-- Sidebar -->
    <aside
      class="h-full inset-y-0 left-0 z-30 flex flex-col bg-white dark:bg-neutral-800 shadow-lg transition-all duration-300 ease-in-out"
      :class="{
        'w-50': sidebarOpen,
        'w-20': !sidebarOpen,
        'translate-x-0': sidebarOpen || !isMobile,
        '-translate-x-full': !sidebarOpen && isMobile,
      }"
    >
      <div class="h-full flex flex-col overflow-y-auto overflow-x-hidden">
        <!-- Kullanıcı Profili -->
        <div class="flex flex-col items-center mt-4 -mx-2" :class="{ 'px-2': !sidebarOpen }">
          <img
            class="object-cover mx-2 rounded-full border-2 border-sky-500"
            :class="{ 'w-16 h-16': sidebarOpen, 'w-10 h-10': !sidebarOpen }"
            :src="user.profilResimUrl"
            alt="Avatar"
          />
          <div :class="{ hidden: !sidebarOpen }">
            <h4 class="mx-2 mt-2 font-medium text-gray-800 dark:text-gray-200">
              {{ user.fullName }}
            </h4>
            <!-- <p
              v-if="user.role == 'SirketSahibi'"
              class="mx-2 mt-1 text-sm font-medium text-gray-600 dark:text-gray-400"
            >
              Şirket Sahibi
            </p> -->
            <p class="mx-2 mt-1 text-sm font-medium text-gray-600 dark:text-gray-400">
              {{ user.pozisyonAd }}- {{ user.role }}
            </p>
          </div>
        </div>

        <div class="mt-4 flex-grow">
          <div
            v-for="item in filteredMenuItems"
            :key="item.name"
            @click="handleMenuClick(item)"
            class="flex items-center mt-2 text-gray-700 dark:text-gray-300 rounded-lg cursor-pointer transition-colors duration-200 hover:bg-gray-100 dark:hover:bg-neutral-700 relative group"
            :class="{
              'px-1 py-2 mx-2': sidebarOpen,
              'px-0 py-3 justify-center mx-3': !sidebarOpen,
              'bg-sky-100 dark:bg-sky-900 text-sky-600 dark:text-sky-400': isMenuItemActive(
                item.path
              ),
            }"
          >
            <div
              class="w-8 h-8 flex items-center justify-center text-lg"
              :class="{ 'mr-3': sidebarOpen, 'mx-auto': !sidebarOpen }"
            >
              <i class="size-5" :class="`fas fa-${item.icon}`"></i>
            </div>
            <!-- Metin - Yalnızca Sidebar Açıkken Görünür -->
            <span
              class="whitespace-nowrap overflow-hidden transition-all duration-300 text-sm font-semibold"
              :class="{ 'opacity-100': sidebarOpen, 'opacity-0 w-0': !sidebarOpen }"
            >
              {{ item.name }}
            </span>
            <div
              v-if="!sidebarOpen"
              class="absolute left-full ml-6 px-2 py-1 bg-gray-800 text-white text-sm rounded opacity-0 invisible group-hover:opacity-100 group-hover:visible transition-opacity duration-300 whitespace-nowrap z-50"
            >
              {{ item.name }}
            </div>
          </div>
        </div>

        <!-- Sidebar Toggle Butonu - Sidebar'ın Ortasında Pozisyonlandırılmış -->
        <div class="absolute inset-y-1/2 -right-0 flex items-center justify-center">
          <button
            @click="toggleSidebar"
            class="w-6 h-16 flex items-center justify-center bg-white dark:bg-neutral-800 text-gray-600 dark:text-gray-300 rounded-r-md shadow-md hover:bg-gray-100 dark:hover:bg-neutral-700 transition-colors duration-200 focus:outline-none"
          >
            <i
              class="fas"
              :class="{ 'fa-chevron-left': sidebarOpen, 'fa-chevron-right': !sidebarOpen }"
            ></i>
          </button>
        </div>
      </div>
    </aside>
  </div>
</template>

<style scoped>
/* Mobil görünüm için ek stiller */
@media (max-width: 1024px) {
  .sidebar-overlay {
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
  }
}
</style>
