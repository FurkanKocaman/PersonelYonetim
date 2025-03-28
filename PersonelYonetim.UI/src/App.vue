<script setup lang="ts">
import { onMounted, ref } from "vue";
import ToastComponent from "./components/ToastComponent.vue";

const isLoading = ref(true);

onMounted(() => {
  // Simulate initial loading

  setTimeout(() => {
    isLoading.value = false;
  }, 500);

  console.log("App mounted, router ready");
});
</script>

<template>
  <!-- Global Loading Screen (only shown on initial app load) -->
  <div
    v-if="isLoading"
    class="fixed inset-0 flex items-center justify-center bg-white dark:bg-neutral-900 z-50"
  >
    <div class="text-center">
      <div
        class="w-16 h-16 border-4 border-sky-600 border-t-transparent rounded-full animate-spin mx-auto"
      ></div>
      <p class="mt-4 text-gray-700 dark:text-gray-300 font-medium">
        Personel Yönetim Sistemi Yükleniyor...
      </p>
    </div>
  </div>

  <!-- Main App Container -->
  <div class="app-container">
    <RouterView v-slot="{ Component }">
      <transition name="fade" mode="out-in">
        <component :is="Component" />
      </transition>
    </RouterView>
  </div>
  <div class="fixed z-50 right-0 top-0 flex flex-col">
    <ToastComponent />
  </div>
</template>

<style>
/* Global Styles */
@import url("https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap");

:root {
  --primary-color: #0ea5e9;
  --primary-hover: #0284c7;
  --secondary-color: #64748b;
  --secondary-hover: #475569;
  --success-color: #10b981;
  --warning-color: #f59e0b;
  --danger-color: #ef4444;
  --info-color: #3b82f6;
}

body {
  font-family: "Inter", sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

/* Fade transition */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

/* Loading animation */
@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.animate-spin {
  animation: spin 1s linear infinite;
}
</style>
