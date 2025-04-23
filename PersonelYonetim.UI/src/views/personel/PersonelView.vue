<script setup lang="ts">
import type { PersonelItem } from "@/models/PersonelModels";
import PersonelService from "@/services/PersonelService";
import { computed, onMounted, ref, type Ref } from "vue";
import PersonelModal from "@/components/modals/PersonelModal.vue";
import TableLayout from "@/components/TableLayout.vue";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import KurumsalBirimService from "@/services/KurumsalBirimService";
import type { KurumsalBirimGetModel } from "@/models/response-models/KurumsalBirimGetModel";

const selectedPersonel = ref<PersonelItem | undefined>(undefined);
const personeller: Ref<PersonelItem[] | undefined> = ref([]);
const filteredPersonellerList: Ref<PersonelItem[] | undefined> = ref([]);

const kurumsalBirimler: Ref<KurumsalBirimGetModel[] | undefined> = ref([]);
const selectedKurumsalBirimId = ref(undefined);
const showPersonelModal = ref(false);

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 5,
  orderBy: undefined,
  filter: undefined,
});

onMounted(async () => {
  const res = await KurumsalBirimService.kurumsalBirimlerGet();
  kurumsalBirimler.value = res?.items;
  getPersoneller();
});

const setPageNumber = (pageNumber: number) => {
  if (paginationParams.value.pageNumber != pageNumber) {
    paginationParams.value.pageNumber = pageNumber;
    getPersoneller();
  }
};

const getPersoneller = async () => {
  const response = await PersonelService.getPersonelList(
    selectedKurumsalBirimId.value,
    paginationParams.value
  );
  if (response) {
    paginationParams.value.count = response.count;
    personeller.value = response.items;
    filteredPersonellerList.value = personeller.value;
  }
};

const filteredPersoneller = computed<Record<string, unknown>[]>(() => {
  return (filteredPersonellerList.value || []).map(
    ({ id, ad, soyad, iletisim, kurumsalBirimAd, pozisyonAd, yoneticiAd, isActive }) => ({
      id,
      ad,
      soyad,
      iletisim: iletisim.eposta,
      kurumsalBirimAd,
      pozisyonAd,
      yoneticiAd,
      isActive: isActive ? "Aktif" : "Pasif",
    })
  );
});

// watch(selectedSube, () => {
//   paginationParams.value.pageNumber = 1;
//   getPersoneller();
// });
// watch(selectedDepartman, () => {
//   paginationParams.value.pageNumber = 1;
//   getPersoneller();
// });

const openEditModal = (personel: PersonelItem) => {
  selectedPersonel.value = personeller.value?.find((p) => p.id == personel.id);
  // selectedPersonel.value!.role = personel.role;
  showPersonelModal.value = true;
};

const orderBy = (order: string) => {
  paginationParams.value.orderBy = paginationParams.value.orderBy?.includes("desc")
    ? order + " asc"
    : order + " desc";
  getPersoneller();
};
</script>

<template>
  <div class="relative">
    <div class="w-full mt-2 ml-5">
      <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">
        Personelleri görüntüleyip düzenleyebilirsiniz.
      </p>
    </div>
    <main class="flex-1 p-6">
      <div class="flex w-full justify-between mb-5">
        <div class="flex flex-1">
          <div class="flex flex-col w-1/4 mr-3">
            <label for="sirket">Birim</label>
            <select
              id="sirket"
              v-model="selectedKurumsalBirimId"
              v-on:change="getPersoneller()"
              class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-800 bg-neutral-400/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded-sm text-sm"
            >
              <option :value="undefined" selected>Birim seçiniz</option>
              <option
                v-for="kurumsalBirim in kurumsalBirimler"
                :key="kurumsalBirim.id"
                :value="kurumsalBirim.id"
              >
                {{ kurumsalBirim.ad }}
              </option>
            </select>
          </div>
        </div>
        <div class="flex items-end">
          <button
            type="button"
            class="cursor-pointer text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
            @click="
              () => {
                selectedPersonel = undefined;
                showPersonelModal = !showPersonelModal;
              }
            "
          >
            + Personel ekle
          </button>
        </div>
      </div>

      <PersonelModal
        :personel="selectedPersonel"
        @close-modal="(p) => (showPersonelModal = p)"
        @refresh="
          () => {
            getPersoneller();
          }
        "
        v-if="showPersonelModal"
      />

      <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
        <TableLayout
          :table-headers="[
            { key: 'ad', value: 'Ad', width: 'w-1' },
            { key: 'soyad', value: 'Soyad' },
            { key: 'iletisim', value: 'Eposta' },
            { key: 'kurumsalBirimAd', value: 'Birim' },
            { key: 'pozisyonAd', value: 'Pozisyon' },
            { key: 'yoneticiAd', value: 'Yonetici' },
            { key: 'isActive', value: 'Durum' },
          ]"
          :table-content="filteredPersoneller"
          :islemler="['edit', 'detaylar']"
          @edit-click="openEditModal"
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
    </main>
  </div>
</template>

<style></style>
