<script setup lang="ts">
import { ref, onMounted } from "vue";
import AuthService from "@/services/AuthService";
import { useRouter } from "vue-router";
import type { RegisterRequest } from "@/models/RegisterRequest";
import axios from "axios";

const router = useRouter();
const isLoading = ref(false);
const loginResponse = ref("");

const registerData = ref<RegisterRequest>({
  sirketAd: "",
  ad: "",
  soyad: "",
  dogumTarihi: new Date(),
  cinsiyet: undefined,
  iletisim: { eposta: "", telefon: "01" },
  adres: { ulke: "Türkiye", sehir: "", ilce: "", tamAdres: "" },
  sifre: "",
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
  if (!registerData.value.adres.sehir) return;
  try {
    const response = await axios.get(
      `https://turkiyeapi.dev/api/v1/provinces?name=${registerData.value.adres.sehir}`
    );
    districts.value = response.data.data[0].districts;
  } catch (error) {
    console.error("İlçe verileri alınırken hata oluştu:", error);
  }
};

onMounted(() => {
  getProvinces();
});

const handleRegister = async () => {
  try {
    isLoading.value = true;
    registerData.value.adres.tamAdres = `${registerData.value.adres.sehir}/${registerData.value.adres.ilce}  ${registerData.value.adres.ulke}`;

    const response = await AuthService.register(registerData.value);
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
  <div class="flex justify-end h-dvh">
    <div
      class="w-full h-full flex items-center justify-center bg-gradient-to-r from-sky-600 to-sky-700"
    >
      <h1 class="text-4xl font-extrabold">Personel Yönetim Sistemi</h1>
    </div>

    <div
      class="flex flex-col items-center justify-center md:w-1/2 h-full bg-neutral-200 dark:bg-neutral-700"
    >
      <form @submit.prevent="handleRegister" class="space-y-4 w-full px-16">
        <div>
          <h1 class="text-2xl font-semibold text-center mb-6">Kayıt Sayfası</h1>
          <!-- Şirket Adı -->
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
                v-model="registerData.sirketAd"
                type="text"
                required
                class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm pl-10"
                placeholder="Sirket1"
              />
            </div>
          </div>
          <!-- Ad Soyad -->
          <div class="flex">
            <div class="mr-1">
              <label for="ad" class="block text-sm/5 font-semibold my-2">Ad</label>
              <div>
                <input
                  id="ad"
                  v-model="registerData.ad"
                  type="text"
                  required
                  class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
                  placeholder="John"
                />
              </div>
            </div>
            <div class="ml-1">
              <label for="soyad" class="block text-sm/5 font-semibold my-2">Soyad</label>
              <div>
                <input
                  id="soyad"
                  v-model="registerData.soyad"
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
                v-model="registerData.iletisim.eposta"
                type="email"
                required
                class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
                placeholder="user@mail.com"
              />
            </div>
          </div>
          <!-- İl Seçim -->
          <div>
            <label for="province" class="block text-sm/5 font-semibold my-2">İl Seçin</label>
            <select
              id="province"
              v-model="registerData.adres.sehir"
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
              v-model="registerData.adres.ilce"
              :disabled="districts.length === 0"
              class="w-full outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
            >
              <option value="">Önce İl Seçin</option>
              <option v-for="district in districts" :key="district.id" :value="district.name">
                {{ district.name }}
              </option>
            </select>
          </div>
          <!-- Şifre -->
          <div>
            <label for="password" class="block text-sm/5 font-semibold my-2">Şifre</label>
            <div>
              <input
                id="password"
                v-model="registerData.sifre"
                type="password"
                required
                class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm"
                placeholder="******"
              />
            </div>
          </div>

          <button
            type="submit"
            class="flex items-center justify-center w-full bg-sky-600 text-neutral-200 p-2 rounded hover:bg-sky-500 mt-5"
            :disabled="isLoading"
          >
            <span>Hesap oluştur</span>
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
</style>
