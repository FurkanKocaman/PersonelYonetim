<script setup lang="ts">
import type { BordroCalisanlarUpdateCommand } from "@/models/request-models/BordroCalisanlarUpdateModel";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { BordroGetCalisanlarModel } from "@/models/response-models/BordroCalisanlarGetModel";
import type { KurumsalBirimGetModel } from "@/models/response-models/KurumsalBirimGetModel";
import BordroService from "@/services/BordroService";
import KurumsalBirimService from "@/services/KurumsalBirimService";
import { useToastStore } from "@/stores/ToastStore";
import { onMounted, ref, watch, type Ref } from "vue";

const bordroCalisanlar: Ref<BordroGetCalisanlarModel[] | undefined> = ref([]);

const isBirimlerMenuOpen = ref(false);
const isIslemlerMenuOpen = ref(false);
const selectedBirim = ref<string | undefined>(undefined);
const birimler: Ref<KurumsalBirimGetModel[] | undefined> = ref([]);
const personelId = ref([]);

const isLoading = ref(false);
const apiUrl = import.meta.env.VITE_API_URL;

const updateRequest: Ref<BordroCalisanlarUpdateCommand[]> = ref([]);

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 5,
  orderBy: undefined,
  filter: undefined,
});
onMounted(() => {
  getAllBirimler();
  getAllCalisanlar();
});

const getAllCalisanlar = async () => {
  if (selectedBirim.value) {
    paginationParams.value.filter = `birimAdi eq '${selectedBirim.value}'`;
  } else {
    paginationParams.value.filter = undefined;
  }
  const res = await BordroService.bordroGetCalisanlarAll(paginationParams.value);
  bordroCalisanlar.value = res?.items;
};

const getAllBirimler = async () => {
  const birimlerRes = await KurumsalBirimService.kurumsalBirimlerGet();
  birimler.value = birimlerRes?.items;
};

const bordroHesapla = async () => {
  isLoading.value = true;
  isIslemlerMenuOpen.value = false;
  if (personelId.value.length != 0) {
    const today = new Date();
    const res = await BordroService.bordroCreate(
      today.getFullYear(),
      new Date().getMonth() + 1,
      personelId.value,
      true
    );

    if (res) {
      isLoading.value = false;
    }
  } else {
    useToastStore().addToast(
      "Bordro hesaplaması için en az 1 personel seçmelisiniz",
      "",
      "error",
      5000,
      true
    );
  }
};

watch(selectedBirim, () => getAllCalisanlar());

defineExpose({
  bordroHesapla,
});
</script>

<template>
  <div class="flex flex-col w-full h-full px-5">
    <div class="flex">
      <div class="flex flex-1">
        <!-- Search box -->
        <div class="mr-10 relative">
          <input
            type="text"
            id="voice-search"
            class="bg-neutral-50 border border-neutral-300 outline-none text-gray-900 text-sm rounded-md block w-full pr-8 p-2.5 dark:bg-neutral-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
            placeholder="Ara"
            required
          />
          <button type="button" class="absolute inset-y-0 end-0 flex items-center pe-3">
            <svg
              class="size-5 text-gray-500 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white fill-none"
              viewBox="0 0 24 24"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M15.7955 15.8111L21 21M18 10.5C18 14.6421 14.6421 18 10.5 18C6.35786 18 3 14.6421 3 10.5C3 6.35786 6.35786 3 10.5 3C14.6421 3 18 6.35786 18 10.5Z"
                class="stroke-2 stroke-neutral-600 dark:stroke-neutral-300"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
            </svg>
          </button>
        </div>

        <div class="relative">
          <button
            id="dropdownBgHoverButton"
            class="text-neutral-700 border border-neutral-300 dark:border-gray-600 bg-neutral-50 cursor-pointer outline-none focus:outline-none font-medium rounded-md text-sm px-5 py-2.5 text-center inline-flex items-center dark:bg-neutral-700 dark:hover:bg-neutral-600 dark:text-neutral-200"
            type="button"
            @click="() => (isBirimlerMenuOpen = !isBirimlerMenuOpen)"
          >
            Birimler
            <svg
              class="w-2.5 h-2.5 ms-3"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 10 6"
            >
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m1 1 4 4 4-4"
              />
            </svg>
          </button>

          <div
            v-if="isBirimlerMenuOpen"
            class="absolute z-50 bg-neutral-200 dark:bg-neutral-700 rounded-md"
          >
            <ul
              class="p-1 w-[10rem] text-sm text-gray-700 dark:text-gray-200 select-none max-h-[40dvh] overflow-y-auto"
            >
              <li
                class="cursor-pointer"
                @click="
                  () => {
                    selectedBirim = undefined;
                  }
                "
              >
                <div
                  class="flex items-center px-2 py-2 rounded-sm hover:bg-gray-50 dark:hover:bg-neutral-600"
                >
                  Hepsi
                </div>
              </li>
              <li
                v-for="birim in birimler"
                :key="birim.id"
                class="cursor-pointer"
                @click="
                  () => {
                    selectedBirim = birim.ad;
                  }
                "
              >
                <div
                  class="flex items-center px-2 py-2 rounded-sm hover:bg-gray-50 dark:hover:bg-neutral-600"
                >
                  {{ birim.ad }}
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div class="relative">
        <button
          class="flex justify-center items-center text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-md text-sm px-4 py-1 text-center me-2 mb-2 group dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
          @click="
            () => {
              isIslemlerMenuOpen = !isIslemlerMenuOpen;
            }
          "
        >
          İşlemler
          <svg class="size-7 fill-none" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path
              d="M7 10L12 15L17 10"
              class="stroke-blue-700 group-hover:stroke-white"
              stroke-width="1.5"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
          </svg>
        </button>
        <div
          v-if="isIslemlerMenuOpen"
          class="absolute bg-neutral-50 dark:bg-neutral-800 z-20 rounded-md border border-blue-600"
        >
          <button
            class="font-semibold text-sm hover:bg-blue-600 hover:text-neutral-100 px-3 py-1 rounded-md"
            @click="bordroHesapla()"
          >
            Bordro Hesapla
          </button>
        </div>
      </div>
    </div>
    <div class="relative overflow-x-auto mt-5">
      <div class="">
        <table class="table-fixed border-collapse w-full text-sm">
          <thead
            class="bg-neutral-100 dark:bg-neutral-800 text-neutral-700 dark:text-neutral-300 border-b border-gray-200 dark:border-neutral-500"
          >
            <tr class="">
              <th class="sticky bg-neutral-100 dark:bg-neutral-800 left-0 px-2 py-2 w-10 z-20">
                <div class="flex items-center justify-center h-full">
                  <input type="checkbox" class="w-4 h-4" />
                </div>
              </th>

              <th
                class="sticky bg-neutral-100 dark:bg-neutral-800 left-10 px-4 py-2 w-60 z-20 text-left"
              >
                Adı
              </th>

              <th
                class="sticky bg-neutral-100 dark:bg-neutral-800 left-[13rem] px-4 py-2 w-24 z-20 text-left border-r border-neutral-300 dark:border-neutral-700"
              >
                <div class="flex">
                  Dahil

                  <svg
                    class="size-5 fill-none ml-1"
                    viewBox="0 0 24 24"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M10.125 8.875C10.125 7.83947 10.9645 7 12 7C13.0355 7 13.875 7.83947 13.875 8.875C13.875 9.56245 13.505 10.1635 12.9534 10.4899C12.478 10.7711 12 11.1977 12 11.75V13"
                      class="stroke-neutral-700 dark:stroke-neutral-200 stroke-2"
                      stroke-linecap="round"
                    />
                    <circle cx="12" cy="16" r="1" class="fill-neutral-700 dark:fill-neutral-200" />
                    <path
                      d="M7 3.33782C8.47087 2.48697 10.1786 2 12 2C17.5228 2 22 6.47715 22 12C22 17.5228 17.5228 22 12 22C6.47715 22 2 17.5228 2 12C2 10.1786 2.48697 8.47087 3.33782 7"
                      class="stroke-neutral-700 dark:stroke-neutral-200 stroke-2"
                      stroke-linecap="round"
                    />
                  </svg>
                </div>
              </th>

              <th class="px-4 py-2 w-40 text-left">TCKN</th>
              <th class="px-4 py-2 w-40 text-left">İşe Başlama Tarihi</th>
              <th class="px-4 py-2 w-40 text-left">İşten Çıkış Tarihi</th>
              <th class="px-4 py-2 w-40 text-left">Engel Derecesi</th>
              <th class="px-4 py-2 w-40 text-left">Tabi Olduğu Kanun</th>
              <th class="px-4 py-2 w-40 text-left">SGK İş Yeri</th>
              <th class="px-4 py-2 w-40 text-left">Vergi Dairesi Adı</th>
              <th class="px-4 py-2 w-40 text-left">Kümülatif Vergi Matrahı</th>
              <!-- Burada üst birimler de yazdırılabilir -->
              <th class="px-4 py-2 w-40 text-left">Birim</th>
              <th class="px-4 py-2 w-40 text-left">Pozisyon</th>
              <th class="px-4 py-2 w-40 text-left">Meslek Kodu</th>
            </tr>
          </thead>
          <tbody
            class="divide-y divide-gray-200 dark:divide-neutral-500 text-neutral-800 dark:text-neutral-200"
          >
            <tr
              v-for="calisan in bordroCalisanlar"
              :key="calisan.id"
              class="bg-neutral-100 dark:bg-neutral-800 hover:bg-gray-50 dark:hover:bg-neutral-600/40"
            >
              <!-- Checkbox -->
              <td class="sticky left-0 bg-neutral-100 dark:bg-neutral-800 px-2 py-2 w-10 z-10">
                <div class="flex items-center justify-center h-full">
                  <input type="checkbox" class="w-4 h-4" />
                </div>
              </td>

              <!-- Adı -->
              <td
                class="sticky left-10 h-full bg-neutral-100 dark:bg-neutral-800 px-4 py-3 z-10 flex items-center"
              >
                <img
                  v-if="calisan.avatarUrl"
                  class="size-8 rounded-md object-cover mr-2"
                  width="100"
                  height="100"
                  :src="apiUrl + calisan.avatarUrl"
                  alt=""
                />
                <div
                  v-else
                  class="text-xl font-semibold text-sky-600 transition-all duration-300 ease-in-out mr-2 rounded-md border-1 border-sky-500 size-8 flex items-center justify-center"
                >
                  {{ calisan.fullName[0] }}
                </div>
                {{ calisan.fullName }}
              </td>

              <!-- Dahil -->
              <td
                class="sticky left-[14rem] bg-neutral-100 dark:bg-neutral-800 px-4 py-2 z-10 border-r border-neutral-300 dark:border-neutral-700"
              >
                <label class="inline-flex items-center cursor-pointer">
                  <input
                    type="checkbox"
                    :value="calisan.id"
                    v-model="personelId"
                    class="sr-only peer"
                  />
                  <div
                    class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-blue-300 dark:peer-focus:ring-blue-800 rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                  ></div>
                </label>
              </td>

              <td class="px-4 py-2 w-40">
                <input
                  class="border-b-1 text-sm border-neutral-400 w-full px-2 py-1.5 bg-neutral-200 dark:bg-neutral-700 focus:outline-none"
                  type="text"
                  :value="calisan.tckn"
                />
              </td>
              <td class="px-4 py-2 w-40">
                {{
                  calisan.iseBaslangicTarihi != null
                    ? new Date(calisan.iseBaslangicTarihi!).toLocaleString("tr-TR", {
                        day: "2-digit",
                        month: "2-digit",
                        year: "numeric",
                        hour: "2-digit",
                        minute: "2-digit",
                        hour12: false,
                      })
                    : "-"
                }}
              </td>
              <td class="px-4 py-2 w-40">
                {{
                  calisan.istenCikisTarihi != null
                    ? new Date(calisan.istenCikisTarihi!).toLocaleString("tr-TR", {
                        day: "2-digit",
                        month: "2-digit",
                        year: "numeric",
                        hour: "2-digit",
                        minute: "2-digit",
                        hour12: false,
                      })
                    : "-"
                }}
              </td>
              <td class="px-4 py-2 w-40">%{{ calisan.engelDerecesi }}</td>
              <td class="px-4 py-2 w-40">
                <input
                  class="border-b-1 text-sm border-neutral-400 w-full px-1 py-1.5 bg-neutral-200 dark:bg-neutral-700 focus:outline-none"
                  type="text"
                  :value="calisan.tabiOlduguKanun"
                />
              </td>
              <td class="px-4 py-2 w-40">
                <input
                  class="border-b-1 text-sm border-neutral-400 w-full px-1 py-1.5 bg-neutral-200 dark:bg-neutral-700 focus:outline-none"
                  type="text"
                  :value="calisan.SGKIsyeri"
                />
              </td>
              <td class="px-4 py-2 w-40">
                <input
                  class="border-b-1 text-sm border-neutral-400 w-full px-2 py-1.5 bg-neutral-200 dark:bg-neutral-700 focus:outline-none"
                  type="text"
                  :value="calisan.vergiDairesiAdi"
                />
              </td>

              <td class="px-4 py-2 w-40">{{ calisan.kumulatifVergiMatrahi }}</td>
              <td class="px-4 py-2 w-40">{{ calisan.birimAdi }}</td>
              <td class="px-4 py-2 w-40">{{ calisan.pozisyonAd }}</td>
              <td class="px-4 py-2 w-40">
                <input
                  class="border-b-1 text-sm border-neutral-400 w-full px-2 py-1.5 bg-neutral-200 dark:bg-neutral-700 focus:outline-none"
                  type="text"
                  :value="calisan.meslekKodu"
                />
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
