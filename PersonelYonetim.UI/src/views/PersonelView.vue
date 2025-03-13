<script setup lang="ts">
import { ref } from "vue";

// interface PayrollItem {
//   id: number;
//   name: string;
//   department: string;
//   status: string;
//   date: string;
// }
// interface Announcement {
//   id: number;
//   title: string;
//   date: string;
//   content: string;
// }
// interface StatItem {
//   title: string;
//   value: number;
//   icon: string;
//   color: string;
// }
const payrollItems = ref([
  { id: 1, name: "Didem Deniz", department: "Pazarlama", status: "Aktif", date: "15.03.2025" },
  { id: 2, name: "Mehmet Kaya", department: "Finans", status: "Aktif", date: "16.03.2025" },
  { id: 3, name: "Ayşe Demir", department: "İK", status: "Pasif", date: "14.03.2025" },
  { id: 4, name: "Can Yılmaz", department: "Satış", status: "Aktif", date: "17.03.2025" },
  { id: 5, name: "Mehmet Yılmaz", department: "Satış", status: "Aktif", date: "17.03.2025" },
  { id: 6, name: "Salih Türk", department: "Satış", status: "Pasif", date: "17.03.2025" },
  { id: 7, name: "Mustafa Kara", department: "Satış", status: "Aktif", date: "17.03.2025" },
]);
</script>

<template>
  <div class="flex-1 transition-all duration-300">
    <header class="bg-white dark:bg-neutral-800 shadow-sm">
      <div class="py-4 px-6">
        <h1 class="text-2xl font-bold text-gray-800 dark:text-gray-200">Personel Paneli</h1>
      </div>
    </header>

    <main class="p-6">
      <!-- Sol Kolon: Bordro İşlemleri -->
      <div class="lg:col-span-2">
        <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 mb-6">
          <div class="p-6">
            <div class="overflow-x-auto">
              <table class="min-w-full">
                <thead>
                  <tr
                    class="text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700"
                  >
                    <th class="px-4 py-3">Personel</th>
                    <th class="px-4 py-3">Departman</th>
                    <th class="px-4 py-3">Pozisyon</th>
                    <th class="px-4 py-3">Durum</th>
                    <th class="px-4 py-3">Detay</th>
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
                    <td>{{ item.department }}</td>
                    <td class="px-4 py-3 whitespace-nowrap">
                      <span
                        class="px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full"
                        :class="{
                          'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200':
                            item.status === 'Aktif',
                          'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200':
                            item.status === 'Pasif',
                        }"
                      >
                        {{ item.status }}
                      </span>
                    </td>
                    <td class="px-4 py-3 whitespace-nowrap text-gray-600 dark:text-gray-300">
                      <button
                        class="px-2 py-1 mx-1 h-8 w-7 bg-sky-600 rounded-md text-neutral-200 cursor-pointer hover:bg-sky-500"
                      >
                        <i class="fa-regular fa-pen-to-square"></i>
                      </button>
                      <button
                        class="px-2 py-1 mx-1 h-8 w-7 bg-green-600 rounded-md text-neutral-200 cursor-pointer hover:bg-green-500"
                      >
                        <i class="fa-solid fa-info"></i>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>

        <!-- Hızlı Erişim Butonları -->
        <!-- <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6">
            <h2 class="text-xl font-bold text-gray-800 dark:text-gray-200 mb-4">Hızlı Erişim</h2>
            <QuickAccessButtons />
          </div> -->
      </div>
    </main>
  </div>
</template>
