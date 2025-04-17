<script setup lang="ts">
import type { PersonelItem } from "@/models/PersonelModels";
import PersonelService from "@/services/PersonelService";

import { onMounted, reactive, ref } from "vue";

const apiUrl = ref(import.meta.env.VITE_API_URL);

const personel: PersonelItem = reactive({
  id: "",
  ad: "",
  soyad: "",
  dogumTarihi: new Date(),
  cinsiyet: undefined,
  avatarUrl: undefined,
  iletisim: {
    eposta: "",
    telefon: "",
  },
  adres: {
    ulke: "",
    sehir: "",
    ilce: "",
    tamAdres: "",
  },
  yoneticiAd: undefined,
  yoneticiPozisyon: undefined,
  kurumsalBirimAd: "",
  pozisyonAd: "",
  sozlesmeTuruValue: 0, //Backendde yok
  baslangicTarih: new Date(),
  sozlesmeBitisTarihi: undefined, //Backendde yok
  izinKuralId: undefined, //Backendde yok
  roleClaims: [],
  isActive: true,
  createdAt: new Date(),
  createUserId: "",
  createUserName: undefined,
  updateAt: undefined,
  updateUserId: undefined,
  updateUserName: undefined,
  isDeleted: false,
  deleteAt: undefined,
});

onMounted(async () => {
  const res = await PersonelService.getCurrentPersonel();
  Object.assign(personel, res);
});
</script>

<template>
  <div class="bg-white dark:bg-neutral-800/80 p-4 mb-3 rounded-lg">
    <div
      class="lg:col-span-2 bg-white dark:bg-neutral-800 rounded-lg shadow-md overflow-hidden hover-card"
    >
      <div class="p-6">
        <div class="overflow-x-auto flex md:flex-row flex-col justify-between">
          <div class="flex flex-row justify-between md:justify-start flex-1">
            <div class="flex flex-col mr-5">
              <span class="text-xl font-semibold">{{ personel.ad + " " + personel.soyad }}</span>
              <span class="text-md dark:text-neutral-500">{{ personel.pozisyonAd }}</span>
              <span class="text-sm dark:text-neutral-300">{{ personel.kurumsalBirimAd }}</span>
            </div>
            <div>
              <img
                v-if="personel.avatarUrl"
                class="object-cover mx-2 size-20 rounded-xl"
                :src="apiUrl + personel.avatarUrl"
                alt="Avatar"
                width="100"
                height="100"
              />
              <div
                v-else
                class="text-4xl font-semibold text-sky-600 transition-all duration-300 ease-in-out mx-2 rounded-full border-1 border-sky-500 w-16 h-16 flex items-center justify-center"
              >
                {{ personel.ad[0] }}
              </div>
            </div>
          </div>
          <div class="flex flex-col flex-1 justify-between mt-10 md:mt-0">
            <div
              class="flex justify-between border-b pb-3 border-neutral-700 dark:border-neutral-400"
            >
              <span class="text-sm text-neutral-500 dark:text-neutral-400">Yoneticim</span>
              <span class="mr-10 text-sm text-neutral-700 dark:text-neutral-200">{{
                personel.yoneticiAd
              }}</span>
            </div>
            <div
              class="flex justify-between border-b pb-3 text-neutral-500 border-neutral-700 dark:border-neutral-400 mt-5 xl:mt-0"
            >
              <span class="text-sm text-neutral-500 dark:text-neutral-400">İse Baslama Tarihi</span>
              <span class="mr-10 text-sm text-neutral-700 dark:text-neutral-200">{{
                new Date(personel.createdAt).toLocaleString("tr-TR", {
                  day: "2-digit",
                  month: "2-digit",
                  year: "numeric",
                  hour: "2-digit",
                  minute: "2-digit",
                  hour12: false,
                })
              }}</span>
            </div>
          </div>
        </div>
      </div>
      <RouterLink
        to="/dashboard/profile"
        class="w-full flex justify-end mb-5 pr-10 text-sky-500 hover:text-sky-700"
        >Hesabım</RouterLink
      >
    </div>
  </div>
</template>
<style scoped>
.hover-card {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}
.hover-card:hover {
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
}
</style>
