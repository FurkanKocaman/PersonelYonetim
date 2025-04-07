<script setup lang="ts">
import type { DuyuruModel } from "@/models/entity-models/DuyuruModel";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import DuyuruService from "@/services/DuyuruService";
import { onMounted, ref, type Ref } from "vue";

const pageSize = ref(0);
const pageCount = ref(0);
const currentPage = ref(0);
const duyurular: Ref<DuyuruModel[]> = ref([]);
const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 5,
  orderBy: "createdAt desc",
  filter: undefined,
});

onMounted(() => {
  getDuyurular();
});

const getDuyurular = async () => {
  const res = await DuyuruService.getDuyurular(paginationParams.value);
  paginationParams.value.count = res!.count;
  duyurular.value = res!.items;
  pageCount.value =
    Math.ceil(paginationParams.value.count / paginationParams.value.pageSize) == 0
      ? 1
      : Math.ceil(paginationParams.value.count / paginationParams.value.pageSize);
  pageSize.value =
    paginationParams.value.pageSize > paginationParams.value.count
      ? paginationParams.value.count
      : paginationParams.value.pageSize;
  currentPage.value = paginationParams.value.pageNumber;
};

const setPage = (pageNumber: number) => {
  if (paginationParams.value.pageNumber != pageNumber) {
    paginationParams.value.pageNumber = pageNumber;
    getDuyurular();
  }
};
</script>

<template>
  <div
    class="lg:col-span-1 bg-white dark:bg-neutral-800 rounded-lg shadow-md overflow-hidden hover-card m-5"
  >
    <div class="px-6 py-4 bg-neutral-300/50 dark:bg-neutral-800/80">
      <h3 class="text-lg font-semibold text-neutral-800 dark:text-neutral-200">Duyurular</h3>
      <p class="text-sm text-neutral-700 dark:text-neutral-300">Güncel duyurular ve bildirimler</p>
    </div>
    <div class="p-6">
      <div v-if="duyurular.length === 0" class="text-center py-4 text-gray-500 dark:text-gray-400">
        Henüz duyuru bulunmamaktadır.
      </div>
      <div v-else class="space-y-4">
        <div
          v-for="duyuru in duyurular"
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
          <span class="text-xs text-neutral-500 dark:text-neutral-400"
            >Oluşturan: {{ duyuru.createUserName }}</span
          >
        </div>
      </div>
    </div>
  </div>
  <nav
    class="flex flex-col justify-center w-full items-center my-3"
    aria-label="Page navigation example"
  >
    <span class="text-sm text-gray-700 dark:text-gray-400 mb-2">
      <span class="font-semibold text-gray-900 dark:text-white">{{
        (currentPage - 1) * pageSize == 0 ? 1 : (currentPage - 1) * pageSize
      }}</span>
      den
      <span class="font-semibold text-gray-900 dark:text-white">{{ pageSize * currentPage }}</span>
      'a kadar olan kayıtlar gösteriliyor.<span class="font-semibold text-gray-900 dark:text-white"
        >Toplam {{ paginationParams.count }}</span
      >
      Kayıt
    </span>
    <ul class="flex items-center -space-x-px h-8 text-sm">
      <li>
        <button
          class="flex items-center justify-center px-3 h-8 ms-0 leading-tight text-gray-500 bg-white border border-e-0 border-gray-300 rounded-s-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-neutral-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
          @click="currentPage != 1 ? setPage(currentPage - 1) : ''"
        >
          <span class="sr-only">Previous</span>
          <svg
            class="w-2.5 h-2.5 rtl:rotate-180"
            aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 6 10"
          >
            <path
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M5 1 1 5l4 4"
            />
          </svg>
        </button>
      </li>
      <li v-for="x in pageCount" :key="x">
        <button
          class="flex items-center justify-center px-3 h-8 leading-tight text-gray-500 border border-gray-300 hover:bg-gray-100 hover:text-gray-700 dark:bg-neutral-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
          @click="setPage(x)"
        >
          {{ x }}
        </button>
      </li>

      <li>
        <button
          class="flex items-center justify-center px-3 h-8 leading-tight text-gray-500 bg-white border border-gray-300 rounded-e-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-neutral-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
          @click="
            currentPage != Math.ceil(paginationParams.count / pageSize)
              ? setPage(currentPage + 1)
              : ''
          "
        >
          <span class="sr-only">Next</span>
          <svg
            class="w-2.5 h-2.5 rtl:rotate-180"
            aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 6 10"
          >
            <path
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="m1 9 4-4-4-4"
            />
          </svg>
        </button>
      </li>
    </ul>
  </nav>
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
