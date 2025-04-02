<script setup lang="ts">
import type { UserModel } from "@/models/entity-models/UserModel";
import type { MenuItem } from "@/types/menu";
import { defineProps, defineEmits, ref, onMounted, computed } from "vue";
import { useRoute } from "vue-router";
import TakvimEtkinlikCreateModal from "../modals/TakvimEtkinlikCreateModal.vue";
import IzinTalepCreateModal from "../modals/IzinTalepCreateModal.vue";

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

const isAddMenuOpen = ref(false);

const isTakvimEtkinlik = ref(false);
const isIzinTalep = ref(false);

const closeTakvimEtkinlik = (state: boolean) => {
  isTakvimEtkinlik.value = state;
};
const closeIzinTalep = (state: boolean) => {
  isIzinTalep.value = state;
};

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
  if (isMobile.value) {
    toggleSidebar();
  }
};

// Sidebar toggle işlevi
const toggleSidebar = () => {
  emit("toggle-sidebar");
};

const isMobile = ref(false);

onMounted(() => {
  checkScreenWidth();
  window.addEventListener("resize", checkScreenWidth);

  if (isMobile.value) toggleSidebar();
});

const checkScreenWidth = () => {
  isMobile.value = window.innerWidth < 1024;
};

// Bir menü öğesinin aktif olup olmadığını mevcut rotaya göre kontrol et
const isMenuItemActive = (itemPath: string): boolean => {
  // Tam eşleşmeler için (örneğin dashboard ana sayfası)
  if (itemPath == route.path) {
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
  <div
    @click="
      () => {
        isAddMenuOpen = false;
      }
    "
  >
    <!-- Sidebar -->
    <aside
      class="h-[100dvh] inset-y-0 left-0 fixed xl:relative xl:flex flex-col z-10 bg-neutral-300 shadow-lg dark:bg-neutral-900 dark:shadow-neutral-800 shadow-neutral-300 transition-all duration-300 ease-in-out"
      :class="{
        'block w-[50dvw] lg:w-[20dvw] xl:w-[15dvw]': sidebarOpen,
        'md:w-[5dvw] hidden': !sidebarOpen,
      }"
    >
      <div class="h-full flex flex-col overflow-y-auto overflow-x-hidden">
        <div
          class="relative flex h-16 items-center justify-center border-b dark:border-neutral-600 border-neutral-400"
        >
          <h1
            class="my-3 text-xl font-semibold text-sky-600 transition-all duration-300 ease-in-out"
            :class="{ 'opacity-100': sidebarOpen, 'opacity-0 w-0': !sidebarOpen }"
          >
            Personel Yönetim
          </h1>
          <div
            class="text-xl font-semibold text-sky-600 transition-all duration-300 ease-in-out"
            :class="{ 'opacity-100': !sidebarOpen, 'opacity-0 w-0': sidebarOpen }"
          >
            <i class="fa-solid fa-list-check"></i>
          </div>
        </div>
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
              {{ user.pozisyonAd != "" ? user.pozisyonAd : user.role }}
            </p>
          </div>
        </div>

        <div class="mt-4 flex-grow">
          <div
            class="flex items-center group mt-2 text-gray-700 dark:text-gray-300 rounded-lg cursor-pointer transition-colors duration-200 hover:bg-sky-100 dark:hover:bg-neutral-700 relative group"
            :class="{
              'px-1 py-2 mx-2': sidebarOpen,
              'px-0 py-3 justify-center mx-3': !sidebarOpen,
            }"
            @click.stop="
              () => {
                isAddMenuOpen = !isAddMenuOpen;
              }
            "
          >
            <div
              class="w-8 h-8 flex items-center justify-center text-lg"
              :class="{ 'mr-3': sidebarOpen, 'mx-auto': !sidebarOpen }"
            >
              <i class="text-2xl text-sky-600" :class="`fas fa-square-plus`"></i>
            </div>
            <!-- Metin - Yalnızca Sidebar Açıkken Görünür -->
            <span
              class="whitespace-nowrap overflow-hidden transition-all duration-300 text-sm font-semibold text-sky-600"
              :class="{ 'opacity-100': sidebarOpen, 'opacity-0 w-0': !sidebarOpen }"
            >
              Talep
            </span>
          </div>
          <ul
            class="fixed left-20 top-40 bg-neutral-200 dark:bg-neutral-700 text-neutral-700 rounded-md text-sm py-2 font-medium z-50 transition-all duration-300"
            :class="sidebarOpen ? 'left-56 lg:left-54 xl:left-60' : 'left-20'"
            v-if="isAddMenuOpen"
          >
            <li
              class="dark:hover:bg-neutral-800 hover:bg-neutral-300 px-8 py-3 rounded-md flex cursor-pointer"
              @click="
                () => {
                  isIzinTalep = true;
                  isAddMenuOpen = false;
                }
              "
            >
              <svg
                class="size-5 fill-neutral-700 dark:fill-neutral-300 mr-2"
                viewBox="0 0 32 32"
                version="1.1"
                xmlns="http://www.w3.org/2000/svg"
              >
                <title>plane</title>
                <path
                  d="M12.25 8.719l-0.469 0.469 4 0.344 2.406-2.406c0.375-0.375 1.063-0.938 2.063-1.563s1.656-0.813 1.906-0.563c0.688 0.688 0 2.031-2.063 4.063l-2.375 2.375 0.344 4 0.469-0.438c0.375-0.406 0.781-0.406 1.094-0.094 0.375 0.375 0.219 0.906-0.531 1.656l-0.75 0.781 0.375 2.688 0.531-0.563c0.344-0.375 0.719-0.375 1.063-0.063 0.375 0.375 0.313 0.844-0.188 1.344l-1.125 1.125c0.438 3 0.5 4.688 0.156 5.031-0.156 0.125-0.375 0.25-0.688 0.313-0.281 0.031-0.531 0-0.688-0.125-0.125-0.156-0.313-0.719-0.5-1.688-0.219-1-0.75-2.688-1.688-5.063-0.906-2.406-1.563-3.75-1.875-4.094-0.125-0.125-0.313-0.125-0.531 0-0.219 0.094-1 0.75-2.25 1.969-1.25 1.25-2.406 2.25-3.5 3.031-0.031 0.188 0.063 0.813 0.219 1.844s0.219 1.719 0.188 1.969c-0.063 0.25-0.313 0.594-0.719 1-0.219 0.219-0.375 0.344-0.531 0.375-0.406-0.406-1.031-1.719-1.875-3.875-2.344-1.031-3.625-1.688-3.875-1.938 0-0.125 0.125-0.313 0.344-0.531 0.406-0.406 0.75-0.656 1.031-0.688 0.25-0.063 0.906 0.031 1.938 0.219 1.031 0.156 1.656 0.219 1.875 0.219 0.75-1.125 2.156-2.781 4.344-4.969 0.625-0.688 0.813-1.125 0.594-1.344-0.313-0.344-1.688-0.969-4.063-1.875-2.406-0.906-4.063-1.5-5.063-1.688-1-0.219-1.563-0.375-1.688-0.531-0.156-0.125-0.188-0.375-0.125-0.688 0.031-0.313 0.156-0.531 0.313-0.688 0.313-0.313 2-0.281 5.031 0.156l1.094-1.125c0.5-0.469 0.969-0.531 1.344-0.156 0.344 0.313 0.313 0.688-0.031 1.031l-0.563 0.531 2.688 0.375 0.75-0.719c0.75-0.75 1.281-0.906 1.656-0.531 0.344 0.344 0.313 0.688-0.063 1.094z"
                ></path>
              </svg>
              İzin
            </li>
            <li
              class="dark:hover:bg-neutral-800 hover:bg-neutral-300 px-8 py-3 rounded-md flex cursor-pointer"
            >
              <svg
                class="size-5 mr-2"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <rect
                  x="3"
                  y="6"
                  width="18"
                  height="13"
                  rx="2"
                  class="dark:stroke-neutral-300 stroke-neutral-700"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                />
                <path
                  d="M3 10H20.5"
                  class="stroke-neutral-700 dark:stroke-neutral-300"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                />
                <path
                  d="M7 15H9"
                  class="stroke-neutral-700 dark:stroke-neutral-300"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                />
              </svg>
              Harcama
            </li>
            <li
              class="dark:hover:bg-neutral-800 hover:bg-neutral-300 px-8 py-3 rounded-md flex cursor-pointer"
            >
              <svg
                class="size-5 mr-2"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M23 12C23 12.3545 22.9832 12.7051 22.9504 13.051C22.3838 12.4841 21.7204 12.014 20.9871 11.6675C20.8122 6.85477 16.8555 3.00683 12 3.00683C7.03321 3.00683 3.00683 7.03321 3.00683 12C3.00683 16.8555 6.85477 20.8122 11.6675 20.9871C12.014 21.7204 12.4841 22.3838 13.051 22.9504C12.7051 22.9832 12.3545 23 12 23C5.92487 23 1 18.0751 1 12C1 5.92487 5.92487 1 12 1C18.0751 1 23 5.92487 23 12Z"
                  class="dark:fill-neutral-300 fill-neutral-700"
                />
                <path
                  d="M13 11.8812L13.8426 12.3677C13.2847 12.7802 12.7902 13.2737 12.3766 13.8307L11.5174 13.3346C11.3437 13.2343 11.2115 13.0898 11.1267 12.9235C11 12.7274 11 12.4667 11 12.4667V6C11 5.44771 11.4477 5 12 5C12.5523 5 13 5.44772 13 6V11.8812Z"
                  class="dark:fill-neutral-300 fill-neutral-700"
                />
                <path
                  d="M18 15C17.4477 15 17 15.4477 17 16V17H16C15.4477 17 15 17.4477 15 18C15 18.5523 15.4477 19 16 19H17V20C17 20.5523 17.4477 21 18 21C18.5523 21 19 20.5523 19 20V19H20C20.5523 19 21 18.5523 21 18C21 17.4477 20.5523 17 20 17H19V16C19 15.4477 18.5523 15 18 15Z"
                  class="dark:fill-neutral-300 fill-neutral-700"
                />
                <path
                  fill-rule="evenodd"
                  clip-rule="evenodd"
                  d="M18 24C21.3137 24 24 21.3137 24 18C24 14.6863 21.3137 12 18 12C14.6863 12 12 14.6863 12 18C12 21.3137 14.6863 24 18 24ZM18 22.0181C15.7809 22.0181 13.9819 20.2191 13.9819 18C13.9819 15.7809 15.7809 13.9819 18 13.9819C20.2191 13.9819 22.0181 15.7809 22.0181 18C22.0181 20.2191 20.2191 22.0181 18 22.0181Z"
                  class="dark:fill-neutral-300 fill-neutral-700"
                />
              </svg>
              Fazla Mesai
            </li>
            <li
              class="dark:hover:bg-neutral-800 hover:bg-neutral-300 px-8 py-3 rounded-md flex cursor-pointer"
              @click="
                () => {
                  isTakvimEtkinlik = true;
                  isAddMenuOpen = false;
                }
              "
            >
              <svg
                class="size-5 mr-2 dark:fill-neutral-300 fill-neutral-700"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
                data-name="Layer 1"
              >
                <path
                  d="M7,10H9A1,1,0,0,0,9,8H7a1,1,0,0,0,0,2ZM21,4H13V3a1,1,0,0,0-2,0V4H3A1,1,0,0,0,2,5V15a3,3,0,0,0,3,3H9.59l-2.3,2.29a1,1,0,0,0,0,1.42,1,1,0,0,0,1.42,0L11,19.41V21a1,1,0,0,0,2,0V19.41l2.29,2.3a1,1,0,0,0,1.42,0,1,1,0,0,0,0-1.42L14.41,18H19a3,3,0,0,0,3-3V5A1,1,0,0,0,21,4ZM20,15a1,1,0,0,1-1,1H5a1,1,0,0,1-1-1V6H20ZM7,14h6a1,1,0,0,0,0-2H7a1,1,0,0,0,0,2Z"
                />
              </svg>
              Etkinlik
            </li>
          </ul>
          <div
            v-for="item in filteredMenuItems"
            :key="item.name"
            @click="handleMenuClick(item)"
            class="flex items-center mt-2 text-gray-700 dark:text-gray-300 rounded-lg cursor-pointer transition-colors duration-200 hover:bg-gray-200 dark:hover:bg-neutral-700 relative group"
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
              <i class="text-base" :class="`fas fa-${item.icon}`"></i>
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
              class="fixed left-12 ml-6 px-2 py-1 bg-gray-800 text-white text-sm rounded opacity-0 invisible group-hover:opacity-100 group-hover:visible transition-opacity duration-300 whitespace-nowrap"
            >
              {{ item.name }}
            </div>
          </div>
        </div>

        <!-- Sidebar Toggle Butonu - Sidebar'ın Ortasında Pozisyonlandırılmış -->
        <div class="absolute inset-y-12 -right-0 flex items-start justify-start">
          <button
            @click="toggleSidebar"
            class="py-3 px-2 flex items-center justify-center bg-white dark:bg-neutral-800 text-gray-600 dark:text-gray-300 rounded-sm shadow-md hover:bg-gray-100 dark:hover:bg-neutral-700 transition-colors duration-200 focus:outline-none"
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
  <TakvimEtkinlikCreateModal v-if="isTakvimEtkinlik" @close-modal="closeTakvimEtkinlik" />
  <IzinTalepCreateModal v-if="isIzinTalep" @close-modal="closeIzinTalep" />
</template>

<style scoped>
/* Mobil görünüm için ek stiller */
</style>
