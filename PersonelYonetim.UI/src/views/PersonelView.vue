<script setup lang="ts">
import type { DepartmanModel } from "@/models/entity-models/DepartmanModel";
import type { PersonelItem } from "@/models/PersonelModels";
import type { SirketModel } from "@/models/entity-models/SirketModel";
import type { SubeModel } from "@/models/entity-models/SubeModel";
import DepartmanService from "@/services/DepartmanService";
import PersonelService from "@/services/PersonelService";
import SirketService from "@/services/SirketService";
import SubeService from "@/services/SubeService";
import { onMounted, reactive, ref, watch, type Ref } from "vue";
import PersonelCreateModal from "@/components/modals/PersonelCreateModal.vue";

const personeller: PersonelItem[] = reactive([]);

const sirketler: Ref<SirketModel[] | undefined> = ref([]);
const selectedSirket = ref("");

const subeler: Ref<SubeModel[] | undefined> = ref([]);
const selectedSube = ref("");

const departmanlar: Ref<DepartmanModel[] | undefined> = ref([]);
const selectedDepartman = ref("");

const showPersonelCreateModal = ref(false);
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
  personeller.splice(0, personeller.length, ...response.items);
};

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
                showPersonelCreateModal = !showPersonelCreateModal;
              }
            "
          >
            + Personel ekle
          </button>
        </div>
      </div>

      <PersonelCreateModal
        @close-modal="(p) => (showPersonelCreateModal = p)"
        v-if="showPersonelCreateModal"
      />

      <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
        <table class="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
          <thead
            class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400"
          >
            <tr>
              <th scope="col" class="px-6 py-3">Personel</th>
              <th scope="col" class="px-6 py-3">
                <div class="flex items-center">
                  E-posta
                  <svg
                    class="w-3 h-3 ms-1.5"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      d="M8.574 11.024h6.852a2.075 2.075 0 0 0 1.847-1.086 1.9 1.9 0 0 0-.11-1.986L13.736 2.9a2.122 2.122 0 0 0-3.472 0L6.837 7.952a1.9 1.9 0 0 0-.11 1.986 2.074 2.074 0 0 0 1.847 1.086Zm6.852 1.952H8.574a2.072 2.072 0 0 0-1.847 1.087 1.9 1.9 0 0 0 .11 1.985l3.426 5.05a2.123 2.123 0 0 0 3.472 0l3.427-5.05a1.9 1.9 0 0 0 .11-1.985 2.074 2.074 0 0 0-1.846-1.087Z"
                    />
                  </svg>
                </div>
              </th>
              <th scope="col" class="px-6 py-3">
                <div class="flex items-center">
                  Cinsiyet
                  <svg
                    class="w-3 h-3 ms-1.5"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      d="M8.574 11.024h6.852a2.075 2.075 0 0 0 1.847-1.086 1.9 1.9 0 0 0-.11-1.986L13.736 2.9a2.122 2.122 0 0 0-3.472 0L6.837 7.952a1.9 1.9 0 0 0-.11 1.986 2.074 2.074 0 0 0 1.847 1.086Zm6.852 1.952H8.574a2.072 2.072 0 0 0-1.847 1.087 1.9 1.9 0 0 0 .11 1.985l3.426 5.05a2.123 2.123 0 0 0 3.472 0l3.427-5.05a1.9 1.9 0 0 0 .11-1.985 2.074 2.074 0 0 0-1.846-1.087Z"
                    />
                  </svg>
                </div>
              </th>
              <th scope="col" class="px-6 py-3">
                <div class="flex items-center">
                  departmanAd
                  <svg
                    class="w-3 h-3 ms-1.5"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      d="M8.574 11.024h6.852a2.075 2.075 0 0 0 1.847-1.086 1.9 1.9 0 0 0-.11-1.986L13.736 2.9a2.122 2.122 0 0 0-3.472 0L6.837 7.952a1.9 1.9 0 0 0-.11 1.986 2.074 2.074 0 0 0 1.847 1.086Zm6.852 1.952H8.574a2.072 2.072 0 0 0-1.847 1.087 1.9 1.9 0 0 0 .11 1.985l3.426 5.05a2.123 2.123 0 0 0 3.472 0l3.427-5.05a1.9 1.9 0 0 0 .11-1.985 2.074 2.074 0 0 0-1.846-1.087Z"
                    />
                  </svg>
                </div>
              </th>
              <th scope="col" class="px-6 py-3">
                <div class="flex items-center">
                  pozisyonAd
                  <svg
                    class="w-3 h-3 ms-1.5"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      d="M8.574 11.024h6.852a2.075 2.075 0 0 0 1.847-1.086 1.9 1.9 0 0 0-.11-1.986L13.736 2.9a2.122 2.122 0 0 0-3.472 0L6.837 7.952a1.9 1.9 0 0 0-.11 1.986 2.074 2.074 0 0 0 1.847 1.086Zm6.852 1.952H8.574a2.072 2.072 0 0 0-1.847 1.087 1.9 1.9 0 0 0 .11 1.985l3.426 5.05a2.123 2.123 0 0 0 3.472 0l3.427-5.05a1.9 1.9 0 0 0 .11-1.985 2.074 2.074 0 0 0-1.846-1.087Z"
                    />
                  </svg>
                </div>
              </th>
              <th scope="col" class="px-6 py-3">
                <div class="flex items-center">
                  telefon
                  <svg
                    class="w-3 h-3 ms-1.5"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      d="M8.574 11.024h6.852a2.075 2.075 0 0 0 1.847-1.086 1.9 1.9 0 0 0-.11-1.986L13.736 2.9a2.122 2.122 0 0 0-3.472 0L6.837 7.952a1.9 1.9 0 0 0-.11 1.986 2.074 2.074 0 0 0 1.847 1.086Zm6.852 1.952H8.574a2.072 2.072 0 0 0-1.847 1.087 1.9 1.9 0 0 0 .11 1.985l3.426 5.05a2.123 2.123 0 0 0 3.472 0l3.427-5.05a1.9 1.9 0 0 0 .11-1.985 2.074 2.074 0 0 0-1.846-1.087Z"
                    />
                  </svg>
                </div>
              </th>
              <th scope="col" class="px-6 py-3">
                <div class="flex items-center">
                  tamAdres
                  <svg
                    class="w-3 h-3 ms-1.5"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="currentColor"
                    viewBox="0 0 24 24"
                  >
                    <path
                      d="M8.574 11.024h6.852a2.075 2.075 0 0 0 1.847-1.086 1.9 1.9 0 0 0-.11-1.986L13.736 2.9a2.122 2.122 0 0 0-3.472 0L6.837 7.952a1.9 1.9 0 0 0-.11 1.986 2.074 2.074 0 0 0 1.847 1.086Zm6.852 1.952H8.574a2.072 2.072 0 0 0-1.847 1.087 1.9 1.9 0 0 0 .11 1.985l3.426 5.05a2.123 2.123 0 0 0 3.472 0l3.427-5.05a1.9 1.9 0 0 0 .11-1.985 2.074 2.074 0 0 0-1.846-1.087Z"
                    />
                  </svg>
                </div>
              </th>

              <th scope="col" class="px-6 py-3">
                <span class="sr-only">Edit</span>
              </th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="personel in personeller"
              :key="personel.id"
              class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 border-gray-200"
            >
              <th
                scope="row"
                class="px-6 py-4 font-medium text-gray-900 dark:text-white flex items-center"
              >
                <img :src="personel.fullName" alt="" class="size-10 rounded-full mr-2" />
                {{ personel.fullName }}
              </th>
              <td class="px-6 py-4">{{ personel.eposta }}</td>
              <td class="px-6 py-4">{{ personel.cinsiyet }}</td>
              <td class="px-6 py-4">{{ personel.departmanAd }}</td>
              <td class="px-6 py-4">{{ personel.pozisyonAd }}</td>
              <td class="px-6 py-4">{{ personel.telefon }}</td>
              <td class="px-6 py-4">{{ personel.tamAdres }}</td>

              <td class="px-6 py-4 text-right">
                <a href="#" class="font-medium text-blue-600 dark:text-blue-500 hover:underline"
                  >Edit</a
                >
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </main>
  </div>
</template>

<style></style>
