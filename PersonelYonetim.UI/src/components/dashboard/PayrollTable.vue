<script setup lang="ts">
import { defineProps } from 'vue';

// Bordro öğesi için tip tanımı
interface PayrollItem {
  id: number;
  name: string;
  department: string;
  status: string;
  date: string;
}

// Komponent prop'ları
const props = defineProps({
  payrollItems: {
    type: Array as () => PayrollItem[],
    required: true
  }
});
</script>

<template>
  <div class="lg:col-span-2 bg-white dark:bg-neutral-800 rounded-lg shadow-md overflow-hidden hover-card">
    <div class="px-6 py-4 bg-sky-600 dark:bg-sky-700">
      <h3 class="text-lg font-semibold text-white">Bordronun artık kolayı var!</h3>
      <p class="text-sky-100 text-sm">Modern ekranlar, hatasız hesaplama yeteneği ve güncel mevzuatlara uyumlu yapısıyla bordro operasyonunu ortadan kaldırıyor.</p>
    </div>
    <div class="p-6">
      <div class="overflow-x-auto">
        <table class="min-w-full">
          <thead>
            <tr class="text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700">
              <th class="px-4 py-3">Personel</th>
              <th class="px-4 py-3">Departman</th>
              <th class="px-4 py-3">Durum</th>
              <th class="px-4 py-3">Tarih</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-200 dark:divide-gray-700">
            <tr v-for="item in payrollItems" :key="item.id" class="hover-row">
              <td class="px-4 py-3 whitespace-nowrap">
                <div class="font-medium text-gray-800 dark:text-white">{{ item.name }}</div>
              </td>
              <td class="px-4 py-3 whitespace-nowrap text-gray-600 dark:text-gray-300">
                {{ item.department }}
              </td>
              <td class="px-4 py-3 whitespace-nowrap">
                <span class="px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full" 
                      :class="{
                        'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200': item.status === 'Onaylandı',
                        'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200': item.status === 'Beklemede',
                        'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200': item.status === 'İşleniyor'
                      }">
                  {{ item.status }}
                </span>
              </td>
              <td class="px-4 py-3 whitespace-nowrap text-gray-600 dark:text-gray-300">
                {{ item.date }}
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="mt-6 flex justify-center">
        <button class="px-4 py-2 bg-sky-600 hover:bg-sky-700 text-white rounded-lg transition-colors duration-200 hover-button">
          Hemen İnceleyin
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Tablo satır hover efekti için stiller */
.hover-row {
  transition: background-color 0.2s ease;
}
.hover-row:hover {
  background-color: rgba(0, 0, 0, 0.03);
}

/* Kart hover efekti */
.hover-card {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}
.hover-card:hover {
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
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
