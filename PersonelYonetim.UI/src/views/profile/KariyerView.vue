<script setup lang="ts">
import TableLayout from "@/components/TableLayout.vue";
import type { PersonelGorevlendirmeModel } from "@/models/entity-models/PersonelGorevlendirmeModel";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import PersonelService from "@/services/PersonelService";
import { computed, onMounted, ref, type Ref } from "vue";

const activeTab2 = ref("pozisyon");

const setActiveTab2 = (tab: string) => {
  activeTab2.value = tab;
};
const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

const personelGorevlendirmeler: Ref<PersonelGorevlendirmeModel[] | undefined> = ref([]);

onMounted(() => {
  getPersonelAtamalar();
});

const getPersonelAtamalar = async () => {
  const res = await PersonelService.getPersonelGorevlendirmeler(undefined, paginationParams.value);
  personelGorevlendirmeler.value = res?.items;
  paginationParams.value.count = res?.count || 0;
};

const filteredPersonelAtamalar = computed<Record<string, unknown>[]>(() => {
  return (personelGorevlendirmeler.value || []).map(
    ({
      id,
      pozisyonBaslangicTarihi,
      pozisyonBitisTarihi,
      kurumsalBirimAd,
      pozisyonAd,
      raporlananPersonelAd,
      isActive,
      createdAt,
      createUserName,
    }) => ({
      id,
      pozisyonBaslangicTarihi: new Date(pozisyonBaslangicTarihi),
      pozisyonBitisTarihi: pozisyonBitisTarihi != null ? new Date(pozisyonBitisTarihi) : null,
      kurumsalBirimAd,
      pozisyonAd,
      raporlananPersonelAd,
      isActive: isActive ? "Aktif" : "Pasif",
      createdAt: new Date(createdAt),
      createUserName,
    })
  );
});

const setPageNumber = (pageNumber: number) => {
  paginationParams.value.pageNumber = pageNumber;
  getPersonelAtamalar();
};

const orderBy = (order: string) => {
  paginationParams.value.orderBy = paginationParams.value.orderBy?.includes("desc")
    ? order + " asc"
    : order + " desc";
  getPersonelAtamalar();
};
</script>

<template>
  <div class="mx-10 my-5">
    <ul
      class="flex flex-wrap text-sm font-medium text-center border-b border-gray-200 dark:border-gray-700"
    >
      <li class="me-2">
        <button
          @click="setActiveTab2('pozisyon')"
          :class="
            activeTab2 === 'pozisyon'
              ? 'text-neutral-100 bg-sky-600  hover:bg-sky-500 dark:text-neutral-50'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Pozisyon
        </button>
      </li>
      <li class="me-2">
        <button
          @click="setActiveTab2('maas')"
          :class="
            activeTab2 === 'maas'
              ? 'bg-sky-600  hover:bg-sky-500 text-neutral-100 dark:text-neutral-50'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Maaş
        </button>
      </li>
      <!-- <li class="me-2">
        <button
          @click="setActiveTab2('calismaTakvimi')"
          :class="
            activeTab2 === 'calismaTakvimi'
              ? 'bg-sky-600 hover:bg-sky-500 text-neutral-100'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Çalışma Takvimi
        </button>
      </li> -->
      <li class="me-2">
        <button
          @click="setActiveTab2('performans')"
          :class="
            activeTab2 === 'performans'
              ? 'bg-sky-600 hover:bg-sky-500 text-neutral-100'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Performans
        </button>
      </li>
    </ul>
    <div v-if="activeTab2 === 'pozisyon'" class="">
      <div class="overflow-x-auto mt-5">
        <TableLayout
          :table-headers="[
            { key: 'pozisyonBaslangicTarihi', value: 'Baslangic', width: 'w-20' },
            { key: 'pozisyonBitisTarihi', value: 'Bitis', width: 'w-[500px]' },
            { key: 'kurumsalBirimAd', value: 'Birim' },
            { key: 'pozisyonAd', value: 'Pozisyon' },
            { key: 'raporlananPersonelAd', value: 'Yonetici' },
            { key: 'isActive', value: 'Durum' },
            { key: 'createdAt', value: 'Oluşturulma Tarihi' },
            { key: 'createUserName', value: 'Oluşturan' },
          ]"
          :table-content="filteredPersonelAtamalar"
          :islemler="['detaylar']"
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
          @order-by="orderBy"
        />
      </div>
    </div>

    <div v-if="activeTab2 === 'maas'" class="space-y-6">
      <div class="flex justify-center items-center mt-10 w-full">
        <div
          class="border-2 border-neutral-200 dark:border-neutral-500 p-6 bg-transparent rounded-lg w-full"
        >
          <div class="text-center my-5">
            <i class="fa-solid fa-wallet fa-2xl" style="color: #3562b1"></i>

            <br />
            <br />
            <p class="text-gray-800 dark:text-neutral-300 text-l mb-4">Maaş bilgisi bulunamadı</p>

            <p class="text-gray-700 dark:text-neutral-400 text-sm">
              Bordro işlemleri için bir maaş bilgisi ekleyin
            </p>
          </div>
        </div>
      </div>
    </div>

    <div v-if="activeTab2 === 'calismaTakvimi'" class="space-y-6">
      <div class="overflow-x-auto mt-10">
        <!-- <TableLayout
          :table-headers="[
            'Başlangıç',
            'Bitiş',
            'Süre',
            'Çalışma Takvimi',
            'Atanma Tarihi',
            'Durum',
          ]"
          :table-content="calismaTakvimiData"
          :islemler="['detaylar']"
        /> -->
      </div>
    </div>

    <div v-if="activeTab2 === 'performans'" class="space-y-6">
      <div class="flex justify-center items-center mt-10 w-full">
        <div
          class="border-2 border-neutral-200 dark:border-neutral-500 p-6 bg-transparent rounded-lg w-full"
        >
          <div class="text-center my-5">
            <i class="fa-solid fa-circle-exclamation fa-2xl" style="color: #3562b1"></i>

            <br />
            <br />
            <p class="text-gray-800 dark:text-neutral-300 text-l mb-4">
              Girilen kriterlere uygun sonuç bulunamadı
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
