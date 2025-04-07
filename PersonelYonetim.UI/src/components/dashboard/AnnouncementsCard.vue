<script setup lang="ts">
import type { DuyuruModel } from "@/models/entity-models/DuyuruModel";
import { defineProps } from "vue";

const props = defineProps({
  duyurular: {
    type: Array as () => DuyuruModel[],
    required: true,
  },
});
</script>

<template>
  <div
    class="lg:col-span-1 bg-white dark:bg-neutral-800 rounded-lg shadow-md overflow-hidden hover-card"
  >
    <div class="px-6 py-4 bg-purple-600 dark:bg-purple-700">
      <h3 class="text-lg font-semibold text-white">Duyurular</h3>
      <p class="text-purple-100 text-sm">Güncel duyurular ve bildirimler</p>
    </div>
    <div class="p-6">
      <div v-if="duyurular.length === 0" class="text-center py-4 text-gray-500 dark:text-gray-400">
        Henüz duyuru bulunmamaktadır.
      </div>
      <div v-else class="space-y-4">
        <div
          v-for="duyuru in props.duyurular"
          :key="duyuru.id"
          class="border-b border-gray-200 dark:border-gray-700 pb-4 last:border-b-0 last:pb-0 hover-item"
        >
          <div class="flex justify-between items-start">
            <h4 class="text-md font-medium text-gray-800 dark:text-white">{{ duyuru.baslik }}</h4>
            <span class="text-xs text-gray-500 dark:text-gray-400">{{
              new Date(duyuru.createdAt).toLocaleString("tr-TR", {
                day: "2-digit",
                month: "2-digit",
                year: "numeric",
                hour: "2-digit",
                minute: "2-digit",
                hour12: false,
              })
            }}</span>
          </div>
          <p class="mt-2 text-sm text-gray-600 dark:text-gray-300">{{ duyuru.aciklama }}</p>
        </div>
      </div>
      <div class="mt-6 flex justify-center">
        <RouterLink
          to="/dashboard/duyurular"
          class="px-4 py-2 bg-purple-600 hover:bg-purple-700 text-white rounded-lg transition-colors duration-200 hover-button"
        >
          Tüm Duyurular
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Kart hover efekti */
.hover-card {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}
.hover-card:hover {
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
}

/* Duyuru öğesi hover efekti */
.hover-item {
  transition: all 0.2s ease;
  padding: 0.5rem;
  border-radius: 0.375rem;
}
.hover-item:hover {
  background-color: rgba(0, 0, 0, 0.02);
  transform: translateX(2px);
}

/* Buton hover efekti */
.hover-button {
  transition: all 0.2s ease;
}
.hover-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
}
</style>
