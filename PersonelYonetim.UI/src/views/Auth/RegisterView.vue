<script setup lang="ts">
import { ref, onMounted, type Ref } from "vue";
import AuthService from "@/services/AuthService";
import { useRouter } from "vue-router";
import type { RegisterRequest } from "@/models/request-models/RegisterRequest";
import axios from "axios";
import Datepicker from "@vuepic/vue-datepicker";
import "@vuepic/vue-datepicker/dist/main.css";

const router = useRouter();
const isLoading = ref(false);
const loginResponse = ref("");
const currentStep = ref(0);
const isDark = ref(true);

const registerRequest: Ref<RegisterRequest> = ref({
  kurumsalBirimCreateCommand: {
    id: undefined,
    ad: "",
    kod: undefined,
    logoUrl: undefined,
    birimTipiId: "4d9ce5a1-0e0b-478b-8258-ef2343d06254",
    ustBirimId: undefined,
    tenantId: undefined,
  },
  personelCreateCommand: {
    ad: "",
    soyad: "",
    dogumTarihi: new Date(),
    cinsiyet: undefined,
    avatarUrl: undefined,
    iletisim: {
      telefon: "",
      eposta: "",
    },
    adres: {
      ulke: "Türkiye",
      sehir: "",
      ilce: "",
      tamAdres: "",
    },
    kurumsalBirimId: undefined,
    pozisyonId: undefined,
    roleId: undefined,
    baslangicTarihi: new Date(),
    bitisTarihi: undefined,
    birincilGorevMi: false,
    gorevlendirmeTipiValue: 1,
    calismaSekliValue: 0,
    raporlananGorevlendirmeId: undefined,
    izinKuralId: undefined,
    calismaTakvimId: undefined,
    brutUcret: 0,
    tenantId: undefined,
  },
});

const provinces = ref<{ id: number; name: string }[]>([]);
const districts = ref<{ id: number; name: string }[]>([]);

const getProvinces = async () => {
  try {
    const response = await fetch("https://turkiyeapi.dev/api/v1/provinces");
    const data = await response.json();
    provinces.value = data.data;
  } catch (error) {
    console.error("İl verileri alınırken hata oluştu:", error);
  }
};

const getDistricts = async () => {
  if (!registerRequest.value.personelCreateCommand.adres?.sehir) return;
  try {
    const response = await axios.get(
      `https://turkiyeapi.dev/api/v1/provinces?name=${registerRequest.value.personelCreateCommand.adres?.sehir}`
    );
    districts.value = response.data.data[0].districts;
  } catch (error) {
    console.error("İlçe verileri alınırken hata oluştu:", error);
  }
};

onMounted(() => {
  isDark.value = localStorage.getItem("theme") == "dark";
  getProvinces();
});

const handleRegister = async () => {
  try {
    if (currentStep.value != 2) {
      currentStep.value++;
      console.log(currentStep.value);
      return;
    }
    isLoading.value = true;
    registerRequest.value.personelCreateCommand.adres!.tamAdres = `${registerRequest.value.personelCreateCommand.adres?.sehir}/${registerRequest.value.personelCreateCommand.adres?.ilce}  ${registerRequest.value.personelCreateCommand.adres?.ulke}`;

    const response = await AuthService.register(registerRequest.value);
    if (response.success) {
      console.log(response);
      loginResponse.value = response.message;
      router.push("/");
    } else {
      loginResponse.value = response.message;
      console.error(response);
    }
    isLoading.value = false;
  } catch (error) {
    console.error("Error", error);
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="flex justify-center items-center h-dvh dark:bg-neutral-900">
    <div
      class="flex flex-col items-center justify-center md:w-[60dvw] w-[90dvw] md:h-[90dvh] h-[90dvh] bg-neutral-200/40 dark:bg-neutral-800/50 rounded-md"
    >
      <form @submit.prevent="handleRegister" class="space-y-4 w-full px-16">
        <div>
          <h1 class="text-2xl font-semibold text-center mb-6">Kayıt Sayfası</h1>
          <!-- Stepper Start -->
          <ol
            class="flex items-center w-full text-sm font-medium text-center text-gray-500 dark:text-gray-400 sm:text-base"
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
                Şirket <span class="hidden sm:inline-flex sm:ms-2">Bilgileri</span>
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
          <!-- Stepper End -->

          <!-- Kişisel bilgiler start -->
          <div v-if="currentStep == 0">
            <div class="flex">
              <div class="mr-1 w-full">
                <label for="ad" class="block text-sm/5 font-semibold my-2">Ad</label>
                <div>
                  <input
                    id="ad"
                    v-model="registerRequest.personelCreateCommand.ad"
                    type="text"
                    required
                    class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
                    placeholder="John"
                  />
                </div>
              </div>
              <div class="ml-1 w-full">
                <label for="soyad" class="block text-sm/5 font-semibold my-2">Soyad</label>
                <div>
                  <input
                    id="soyad"
                    v-model="registerRequest.personelCreateCommand.soyad"
                    type="text"
                    required
                    class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
                    placeholder="Doe"
                  />
                </div>
              </div>
            </div>
            <!-- Email -->
            <div>
              <label for="email" class="block text-sm/5 font-semibold my-2">Eposta</label>
              <div>
                <input
                  id="email"
                  v-model="registerRequest.personelCreateCommand.iletisim.eposta"
                  type="email"
                  required
                  class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
                  placeholder="user@mail.com"
                />
              </div>
            </div>
            <!-- Telefon -->
            <div>
              <label for="telefon" class="block text-sm/5 font-semibold my-2">Telefon</label>
              <div>
                <input
                  id="telefon"
                  v-model="registerRequest.personelCreateCommand.iletisim.telefon"
                  type="phone"
                  required
                  class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
                  placeholder="0530 000 00 00"
                />
              </div>
            </div>
            <!-- Tarih  -->
            <div class="flex items-center justify-between">
              <div class="w-full mr-1">
                <label for="tarih" class="block text-sm/5 font-semibold my-2">Doğum Tarihi</label>
                <Datepicker
                  id="tarih"
                  v-model="registerRequest.personelCreateCommand.dogumTarihi"
                  locale="tr"
                  :enable-time-picker="false"
                  :format="'dd-MM-yyyy'"
                  :class="{ 'dark dp__theme_dark': isDark }"
                />
              </div>
              <div class="w-full ml-1">
                <label for="cinsiyet" class="block text-sm/5 font-semibold my-2">Cinsiyet</label>
                <select
                  id="cinsiyet"
                  v-model="registerRequest.personelCreateCommand.cinsiyet"
                  class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
                >
                  <option :value="undefined">Cinsiyet Seçin</option>
                  <option :value="true">Erkek</option>
                  <option :value="false">Kadın</option>
                </select>
              </div>
            </div>
            <!-- İl Seçim -->
            <div>
              <label for="province" class="block text-sm/5 font-semibold my-2">İl Seçin</label>
              <select
                id="province"
                v-model="registerRequest.personelCreateCommand.adres!.sehir"
                @change="getDistricts"
                class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
              >
                <option value="">İl Seçin</option>
                <option v-for="province in provinces" :key="province.id" :value="province.name">
                  {{ province.name }}
                </option>
              </select>
            </div>
            <!-- İlçe Seçim -->
            <div>
              <label for="district" class="block text-sm/5 font-semibold my-2">İlçe Seçin</label>
              <select
                id="district"
                v-model="registerRequest.personelCreateCommand.adres!.ilce"
                :disabled="districts.length === 0"
                class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
              >
                <option value="">Önce İl Seçin</option>
                <option v-for="district in districts" :key="district.id" :value="district.name">
                  {{ district.name }}
                </option>
              </select>
            </div>
          </div>
          <!-- Kişisel bilgiler end -->

          <!-- Şirket bilgileri start -->
          <div v-if="currentStep == 1">
            <div>
              <label for="sirketAd" class="block text-sm/5 font-semibold my-2">Şirket Adı</label>
              <div class="relative">
                <svg
                  class="size-7 fill-black absolute mt-2 left-2"
                  viewBox="0 0 50 50"
                  xmlns="http://www.w3.org/2000/svg"
                  xmlns:xlink="http://www.w3.org/1999/xlink"
                >
                  <path
                    d="M8 2L8 6L4 6L4 48L46 48L46 14L30 14L30 6L26 6L26 2 Z M 10 4L24 4L24 8L28 8L28 46L19 46L19 39L15 39L15 46L6 46L6 8L10 8 Z M 10 10L10 12L12 12L12 10 Z M 14 10L14 12L16 12L16 10 Z M 18 10L18 12L20 12L20 10 Z M 22 10L22 12L24 12L24 10 Z M 10 15L10 19L12 19L12 15 Z M 14 15L14 19L16 19L16 15 Z M 18 15L18 19L20 19L20 15 Z M 22 15L22 19L24 19L24 15 Z M 30 16L44 16L44 46L30 46 Z M 32 18L32 20L34 20L34 18 Z M 36 18L36 20L38 20L38 18 Z M 40 18L40 20L42 20L42 18 Z M 10 21L10 25L12 25L12 21 Z M 14 21L14 25L16 25L16 21 Z M 18 21L18 25L20 25L20 21 Z M 22 21L22 25L24 25L24 21 Z M 32 22L32 24L34 24L34 22 Z M 36 22L36 24L38 24L38 22 Z M 40 22L40 24L42 24L42 22 Z M 32 26L32 28L34 28L34 26 Z M 36 26L36 28L38 28L38 26 Z M 40 26L40 28L42 28L42 26 Z M 10 27L10 31L12 31L12 27 Z M 14 27L14 31L16 31L16 27 Z M 18 27L18 31L20 31L20 27 Z M 22 27L22 31L24 31L24 27 Z M 32 30L32 32L34 32L34 30 Z M 36 30L36 32L38 32L38 30 Z M 40 30L40 32L42 32L42 30 Z M 10 33L10 37L12 37L12 33 Z M 14 33L14 37L16 37L16 33 Z M 18 33L18 37L20 37L20 33 Z M 22 33L22 37L24 37L24 33 Z M 32 34L32 36L34 36L34 34 Z M 36 34L36 36L38 36L38 34 Z M 40 34L40 36L42 36L42 34 Z M 32 38L32 40L34 40L34 38 Z M 36 38L36 40L38 40L38 38 Z M 40 38L40 40L42 40L42 38 Z M 10 39L10 44L12 44L12 39 Z M 22 39L22 44L24 44L24 39 Z M 32 42L32 44L34 44L34 42 Z M 36 42L36 44L38 44L38 42 Z M 40 42L40 44L42 44L42 42Z"
                    class="dark:stroke-neutral-200 stroke-neutral-800 stroke-1"
                  />
                </svg>
                <input
                  id="sirketAd"
                  v-model="registerRequest.kurumsalBirimCreateCommand.ad"
                  type="text"
                  required
                  class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm pl-10"
                  placeholder="Sirket1"
                />
              </div>
            </div>
            <!-- Email -->
            <!-- <div>
              <label for="email" class="block text-sm/5 font-semibold my-2">Şirket Mail</label>
              <div>
                <input
                  id="email"
                  v-model="registerData.sirketIletisim.eposta"
                  type="email"
                  required
                  class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
                  placeholder="info@sirket1.com"
                />
              </div>
            </div> -->
            <!-- Telefon -->
            <!-- <div>
              <label for="telefon" class="block text-sm/5 font-semibold my-2">Şirket Telefon</label>
              <div>
                <input
                  id="telefon"
                  v-model="registerData.sirketIletisim.telefon"
                  type="phone"
                  required
                  class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
                  placeholder="0850 001 00 00"
                />
              </div>
            </div> -->
            <!-- Tarih  -->
            <!-- <div>
              <label for="tarih" class="block text-sm/5 font-semibold my-2"
                >Şirket Kuruluş Tarihi</label
              >
              <Datepicker
                id="tarih"
                v-model="registerData.sirketKurulusTarihi"
                locale="tr"
                :enable-time-picker="false"
                :format="'dd-MM-yyyy'"
                :class="{ 'dark dp__theme_dark': isDark }"
              />
            </div> -->
            <!-- İl Seçim -->
            <!-- <div>
              <label for="province" class="block text-sm/5 font-semibold my-2">Şirket İl</label>
              <select
                id="province"
                v-model="registerData.sirketAdres.sehir"
                @change="getDistrictsSirket"
                class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
              >
                <option value="">İl Seçin</option>
                <option v-for="province in provinces" :key="province.id" :value="province.name">
                  {{ province.name }}
                </option>
              </select>
            </div> -->
            <!-- İlçe Seçim -->
            <!-- <div>
              <label for="district" class="block text-sm/5 font-semibold my-2">Şirket İlçe</label>
              <select
                id="district"
                v-model="registerData.sirketAdres.ilce"
                :disabled="districts.length === 0"
                class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
              >
                <option value="">Önce İl Seçin</option>
                <option v-for="district in districts" :key="district.id" :value="district.name">
                  {{ district.name }}
                </option>
              </select>
            </div> -->
          </div>
          <!-- Şirket bilgileri end -->

          <!-- Onaylama start -->
          <div v-if="currentStep == 2" class="flex xl:flex-row flex-col w-full justify-between">
            <div class="xl:border-r flex-1">
              <h1>Kişisel Bilgiler</h1>
              <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{ registerRequest.personelCreateCommand.ad }}
                {{ registerRequest.personelCreateCommand.soyad }}
              </div>
              <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{ registerRequest.personelCreateCommand.dogumTarihi!.toISOString().split("T")[0] }}
              </div>
              <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{
                  registerRequest.personelCreateCommand.cinsiyet == undefined
                    ? "Cinsiyet Belirtilmedi"
                    : registerRequest.personelCreateCommand.cinsiyet
                    ? "Erkek"
                    : "Kadın"
                }}
              </div>
              <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{ registerRequest.personelCreateCommand.iletisim.eposta }}
              </div>
              <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{ registerRequest.personelCreateCommand.iletisim.telefon }}
              </div>
              <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{
                  registerRequest.personelCreateCommand.adres!.ulke +
                  " " +
                  registerRequest.personelCreateCommand.adres!.sehir +
                  "/" +
                  registerRequest.personelCreateCommand.adres!.ilce
                }}
              </div>
            </div>
            <!-- Şirket Bilgileri -->
            <div class="ml-2 flex-1">
              <h1>Şirket Bilgileri</h1>
              <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{ registerRequest.kurumsalBirimCreateCommand.ad }}
              </div>
              <!-- <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{ registerData.sirketKurulusTarihi.toISOString().split("T")[0] }}
              </div>
              <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{ registerData.sirketIletisim.eposta }}
              </div>
              <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{ registerData.sirketIletisim.telefon }}
              </div>
              <div class="dark:bg-neutral-800/30 rounded-md my-2 mr-2 p-2">
                {{
                  registerData.sirketAdres.ulke +
                  " " +
                  registerData.sirketAdres.sehir +
                  "/" +
                  registerData.sirketAdres.ilce
                }}
              </div> -->
            </div>
          </div>
          <!-- Onaylama end -->

          <button
            type="submit"
            class="flex items-center justify-center w-full bg-sky-600 text-neutral-200 p-2 rounded hover:bg-sky-500 mt-5"
            :disabled="isLoading"
          >
            <span v-if="currentStep != 2">Devam Et</span>
            <span v-if="currentStep == 2">Hesap oluştur</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style>
input:-webkit-autofill,
input:-webkit-autofill:hover,
input:-webkit-autofill:focus,
input:-webkit-autofill:active {
  -webkit-box-shadow: 0 0 0px 1000px white inset !important;
  box-shadow: 0 0 0px 1000px white inset !important;
  background-color: red !important;
  color: black !important;
}
.dp__theme_dark {
  --dp-background-color: #444444;
  --dp-text-color: #fff;
  --dp-hover-color: #484848;
  --dp-hover-text-color: #fff;
  --dp-hover-icon-color: #959595;
  --dp-primary-color: #005cb2;
  --dp-primary-disabled-color: #61a8ea;
  --dp-primary-text-color: #fff;
  --dp-secondary-color: #a9a9a9;
  --dp-border-color: #2d2d2d;
  --dp-menu-border-color: #2d2d2d;
  --dp-border-color-hover: #aaaeb7;
  --dp-border-color-focus: #aaaeb7;
  --dp-disabled-color: #737373;
  --dp-disabled-color-text: #d0d0d0;
  --dp-scroll-bar-background: #212121;
  --dp-scroll-bar-color: #484848;
  --dp-success-color: #00701a;
  --dp-success-color-disabled: #428f59;
  --dp-icon-color: #959595;
  --dp-danger-color: #e53935;
  --dp-marker-color: #e53935;
  --dp-tooltip-color: #3e3e3e;
  --dp-highlight-color: rgb(0 92 178 / 20%);
  --dp-range-between-dates-background-color: var(--dp-hover-color, #484848);
  --dp-range-between-dates-text-color: var(--dp-hover-text-color, #fff);
  --dp-range-between-border-color: var(--dp-hover-color, #fff);
}
.dp__theme_light {
  --dp-background-color: #e7e7e7;
  --dp-text-color: #212121;
  --dp-hover-color: #f3f3f3;
  --dp-hover-text-color: #212121;
  --dp-hover-icon-color: #959595;
  --dp-primary-color: #1976d2;
  --dp-primary-disabled-color: #6bacea;
  --dp-primary-text-color: #f8f5f5;
  --dp-secondary-color: #c0c4cc;
  --dp-border-color: #ddd;
  --dp-menu-border-color: #ddd;
  --dp-border-color-hover: #aaaeb7;
  --dp-border-color-focus: #aaaeb7;
  --dp-disabled-color: #f6f6f6;
  --dp-scroll-bar-background: #f3f3f3;
  --dp-scroll-bar-color: #959595;
  --dp-success-color: #76d275;
  --dp-success-color-disabled: #a3d9b1;
  --dp-icon-color: #959595;
  --dp-danger-color: #ff6f60;
  --dp-marker-color: #ff6f60;
  --dp-tooltip-color: #fafafa;
  --dp-disabled-color-text: #8e8e8e;
  --dp-highlight-color: rgb(25 118 210 / 10%);
  --dp-range-between-dates-background-color: var(--dp-hover-color, #f3f3f3);
  --dp-range-between-dates-text-color: var(--dp-hover-text-color, #212121);
  --dp-range-between-border-color: var(--dp-hover-color, #f3f3f3);
}
</style>
