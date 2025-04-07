<script setup lang="ts">
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { IzinTalepGetResponse } from "@/models/response-models/izinler/IzinTalepGetResponse";
import type { IzinlerKalanResponse } from "@/models/response-models/izinler/İzinlerKalanResponse";
import IzinService from "@/services/IzinService";
import { onMounted, ref, type Ref } from "vue";

const izinList = ref<IzinTalepGetResponse[]>([]);

const kalanIzin: Ref<IzinlerKalanResponse> = ref({
  izinTurAd: "",
  kullanilan: 0,
  kalan: 0,
  guncelHakEdisDonem: "",
});

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 5,
  orderBy: "createdAt desc",
  filter: "",
});

onMounted(() => {
  personelGetIzinTalepler();
  getKalanIzin();
});

const personelGetIzinTalepler = async () => {
  const res = await IzinService.getPersonelIzinTalepler(paginationParams.value);
  izinList.value = res!.items;
  paginationParams.value.count = res!.count;
};

const getKalanIzin = async () => {
  const res = await IzinService.getIzinlerKalan();
  kalanIzin.value = res!;
};
</script>

<template>
  <div
    class="lg:col-span-2 bg-white dark:bg-neutral-800/80 rounded-lg shadow-md overflow-hidden hover-card"
  >
    <div class="xl:p-6">
      <div class="overflow-x-auto">
        <table class="min-w-full">
          <thead>
            <tr
              class="text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider border-b border-gray-200 dark:border-gray-700"
            >
              <th class="px-4 py-3">İzin Türü</th>
              <th class="px-4 py-3">Toplam Süre</th>
              <th class="px-4 py-3">Durum</th>
              <th class="px-4 py-3">Baslangic Tarihi</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-200 dark:divide-gray-700">
            <tr v-for="izinTalep in izinList" :key="izinTalep.id" class="hover-row">
              <td class="px-4 py-3 whitespace-nowrap">
                <div class="font-medium text-gray-800 dark:text-white">
                  {{ izinTalep.izinTuru }}
                </div>
              </td>
              <td class="px-4 py-3 whitespace-nowrap text-gray-600 dark:text-gray-300">
                {{ izinTalep.toplamSure }} gün
              </td>
              <td class="px-4 py-3 whitespace-nowrap">
                <span
                  class="px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full"
                  :class="{
                    'bg-neutral-100 text-green-800 dark:bg-green-900 dark:text-neutral-200':
                      izinTalep.degerlendirmeDurumu === 'Onaylandı',
                    'bg-neutral-100 text-yellow-800 dark:bg-yellow-900 dark:text-neutral-200':
                      izinTalep.degerlendirmeDurumu === 'Beklemede',
                    'bg-neutral-100 text-red-800 dark:bg-red-900 dark:text-neutral-200':
                      izinTalep.degerlendirmeDurumu === 'Reddedildi',
                  }"
                >
                  {{ izinTalep.degerlendirmeDurumu }}
                </span>
              </td>
              <td class="px-4 py-3 whitespace-nowrap text-gray-600 dark:text-gray-300">
                {{
                  new Date(izinTalep.baslangicTarihi).toLocaleString("tr-TR", {
                    day: "2-digit",
                    month: "2-digit",
                    year: "numeric",
                    hour: "2-digit",
                    minute: "2-digit",
                    hour12: false,
                  })
                }}
              </td>
            </tr>
          </tbody>
        </table>
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
