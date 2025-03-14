<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();

// Navigate back to dashboard
const goToDashboard = () => {
  router.push('/dashboard');
};

// Navigate back to previous page
const goBack = () => {
  window.history.back();
};
</script>

<template>
  <div class="min-h-screen flex flex-col items-center justify-center bg-gray-100 dark:bg-neutral-900 p-4">
    <div class="text-center">
      <h1 class="text-9xl font-bold text-sky-600">404</h1>
      <h2 class="text-3xl font-semibold text-gray-800 dark:text-white mt-4">Sayfa Bulunamadı</h2>
      <p class="text-gray-600 dark:text-gray-400 mt-2 mb-8">Aradığınız sayfa mevcut değil veya kaldırılmış olabilir.</p>
      
      <div class="flex flex-col sm:flex-row justify-center gap-4">
        <button 
          @click="goToDashboard" 
          class="bg-sky-600 hover:bg-sky-700 text-white font-bold py-3 px-6 rounded-lg flex items-center justify-center"
        >
          <i class="fas fa-home mr-2"></i> Ana Sayfaya Dön
        </button>
        
        <button 
          @click="goBack" 
          class="bg-gray-600 hover:bg-gray-700 text-white font-bold py-3 px-6 rounded-lg flex items-center justify-center"
        >
          <i class="fas fa-arrow-left mr-2"></i> Geri Dön
        </button>
      </div>
    </div>
    
    <div class="mt-12 text-gray-500 dark:text-gray-400">
      <p>Personel Yönetim Sistemi &copy; 2025</p>
    </div>
  </div>
</template>

<style scoped>
/* Custom animations could be added here if desired */
</style>
