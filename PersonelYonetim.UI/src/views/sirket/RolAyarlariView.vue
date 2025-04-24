<script setup lang="ts">
import type { RoleModel } from "@/models/response-models/RoleModel";
import RoleService from "@/services/RoleService";
import { onMounted, ref, type Ref } from "vue";
import dayjs from "dayjs";
import "dayjs/locale/tr";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { RoleCreateCommand } from "@/models/request-models/RoleCreateCommand";

dayjs.locale("tr");

const isModalOpen = ref(false);
const roller: Ref<RoleModel[]> = ref([]);
const claims: Ref<{ label: string; value: string }[]> = ref([]);

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

const request: Ref<RoleCreateCommand> = ref({
  roleName: "",
  yapisalRolMu: true,
  claims: [],
  aciklama: undefined,
});

onMounted(() => {
  getRoller();
  getClaims();
});

const getRoller = async () => {
  const res = await RoleService.rollerGet(undefined);
  if (res) {
    roller.value = res.items;
  }
};

const getClaims = async () => {
  const res = await RoleService.claimsGet(undefined);
  if (res) {
    claims.value = res.items;
    console.log(res);
  }
};

const setPageNumber = (pageNumber: number) => {
  paginationParams.value.pageNumber = pageNumber;
  getRoller();
};

const createRol = async () => {
  isModalOpen.value = false;
  await RoleService.rolCreate(request.value);
  getRoller();
};

const deleteRole = async (id: string) => {
  const res = await RoleService.rolDelete(id);
  if (res) getRoller();
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
      class="flex flex-col bg-white border px-5 border-gray-200 shadow-2xs rounded-xl pointer-events-auto dark:bg-neutral-800 dark:border-neutral-700 dark:shadow-neutral-700/70"
    >
      <div
        class="flex justify-between items-center py-3 px-4 border-b border-gray-200 dark:border-neutral-700"
      >
        <h3 id="hs-focus-management-modal-label" class="font-bold text-gray-800 dark:text-white">
          Rol Oluştur
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
        <label for="input-label" class="block text-sm font-medium mb-2 dark:text-white">Ad</label>
        <input
          type="email"
          id="input-label"
          v-model="request.roleName"
          class="py-3 px-4 block w-full border-gray-200 rounded-lg text-sm focus:border-blue-500 focus:ring-blue-500 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-400"
          placeholder="Standart Kullanıcı"
        />
      </div>

      <div class="px-2 overflow-y-auto">
        <label for="input-label" class="block text-sm font-medium mb-2 dark:text-white"
          >Yapisal mi?</label
        >
        <label class="inline-flex items-center cursor-pointer">
          <input type="checkbox" value="" class="sr-only peer" v-model="request.yapisalRolMu" />
          <div
            class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-blue-300 dark:peer-focus:ring-blue-800 rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
          ></div>
        </label>
      </div>
      <div class="p-4 overflow-y-auto">
        <label for="input-label" class="block text-sm font-medium dark:text-white">Yetkiler</label>
        <div class="mt-3 grid grid-cols-1 md:grid-cols-2 gap-y-4 gap-x-6">
          <div class="flex items-center gap-x-3" v-for="claim in claims" :key="claim.value">
            <div class="group grid size-4 grid-cols-1">
              <input
                :id="claim.value"
                type="checkbox"
                :value="claim.value"
                v-model="request.claims"
                class="col-start-1 row-start-1 appearance-none rounded-sm border border-gray-300 bg-white checked:border-indigo-600 checked:bg-indigo-600 indeterminate:border-indigo-600 indeterminate:bg-indigo-600 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 disabled:border-gray-300 disabled:bg-gray-100 disabled:checked:bg-gray-100 forced-colors:appearance-auto"
              />
              <svg
                class="pointer-events-none col-start-1 row-start-1 size-3.5 self-center justify-self-center stroke-white group-has-disabled:stroke-gray-950/25"
                viewBox="0 0 14 14"
                fill="none"
              >
                <path
                  class="opacity-0 group-has-checked:opacity-100"
                  d="M3 8L6 11L11 3.5"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                />
                <path
                  class="opacity-0 group-has-indeterminate:opacity-100"
                  d="M3 7H11"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                />
              </svg>
            </div>
            <label :for="`claim-${claim.value}`" class="block text-sm/6 font-medium">
              {{ claim.label }}
            </label>
          </div>
        </div>
      </div>
      <div class="px-2 py-3 overflow-y-auto">
        <label for="input-label" class="block text-sm font-medium mb-2 dark:text-white"
          >Aciklama<span class="dark:text-neutral-400 text-neutral-500">(Opsiyonel)</span>
        </label>
        <input
          type="email"
          id="input-label"
          v-model="request.aciklama"
          class="py-3 px-4 block w-full border-gray-200 rounded-lg text-sm focus:border-blue-500 focus:ring-blue-500 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-400"
          placeholder="you@site.com"
        />
      </div>

      <div class="flex justify-end items-center gap-x-2 py-3 px-4">
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
          @click="createRol()"
        >
          Kaydet
        </button>
      </div>
    </div>
  </div>

  <!-- Modal end -->

  <div class="flex flex-col mx-5 mt-5">
    <div class="flex justify-between">
      <h2 class="text-sm text-neutral-500 dark:text-neutral-400 mb-10">
        Şirketinizde bulunan rolleri buradan yönetebilirsiniz.
      </h2>
      <button
        type="button"
        class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-1 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800 cursor-pointer"
        @click="isModalOpen = !isModalOpen"
      >
        Rol Oluştur
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
            <table class="min-w-full divide-y divide-gray-200 dark:divide-neutral-700">
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
                    Yapısal mı?
                  </th>
                  <th
                    scope="col"
                    class="px-6 py-3 text-start text-xs font-medium text-gray-500 uppercase dark:text-neutral-500 truncate"
                  >
                    Aciklama
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
                    class="px-6 py-3 text-end text-xs font-medium text-gray-500 uppercase dark:text-neutral-500"
                  >
                    İşlemler
                  </th>
                </tr>
              </thead>
              <tbody class="divide-y divide-gray-200 dark:divide-neutral-700">
                <tr v-for="rol in roller" :key="rol.id">
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
                    {{ rol.ad }}
                  </td>
                  <td
                    class="px-6 py-4 whitespace-nowrap text-sm text-gray-800 dark:text-neutral-200"
                  >
                    {{ rol.yapisalRolMu ? "Evet" : "Hayır" }}
                  </td>

                  <td
                    class="px-6 py-4 whitespace-nowrap text-sm text-gray-800 dark:text-neutral-200 md:max-w-full max-w-[200px] overflow-hidden text-ellipsis truncate"
                  >
                    {{ rol.aciklama }}
                  </td>
                  <td
                    class="px-6 py-4 whitespace-nowrap text-sm text-gray-800 dark:text-neutral-200"
                  >
                    {{ dayjs(rol.createdAt).format("D MMMM YYYY ") }}
                  </td>
                  <td
                    class="px-6 py-4 whitespace-nowrap text-sm text-gray-800 dark:text-neutral-200"
                  >
                    {{ rol.createUserName }}
                  </td>
                  <td class="px-6 text-center py-4 whitespace-nowrap text-sm font-medium">
                    <button
                      type="button"
                      class="text-center gap-x-2 text-sm font-semibold rounded-lg border border-transparent text-red-600 hover:text-red-800 focus:outline-hidden focus:text-red-800 disabled:opacity-50 disabled:pointer-events-none dark:text-red-500 dark:hover:text-red-400 dark:focus:text-red-400"
                      @click="deleteRole(rol.id)"
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
