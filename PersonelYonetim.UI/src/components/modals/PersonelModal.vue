<script setup lang="ts">
import type { CalismaTakvimModel } from "@/models/entity-models/calisma-takvim/CalismaTakvimModel";
import type { DepartmanModel } from "@/models/entity-models/DepartmanModel";
import type { PozisyonModel } from "@/models/entity-models/PozisyonModel";
import type { SirketModel } from "@/models/entity-models/SirketModel";
import type { SubeModel } from "@/models/entity-models/SubeModel";
import type { PersonelItem } from "@/models/PersonelModels";
import type { PersonelCreateRequest } from "@/models/request-models/PersonelCreateRequest";
import Roles from "@/models/Roles";
import { SozlesmeTuru } from "@/models/UserModel";
import CalismaTakvimService from "@/services/CalismaTakvimService";
import DepartmanService from "@/services/DepartmanService";
import PersonelService from "@/services/PersonelService";
import PozisyonService from "@/services/PozisyonService";
import SirketService from "@/services/SirketService";
import SubeService from "@/services/SubeService";
import Datepicker from "@vuepic/vue-datepicker";
import "@vuepic/vue-datepicker/dist/main.css";
import { onMounted, reactive, ref, type Ref } from "vue";

const currentStep = ref(0);
const sirketler: Ref<SirketModel[] | undefined> = ref([]);
const subeler: Ref<SubeModel[] | undefined> = ref([]);
const departmanlar: Ref<DepartmanModel[] | undefined> = ref([]);
const pozisyonlar: Ref<PozisyonModel[] | undefined> = ref([]);
const personeller: Ref<PersonelItem[] | undefined> = ref([]);
const calismaTakvimler: Ref<CalismaTakvimModel[] | undefined> = ref([]);

const props = defineProps<{
  personel?: PersonelCreateRequest;
}>();

const request: PersonelCreateRequest = reactive(
  props.personel
    ? JSON.parse(JSON.stringify(props.personel))
    : {
        ad: "",
        soyad: "",
        dogumTarihi: new Date(),
        cinsiyet: undefined,
        profilResimUrl: undefined,
        iletisim: {
          eposta: "",
          telefon: "",
        },
        adres: {
          ulke: "Türkiye",
          sehir: "",
          ilce: "",
          tamAdres: "",
        },
        iseGirisTarihi: new Date(),
        yoneticiId: undefined,
        sirketId: "",
        subeId: undefined,
        departmanId: undefined,
        pozisyonId: undefined,
        calismaTakvimiValue: -1,
        sozlesmeTuruValue: -1,
        sozlesmeBitisTarihi: undefined,
        rolValue: -1,
        calismaTakvimiId: undefined,
        pozisyonBaslangicTarih: new Date(),
        izinKuralId: undefined,
      }
);
onMounted(() => {
  getSirketler();
  getSubeler();
  getDepartmanlar();
  getPozisyonlar();
  getCalismaTakvimler();
  console.log(request);
  if (request.sirketId != "") getPersoneller();
});

const handlePersonel = async () => {
  if (props.personel) {
    const response = await PersonelService.updatePersonel(request);
    console.log(response);
  } else {
    const response = await PersonelService.createPersonel(request);
    console.log(response);
  }
};
const getSirketler = async () => {
  try {
    const res = await SirketService.sirketlerGet();
    sirketler.value = res?.Sirketler;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  }
};
const getSubeler = async () => {
  try {
    const res = await SubeService.subelerGet("");

    subeler.value = res?.Subeler;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  }
};

const getDepartmanlar = async () => {
  try {
    const res = await DepartmanService.departmanlarGet("");
    departmanlar.value = res?.Departmanlar;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  }
};
const getPozisyonlar = async () => {
  try {
    const res = await PozisyonService.pozisyonlarGet("");
    pozisyonlar.value = res?.Pozisyonlar;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  }
};
const getPersoneller = async () => {
  try {
    if (request.sirketId != "0") {
      console.log("here");
      const res = await PersonelService.getPersonelList(request.sirketId, "", "");
      personeller.value = res.items;
    }
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  }
};

const getCalismaTakvimler = async () => {
  const res = await CalismaTakvimService.getCalismaTakvimleri();
  calismaTakvimler.value = res;
};
</script>
<template>
  <div
    class="overflow-y-auto overflow-x-hidden fixed flex justify-center items-center top-0 right-0 left-0 z-50 backdrop-blur-sm bg-black/30 w-full h-full"
  >
    <div class="relative p-4 max-w-4xl w-full max-h-full">
      <div class="relative bg-white rounded-lg shadow-sm dark:bg-neutral-800 w-full">
        <div
          class="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600 border-gray-200"
        >
          <h3 class="text-xl font-semibold text-gray-900 dark:text-white">Personel Ekle</h3>
          <button
            type="button"
            class="end-2.5 text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
            @click="$emit('closeModal', false)"
          >
            <svg
              class="w-3 h-3"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 14 14"
            >
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
              />
            </svg>
            <span class="sr-only">Close modal</span>
          </button>
        </div>

        <!-- Stepper Start -->
        <div class="w-full flex items-center justify-center">
          <ol
            class="flex items-center text-sm font-medium text-center text-gray-500 dark:text-gray-400 sm:text-base"
          >
            <li
              :class="currentStep == 0 ? 'text-blue-600 dark:text-blue-500' : ''"
              class="flex md:w-full items-center sm:after:content-[''] after:w-full after:h-1 after:border-b after:border-gray-300 after:border-1 after:hidden sm:after:inline-block after:mx-6 xl:after:mx-10 dark:after:border-neutral-800/50 cursor-pointer"
              v-on:click="
                () => {
                  currentStep = 0;
                }
              "
            >
              <span
                class="flex items-center after:content-['/'] sm:after:hidden after:mx-2 after:text-gray-200 dark:after:text-gray-500"
              >
                <svg
                  v-if="currentStep == 0"
                  class="w-3.5 h-3.5 sm:w-4 sm:h-4 me-2.5"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="currentColor"
                  viewBox="0 0 20 20"
                >
                  <path
                    d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 8.207-4 4a1 1 0 0 1-1.414 0l-2-2a1 1 0 0 1 1.414-1.414L9 10.586l3.293-3.293a1 1 0 0 1 1.414 1.414Z"
                  />
                </svg>
                <span v-if="currentStep != 0" class="me-2">1 </span>
                Kişisel<span class="hidden sm:inline-flex sm:ms-2">Bilgiler</span>
              </span>
            </li>
            <li
              :class="currentStep == 1 ? 'text-blue-600 dark:text-blue-500' : ''"
              class="flex md:w-full items-center after:content-[''] after:w-full after:h-1 after:border-b after:border-gray-300 after:border-1 after:hidden sm:after:inline-block after:mx-6 xl:after:mx-10 dark:after:border-neutral-800/50 cursor-pointer"
              v-on:click="
                () => {
                  currentStep = 1;
                }
              "
            >
              <span
                class="flex items-center after:content-['/'] sm:after:hidden after:mx-2 after:text-gray-200 dark:after:text-gray-500"
              >
                <svg
                  v-if="currentStep == 1"
                  class="w-3.5 h-3.5 sm:w-4 sm:h-4 me-2.5"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="currentColor"
                  viewBox="0 0 20 20"
                >
                  <path
                    d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 8.207-4 4a1 1 0 0 1-1.414 0l-2-2a1 1 0 0 1 1.414-1.414L9 10.586l3.293-3.293a1 1 0 0 1 1.414 1.414Z"
                  />
                </svg>
                <span v-if="currentStep != 1" class="me-2">2</span>
                Çalışma <span class="hidden sm:inline-flex sm:ms-2">Bilgileri</span>
              </span>
            </li>
            <li
              :class="currentStep == 2 ? 'text-blue-600 dark:text-blue-500' : ''"
              class="flex items-center cursor-pointer"
              v-on:click="
                () => {
                  currentStep = 2;
                }
              "
            >
              <svg
                v-if="currentStep == 2"
                class="w-3.5 h-3.5 sm:w-4 sm:h-4 me-2.5"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="currentColor"
                viewBox="0 0 20 20"
              >
                <path
                  d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 8.207-4 4a1 1 0 0 1-1.414 0l-2-2a1 1 0 0 1 1.414-1.414L9 10.586l3.293-3.293a1 1 0 0 1 1.414 1.414Z"
                />
              </svg>
              <span v-if="currentStep != 2" class="me-2">3</span>
              Onaylama
            </li>
          </ol>
        </div>

        <!-- Stepper End -->

        <div class="p-4 md:p-5 w-full">
          <form class="space-y-4 w-full" @submit.prevent="handlePersonel()">
            <!-- Kişisel bilgiler start -->
            <div v-if="currentStep == 0" class="flex">
              <div class="flex flex-col mr-2 w-full">
                <div class="mb-2 flex flex-col">
                  <label
                    for="ad"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Ad</label
                  >
                  <input
                    type="text"
                    name="ad"
                    id="ad"
                    v-model="request.ad"
                    class="bg-gray-50 border border-neutral-900 text-gray-900 text-sm rounded-lg min-w-sm block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="Caesar"
                    required
                  />
                </div>
                <div class="mb-2 flex flex-col">
                  <label
                    for="soyad"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Soyad</label
                  >
                  <input
                    type="text"
                    name="soyad"
                    id="soyad"
                    v-model="request.soyad"
                    class="bg-gray-50 border border-neutral-900 text-gray-900 text-sm rounded-lg min-w-sm block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="Iulius"
                    required
                  />
                </div>
                <div class="mb-2">
                  <label
                    for="email"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >E-posta</label
                  >
                  <input
                    type="email"
                    name="email"
                    id="email"
                    v-model="request.iletisim.eposta"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="user@mail.com"
                    required
                  />
                </div>
                <div class="mb-2">
                  <label
                    for="telefon"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Telefon</label
                  >
                  <input
                    type="phone"
                    name="telefon"
                    id="telefon"
                    v-model="request.iletisim.telefon"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="0850 000 00 00"
                    required
                  />
                </div>
                <div class="mb-2 flex flex-col">
                  <label
                    for="sirket"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Cinsiyet</label
                  >
                  <select
                    id="sirket"
                    class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                    v-model="request.cinsiyet"
                  >
                    <option
                      class="text-neutral-800 dark:text-neutral-200"
                      :value="undefined"
                      selected
                    >
                      Cinsiyet seçin
                    </option>
                    <option class="text-neutral-800 dark:text-neutral-200" :value="true" selected>
                      Erkek
                    </option>
                    <option class="text-neutral-800 dark:text-neutral-200" :value="false" selected>
                      Kadın
                    </option>
                    <option
                      class="text-neutral-800 dark:text-neutral-200"
                      :value="undefined"
                      selected
                    >
                      Belirtmek istemiyorum
                    </option>
                  </select>
                </div>
              </div>

              <div class="flex flex-col ml-2 w-full">
                <div class="mb-2 flex flex-col">
                  <label
                    for="sehir"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Sehir</label
                  >
                  <input
                    type="text"
                    name="sehir"
                    id="sehir"
                    v-model="request.adres.sehir"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg min-w-sm block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="Trabzon"
                    required
                  />
                </div>
                <div class="mb-2">
                  <label
                    for="ilce"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >İlçe</label
                  >
                  <input
                    type="text"
                    name="ilce"
                    id="ilce"
                    v-model="request.adres.ilce"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="Ortahisar"
                    required
                  />
                </div>
                <div class="mb-2">
                  <label
                    for="tam-adres"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Tam Adres</label
                  >
                  <input
                    type="text"
                    name="tam-adres"
                    id="tam-adres"
                    v-model="request.adres.tamAdres"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    required
                  />
                </div>
                <div class="w-full mr-1">
                  <label for="isebaslamatarih" class="block text-sm/5 font-semibold my-2"
                    >İşe Başlama Tarihi</label
                  >
                  <Datepicker
                    id="isebaslamatarih"
                    v-model="request.pozisyonBaslangicTarih"
                    locale="TR"
                    :enable-time-picker="true"
                    :format="'dd-MM-yyyy'"
                  />
                </div>
                <div class="w-full mr-1">
                  <label for="tarih" class="block text-sm/5 font-semibold my-2">Doğum Tarihi</label>
                  <Datepicker
                    id="tarih"
                    v-model="request.dogumTarihi"
                    locale="TR"
                    :enable-time-picker="true"
                    :format="'dd-MM-yyyy'"
                  />
                </div>
              </div>
            </div>

            <div v-if="currentStep == 0" class="transition duration-300">
              <label
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                for="file_input"
                >Resim</label
              >
              <input
                class="block w-full text-sm text-gray-900 border border-gray-300 rounded-lg cursor-pointer p-3 bg-gray-50 dark:text-gray-400 focus:outline-none dark:bg-neutral-700 dark:border-gray-600 dark:placeholder-gray-400"
                id="file_input"
                type="file"
              />
              <p class="mt-1 text-sm text-gray-500 dark:text-gray-300" id="file_input_help">
                SVG, PNG, JPG or GIF (MAX 4MB).
              </p>
            </div>
            <!-- Kişisel bilgiler end -->

            <!-- Pozisyon bilgileri start -->
            <div v-if="currentStep == 1" class="flex transition duration-300">
              <div class="flex flex-col mr-2 w-full">
                <div class="mb-2 flex flex-col">
                  <label
                    for="sirket"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Şirket</label
                  >
                  <select
                    id="sirket"
                    class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                    v-model="request.sirketId"
                    @change="getPersoneller()"
                  >
                    <option class="text-neutral-800 dark:text-neutral-200" value="" selected>
                      Personelin çalışdacağı şirketi seçin
                    </option>
                    <option
                      v-for="sirket in sirketler"
                      :key="sirket.id"
                      :value="sirket.id"
                      class="text-neutral-800 dark:text-neutral-200"
                    >
                      {{ sirket.ad }}
                    </option>
                  </select>
                </div>
                <div class="mb-2 flex flex-col">
                  <label
                    for="sube"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Şube</label
                  >
                  <select
                    id="sube"
                    class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                    :class="request.sirketId == '' ? 'opacity-50' : ''"
                    v-model="request.subeId"
                    :disabled="request.sirketId == ''"
                  >
                    <option
                      class="text-neutral-800 dark:text-neutral-200"
                      :value="undefined"
                      selected
                    >
                      Personelin çalışdacağı şubeyi seçin
                    </option>
                    <option
                      v-for="sube in subeler"
                      :key="sube.id"
                      :value="sube.id"
                      class="text-neutral-800 dark:text-neutral-200"
                    >
                      {{ sube.ad }}
                    </option>
                  </select>
                </div>
                <div class="mb-2">
                  <label
                    for="departman"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Departman</label
                  >
                  <select
                    id="departman"
                    class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                    :class="request.subeId == undefined ? 'opacity-50' : ''"
                    v-model="request.departmanId"
                    :disabled="request.subeId == undefined"
                  >
                    <option
                      class="text-neutral-800 dark:text-neutral-200"
                      :value="undefined"
                      selected
                    >
                      Personelin çalışdacağı departmanı seçin
                    </option>
                    <option
                      v-for="departman in departmanlar"
                      :key="departman.id"
                      :value="departman.id"
                      class="text-neutral-800 dark:text-neutral-200"
                    >
                      {{ departman.ad }}
                    </option>
                  </select>
                </div>
                <div class="mb-2">
                  <label
                    for="pozisyon"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Pozisyon</label
                  >
                  <select
                    id="pozisyon"
                    class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                    :class="request.departmanId == undefined ? 'opacity-50' : ''"
                    v-model="request.pozisyonId"
                    :disabled="request.departmanId == undefined"
                  >
                    <option
                      class="text-neutral-800 dark:text-neutral-200"
                      :value="undefined"
                      selected
                    >
                      Personelin çalışdacağı pozisyonu seçin
                    </option>
                    <option
                      v-for="pozisyon in pozisyonlar"
                      :key="pozisyon.id"
                      :value="pozisyon.id"
                      class="text-neutral-800 dark:text-neutral-200"
                    >
                      {{ pozisyon.ad }}
                    </option>
                  </select>
                </div>
              </div>

              <div class="flex flex-col ml-2 w-full">
                <div class="mb-2 flex flex-col">
                  <label
                    for="sehir"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Erişim Şekli</label
                  >
                  <select
                    id="pozisyon"
                    class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                    v-model="request.rolValue"
                  >
                    <option class="text-neutral-800 dark:text-neutral-200" :value="-1" selected>
                      Personelin unvanını seçin
                    </option>
                    <option
                      v-for="rol in [0, 1, 2, 3, 4, 5, 6]"
                      :key="rol"
                      :value="rol"
                      class="text-neutral-800 dark:text-neutral-200"
                    >
                      {{ Roles.getRoleByValue(rol).name }}
                    </option>
                  </select>
                </div>
                <div class="mb-2">
                  <label
                    for="sozlesme"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Sözleşme Türünü Seçin</label
                  >
                  <select
                    id="sozlesme"
                    class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                    v-model="request.sozlesmeTuruValue"
                  >
                    <option class="text-neutral-800 dark:text-neutral-200" :value="-1" selected>
                      Sozlesme türünü seçin
                    </option>
                    <option
                      v-for="sozlesme in [0, 1, 2, 3]"
                      :key="sozlesme"
                      :value="SozlesmeTuru.getSozlesmeByValue(sozlesme).value"
                      class="text-neutral-800 dark:text-neutral-200"
                    >
                      {{ SozlesmeTuru.getSozlesmeByValue(sozlesme).name }}
                    </option>
                  </select>
                  <div v-if="request.sozlesmeTuruValue == 0" class="w-full mr-1">
                    <label for="tarih" class="block text-sm/5 font-semibold my-2"
                      >Sozlesme Bitis Tarihi</label
                    >
                    <Datepicker
                      id="tarih"
                      v-model="request.sozlesmeBitisTarihi"
                      locale="tr"
                      :enable-time-picker="false"
                      :format="'dd-MM-yyyy'"
                    />
                  </div>
                </div>
                <div class="mb-2 flex flex-col">
                  <label
                    for="calisma"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Çalışma Takvimi</label
                  >
                  <select
                    id="calisma"
                    class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                    v-model="request.calismaTakvimiId"
                  >
                    <option
                      class="text-neutral-800 dark:text-neutral-200"
                      :value="undefined"
                      selected
                    >
                      Personelin çalışma takvimini seçin
                    </option>
                    <option
                      v-for="calismaTakvim in calismaTakvimler"
                      :key="calismaTakvim.id"
                      :value="calismaTakvim.id"
                      class="text-neutral-800 dark:text-neutral-200"
                    >
                      {{ calismaTakvim.ad }}
                    </option>
                  </select>
                </div>
                <div class="mb-2 flex flex-col">
                  <label
                    for="sehir"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Personelden Sorumlu Yonetici</label
                  >
                  <select
                    id="pozisyon"
                    class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                    v-model="request.yoneticiId"
                  >
                    <option class="text-neutral-800 dark:text-neutral-200" :value="null" selected>
                      Personelden sorumlu yöneticiyi seçin
                    </option>
                    <option
                      v-for="personel in personeller"
                      :key="personel.id"
                      :value="personel.id"
                      class="text-neutral-800 dark:text-neutral-200"
                    >
                      {{ personel.fullName }}
                    </option>
                  </select>
                </div>
              </div>
            </div>

            <!-- Pozisyon bilgileri end -->

            <!-- Onaylama start -->

            <!-- Onaylama end -->

            <button
              type="submit"
              class="w-full text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
            >
              Oluştur
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
