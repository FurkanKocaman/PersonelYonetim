<script setup lang="ts">
import { ref, onMounted, onActivated, computed, type Ref } from "vue";
import IzinService from "@/services/IzinService";
import type { IzinTalepGetResponse } from "@/models/response-models/izinler/IzinTalepGetResponse";
import TableLayout from "@/components/TableLayout.vue";
import type { PaginationParams } from "@/models/request-models/PaginationParams";

const izinList = ref<IzinTalepGetResponse[]>([]);
const selectedIzin = ref<IzinTalepGetResponse | undefined>(undefined);

const loading = ref(true);
const error = ref(false);

const showDetailModal = ref(false);

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

const statusColors: Record<string, string> = {
  Onaylandı: "text-green-600 bg-green-100 dark:bg-green-900 dark:text-green-300",
  Beklemede: "text-amber-600 bg-amber-100 dark:bg-amber-900 dark:text-amber-300",
  Reddedildi: "text-red-600 bg-red-100 dark:bg-red-900 dark:text-red-300",
};
const izinDurumlar = ref(["Beklemede", "Onaylandı", "Reddedildi"]);

const filterOptions = ref({
  durum: "",
  izinTipi: "",
});

const filteredIzinList = computed(() => {
  return izinList.value.filter((izin) => {
    const durumMatch =
      !filterOptions.value.durum || izin.degerlendirmeDurumu === filterOptions.value.durum;
    const izinTipiMatch =
      !filterOptions.value.izinTipi || izin.izinTuru === filterOptions.value.izinTipi;
    return durumMatch && izinTipiMatch;
  });
});

const filteredIzinTalepler = computed<Record<string, unknown>[]>(() => {
  return (filteredIzinList.value || []).map(
    ({
      id,
      personelFullName,
      baslangicTarihi,
      bitisTarihi,
      mesaiBaslangicTarihi,
      toplamSure,
      izinTuru,
      degerlendirmeDurumu,
    }) => ({
      id,
      personelFullName,
      baslangicTarihi: new Date(baslangicTarihi),
      bitisTarihi: new Date(bitisTarihi),
      mesaiBaslangicTarihi: new Date(mesaiBaslangicTarihi),
      toplamSure,
      izinTuru,
      degerlendirmeDurumu,
    })
  );
});

onMounted(() => {
  getIzinTalepler();
});

onActivated(() => {
  getIzinTalepler();
});

const setPageNumber = (pageNumber: number) => {
  paginationParams.value.pageNumber = pageNumber;
  getIzinTalepler();
};

const getIzinTalepler = async () => {
  const response = await IzinService.getIzinTalepler(paginationParams.value);
  izinList.value = response!.items;
  loading.value = false;
  paginationParams.value.count = response!.count;
};

const izinDegerlendir = async (id: string, degerlendirme: number) => {
  const response = await IzinService.izinTalepDegerlendir(id, degerlendirme);
  showDetailModal.value = false;
  console.log(response);
  getIzinTalepler();
};

const openIzinEdit = (item: IzinTalepGetResponse) => {
  selectedIzin.value = izinList.value.find((p) => p.id == item.id);
  showDetailModal.value = true;
};
</script>
<template>
  <!-- İçerik Alanı -->
  <main class="p-2 flex flex-col max-w[80dvw]">
    <!-- Üst Kontroller -->
    <div class="mb-6 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
      <!-- Filtreler -->
      <div class="flex flex-wrap gap-4">
        <div class="w-full sm:w-auto">
          <select
            v-model="filterOptions.durum"
            class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
          >
            <option value="">Tüm Durumlar</option>
            <option v-for="durum in izinDurumlar" :key="durum" :value="durum">
              {{ durum }}
            </option>
          </select>
        </div>
        <div class="w-full sm:w-auto">
          <select
            v-model="filterOptions.izinTipi"
            class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
          >
            <option value="">Tüm İzin Türleri</option>
            <!-- <option v-for="izinTipi in izinTurleri" :key="izinTipi" :value="izinTipi">
              {{ izinTipi }}
            </option> -->
          </select>
        </div>
      </div>
    </div>

    <div v-if="loading" class="flex justify-center items-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
    </div>

    <div
      v-else-if="error"
      class="bg-red-100 border border-red-200 text-red-700 dark:bg-red-900 dark:border-red-800 dark:text-red-300 rounded-lg p-4 mb-6"
    >
      <div class="flex items-center">
        <i class="fas fa-exclamation-circle mr-2"></i>
        <span>İzin listesi yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.</span>
      </div>
    </div>

    <div v-else-if="izinList.length > 0" class="overflow-auto">
      <TableLayout
        :table-headers="[
          'Personel',
          'Baslangic',
          'Bitis',
          'Mesai Baslangic',
          'Süre',
          'İzin Tipi',
          'Durum',
        ]"
        :table-content="filteredIzinTalepler"
        :islemler="['edit']"
        @edit-click="openIzinEdit"
        :page-count="
          Math.ceil(paginationParams.count / paginationParams.pageSize) == 0
            ? 1
            : Math.ceil(paginationParams.count / paginationParams.pageSize)
        "
        :count="paginationParams.count"
        :page-size="
          paginationParams.pageSize > paginationParams.count
            ? paginationParams.count
            : paginationParams.pageSize
        "
        :current-page="paginationParams.pageNumber"
        @set-page="setPageNumber"
      />
    </div>

    <!-- Veri Yoksa -->
    <div v-else class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 text-center">
      <div class="flex flex-col items-center justify-center py-12">
        <i class="fas fa-calendar-times text-4xl text-gray-400 dark:text-gray-600 mb-4"></i>
        <h3 class="text-lg font-medium text-gray-900 dark:text-gray-200 mb-2">
          İzin Kaydı Bulunamadı
        </h3>
        <p class="text-gray-500 dark:text-gray-400 mb-6">Henüz hiç izin talebi oluşturulmamış.</p>
      </div>
    </div>
  </main>

  <div v-if="showDetailModal" class="fixed inset-0 z-30 overflow-y-auto">
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
                      {{ selectedIzin?.personelFullName }}
                    </div>
                  </div>
                  <div>
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">
                      İzin Tipi
                    </div>
                    <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">
                      {{ selectedIzin?.izinTuru }}
                    </div>
                  </div>

                  <div>
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">
                      Başlangıç Tarihi
                    </div>
                    <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">
                      {{
                        new Date(selectedIzin!.baslangicTarihi).toLocaleString("tr-TR", {
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
                        new Date(selectedIzin!.bitisTarihi).toLocaleString("tr-TR", {
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
                      {{ selectedIzin?.toplamSure }}
                    </div>
                  </div>
                  <div>
                    <div class="text-sm font-medium text-gray-500 dark:text-gray-400">
                      Mesai Baslangic Tarihi
                    </div>
                    <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">
                      {{
                        new Date(selectedIzin!.mesaiBaslangicTarihi).toLocaleString("tr-TR", {
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
                        {{ selectedIzin?.degerlendirmeDurumu }}
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
                showDetailModal = false;
              }
            "
          >
            Kapat
          </button>
          <button
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
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
