<script setup lang="ts">
import { useThemeStore } from "@/stores/ThemeStore";
import { defineProps, defineEmits, onMounted } from "vue";

// Prop'lar
const props = defineProps({
  sidebarOpen: {
    type: Boolean,
    default: true,
  },
  header: {
    type: String,
    default: "",
  },
});

const themeStore = useThemeStore();
onMounted(() => {
  const theme: string = themeStore.theme;
});

// Event'ler
const emit = defineEmits(["toggle-sidebar"]);

// Kenar çubuğunu aç/kapat
const toggleSidebar = () => {
  emit("toggle-sidebar");
};
</script>

<template>
  <header class="w-full bg-white dark:bg-neutral-800/60 shadow-sm h-16 flex items-center">
    <div class="container mx-auto px-4 flex justify-between items-center">
      <!-- Sol taraf - Mobil için açma/kapama düğmesi -->
      <div class="flex items-center">
        <button
          @click="toggleSidebar"
          class="lg:hidden p-2 rounded-md text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 focus:outline-none"
        >
          <i class="fas fa-bars"></i>
        </button>
        {{ props.header }}
      </div>

      <!-- Sağ taraf - Kullanıcı işlemleri -->
      <div class="flex items-center space-x-4">
        <div class="cursor-pointer">
          <svg
            v-if="themeStore.theme == 'light'"
            class="size-7 fill-none"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
            v-on:click="themeStore.toggleTheme()"
          >
            <path
              d="M3.32031 11.6835C3.32031 16.6541 7.34975 20.6835 12.3203 20.6835C16.1075 20.6835 19.3483 18.3443 20.6768 15.032C19.6402 15.4486 18.5059 15.6834 17.3203 15.6834C12.3497 15.6834 8.32031 11.654 8.32031 6.68342C8.32031 5.50338 8.55165 4.36259 8.96453 3.32996C5.65605 4.66028 3.32031 7.89912 3.32031 11.6835Z"
              class="stroke-black stroke-2"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
          </svg>
          <svg
            v-if="themeStore.theme == 'dark'"
            class="size-7 fill-none"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
            v-on:click="themeStore.toggleTheme()"
          >
            <path
              d="M12 3V4M12 20V21M4 12H3M6.31412 6.31412L5.5 5.5M17.6859 6.31412L18.5 5.5M6.31412 17.69L5.5 18.5001M17.6859 17.69L18.5 18.5001M21 12H20M16 12C16 14.2091 14.2091 16 12 16C9.79086 16 8 14.2091 8 12C8 9.79086 9.79086 8 12 8C14.2091 8 16 9.79086 16 12Z"
              class="stroke-white stroke-2"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
          </svg>
        </div>
        <!-- Bildirimler -->
        <div class="relative">
          <button
            class="p-2 rounded-full text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 focus:outline-none"
          >
            <i class="fas fa-bell"></i>
            <span
              class="absolute top-0 right-0 bg-red-500 text-white rounded-full w-4 h-4 flex items-center justify-center text-xs"
              >3</span
            >
          </button>
        </div>

        <!-- Mesajlar -->
        <div class="relative">
          <button
            class="p-2 rounded-full text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 focus:outline-none"
          >
            <i class="fas fa-envelope"></i>
            <span
              class="absolute top-0 right-0 bg-blue-500 text-white rounded-full w-4 h-4 flex items-center justify-center text-xs"
              >2</span
            >
          </button>
        </div>

        <!-- Kullanıcı menüsü -->
        <div class="relative">
          <button
            class="flex items-center space-x-2 text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 rounded-full p-1 focus:outline-none"
          >
            <img
              src="https://randomuser.me/api/portraits/men/1.jpg"
              alt="Kullanıcı"
              class="w-8 h-8 rounded-full border border-gray-200 dark:border-neutral-600"
            />
          </button>
        </div>
      </div>
    </div>
  </header>
</template>

<style scoped>
/* Özel stiller için TopBar */
</style>
