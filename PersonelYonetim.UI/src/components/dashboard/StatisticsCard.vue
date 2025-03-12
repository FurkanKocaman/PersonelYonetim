<script setup lang="ts">
import { defineProps } from 'vue';

// İstatistik öğesi için tip tanımı
interface StatItem {
  title: string;
  value: number;
  icon: string;
  color: string;
}

// Komponent prop'ları
const props = defineProps({
  stats: {
    type: Array as () => StatItem[],
    required: true
  }
});
</script>

<template>
  <!-- İstatistik Kartları -->
  <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-6">
    <div v-for="stat in stats" :key="stat.title" 
         class="bg-white dark:bg-neutral-800 rounded-lg shadow-md overflow-hidden hover-scale">
      <div class="flex items-center p-4">
        <div :class="`${stat.color} text-white p-3 rounded-lg`">
          <i :class="`fas fa-${stat.icon} text-xl`"></i>
        </div>
        <div class="ml-4">
          <h2 class="text-sm font-medium text-gray-600 dark:text-gray-400">{{ stat.title }}</h2>
          <p class="text-2xl font-semibold text-gray-800 dark:text-white">{{ stat.value }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Kart animasyonları için stiller */
.hover-scale {
  transition: all 0.2s ease;
}
.hover-scale:hover {
  transform: scale(1.05);
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
}
</style>
