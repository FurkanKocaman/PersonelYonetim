<script setup lang="ts">
import { defineProps, defineEmits } from "vue";

// Prop'lar
const props = defineProps({
  sidebarOpen: {
    type: Boolean,
    default: true,
  },
});

// Event'ler
const emit = defineEmits(["toggle-sidebar"]);

// Kenar çubuğunu aç/kapat
const toggleSidebar = () => {
  emit("toggle-sidebar");
};
</script>

<template>
  <header class="bg-white dark:bg-neutral-800 shadow-sm h-16 flex items-center">
    <div class="container mx-auto px-4 flex justify-between items-center">
      <!-- Sol taraf - Mobil için açma/kapama düğmesi -->
      <div class="flex items-center">
        <button
          @click="toggleSidebar"
          class="lg:hidden p-2 rounded-md text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 focus:outline-none"
        >
          <i class="fas fa-bars"></i>
        </button>
      </div>

      <!-- Sağ taraf - Kullanıcı işlemleri -->
      <div class="flex items-center space-x-4">
        <!-- Bildirimler -->
        <div class="relative">
          <button class="p-2 rounded-full text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 focus:outline-none">
            <i class="fas fa-bell"></i>
            <span class="absolute top-0 right-0 bg-red-500 text-white rounded-full w-4 h-4 flex items-center justify-center text-xs">3</span>
          </button>
        </div>

        <!-- Mesajlar -->
        <div class="relative">
          <button class="p-2 rounded-full text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 focus:outline-none">
            <i class="fas fa-envelope"></i>
            <span class="absolute top-0 right-0 bg-blue-500 text-white rounded-full w-4 h-4 flex items-center justify-center text-xs">2</span>
          </button>
        </div>

        <!-- Kullanıcı menüsü -->
        <div class="relative">
          <button class="flex items-center space-x-2 text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 rounded-full p-1 focus:outline-none">
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
