<script setup lang="ts">
import type { DepartmanModel } from "@/models/entity-models/DepartmanModel";
import type { PersonelItem } from "@/models/PersonelModels";
import type { SirketModel } from "@/models/entity-models/SirketModel";
import type { SubeModel } from "@/models/entity-models/SubeModel";
import DepartmanService from "@/services/DepartmanService";
import PersonelService from "@/services/PersonelService";
import SirketService from "@/services/SirketService";
import SubeService from "@/services/SubeService";
import { computed, onMounted, ref, watch, type Ref } from "vue";
import PersonelModal from "@/components/modals/PersonelModal.vue";
import TableLayout from "@/components/TableLayout.vue";
import Roles from "@/models/Roles";
import type { PaginationParams } from "@/models/request-models/PaginationParams";

const selectedPersonel = ref<PersonelItem | undefined>(undefined);
const personeller: Ref<PersonelItem[] | undefined> = ref([]);
const filteredPersonellerList: Ref<PersonelItem[] | undefined> = ref([]);

const sirketler: Ref<SirketModel[] | undefined> = ref([]);
const selectedSirket = ref("");

const subeler: Ref<SubeModel[] | undefined> = ref([]);
const selectedSube: Ref<string | undefined> = ref(undefined);

const departmanlar: Ref<DepartmanModel[] | undefined> = ref([]);
const selectedDepartman: Ref<string | undefined> = ref(undefined);

const showPersonelModal = ref(false);

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 5,
  orderBy: undefined,
  filter: undefined,
});

onMounted(async () => {
  const res = await SirketService.sirketlerGet();
  sirketler.value = res?.items;
  if (sirketler.value) {
    selectedSirket.value = sirketler.value[0].id;
  }
  getPersoneller();
});

const setPageNumber = (pageNumber: number) => {
  paginationParams.value.pageNumber = pageNumber;
  getPersoneller();
};

const getPersoneller = async () => {
  const response = await PersonelService.getPersonelList(
    selectedSirket.value,
    selectedSube.value,
    selectedDepartman.value,
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
    ({ id, fullName, iletisim, subeAd, departmanAd, pozisyonAd, rolAd, isActive }) => ({
      id,
      fullName,
      iletisim: iletisim.eposta,
      subeAd,
      departmanAd,
      pozisyonAd,
      rolAd,
      isActive: isActive ? "Aktif" : "Pasif",
    })
  );
});

const getSubeler = async () => {
  const res = await SubeService.subelerGet(selectedSirket.value);
  subeler.value = res?.items;
};

const getDepartmanlar = async () => {
  if (selectedSube.value == undefined) {
    selectedDepartman.value = undefined;
  } else {
    const res = await DepartmanService.departmanlarGet(selectedSube.value!);
    departmanlar.value = res?.items;
  }
};

watch(selectedSirket, getSubeler);
watch(selectedSube, getDepartmanlar);
watch(selectedSube, () => {
  paginationParams.value.pageNumber = 1;
  getPersoneller();
});
watch(selectedDepartman, () => {
  paginationParams.value.pageNumber = 1;
  getPersoneller();
});

const openEditModal = (personel: PersonelItem) => {
  selectedPersonel.value = personeller.value?.find((p) => p.id == personel.id);
  selectedPersonel.value!.rolValue = Roles.getRoleByName(personel.rolAd).value;
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
            <label for="sirket">Şirket</label>
            <select
              id="sirket"
              v-model="selectedSirket"
              v-on:change="getSubeler()"
              class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-800 bg-neutral-400/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded-sm text-sm"
            >
              <option v-for="sirket in sirketler" :key="sirket.id" :value="sirket.id">
                {{ sirket.ad }}
              </option>
            </select>
          </div>
          <div class="flex flex-col w-1/4 mr-3">
            <label for="sube">Şube</label>
            <select
              id="sube"
              v-model="selectedSube"
              v-on:change="getDepartmanlar()"
              class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-800 bg-neutral-400/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
              :disabled="selectedSirket == ''"
            >
              <option :value="undefined" selected>Şube seçiniz</option>
              <option v-for="sube in subeler" :key="sube.id" :value="sube.id">
                {{ sube.ad }}
              </option>
            </select>
          </div>
          <div class="flex flex-col w-1/4">
            <label for="departman">Departman</label>
            <select
              id="departman"
              v-model="selectedDepartman"
              class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-800 bg-neutral-400/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
              :disabled="selectedSube == undefined"
            >
              <option :value="undefined" selected>Departman seçiniz</option>
              <option v-for="departman in departmanlar" :key="departman.id" :value="departman.id">
                {{ departman.ad }}
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
            { key: 'fullName', value: 'Ad', width: 'w-1/7' },
            { key: 'iletisim', value: 'Eposta', width: 'w-1/6' },
            { key: 'subeAd', value: 'Şube', width: 'w-1/12' },
            { key: 'departmanAd', value: 'Departman', width: 'w-1/10' },
            { key: 'pozisyonAd', value: 'Pozisyon' },
            { key: 'rolAd', value: 'Rol' },
            { key: 'isActive', value: 'Durum', width: 'w-1/12' },
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
