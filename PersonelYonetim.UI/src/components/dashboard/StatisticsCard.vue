<script setup lang="ts">
import { defineProps } from "vue";

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
    required: true,
  },
});
</script>

<template>
  <!-- İstatistik Kartları -->
  <div class="w-full grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-6">
    <div
      v-for="stat in props.stats"
      :key="stat.title"
      class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-4 flex items-center"
    >
      <div class="rounded-full w-12 h-12 flex items-center justify-center mr-4" :class="stat.color">
        <i class="fas text-white text-xl" :class="'fa-' + stat.icon"></i>
      </div>
      <div>
        <h3 class="text-lg font-semibold text-gray-800 dark:text-gray-200">
          {{ stat.value }}
        </h3>
        <p class="text-sm text-gray-600 dark:text-gray-400">{{ stat.title }}</p>
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
