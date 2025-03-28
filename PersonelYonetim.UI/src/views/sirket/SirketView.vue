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
import SirketDetailModal from "@/components/modals/SirketDetailModal.vue";
import SubeDetailModal from "@/components/modals/SubeDetailModal.vue";
import DepartmanDetailModal from "@/components/modals/DepartmanDetailModal.vue";
import PozisyonDetailModal from "@/components/modals/PozisyonDetailModal.vue";

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

const showDetailModal = ref({
  sirketler: false,
  subeler: false,
  departmanlar: false,
  pozisyonlar: false,
});

const editMode = ref(false);
const selectedSirket = ref<SirketModel | null>(null);
const selectedSube = ref<SubeModel | null>(null);
const selectedDepartman = ref<DepartmanModel | null>(null);
const selectedPozisyon = ref<PozisyonModel | null>(null);

const sirketler: Ref<SirketModel[] | undefined> = ref([]);
const subeler: Ref<SubeModel[] | undefined> = ref([]);
const departmanlar: Ref<DepartmanModel[] | undefined> = ref([]);
const pozisyonlar: Ref<PozisyonModel[] | undefined> = ref([]);

const filteredSirketler = computed<Record<string, unknown>[]>(() => {
  return (sirketler.value || []).map(
    ({ id, ad, iletisim, adres, createUserName, createdAt, isActive }) => ({
      id,
      ad,
      eposta: iletisim.eposta,
      telefon: iletisim.telefon,
      adres: `${adres.ilce}, ${adres.sehir}`,
      createUserName,
      createdAt: new Date(createdAt).toLocaleDateString("tr-TR"),
      isActive: isActive ? "Aktif" : "Pasif",
      actions: {
        detail: (row: any) => handleSirketDetail(row),
        edit: (row: any) => handleSirketEdit(row),
      },
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
      createdAt: new Date(createdAt).toDateString(),
      createUserName: createUserName,
      sirketAd,
      isActive: isActive ? "Aktif" : "Pasif",
      actions: {
        detail: (row: any) => handleSubeDetail(row),
        edit: (row: any) => handleSubeEdit(row),
      },
    })
  );
});
const filteredDepartmanlar = computed<Record<string, unknown>[]>(() => {
  return (departmanlar.value || []).map(
    ({ id, ad, createdAt, createUserName, subeAd, sirketAd, isActive }) => ({
      id,
      ad,
      createdAt: new Date(createdAt).toDateString(),
      createUserName: createUserName,
      subeAd,
      sirketAd,
      isActive: isActive ? "Aktif" : "Pasif",
      actions: {
        detail: (row: any) => handleDepartmanDetail(row),
        edit: (row: any) => handleDepartmanEdit(row),
      },
    })
  );
});

const filteredPozisyonlar = computed<Record<string, unknown>[]>(() => {
  return (pozisyonlar.value || []).map(({ id, ad, createdAt, createUserName, sirketAd, isActive }) => ({
    id,
    ad,
    createdAt: new Date(createdAt).toDateString(),
    createUserName: createUserName,
    sirketAd,
    isActive: isActive ? "Aktif" : "Pasif",
    actions: {
      detail: (row: any) => handlePozisyonDetail(row),
      edit: (row: any) => handlePozisyonEdit(row),
    },
  }));
});

onMounted(async () => {
  console.log("mounted");
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
    console.log("Sirketler", sirketler);
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
    console.log(res);
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
    console.log(res);
    count.value.pozisyonlar = res!.count;
    pozisyonlar.value = res?.Pozisyonlar;
    console.log(pozisyonlar.value);
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  } finally {
    loading.value.pozisyonlar = false;
  }
};

// Şirket detay fonksiyonu
const handleSirketDetail = (row: any) => {
  const sirket = sirketler.value?.find((s) => s.id === row.id);
  if (sirket) {
    selectedSirket.value = sirket;
    showDetailModal.value.sirketler = true;
  }
};

// Şirket düzenleme fonksiyonu
const handleSirketEdit = (row: any) => {
  const sirket = sirketler.value?.find((s) => s.id === row.id);
  if (sirket) {
    selectedSirket.value = sirket;
    editMode.value = true;
    showModal.value.sirketler = true;
  }
};

// Şube detay fonksiyonu
const handleSubeDetail = (row: any) => {
  const sube = subeler.value?.find((s) => s.id === row.id);
  if (sube) {
    selectedSube.value = sube;
    showDetailModal.value.subeler = true;
  }
};

// Şube düzenleme fonksiyonu
const handleSubeEdit = (row: any) => {
  const sube = subeler.value?.find((s) => s.id === row.id);
  if (sube) {
    selectedSube.value = sube;
    editMode.value = true;
    showModal.value.subeler = true;
  }
};

// Departman detay fonksiyonu
const handleDepartmanDetail = (row: any) => {
  const departman = departmanlar.value?.find((d) => d.id === row.id);
  if (departman) {
    selectedDepartman.value = departman;
    showDetailModal.value.departmanlar = true;
  }
};

// Departman düzenleme fonksiyonu
const handleDepartmanEdit = (row: any) => {
  const departman = departmanlar.value?.find((d) => d.id === row.id);
  if (departman) {
    selectedDepartman.value = departman;
    editMode.value = true;
    showModal.value.departmanlar = true;
  }
};

// Pozisyon detay fonksiyonu
const handlePozisyonDetail = (row: any) => {
  const pozisyon = pozisyonlar.value?.find((p) => p.id === row.id);
  if (pozisyon) {
    selectedPozisyon.value = pozisyon;
    showDetailModal.value.pozisyonlar = true;
  }
};

// Pozisyon düzenleme fonksiyonu
const handlePozisyonEdit = (row: any) => {
  const pozisyon = pozisyonlar.value?.find((p) => p.id === row.id);
  if (pozisyon) {
    selectedPozisyon.value = pozisyon;
    editMode.value = true;
    showModal.value.pozisyonlar = true;
  }
};

// Detay modalından düzenleme moduna geçiş
const handleEditFromDetail = (sirket: SirketModel) => {
  selectedSirket.value = sirket;
  showDetailModal.value.sirketler = false;
  editMode.value = true;
  showModal.value.sirketler = true;
};

// Şube detay modalından düzenleme moduna geçiş
const handleEditFromSubeDetail = (sube: SubeModel) => {
  selectedSube.value = sube;
  showDetailModal.value.subeler = false;
  editMode.value = true;
  showModal.value.subeler = true;
};

// Departman detay modalından düzenleme moduna geçiş
const handleEditFromDepartmanDetail = (departman: DepartmanModel) => {
  selectedDepartman.value = departman;
  showDetailModal.value.departmanlar = false;
  editMode.value = true;
  showModal.value.departmanlar = true;
};

// Pozisyon detay modalından düzenleme moduna geçiş
const handleEditFromPozisyonDetail = (pozisyon: PozisyonModel) => {
  selectedPozisyon.value = pozisyon;
  showDetailModal.value.pozisyonlar = false;
  editMode.value = true;
  showModal.value.pozisyonlar = true;
};

// Modal kapatma işlemi
const handleCloseModal = (type: string) => {
  showModal.value[type as keyof typeof showModal.value] = false;
  showDetailModal.value[type as keyof typeof showDetailModal.value] = false;
  editMode.value = false;
  selectedSirket.value = null;
  selectedSube.value = null;
  selectedDepartman.value = null;
  selectedPozisyon.value = null;
};

// Yeni kayıt oluşturma modalını açma
const handleNewRecord = (type: string) => {
  editMode.value = false;
  showModal.value[type as keyof typeof showModal.value] = true;
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
        class="mx-10 my-3 p-1 bg-neutral-200 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300"
      >
        <div class="flex justify-between items-center p-2">
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="size-6 cursor-pointer select-none transform transition-transform duration-200"
              :class="{ 'rotate-90': expand.sirketler }"
              xmlns="http://www.w3.org/2000/svg"
              @click="
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
              class="bg-blue-100 dark:bg-blue-700 text-blue-600 dark:text-blue-200 py-1 px-2 rounded-full text-xs font-semibold mr-2"
            >
              {{ count.sirketler || 0 }}
            </div>
            <button
              class="px-3 py-1 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors duration-300"
              @click="handleNewRecord('sirketler')"
            >
              Yeni Şirket
            </button>
          </div>
        </div>
        <div v-if="loading.sirketler" class="w-full flex items-center justify-center py-4">
          <div
            class="size-7 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
          ></div>
        </div>
        <SirketCreateModal
          :edit-mode="editMode"
          :sirket="selectedSirket as SirketModel"
          @close-modal="(p) => handleCloseModal('sirketler')"
          @refresh="getSirketler"
          v-if="showModal.sirketler"
        />

        <SirketDetailModal
          :sirket="selectedSirket as SirketModel"
          @close-modal="(p) => (showDetailModal.sirketler = p)"
          @edit-sirket="handleEditFromDetail"
          v-if="showDetailModal.sirketler && selectedSirket"
        />

        <TableLayout
          v-if="expand.sirketler && !loading.sirketler"
          :tableHeaders="['Ad', 'Eposta', 'Telefon', 'Adres', 'Oluşturulma Tarihi', 'Oluşturan', 'Durum']"
          :tableContent="filteredSirketler"
          :islemler="['detail', 'edit']"
        />
      </div>
      <div
        class="mx-10 p-1 my-3 bg-neutral-200 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300"
      >
        <div class="flex justify-between items-center p-2">
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="size-6 cursor-pointer select-none transform transition-transform duration-200"
              :class="{ 'rotate-90': expand.subeler }"
              xmlns="http://www.w3.org/2000/svg"
              @click="
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
              class="bg-blue-100 dark:bg-blue-700 text-blue-600 dark:text-blue-200 py-1 px-2 rounded-full text-xs font-semibold mr-2"
            >
              {{ count.subeler || 0 }}
            </div>
            <button
              type="button"
              class="cursor-pointer text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
              @click="
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
          @close-modal="(p) => (showModal.subeler = p)"
          v-if="showModal.subeler"
        />

        <SubeDetailModal
          :sube="selectedSube as SubeModel"
          @close-modal="(p) => (showDetailModal.subeler = p)"
          @edit-sube="handleEditFromSubeDetail"
          v-if="showDetailModal.subeler && selectedSube"
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
          :islemler="['detail', 'edit']"
        />
      </div>
      <div
        class="mx-10 p-1 my-3 bg-neutral-200 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300"
      >
        <div class="flex justify-between items-center p-2">
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="size-6 cursor-pointer select-none transform transition-transform duration-200"
              :class="{ 'rotate-90': expand.departmanlar }"
              xmlns="http://www.w3.org/2000/svg"
              @click="
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
              class="bg-blue-100 dark:bg-blue-700 text-blue-600 dark:text-blue-200 py-1 px-2 rounded-full text-xs font-semibold mr-2"
            >
              {{ count.departmanlar || 0 }}
            </div>
            <button
              type="button"
              class="cursor-pointer text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
              @click="
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
          @close-modal="(p) => (showModal.departmanlar = p)"
          v-if="showModal.departmanlar"
        />

        <DepartmanDetailModal
          :departman="selectedDepartman as DepartmanModel"
          @close-modal="(p) => (showDetailModal.departmanlar = p)"
          @edit-departman="handleEditFromDepartmanDetail"
          v-if="showDetailModal.departmanlar && selectedDepartman"
        />

        <TableLayout
          v-if="expand.departmanlar && !loading.departmanlar"
          :tableHeaders="['Ad', 'Oluşturulma Tarihi', 'Oluşturan', 'Şube', 'Şirket', 'Durum']"
          :tableContent="filteredDepartmanlar"
          :islemler="['detail', 'edit']"
        />
      </div>
      <div
        class="mx-10 p-1 my-3 bg-neutral-200 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300"
      >
        <div class="flex justify-between items-center p-2">
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="size-6 cursor-pointer select-none transform transition-transform duration-200"
              :class="{ 'rotate-90': expand.pozisyonlar }"
              xmlns="http://www.w3.org/2000/svg"
              @click="
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
              class="bg-blue-100 dark:bg-blue-700 text-blue-600 dark:text-blue-200 py-1 px-2 rounded-full text-xs font-semibold mr-2"
            >
              {{ count.pozisyonlar || 0 }}
            </div>
            <button
              type="button"
              class="cursor-pointer text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
              @click="
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
          @close-modal="(p) => (showModal.pozisyonlar = p)"
          v-if="showModal.pozisyonlar"
        />

        <PozisyonDetailModal
          :pozisyon="selectedPozisyon as PozisyonModel"
          @close-modal="(p) => (showDetailModal.pozisyonlar = p)"
          @edit-pozisyon="handleEditFromPozisyonDetail"
          v-if="showDetailModal.pozisyonlar && selectedPozisyon"
        />

        <TableLayout
          v-if="expand.pozisyonlar && !loading.pozisyonlar"
          :tableHeaders="['Ad', 'Oluşturulma Tarihi', 'Oluşturan', 'Şirket', 'Durum']"
          :tableContent="filteredPozisyonlar"
          :islemler="['detail', 'edit']"
        />
      </div>
    </div>
  </div>
</template>

<style scoped>
/* İhtiyaç duyulursa ek stiller buraya */
</style>
