<script setup lang="ts">
import type { DepartmanModel } from "@/models/entity-models/DepartmanModel";
import type { PozisyonModel } from "@/models/entity-models/PozisyonModel";
import type { SirketModel } from "@/models/entity-models/SirketModel";
import type { SubeModel } from "@/models/entity-models/SubeModel";
import DepartmanService from "@/services/DepartmanService";
import PozisyonService from "@/services/PozisyonService";
import SirketService from "@/services/SirketService";
import SubeService from "@/services/SubeService";
import { computed, onMounted, ref, type Ref } from "vue";
import TableLayout from "@/components/TableLayout.vue";
import SirketCreateModal from "@/components/modals/SirketCreateModal.vue";
import SubeCreateModal from "@/components/modals/SubeCreateModal.vue";
import DepartmanCreateModal from "@/components/modals/DepartmanCreateModal.vue";
import PozisyonCreateModal from "@/components/modals/PozisyonCreateModal.vue";
import type { PaginationParams } from "@/models/request-models/PaginationParams";

const expand = ref({
  sirketler: false,
  subeler: false,
  departmanlar: false,
  pozisyonlar: false,
});
const loading = ref({
  sirketler: false,
  subeler: false,
  departmanlar: false,
  pozisyonlar: false,
});
const count = ref({
  sirketler: 0,
  subeler: 0,
  departmanlar: 0,
  pozisyonlar: 0,
});
const showModal = ref({
  sirketler: false,
  subeler: false,
  departmanlar: false,
  pozisyonlar: false,
});

const sirketler: Ref<SirketModel[] | undefined> = ref([]);
const subeler: Ref<SubeModel[] | undefined> = ref([]);
const departmanlar: Ref<DepartmanModel[] | undefined> = ref([]);
const pozisyonlar: Ref<PozisyonModel[] | undefined> = ref([]);

const selectedSirket: Ref<SirketModel | undefined> = ref(undefined);
const selectedSube: Ref<SubeModel | undefined> = ref(undefined);
const selectedDepartman: Ref<DepartmanModel | undefined> = ref(undefined);
const selectedPozisyon: Ref<PozisyonModel | undefined> = ref(undefined);

const paginationParamsSirket: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});
const paginationParamsSube: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});
const paginationParamsDepartman: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});
const paginationParamsPozisyon: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

const filteredSirketler = computed<Record<string, unknown>[]>(() => {
  return (sirketler.value || []).map(
    ({ id, ad, iletisim, adres, createUserName, createdAt, isActive }) => ({
      id,
      ad,
      eposta: iletisim.eposta,
      adres: adres.sehir,
      createdAt: new Date(createdAt),
      createUserName,
      isActive: isActive ? "Aktif" : "Pasif", // "isActive" değerini metne dönüştür
    })
  );
});

const filteredSubeler = computed<Record<string, unknown>[]>(() => {
  return (subeler.value || []).map(
    ({ id, ad, iletisim, adres, createdAt, createUserName, sirketAd, isActive }) => ({
      id,
      ad,
      eposta: iletisim.eposta,
      adres: adres.sehir,
      createdAt: new Date(createdAt),
      createUserName: createUserName,
      sirketAd,
      isActive: isActive ? "Aktif" : "Pasif",
    })
  );
});
const filteredDepartmanlar = computed<Record<string, unknown>[]>(() => {
  return (departmanlar.value || []).map(
    ({ id, ad, createdAt, createUserName, subeAd, sirketAd, isActive }) => ({
      id,
      ad,
      createdAt: new Date(createdAt),
      createUserName: createUserName,
      subeAd,
      sirketAd,
      isActive: isActive ? "Aktif" : "Pasif",
    })
  );
});

const filteredPozisyonlar = computed<Record<string, unknown>[]>(() => {
  return (pozisyonlar.value || []).map(
    ({ id, ad, createdAt, createUserName, sirketAd, isActive }) => ({
      id,
      ad,
      createdAt: new Date(createdAt),
      createUserName: createUserName,
      sirketAd,
      isActive: isActive ? "Aktif" : "Pasif",
    })
  );
});

onMounted(async () => {
  getSirketler();
  getSubeler();
  getDepartmanlar();
  getPozisyonlar();
});
const getSirketler = async () => {
  loading.value.sirketler = true;
  try {
    const res = await SirketService.sirketlerGet(paginationParamsSirket.value);
    count.value.sirketler = res!.count;
    sirketler.value = res?.items;
    paginationParamsSirket.value.count = res!.count;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  } finally {
    loading.value.sirketler = false;
  }
};

const getSubeler = async () => {
  loading.value.subeler = true;
  try {
    const res = await SubeService.subelerGet("", paginationParamsSube.value);
    count.value.subeler = res!.count;
    subeler.value = res?.items;
    paginationParamsSube.value.count = res!.count;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  } finally {
    loading.value.subeler = false;
  }
};

const getDepartmanlar = async () => {
  loading.value.departmanlar = true;
  try {
    const res = await DepartmanService.departmanlarGet("", paginationParamsDepartman.value);
    count.value.departmanlar = res!.count;
    departmanlar.value = res?.items;
    paginationParamsDepartman.value.count = res!.count;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  } finally {
    loading.value.departmanlar = false;
  }
};
const getPozisyonlar = async () => {
  loading.value.pozisyonlar = true;
  try {
    const res = await PozisyonService.pozisyonlarGet("", paginationParamsPozisyon.value);
    count.value.pozisyonlar = res!.count;
    pozisyonlar.value = res?.items;
    paginationParamsPozisyon.value.count = res!.count;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  } finally {
    loading.value.pozisyonlar = false;
  }
};
const openSirketModal = (sirket: SirketModel) => {
  selectedSirket.value = sirketler.value?.find((p) => p.id == sirket.id);
  showModal.value.sirketler = true;
};
const openSubeModal = (sube: SubeModel) => {
  selectedSube.value = subeler.value?.find((p) => p.id == sube.id);
  showModal.value.subeler = true;
};
const openDepartmanModal = (departman: DepartmanModel) => {
  selectedDepartman.value = departmanlar.value?.find((p) => p.id == departman.id);
  showModal.value.departmanlar = true;
};
const openPozisyonModal = (pozisyon: PozisyonModel) => {
  selectedPozisyon.value = pozisyonlar.value?.find((p) => p.id == pozisyon.id);
  showModal.value.pozisyonlar = true;
};

const setPageNumberSirket = (pageNumber: number) => {
  if (pageNumber != paginationParamsSirket.value.pageNumber) {
    paginationParamsSirket.value.pageNumber = pageNumber;
    getSirketler();
  }
};
const setPageNumberSube = (pageNumber: number) => {
  if (pageNumber != paginationParamsSube.value.pageNumber) {
    paginationParamsSube.value.pageNumber = pageNumber;
    getSubeler();
  }
};
const setPageNumberDepartman = (pageNumber: number) => {
  if (pageNumber != paginationParamsDepartman.value.pageNumber) {
    paginationParamsDepartman.value.pageNumber = pageNumber;
    getDepartmanlar();
  }
};
const setPageNumberPozisyon = (pageNumber: number) => {
  if (pageNumber != paginationParamsPozisyon.value.pageNumber) {
    paginationParamsPozisyon.value.pageNumber = pageNumber;
    getPozisyonlar();
  }
};
</script>

<template>
  <div class="flex flex-col">
    <div class="w-full mt-2 ml-5">
      <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">
        Şirketteki birimleri buradan düzenleyebilirsiniz.
      </p>
    </div>
    <div class="flex flex-col mt-5">
      <div
        class="mx-10 my-3 p-1 bg-neutral-200/30 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300 select-none"
      >
        <div
          class="flex justify-between items-center p-2"
          @click="
            () => {
              expand.sirketler = !expand.sirketler;
            }
          "
        >
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="size-6 cursor-pointer select-none transform transition-transform duration-200"
              :class="{ 'rotate-90': expand.sirketler }"
              xmlns="http://www.w3.org/2000/svg"
              @click.stop="
                () => {
                  expand.sirketler = !expand.sirketler;
                }
              "
            >
              <path
                d="M903.232 256l56.768 50.432L512 768 64 306.432 120.768 256 512 659.072z"
                class="fill-current"
              />
            </svg>
            <span class="text-xl font-medium ml-2">Şirketler</span>
          </div>
          <div class="flex items-center">
            <div
              class="bg-blue-600 size-6 flex items-center justify-center text-neutral-200 py-1 px-2 rounded-full text-xs font-semibold mr-2"
            >
              <span class="text-xs">{{ count.sirketler || 0 }}</span>
            </div>
            <button
              type="button"
              class="cursor-pointer text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
              @click.stop="
                () => {
                  showModal.sirketler = !showModal.sirketler;
                }
              "
            >
              Yeni Ekle
            </button>
          </div>
        </div>
        <div v-if="loading.sirketler" class="w-full flex items-center justify-center py-4">
          <div
            class="size-7 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
          ></div>
        </div>
        <SirketCreateModal
          :sirket="selectedSirket"
          :edit-mode="selectedSirket ? true : false"
          @close-modal="
            (p) => {
              showModal.sirketler = p;

              selectedSirket = undefined;
            }
          "
          @refresh="getSirketler"
          v-if="showModal.sirketler"
        />
        <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
          <TableLayout
            v-if="expand.sirketler && !loading.sirketler"
            {
            id,
            ad,
            iletisim,
            adres,
            createdAt,
            createUserName,
            sirketAd,
            isActive
            }
            :tableHeaders="[
              { key: 'ad', value: 'Ad' },
              { key: 'eposta', value: 'Eposta' },
              { key: 'adres', value: 'Adres' },
              { key: 'createdAt', value: 'Oluşturulma Tarihi' },
              { key: 'createUserName', value: 'Oluşturan' },
              { key: 'sirketAd', value: 'Şirket' },
              { key: 'isActive', value: 'Durum' },
            ]"
            :tableContent="filteredSirketler"
            :islemler="['edit', 'detaylar']"
            @edit-click="openSirketModal"
            :page-count="
              Math.ceil(paginationParamsSirket.count / paginationParamsSirket.pageSize) == 0
                ? 1
                : Math.ceil(paginationParamsSirket.count / paginationParamsSirket.pageSize)
            "
            :count="paginationParamsSirket.count"
            :page-size="
              paginationParamsSirket.pageSize > paginationParamsSirket.count
                ? paginationParamsSirket.count
                : paginationParamsSirket.pageSize
            "
            :current-page="paginationParamsSirket.pageNumber"
            @set-page="setPageNumberSirket"
          />
        </div>
      </div>
      <div
        class="mx-10 p-1 my-3 bg-neutral-200/30 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300 select-none"
      >
        <div
          class="flex justify-between items-center p-2"
          @click="
            () => {
              expand.subeler = !expand.subeler;
            }
          "
        >
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="size-6 cursor-pointer select-none transform transition-transform duration-200"
              :class="{ 'rotate-90': expand.subeler }"
              xmlns="http://www.w3.org/2000/svg"
              @click.stop="
                () => {
                  expand.subeler = !expand.subeler;
                }
              "
            >
              <path
                d="M903.232 256l56.768 50.432L512 768 64 306.432 120.768 256 512 659.072z"
                class="fill-current"
              />
            </svg>
            <span class="text-xl font-medium ml-2">Şubeler</span>
          </div>
          <div class="flex items-center">
            <div
              class="bg-blue-600 size-6 flex items-center justify-center text-neutral-200 py-1 px-2 rounded-full text-xs font-semibold mr-2"
            >
              <span class="text-xs">{{ count.subeler || 0 }}</span>
            </div>
            <button
              type="button"
              class="cursor-pointer text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
              @click.stop="
                () => {
                  showModal.subeler = !showModal.subeler;
                }
              "
            >
              Yeni Ekle
            </button>
          </div>
        </div>
        <div v-if="loading.subeler" class="w-full flex items-center justify-center py-4">
          <div
            class="size-7 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
          ></div>
        </div>
        <SubeCreateModal
          :sirketler="sirketler!"
          :sube="selectedSube"
          :edit-mode="selectedSube ? true : false"
          @close-modal="
            (p) => {
              showModal.subeler = p;
              selectedSube = undefined;
            }
          "
          @refresh="getSubeler"
          v-if="showModal.subeler"
        />

        <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
          <TableLayout
            v-if="expand.subeler && !loading.subeler"
            :tableHeaders="[
              { key: 'ad', value: 'Ad' },
              { key: 'eposta', value: 'Eposta' },
              { key: 'adres', value: 'Adres' },
              { key: 'createdAt', value: 'Oluşturulma Tarihi' },
              { key: 'createUserName', value: 'Oluşturan' },
              { key: 'sirketAd', value: 'Şirket' },
              { key: 'isActive', value: 'Durum' },
            ]"
            :tableContent="filteredSubeler"
            :islemler="['edit', 'detaylar']"
            @edit-click="openSubeModal"
            :page-count="
              Math.ceil(paginationParamsSube.count / paginationParamsSube.pageSize) == 0
                ? 1
                : Math.ceil(paginationParamsSube.count / paginationParamsSube.pageSize)
            "
            :count="paginationParamsSube.count"
            :page-size="
              paginationParamsSube.pageSize > paginationParamsSube.count
                ? paginationParamsSube.count
                : paginationParamsSube.pageSize
            "
            :current-page="paginationParamsSube.pageNumber"
            @set-page="setPageNumberSube"
          />
        </div>
      </div>
      <div
        class="mx-10 p-1 my-3 bg-neutral-200/30 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300 select-none"
      >
        <div
          class="flex justify-between items-center p-2"
          @click="
            () => {
              expand.departmanlar = !expand.departmanlar;
            }
          "
        >
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="size-6 cursor-pointer select-none transform transition-transform duration-200"
              :class="{ 'rotate-90': expand.departmanlar }"
              xmlns="http://www.w3.org/2000/svg"
              @click.stop="
                () => {
                  expand.departmanlar = !expand.departmanlar;
                }
              "
            >
              <path
                d="M903.232 256l56.768 50.432L512 768 64 306.432 120.768 256 512 659.072z"
                class="fill-current"
              />
            </svg>
            <span class="text-xl font-medium ml-2">Departmanlar</span>
          </div>
          <div class="flex items-center">
            <div
              class="bg-blue-600 size-6 flex items-center justify-center text-neutral-200 py-1 px-2 rounded-full text-xs font-semibold mr-2"
            >
              <span class="text-xs">{{ count.departmanlar || 0 }}</span>
            </div>
            <button
              type="button"
              class="cursor-pointer text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
              @click.stop="
                () => {
                  showModal.departmanlar = !showModal.departmanlar;
                }
              "
            >
              Yeni Ekle
            </button>
          </div>
        </div>
        <div v-if="loading.departmanlar" class="w-full flex items-center justify-center py-4">
          <div
            class="size-7 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
          ></div>
        </div>

        <DepartmanCreateModal
          :subeler="subeler!"
          :departman="selectedDepartman"
          :edit-mode="selectedDepartman ? true : false"
          @close-modal="
            (p) => {
              showModal.departmanlar = p;
              getDepartmanlar();
            }
          "
          v-if="showModal.departmanlar"
        />

        <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
          <TableLayout
            v-if="expand.departmanlar && !loading.departmanlar"
            :tableHeaders="[
              { key: 'ad', value: 'Ad' },
              { key: 'createdAt', value: 'Oluşturulma Tarihi' },
              { key: 'createUserName', value: 'Oluşturan' },
              { key: 'subeAd', value: 'Şube' },
              { key: 'sirketAd', value: 'Şirket' },
              { key: 'isActive', value: 'Durum' },
            ]"
            :tableContent="filteredDepartmanlar"
            :islemler="['edit', 'detaylar']"
            @edit-click="openDepartmanModal"
            :page-count="
              Math.ceil(paginationParamsDepartman.count / paginationParamsDepartman.pageSize) == 0
                ? 1
                : Math.ceil(paginationParamsDepartman.count / paginationParamsDepartman.pageSize)
            "
            :count="paginationParamsDepartman.count"
            :page-size="
              paginationParamsDepartman.pageSize > paginationParamsDepartman.count
                ? paginationParamsDepartman.count
                : paginationParamsDepartman.pageSize
            "
            :current-page="paginationParamsDepartman.pageNumber"
            @set-page="setPageNumberDepartman"
          />
        </div>
      </div>
      <div
        class="mx-10 p-1 my-3 bg-neutral-200/30 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300 select-none"
      >
        <div
          class="flex justify-between items-center p-2"
          @click="
            () => {
              expand.pozisyonlar = !expand.pozisyonlar;
            }
          "
        >
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="size-6 cursor-pointer select-none transform transition-transform duration-200"
              :class="{ 'rotate-90': expand.pozisyonlar }"
              xmlns="http://www.w3.org/2000/svg"
              @click.stop="
                () => {
                  expand.pozisyonlar = !expand.pozisyonlar;
                }
              "
            >
              <path
                d="M903.232 256l56.768 50.432L512 768 64 306.432 120.768 256 512 659.072z"
                class="fill-current"
              />
            </svg>
            <span class="text-xl font-medium ml-2">Pozisyonlar</span>
          </div>
          <div class="flex items-center">
            <div
              class="bg-blue-600 size-6 flex items-center justify-center text-neutral-200 py-1 px-2 rounded-full text-xs font-semibold mr-2"
            >
              <span class="text-xs">{{ count.pozisyonlar || 0 }}</span>
            </div>
            <button
              type="button"
              class="cursor-pointer text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
              @click.stop="
                () => {
                  showModal.pozisyonlar = !showModal.pozisyonlar;
                }
              "
            >
              Yeni Ekle
            </button>
          </div>
        </div>
        <div v-if="loading.pozisyonlar" class="w-full flex items-center justify-center py-4">
          <div
            class="size-7 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
          ></div>
        </div>

        <PozisyonCreateModal
          :sirketler="sirketler!"
          :pozisyon="selectedPozisyon"
          :edit-mode="selectedPozisyon ? true : false"
          @close-modal="
            (p) => {
              showModal.pozisyonlar = p;
              getPozisyonlar();
            }
          "
          v-if="showModal.pozisyonlar"
        />
        <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
          <TableLayout
            v-if="expand.pozisyonlar && !loading.pozisyonlar"
            {
            id,
            ad,
            createdAt,
            createUserName,
            sirketAd,
            isActive
            }
            :tableHeaders="[
              { key: 'ad', value: 'Ad' },
              { key: 'createdAt', value: 'Oluşturma Tarihi' },
              { key: 'createUserName', value: 'Oluşturan' },
              { key: 'sirketAd', value: 'Şirket' },
              { key: 'isActive', value: 'Durum' },
            ]"
            :tableContent="filteredPozisyonlar"
            :islemler="['edit', 'detaylar']"
            @edit-click="openPozisyonModal"
            :page-count="
              Math.ceil(paginationParamsPozisyon.count / paginationParamsPozisyon.pageSize) == 0
                ? 1
                : Math.ceil(paginationParamsPozisyon.count / paginationParamsPozisyon.pageSize)
            "
            :count="paginationParamsPozisyon.count"
            :page-size="
              paginationParamsPozisyon.pageSize > paginationParamsPozisyon.count
                ? paginationParamsPozisyon.count
                : paginationParamsPozisyon.pageSize
            "
            :current-page="paginationParamsPozisyon.pageNumber"
            @set-page="setPageNumberPozisyon"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* İhtiyaç duyulursa ek stiller buraya */
</style>
