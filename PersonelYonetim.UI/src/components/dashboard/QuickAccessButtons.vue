<script setup lang="ts">
import { ref } from 'vue';

// Hızlı erişim butonları için veriler
const quickAccessButtons = ref([
  { id: 1, title: 'Personel Ekle', icon: 'user-plus', color: 'bg-blue-500 hover:bg-blue-600' },
  { id: 2, title: 'Bordro Oluştur', icon: 'file-invoice-dollar', color: 'bg-green-500 hover:bg-green-600' },
  { id: 3, title: 'İzin Talebi', icon: 'calendar-plus', color: 'bg-purple-500 hover:bg-purple-600' },
  { id: 4, title: 'Rapor Oluştur', icon: 'chart-line', color: 'bg-amber-500 hover:bg-amber-600' },
  { id: 5, title: 'Mesai Kaydet', icon: 'clock', color: 'bg-red-500 hover:bg-red-600' },
  { id: 6, title: 'Eğitim Planla', icon: 'graduation-cap', color: 'bg-indigo-500 hover:bg-indigo-600' }
]);
</script>

<template>
  <div class="mt-6">
    <h3 class="text-xl font-semibold text-gray-800 dark:text-white mb-4">Hızlı Erişim</h3>
    <div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4">
      <div v-for="button in quickAccessButtons" :key="button.id" 
           :class="`${button.color} rounded-lg shadow-md overflow-hidden hover-button`">
        <div class="p-4 flex flex-col items-center justify-center text-white h-full min-h-[100px]">
          <div class="text-2xl mb-2">
            <i :class="`fas fa-${button.icon}`"></i>
          </div>
          <span class="text-sm font-medium text-center">{{ button.title }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Buton hover efekti */
.hover-button {
  transition: all 0.3s ease;
  cursor: pointer;
}
.hover-button:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
}
</style>
