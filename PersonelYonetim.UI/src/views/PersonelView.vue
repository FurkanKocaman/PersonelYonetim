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

const selectedPersonel = ref<PersonelItem | undefined>(undefined);
const personeller: Ref<PersonelItem[] | undefined> = ref([]);

const sirketler: Ref<SirketModel[] | undefined> = ref([]);
const selectedSirket = ref("");

const subeler: Ref<SubeModel[] | undefined> = ref([]);
const selectedSube = ref("");

const departmanlar: Ref<DepartmanModel[] | undefined> = ref([]);
const selectedDepartman = ref("");

const showPersonelModal = ref(false);
onMounted(async () => {
  const res = await SirketService.sirketlerGet();
  sirketler.value = res?.Sirketler;
  if (sirketler.value) {
    selectedSirket.value = sirketler.value[0].id;
  }
  getPersoneller();
});

const getPersoneller = async () => {
  const response = await PersonelService.getPersonelList(
    selectedSirket.value,
    selectedSube.value,
    selectedDepartman.value
  );
  personeller.value = response.items;
};

const filteredPersoneller = computed<Record<string, unknown>[]>(() => {
  return (personeller.value || []).map(
    ({ id, fullName, sirketAd, subeAd, departmanAd, pozisyonAd, rolAd, isActive }) => ({
      id,
      fullName,
      sirketAd,
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
  subeler.value = res?.Subeler;
};

const getDepartmanlar = async () => {
  if (selectedSube.value == "") {
    selectedDepartman.value = "";
  }
  const res = await DepartmanService.departmanlarGet(selectedSube.value);
  departmanlar.value = res?.Departmanlar;
};

watch(selectedSirket, getSubeler);
watch(selectedSube, getDepartmanlar);
watch(selectedSube, getPersoneller);
watch(selectedDepartman, getPersoneller);

const openEditModal = (personel: PersonelItem) => {
  selectedPersonel.value = personeller.value?.find((p) => p.id == personel.id);
  selectedPersonel.value!.rolValue = Roles.getRoleByName(personel.rolAd).value;
  showPersonelModal.value = true;
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
            <label for="sirket">Sirket</label>
            <select
              id="sirket"
              v-model="selectedSirket"
              v-on:change="getSubeler()"
              class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
            >
              <option v-for="sirket in sirketler" :key="sirket.id" :value="sirket.id">
                {{ sirket.ad }}
              </option>
            </select>
          </div>
          <div class="flex flex-col w-1/4 mr-3">
            <label for="sube">Sube</label>
            <select
              id="sube"
              v-model="selectedSube"
              v-on:change="getDepartmanlar()"
              class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
              :disabled="selectedSirket == ''"
            >
              <option value="" selected>Şube seçiniz</option>
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
              class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
              :disabled="selectedSube == ''"
            >
              <option value="" selected>Departman seçiniz</option>
              <option v-for="departman in departmanlar" :key="departman.id" :value="departman.id">
                {{ departman.ad }}
              </option>
            </select>
          </div>
        </div>
        <div class="flex items-end">
          <button
            class="w-full px-4 py-3 bg-sky-600 h-fit rounded-md hover:bg-sky-700 text-neutral-300 hover:text-neutral-200 transition duration-300 ease-in-out cursor-pointer"
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
        v-if="showPersonelModal"
      />

      <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
        <TableLayout
          :table-headers="['ad', 'sirket', 'şube', 'departman', 'pozisyon', 'rol', 'durum']"
          :table-content="filteredPersoneller"
          :islemler="['edit', 'detaylar']"
          @edit-click="openEditModal"
        />
      </div>
    </main>
  </div>
</template>

<style></style>
