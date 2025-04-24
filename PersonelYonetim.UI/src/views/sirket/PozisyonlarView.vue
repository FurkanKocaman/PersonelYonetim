<script setup lang="ts">
import type { PozisyonModel } from "@/models/entity-models/PozisyonModel";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import PozisyonService from "@/services/PozisyonService";
import { onMounted, ref, type Ref } from "vue";
import dayjs from "dayjs";
import "dayjs/locale/tr";
import type { PozisyonCreateRequest } from "@/models/request-models/PozisyonlarCreateRequest";

dayjs.locale("tr");

const isModalOpen = ref(false);
const pozisyonlar: Ref<PozisyonModel[]> = ref([]);
const request: Ref<PozisyonCreateRequest> = ref({
  id: undefined,
  ad: "",
  kod: undefined,
  aciklama: undefined,
  tenantId: undefined,
});

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

onMounted(() => {
  getPozisyonlar();
});

const getPozisyonlar = async () => {
  const res = await PozisyonService.pozisyonlarGet(undefined, paginationParams.value);

  if (res) {
    pozisyonlar.value = res?.items;
    paginationParams.value.count = res!.count;
  }
};

const setPageNumber = (pageNumber: number) => {
  paginationParams.value.pageNumber = pageNumber;
  getPozisyonlar();
};

const createPozisyon = async () => {
  isModalOpen.value = false;
  await PozisyonService.pozisyonlarCreate(request.value);
  getPozisyonlar();
};

const deletePozisyon = async (id: string) => {
  await PozisyonService.pozisyonlarDelete(id);
  getPozisyonlar();
};
</script>
<template>
  <!-- Modal start -->
  <div
    v-if="isModalOpen"
    id="hs-focus-management-modal"
    class="size-full fixed top-0 start-0 z-50 overflow-x-hidden overflow-y-auto flex justify-center items-center backdrop-blur-xl"
  >
    <div
      class="-open:mt-7 -open:opacity-100 -open:duration-500 mt-0 ease-out transition-all sm:max-w-lg sm:w-full m-3 sm:mx-auto"
    >
      <div
        class="flex flex-col bg-white border border-gray-200 shadow-2xs rounded-xl pointer-events-auto dark:bg-neutral-800 dark:border-neutral-700 dark:shadow-neutral-700/70"
      >
        <div
          class="flex justify-between items-center py-3 px-4 border-b border-gray-200 dark:border-neutral-700"
        >
          <h3 id="hs-focus-management-modal-label" class="font-bold text-gray-800 dark:text-white">
            Pozisyon Oluştur
          </h3>
          <button
            type="button"
            class="size-8 inline-flex justify-center items-center gap-x-2 rounded-full border border-transparent bg-gray-100 text-gray-800 hover:bg-gray-200 focus:outline-hidden focus:bg-gray-200 disabled:opacity-50 disabled:pointer-events-none dark:bg-neutral-700 dark:hover:bg-neutral-600 dark:text-neutral-400 dark:focus:bg-neutral-600 cursor-pointer"
            @click="isModalOpen = false"
          >
            <span class="sr-only">Close</span>
            <svg
              class="shrink-0 size-4"
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <path d="M18 6 6 18"></path>
              <path d="m6 6 12 12"></path>
            </svg>
          </button>
        </div>
        <div class="p-4 overflow-y-auto">
          <label for="ad" class="block text-sm font-medium mb-2 dark:text-white">Ad</label>
          <input
            type="text"
            id="ad"
            v-model="request.ad"
            class="py-3 px-4 block w-full border border-neutral-400 rounded-lg text-sm focus:outline-none focus:border-blue-600 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-200"
            placeholder="Yönetici"
          />
        </div>
        <div class="p-4 overflow-y-auto">
          <label for="kod" class="block text-sm font-medium mb-2 dark:text-white"
            >Kod <span class="dark:text-neutral-400 text-neutral-500">(Opsiyonel)</span></label
          >
          <input
            type="text"
            id="kod"
            v-model="request.kod"
            class="py-3 px-4 block w-full border border-neutral-400 rounded-lg text-sm focus:outline-none focus:border-blue-600 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-200"
            placeholder="YNTC"
          />
        </div>

        <div class="p-4 overflow-y-auto">
          <label for="aciklama" class="block text-sm font-medium mb-2 dark:text-white"
            >Aciklama<span class="dark:text-neutral-400 text-neutral-500">(Opsiyonel)</span>
          </label>
          <input
            type="text"
            id="aciklama"
            v-model="request.aciklama"
            class="py-3 px-4 block w-full border border-neutral-400 rounded-lg text-sm focus:outline-none focus:border-blue-600 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-200"
            placeholder=""
          />
        </div>
        <div
          class="flex justify-end items-center gap-x-2 py-3 px-4 border-t border-gray-200 dark:border-neutral-700"
        >
          <button
            type="button"
            class="py-2 px-3 inline-flex items-center gap-x-2 text-sm font-medium rounded-lg border border-gray-200 bg-white text-gray-800 shadow-2xs hover:bg-gray-50 focus:outline-hidden focus:bg-gray-50 disabled:opacity-50 disabled:pointer-events-none dark:bg-neutral-800 dark:border-neutral-700 dark:text-white dark:hover:bg-neutral-700 dark:focus:bg-neutral-700"
            @click="isModalOpen = false"
          >
            İptal
          </button>
          <button
            type="button"
            class="py-2 px-3 inline-flex items-center gap-x-2 text-sm font-medium rounded-lg border border-transparent bg-blue-600 text-white hover:bg-blue-700 focus:outline-hidden focus:bg-blue-700 disabled:opacity-50 disabled:pointer-events-none"
            @click="createPozisyon()"
          >
            Kaydet
          </button>
        </div>
      </div>
    </div>
  </div>
  <!-- Modal end -->
  <div class="flex flex-col mx-5 mt-5">
    <div class="flex justify-between">
      <h2 class="text-sm text-neutral-500 dark:text-neutral-400 mb-10">
        Şirketinizde bulunan pozisyonları buradan yönetebilirsiniz.
      </h2>
      <button
        type="button"
        class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-1 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800 cursor-pointer"
        @click="isModalOpen = !isModalOpen"
      >
        Pozisyon Oluştur
      </button>
    </div>
    <div class="-m-1.5 overflow-x-auto">
      <div class="p-1.5 min-w-full inline-block align-middle">
        <div
          class="border border-gray-200 rounded-lg divide-y divide-gray-200 dark:border-neutral-700 dark:divide-neutral-700"
        >
          <div class="py-3 px-4 mb-5">
            <div class="relative max-w-xs">
              <label class="sr-only">Search</label>
              <input
                type="text"
                name="hs-table-with-pagination-search"
                id="hs-table-with-pagination-search"
                class="py-1.5 sm:py-2 px-3 ps-9 block w-full border-gray-200 shadow-2xs rounded-lg sm:text-sm focus:z-10 focus:border-blue-500 focus:ring-blue-500 disabled:opacity-50 disabled:pointer-events-none dark:bg-neutral-900 dark:border-neutral-700 dark:text-neutral-400 dark:placeholder-neutral-500 dark:focus:ring-neutral-600"
                placeholder="Ara"
              />
              <div class="absolute inset-y-0 start-0 flex items-center pointer-events-none ps-3">
                <svg
                  class="size-4 text-gray-400 dark:text-neutral-500"
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <circle cx="11" cy="11" r="8"></circle>
                  <path d="m21 21-4.3-4.3"></path>
                </svg>
              </div>
            </div>
          </div>
          <div class="overflow-hidden">
            <table class="min-w-full divide-y divide-gray-200 dark:divide-neutral-700 table-fixed">
              <thead class="bg-gray-50 dark:bg-neutral-700">
                <tr>
                  <th scope="col" class="py-3 px-4 pe-0">
                    <div class="flex items-center h-5">
                      <input
                        id="hs-table-pagination-checkbox-all"
                        type="checkbox"
                        class="border-gray-200 rounded-sm text-blue-600 focus:ring-blue-500 dark:bg-neutral-700 dark:border-neutral-500 dark:checked:bg-blue-500 dark:checked:border-blue-500 dark:focus:ring-offset-gray-800"
                      />
                      <label for="hs-table-pagination-checkbox-all" class="sr-only">Checkbox</label>
                    </div>
                  </th>
                  <th
                    scope="col"
                    class="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase dark:text-neutral-500"
                  >
                    Ad
                  </th>
                  <th
                    scope="col"
                    class="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase dark:text-neutral-500"
                  >
                    Kod
                  </th>
                  <th
                    scope="col"
                    class="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase dark:text-neutral-500"
                  >
                    Açıklama
                  </th>
                  <th
                    scope="col"
                    class="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase dark:text-neutral-500"
                  >
                    Oluşturulma Tarihi
                  </th>
                  <th
                    scope="col"
                    class="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase dark:text-neutral-500"
                  >
                    Oluşturan
                  </th>
                  <th
                    scope="col"
                    class="px-6 py-3 text-center text-xs font-medium text-gray-500 uppercase dark:text-neutral-500"
                  >
                    İşlemler
                  </th>
                </tr>
              </thead>
              <tbody class="divide-y divide-gray-200 dark:divide-neutral-700">
                <tr v-for="pozisyon in pozisyonlar" :key="pozisyon.id">
                  <td class="py-3 ps-4">
                    <div class="flex items-center h-5">
                      <input
                        id="hs-table-pagination-checkbox-1"
                        type="checkbox"
                        class="border-gray-200 rounded-sm text-blue-600 focus:ring-blue-500 dark:bg-neutral-800 dark:border-neutral-700 dark:checked:bg-blue-500 dark:checked:border-blue-500 dark:focus:ring-offset-gray-800"
                      />
                      <label for="hs-table-pagination-checkbox-1" class="sr-only">Checkbox</label>
                    </div>
                  </td>
                  <td
                    class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-800 dark:text-neutral-200"
                  >
                    {{ pozisyon.ad }}
                  </td>
                  <td
                    class="px-6 py-4 whitespace-nowrap text-sm text-gray-800 dark:text-neutral-200"
                  >
                    {{ pozisyon.kod }}
                  </td>

                  <td
                    class="px-6 py-4 whitespace-nowrap text-sm text-gray-800 dark:text-neutral-200"
                  >
                    {{ pozisyon.aciklama ?? "-" }}
                  </td>
                  <td
                    class="px-6 py-4 whitespace-nowrap text-sm text-gray-800 dark:text-neutral-200"
                  >
                    {{ dayjs(pozisyon.createdAt).format("D MMMM YYYY ") }}
                  </td>
                  <td
                    class="px-6 py-4 whitespace-nowrap text-sm text-gray-800 dark:text-neutral-200"
                  >
                    {{ pozisyon.createUserName }}
                  </td>
                  <td class="px-6 text-center py-4 whitespace-nowrap text-sm font-medium">
                    <button
                      type="button"
                      class="text-center gap-x-2 text-sm font-semibold rounded-lg border border-transparent text-red-600 hover:text-red-800 focus:outline-hidden focus:text-red-800 disabled:opacity-50 disabled:pointer-events-none dark:text-red-500 dark:hover:text-red-400 dark:focus:text-red-400"
                      @click="deletePozisyon(pozisyon.id)"
                    >
                      Sil
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div class="py-1 px-4">
            <nav
              class="flex flex-col justify-center w-full items-center my-3"
              aria-label="Page navigation example"
            >
              <span class="text-sm text-gray-700 dark:text-gray-400 mb-2">
                <span class="font-semibold text-gray-900 dark:text-white">{{
                  (paginationParams.pageNumber - 1) * paginationParams.pageSize == 0
                    ? 1
                    : (paginationParams.pageNumber - 1) * paginationParams.pageSize
                }}</span>
                den
                <span class="font-semibold text-gray-900 dark:text-white">{{
                  paginationParams.pageSize * paginationParams.pageNumber
                }}</span>
                'a kadar olan kayıtlar gösteriliyor.<span
                  class="font-semibold text-gray-900 dark:text-white"
                  >Toplam {{ paginationParams.count }}</span
                >
                Kayıt
              </span>
              <ul class="flex items-center -space-x-px h-8 text-sm">
                <li>
                  <button
                    class="flex items-center justify-center px-3 h-8 ms-0 leading-tight text-gray-500 bg-white border border-e-0 border-gray-300 rounded-s-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-neutral-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
                    @click="
                      paginationParams.pageNumber != 1
                        ? setPageNumber(paginationParams.pageNumber - 1)
                        : ''
                    "
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
                <li
                  v-for="x in Math.ceil(paginationParams.count / paginationParams.pageSize) == 0
                    ? 1
                    : Math.ceil(paginationParams.count / paginationParams.pageSize)"
                  :key="x"
                >
                  <button
                    class="flex items-center justify-center px-3 h-8 leading-tight text-gray-500 border border-gray-300 hover:bg-gray-100 hover:text-gray-700 dark:bg-neutral-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
                    @click="setPageNumber(x)"
                  >
                    {{ x }}
                  </button>
                </li>

                <li>
                  <button
                    class="flex items-center justify-center px-3 h-8 leading-tight text-gray-500 bg-white border border-gray-300 rounded-e-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-neutral-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
                    @click="
                      paginationParams.pageNumber !=
                      Math.ceil(paginationParams.count / paginationParams.pageSize)
                        ? setPageNumber(paginationParams.pageNumber + 1)
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
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
