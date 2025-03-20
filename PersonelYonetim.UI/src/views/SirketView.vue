<script setup lang="ts">
import type { DepartmanModel } from "@/models/DepartmanModel";
import type { IzinItem } from "@/models/IzinModels";
import type { PozisyonModel } from "@/models/PozisyonModel";
import type { SirketModel } from "@/models/SirketModel";
import type { SubeModel } from "@/models/SubeModel";
import DepartmanService from "@/services/DepartmanService";
import PozisyonService from "@/services/PozisyonService";
import SirketService from "@/services/SirketService";
import SubeService from "@/services/SubeService";
import { onMounted, ref, type Ref } from "vue";

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

const sirketler: Ref<SirketModel[] | undefined> = ref([]);
const subeler: Ref<SubeModel[] | undefined> = ref([]);
const departmanlar: Ref<DepartmanModel[] | undefined> = ref([]);
const pozisyonlar: Ref<PozisyonModel[] | undefined> = ref([]);

onMounted(async () => {});
const getSirketler = async () => {
  if (expand.value.sirketler) {
    loading.value.sirketler = true;
    try {
      sirketler.value = await SirketService.getSirketler();
    } catch (error) {
      console.error("Veri çekme hatası:", error);
    } finally {
      loading.value.sirketler = false;
    }
  }
};

const getSubeler = async () => {
  if (expand.value.subeler) {
    loading.value.subeler = true;
    try {
      subeler.value = await SubeService.getSubeler("");
    } catch (error) {
      console.error("Veri çekme hatası:", error);
    } finally {
      loading.value.subeler = false;
    }
  }
};

const getDepartmanlar = async () => {
  if (expand.value.departmanlar) {
    loading.value.departmanlar = true;
    try {
      departmanlar.value = await DepartmanService.getDepartmanlar("");
    } catch (error) {
      console.error("Veri çekme hatası:", error);
    } finally {
      loading.value.departmanlar = false;
    }
  }
};
const getPozisyonlar = async () => {
  if (expand.value.pozisyonlar) {
    loading.value.pozisyonlar = true;
    try {
      pozisyonlar.value = await PozisyonService.getPozisyonlar("");
      console.log(pozisyonlar.value);
    } catch (error) {
      console.error("Veri çekme hatası:", error);
    } finally {
      loading.value.pozisyonlar = false;
    }
  }
};

const izinList = ref<IzinItem[]>([]);

izinList.value = [
  {
    id: 1,
    personelAdi: "Ahmet Yılmaz",
    izinTipi: "Yıllık İzin",
    baslangicTarihi: "15.03.2025",
    bitisTarihi: "20.03.2025",
    gun: 5,
    durum: "Onaylandı",
    aciklama: "Yıllık izin talebi",
  },
  {
    id: 2,
    personelAdi: "Ayşe Demir",
    izinTipi: "Mazeret İzni",
    baslangicTarihi: "22.03.2025",
    bitisTarihi: "24.03.2025",
    gun: 2,
    durum: "Beklemede",
    aciklama: "Aile ziyareti için izin",
  },
  {
    id: 3,
    personelAdi: "Mehmet Kaya",
    izinTipi: "Yıllık İzin",
    baslangicTarihi: "01.04.2025",
    bitisTarihi: "10.04.2025",
    gun: 9,
    durum: "Reddedildi",
    aciklama: "Yoğun iş dönemi nedeniyle reddedildi",
  },
  {
    id: 4,
    personelAdi: "Zeynep Öztürk",
    izinTipi: "Hastalık İzni",
    baslangicTarihi: "05.04.2025",
    bitisTarihi: "07.04.2025",
    gun: 2,
    durum: "Onaylandı",
    aciklama: "Doktor raporu ile onaylandı",
  },
  {
    id: 5,
    personelAdi: "Can Yılmaz",
    izinTipi: "Yıllık İzin",
    baslangicTarihi: "12.04.2025",
    bitisTarihi: "16.04.2025",
    gun: 4,
    durum: "Beklemede",
    aciklama: "Tatil planı için izin",
  },
];
</script>

<template>
  <div class="flex flex-col">
    <div class="w-full mt-2 ml-5">
      <h1 class="text-2xl">Şirket Birimleri</h1>
      <p class="text-sm text-neutral-400 mt-2">Şirketteki birimleri buradan düzenleyebilirsiniz.</p>
    </div>
    <div class="flex flex-col mt-5">
      <!-- Şirketler start -->
      <div
        class="mx-10 p-3 my-3 bg-neutral-200 dark:bg-neutral-700 rounded-sm outline-1 outline-neutral-300"
      >
        <div class="flex justify-between">
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="icon size-6 cursor-pointer select-none"
              xmlns="http://www.w3.org/2000/svg"
              v-on:click="
                () => {
                  expand.sirketler = !expand.sirketler;
                  getSirketler();
                }
              "
            >
              <path
                d="M903.232 256l56.768 50.432L512 768 64 306.432 120.768 256 512 659.072z"
                class="fill-neutral-500 dark:fill-neutral-200"
              />
            </svg>
            <span class="text-xl ml-2">Sirketler</span>
          </div>
          <div class="flex items-center">
            <div class="bg-neutral-300 dark:bg-neutral-600 py-1 px-2 rounded-sm mr-2">1</div>
            <button
              class="bg-sky-600 text-neutral-100 px-3 py-1 rounded-sm text-base font-normal cursor-pointer hover:text-neutral-200"
            >
              Yeni Ekle
            </button>
          </div>
        </div>
        <div v-if="loading.sirketler" class="w-full flex items-center justify-center">
          <div
            class="size-7 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
          ></div>
        </div>

        <transition v-if="!loading.sirketler">
          <div
            v-show="expand.sirketler"
            class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm mt-5"
          >
            <div class="overflow-x-auto">
              <table class="min-w-full divide-y divide-gray-200 dark:divide-neutral-700">
                <thead class="bg-gray-50 dark:bg-neutral-700">
                  <tr>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Ad
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      E-posta
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Adres
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Oluşturulma Tarihi
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Oluşturan
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Toplam Personel
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      İşlemler
                    </th>
                  </tr>
                </thead>
                <tbody
                  class="bg-white dark:bg-neutral-800 divide-y divide-gray-200 dark:divide-neutral-700"
                >
                  <tr
                    v-for="sirket in sirketler"
                    :key="sirket.id"
                    class="hover:bg-gray-50 dark:hover:bg-neutral-700"
                  >
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ sirket.ad }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap">
                      <div class="flex items-center">
                        <div class="ml-4">
                          <div class="text-sm font-medium text-gray-900 dark:text-gray-200">
                            {{ sirket.iletisim.eposta }}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ sirket.adres.sehir }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ new Date(sirket.createdAt).toISOString().split("T")[0] }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ sirket.createUserName }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm">
                      <span class="px-2 py-1 rounded-full text-xs font-medium">
                        {{ sirket.isActive }}
                      </span>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <button
                        class="text-sky-600 dark:text-sky-400 hover:text-sky-800 dark:hover:text-sky-300 mr-3"
                      >
                        <i class="fas fa-eye"></i>
                      </button>
                      <button
                        class="text-amber-600 dark:text-amber-400 hover:text-amber-800 dark:hover:text-amber-300"
                      >
                        <i class="fas fa-edit"></i>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </transition>
      </div>
      <!-- Şirketler end -->
      <!-- Şubeler start -->
      <div
        class="mx-10 p-3 my-3 bg-neutral-200 dark:bg-neutral-700 rounded-sm outline-1 outline-neutral-300"
      >
        <div class="flex justify-between">
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="icon size-6 cursor-pointer select-none"
              xmlns="http://www.w3.org/2000/svg"
              v-on:click="
                () => {
                  expand.subeler = !expand.subeler;
                  getSubeler();
                }
              "
            >
              <path
                d="M903.232 256l56.768 50.432L512 768 64 306.432 120.768 256 512 659.072z"
                class="fill-neutral-500 dark:fill-neutral-200"
              />
            </svg>
            <span class="text-xl ml-2">Subeler</span>
          </div>
          <div class="flex items-center">
            <div class="bg-neutral-300 dark:bg-neutral-600 py-1 px-2 rounded-sm mr-2">1</div>
            <button
              class="bg-sky-600 text-neutral-100 px-3 py-1 rounded-sm text-base font-normal cursor-pointer hover:text-neutral-200"
            >
              Yeni Ekle
            </button>
          </div>
        </div>
        <div v-if="loading.subeler" class="w-full flex items-center justify-center">
          <div
            class="size-7 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
          ></div>
        </div>

        <transition v-if="!loading.subeler">
          <div
            v-show="expand.subeler"
            class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm mt-5"
          >
            <div class="overflow-x-auto">
              <table class="min-w-full divide-y divide-gray-200 dark:divide-neutral-700">
                <thead class="bg-gray-50 dark:bg-neutral-700">
                  <tr>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Ad
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      E-posta
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Adres
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Oluşturulma Tarihi
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Oluşturan
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Şirket
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Durum
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      İşlemler
                    </th>
                  </tr>
                </thead>
                <tbody
                  class="bg-white dark:bg-neutral-800 divide-y divide-gray-200 dark:divide-neutral-700"
                >
                  <tr
                    v-for="sube in subeler"
                    :key="sube.id"
                    class="hover:bg-gray-50 dark:hover:bg-neutral-700"
                  >
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ sube.ad }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap">
                      <div class="flex items-center">
                        <div class="ml-4">
                          <div class="text-sm font-medium text-gray-900 dark:text-gray-200">
                            {{ sube.iletisim.eposta }}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ sube.adres.sehir }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ new Date(sube.createdAt).toISOString().split("T")[0] }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ sube.createUserName }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ sube.sirketAd }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm">
                      {{ sube.isActive }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <button
                        class="text-sky-600 dark:text-sky-400 hover:text-sky-800 dark:hover:text-sky-300 mr-3"
                      >
                        <i class="fas fa-eye"></i>
                      </button>
                      <button
                        class="text-amber-600 dark:text-amber-400 hover:text-amber-800 dark:hover:text-amber-300"
                      >
                        <i class="fas fa-edit"></i>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </transition>
      </div>
      <!-- Şubeler end -->
      <!-- Departmanlar start -->
      <div
        class="mx-10 p-3 my-3 bg-neutral-200 dark:bg-neutral-700 rounded-sm outline-1 outline-neutral-300"
      >
        <div class="flex justify-between">
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="icon size-6 cursor-pointer select-none"
              xmlns="http://www.w3.org/2000/svg"
              v-on:click="
                () => {
                  expand.departmanlar = !expand.departmanlar;
                  getDepartmanlar();
                }
              "
            >
              <path
                d="M903.232 256l56.768 50.432L512 768 64 306.432 120.768 256 512 659.072z"
                class="fill-neutral-500 dark:fill-neutral-200"
              />
            </svg>
            <span class="text-xl ml-2">Departmanlar</span>
          </div>

          <div class="flex items-center">
            <div class="bg-neutral-300 dark:bg-neutral-600 py-1 px-2 rounded-sm mr-2">1</div>
            <button
              class="bg-sky-600 text-neutral-100 px-3 py-1 rounded-sm text-base font-normal cursor-pointer hover:text-neutral-200"
            >
              Yeni Ekle
            </button>
          </div>
        </div>
        <div v-if="loading.departmanlar" class="w-full flex items-center justify-center">
          <div
            class="size-7 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
          ></div>
        </div>

        <transition v-if="!loading.departmanlar">
          <div
            v-show="expand.departmanlar"
            class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm mt-5"
          >
            <div class="overflow-x-auto">
              <table class="min-w-full divide-y divide-gray-200 dark:divide-neutral-700">
                <thead class="bg-gray-50 dark:bg-neutral-700">
                  <tr>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Ad
                    </th>

                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Oluşturulma Tarihi
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Oluşturan
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Şube
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Toplam Personel
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Durum
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      İşlemler
                    </th>
                  </tr>
                </thead>
                <tbody
                  class="bg-white dark:bg-neutral-800 divide-y divide-gray-200 dark:divide-neutral-700"
                >
                  <tr
                    v-for="departman in departmanlar"
                    :key="departman.id"
                    class="hover:bg-gray-50 dark:hover:bg-neutral-700"
                  >
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ departman.ad }}
                    </td>

                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ new Date(departman.createdAt).toISOString().split("T")[0] }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ departman.createUserName }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm">
                      {{ departman.subeAd }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm">
                      <span class="px-2 py-1 rounded-full text-xs font-medium"> 12 </span>
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ departman.isActive }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <button
                        class="text-sky-600 dark:text-sky-400 hover:text-sky-800 dark:hover:text-sky-300 mr-3"
                      >
                        <i class="fas fa-eye"></i>
                      </button>
                      <button
                        class="text-amber-600 dark:text-amber-400 hover:text-amber-800 dark:hover:text-amber-300"
                      >
                        <i class="fas fa-edit"></i>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </transition>
      </div>

      <!-- Departmanlar end -->
      <!-- Pozisyonlar start -->
      <div
        class="mx-10 p-3 my-3 bg-neutral-200 dark:bg-neutral-700 rounded-sm outline-1 outline-neutral-300"
      >
        <div class="flex justify-between">
          <div class="flex items-center">
            <svg
              viewBox="0 0 1024 1024"
              class="icon size-6 cursor-pointer select-none"
              xmlns="http://www.w3.org/2000/svg"
              v-on:click="
                () => {
                  expand.pozisyonlar = !expand.pozisyonlar;
                  getPozisyonlar();
                }
              "
            >
              <path
                d="M903.232 256l56.768 50.432L512 768 64 306.432 120.768 256 512 659.072z"
                class="fill-neutral-500 dark:fill-neutral-200"
              />
            </svg>
            <span class="text-xl ml-2">Pozisyonlar</span>
          </div>
          <div class="flex items-center">
            <div class="bg-neutral-300 dark:bg-neutral-600 py-1 px-2 rounded-sm mr-2">1</div>
            <button
              class="bg-sky-600 text-neutral-100 px-3 py-1 rounded-sm text-base font-normal cursor-pointer hover:text-neutral-200"
            >
              Yeni Ekle
            </button>
          </div>
        </div>
        <div v-if="loading.pozisyonlar" class="w-full flex items-center justify-center">
          <div
            class="size-7 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
          ></div>
        </div>
        <transition v-if="!loading.pozisyonlar">
          <div
            v-show="expand.pozisyonlar"
            class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm mt-5"
          >
            <div class="overflow-x-auto">
              <table class="min-w-full divide-y divide-gray-200 dark:divide-neutral-700">
                <thead class="bg-gray-50 dark:bg-neutral-700">
                  <tr>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Ad
                    </th>

                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Oluşturulma Tarihi
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Oluşturan
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Şirket
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Durum
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      İşlemler
                    </th>
                  </tr>
                </thead>
                <tbody
                  class="bg-white dark:bg-neutral-800 divide-y divide-gray-200 dark:divide-neutral-700"
                >
                  <tr
                    v-for="pozisyon in pozisyonlar"
                    :key="pozisyon.id"
                    class="hover:bg-gray-50 dark:hover:bg-neutral-700"
                  >
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ pozisyon.ad }}
                    </td>

                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ new Date(pozisyon.createdAt).toISOString().split("T")[0] }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ pozisyon.createUserName }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm">
                      {{ pozisyon.sirketAd }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm">
                      <span class="px-2 py-1 rounded-full text-xs font-medium"> 12 </span>
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
                    >
                      {{ pozisyon.isActive }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <button
                        class="text-sky-600 dark:text-sky-400 hover:text-sky-800 dark:hover:text-sky-300 mr-3"
                      >
                        <i class="fas fa-eye"></i>
                      </button>
                      <button
                        class="text-amber-600 dark:text-amber-400 hover:text-amber-800 dark:hover:text-amber-300"
                      >
                        <i class="fas fa-edit"></i>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </transition>
      </div>
      <!-- Pozisyonlar end -->
    </div>
  </div>
</template>

<style scoped>
.expand-enter-active,
.expand-leave-active {
  transition: opacity 0.1s ease;
}

.expand-enter-from,
.expand-leave-to {
  opacity: 0;
}
</style>
