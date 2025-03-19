<script setup lang="ts">
import type { DepartmanModel } from "@/models/DepartmanModel";
import type { PersonelItem } from "@/models/PersonelModels";
import type { SirketModel } from "@/models/SirketModel";
import type { SubeModel } from "@/models/SubeModel";
import DepartmanService from "@/services/DepartmanService";
import PersonelService from "@/services/PersonelService";
import SirketService from "@/services/SirketService";
import SubeService from "@/services/SubeService";
import { onMounted, reactive, ref, watch, type Ref } from "vue";

const personeller: PersonelItem[] = reactive([]);

const showForm = ref(false);
const newPerson = ref({
  name: "",
  department: "",
  status: "Aktif",
  email: "",
  photo: "",
});
const sirketler: Ref<SirketModel[] | undefined> = ref([]);
const selectedSirket = ref("");

const subeler: Ref<SubeModel[] | undefined> = ref([]);
const selectedSube = ref("");

const departmanlar: Ref<DepartmanModel[] | undefined> = ref([]);
const selectedDepartman = ref("");
onMounted(async () => {
  sirketler.value = await SirketService.getSirketler();
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
  subeler.value = await SubeService.getSubeler(selectedSirket.value);
  console.log(subeler);
};

const getDepartmanlar = async () => {
  if (selectedSube.value == "") {
    selectedDepartman.value = "";
  }
  departmanlar.value = await DepartmanService.getDepartmanlar(selectedSube.value);
  console.log(departmanlar.value);
};

watch(selectedSirket, getSubeler);
watch(selectedSube, getDepartmanlar);
watch(selectedSube, getPersoneller);
watch(selectedDepartman, getPersoneller);

const addPerson = () => {
  if (
    !newPerson.value.name ||
    !newPerson.value.department ||
    !newPerson.value.email ||
    !newPerson.value.photo
  ) {
    alert("Lütfen tüm alanları doldurunuz!");
    return;
  }
  newPerson.value = { name: "", department: "", status: "Aktif", email: "", photo: "" };
  showForm.value = false;
};
</script>

<template>
  <div class="flex relative">
    <main class="flex-1 p-6">
      <div class="flex w-full justify-start mb-5">
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
      <!-- </div> -->
    </main>

    <div v-if="showForm" class="fixed inset-0 flex justify-center items-center z-50">
      <div class="absolute inset-0 bg-gray-900 opacity-50 backdrop-blur-sm"></div>

      <div class="bg-white p-6 rounded-lg shadow-lg w-96 relative z-10">
        <button
          @click="showForm = false"
          class="absolute top-2 right-2 text-gray-500 hover:text-gray-800 text-xl"
        >
          ✖
        </button>
        <h3 class="text-lg font-bold mb-2 text-center">Yeni Personel Ekle</h3>

        <div class="grid grid-cols-1 gap-4">
          <input
            v-model="newPerson.name"
            type="text"
            placeholder="İsim"
            class="p-2 border rounded"
          />
          <input
            v-model="newPerson.department"
            type="text"
            placeholder="Departman"
            class="p-2 border rounded"
          />
          <select v-model="newPerson.status" class="p-2 border rounded">
            <option value="Aktif">Aktif</option>
            <option value="Pasif">Pasif</option>
          </select>
          <input
            v-model="newPerson.email"
            type="email"
            placeholder="E-posta"
            class="p-2 border rounded"
          />
          <input
            v-model="newPerson.photo"
            type="text"
            placeholder="Fotoğraf URL"
            class="p-2 border rounded"
          />
        </div>

        <button @click="addPerson" class="bg-blue-500 text-white px-4 py-2 rounded mt-4 w-full">
          Tamam
        </button>
      </div>
    </div>
  </div>
</template>

<style>
body {
  font-family: Arial, sans-serif;
}
button:hover {
  transition: 0.5s ease;
  cursor: pointer;
}
</style>
