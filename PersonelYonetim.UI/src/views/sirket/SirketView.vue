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
import { useToastStore } from "@/stores/ToastStore";

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

const filteredSirketler = computed<Record<string, unknown>[]>(() => {
  return (sirketler.value || []).map(
    ({ ad, iletisim, adres, createUserName, createdAt, isActive }) => ({
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
    ({ ad, iletisim, adres, createdAt, createUserName, sirketAd, isActive }) => ({
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
    ({ ad, createdAt, createUserName, subeAd, sirketAd, isActive }) => ({
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
  return (pozisyonlar.value || []).map(({ ad, createdAt, createUserName, sirketAd, isActive }) => ({
    ad,
    createdAt: new Date(createdAt),
    createUserName: createUserName,
    sirketAd,
    isActive: isActive ? "Aktif" : "Pasif",
  }));
});

onMounted(async () => {
  const toastStore = useToastStore();
  console.log(toastStore.toasts);
  getSirketler();
  getSubeler();
  getDepartmanlar();
  getPozisyonlar();
});
const getSirketler = async () => {
  loading.value.sirketler = true;
  try {
    const res = await SirketService.sirketlerGet();
    count.value.sirketler = res!.count;
    sirketler.value = res?.Sirketler;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  } finally {
    loading.value.sirketler = false;
  }
};

const getSubeler = async () => {
  loading.value.subeler = true;
  try {
    const res = await SubeService.subelerGet("");
    count.value.subeler = res!.count;
    subeler.value = res?.Subeler;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  } finally {
    loading.value.subeler = false;
  }
};

const getDepartmanlar = async () => {
  loading.value.departmanlar = true;
  try {
    const res = await DepartmanService.departmanlarGet("");
    count.value.departmanlar = res!.count;
    departmanlar.value = res?.Departmanlar;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  } finally {
    loading.value.departmanlar = false;
  }
};
const getPozisyonlar = async () => {
  loading.value.pozisyonlar = true;
  try {
    const res = await PozisyonService.pozisyonlarGet("");
    count.value.pozisyonlar = res!.count;
    pozisyonlar.value = res?.Pozisyonlar;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  } finally {
    loading.value.pozisyonlar = false;
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
        class="mx-10 my-3 p-1 bg-neutral-200 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300 select-none"
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
          @close-modal="
            (p) => {
              showModal.sirketler = p;
              getSirketler();
            }
          "
          v-if="showModal.sirketler"
        />

        <TableLayout
          v-if="expand.sirketler && !loading.sirketler"
          :tableHeaders="['Ad', 'Eposta', 'Adres', 'Oluşturulma Tarihi', 'Oluşturan', 'Durum']"
          :tableContent="filteredSirketler"
          :islemler="['edit', 'detaylar']"
        />
      </div>
      <div
        class="mx-10 p-1 my-3 bg-neutral-200 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300 select-none"
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
          @close-modal="
            (p) => {
              showModal.subeler = p;
              getSubeler();
            }
          "
          v-if="showModal.subeler"
        />

        <TableLayout
          v-if="expand.subeler && !loading.subeler"
          :tableHeaders="[
            'Ad',
            'Eposta',
            'Adres',
            'Oluşturulma Tarihi',
            'Oluşturan',
            'Şirket',
            'Durum',
          ]"
          :tableContent="filteredSubeler"
          :islemler="['edit', 'detaylar']"
        />
      </div>
      <div
        class="mx-10 p-1 my-3 bg-neutral-200 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300 select-none"
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
          @close-modal="
            (p) => {
              showModal.departmanlar = p;
              getDepartmanlar();
            }
          "
          v-if="showModal.departmanlar"
        />

        <TableLayout
          v-if="expand.departmanlar && !loading.departmanlar"
          :tableHeaders="['Ad', 'Oluşturulma Tarihi', 'Oluşturan', 'Şube', 'Şirket', 'Durum']"
          :tableContent="filteredDepartmanlar"
          :islemler="['edit', 'detaylar']"
        />
      </div>
      <div
        class="mx-10 p-1 my-3 bg-neutral-200 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300 select-none"
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
          @close-modal="
            (p) => {
              showModal.pozisyonlar = p;
              getPozisyonlar();
            }
          "
          v-if="showModal.pozisyonlar"
        />

        <TableLayout
          v-if="expand.pozisyonlar && !loading.pozisyonlar"
          :tableHeaders="['Ad', 'Oluşturulma Tarihi', 'Oluşturan', 'Şirket', 'Durum']"
          :tableContent="filteredPozisyonlar"
          :islemler="['edit', 'detaylar']"
        />
      </div>
    </div>
  </div>
</template>

<style scoped>
/* İhtiyaç duyulursa ek stiller buraya */
</style>
