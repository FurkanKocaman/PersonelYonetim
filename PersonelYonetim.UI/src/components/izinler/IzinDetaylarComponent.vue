<script setup lang="ts">
import type { IzinTalepGetResponse } from "@/models/response-models/izinler/IzinTalepGetResponse";

const props = defineProps<{
  selectedIzin: IzinTalepGetResponse | null;
}>();

const statusColors: Record<string, string> = {
  Onaylandı: "text-green-600 bg-green-100 dark:bg-green-900 dark:text-green-300",
  Beklemede: "text-amber-600 bg-amber-100 dark:bg-amber-900 dark:text-amber-300",
  Reddedildi: "text-red-600 bg-red-100 dark:bg-red-900 dark:text-red-300",
};
</script>

<template>
  <div class="fixed inset-0 z-30 overflow-y-auto">
    <div
      class="flex items-center justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0"
    >
      <!-- Arkaplan Overlay -->
      <div class="fixed inset-0 transition-opacity">
        <div class="absolute inset-0 bg-gray-500 dark:bg-neutral-900 opacity-75"></div>
      </div>

      <!-- Modal İçeriği -->
      <div
        class="inline-block align-bottom bg-white dark:bg-neutral-800 rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full"
      >
        <div class="bg-white dark:bg-neutral-800 px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
          <div class="sm:flex sm:items-start">
            <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left w-full">
              <h3 class="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200 mb-4">
                İzin Detayları
              </h3>
              <div class="mt-2 space-y-4">
                <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
                  <div>
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">Personel</div>
                    <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">
                      {{ props.selectedIzin?.personelFullName }}
                    </div>
                  </div>
                  <div>
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">
                      İzin Tipi
                    </div>
                    <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">
                      {{ props.selectedIzin?.izinTuru }}
                    </div>
                  </div>

                  <div>
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">
                      Başlangıç Tarihi
                    </div>
                    <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">
                      {{
                        new Date(props.selectedIzin!.baslangicTarihi).toLocaleString("tr-TR", {
                          day: "2-digit",
                          month: "2-digit",
                          year: "numeric",
                          hour: "2-digit",
                          minute: "2-digit",
                          hour12: false,
                        })
                      }}
                    </div>
                  </div>
                  <div>
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">
                      Bitiş Tarihi
                    </div>
                    <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">
                      {{
                        new Date(props.selectedIzin!.bitisTarihi).toLocaleString("tr-TR", {
                          day: "2-digit",
                          month: "2-digit",
                          year: "numeric",
                          hour: "2-digit",
                          minute: "2-digit",
                          hour12: false,
                        })
                      }}
                    </div>
                  </div>
                  <div>
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">
                      Toplam Gün
                    </div>
                    <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">
                      {{ props.selectedIzin?.toplamSure }}
                    </div>
                  </div>
                  <div>
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">
                      Mesai Baslangic Tarihi
                    </div>
                    <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">
                      {{
                        new Date(props.selectedIzin!.mesaiBaslangicTarihi).toLocaleString("tr-TR", {
                          day: "2-digit",
                          month: "2-digit",
                          year: "numeric",
                          hour: "2-digit",
                          minute: "2-digit",
                          hour12: false,
                        })
                      }}
                    </div>
                  </div>
                  <div>
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">Durum</div>
                    <div class="mt-1">
                      <span
                        class="px-2 py-1 rounded-full text-xs font-medium"
                        :class="statusColors[selectedIzin!.degerlendirmeDurumu]"
                      >
                        {{ props.selectedIzin?.degerlendirmeDurumu }}
                      </span>
                    </div>
                  </div>
                  <div class="sm:col-span-2">
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">Açıklama</div>
                    <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">
                      {{ selectedIzin?.aciklama }}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="bg-gray-50 dark:bg-neutral-700 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
          <button
            type="button"
            class="text-gray-900 hover:text-white border border-gray-800 hover:bg-gray-900 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-gray-600 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-800"
            @click="
              () => {
                $emit('closeDetailModal', false);
              }
            "
          >
            Kapat
          </button>
          <!-- <button
            type="button"
            class="text-red-700 hover:text-white border border-red-700 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-red-500 dark:text-red-500 dark:hover:text-white dark:hover:bg-red-600 dark:focus:ring-red-900"
            @click="izinDegerlendir(selectedIzin?.id!, 1)"
          >
            Reddet
          </button>
          <button
            type="button"
            class="text-green-700 hover:text-white border border-green-700 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-green-500 dark:text-green-500 dark:hover:text-white dark:hover:bg-green-600 dark:focus:ring-green-800"
            @click="izinDegerlendir(selectedIzin?.id!, 0)"
          >
            Onayla
          </button> -->
        </div>
      </div>
    </div>
  </div>
</template>
