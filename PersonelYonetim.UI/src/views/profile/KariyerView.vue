<script setup lang="ts">
import TableLayout from "@/components/TableLayout.vue";
import { ref } from "vue";

const activeTab2 = ref("pozisyon");

const setActiveTab2 = (tab: string) => {
  activeTab2.value = tab;
};

const pozisyonData = [
  {
    baslangic: "8 Ağustos 2024",
    bitis: "8 Ağustos 2024",
    calismaSekli: "Tam zamanlı",
    yonetici: "",

    sirket: "ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ",
    sube: "",
    departman: "Yazılım Üretim",
    unvan: "Yazılım Personeli",
    varsayilan: false,
  },
  {
    baslangic: "8 Ağustos 2024",
    bitis: null,
    calismaSekli: "Tam zamanlı",
    yonetici: "Adil Mert Şahin",

    sirket: "ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ",
    sube: "",
    departman: "Yazılım Üretim",
    unvan: "Yazılım Personeli",
    varsayilan: true,
  },
  {
    baslangic: "7 Ağustos 2024",
    bitis: "8 Ağustos 2024",
    calismaSekli: "Tam zamanlı",
    yonetici: "BAHAR SERDAR",

    sirket: "ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ",
    sube: "",
    departman: "Yazılım Üretim",
    unvan: "Yazılım Personeli",
    varsayilan: false,
  },
  {
    baslangic: "5 Nisan 2024",
    bitis: "7 Ağustos 2024",
    calismaSekli: "Tam zamanlı",
    yonetici: "Adil Mert Şahin",

    sirket: "ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ",
    sube: "",
    departman: "Yazılım Üretim",
    unvan: "Yazılım Personeli",
    varsayilan: false,
  },
];

// ÇALIŞMA TAKVİMİ
const calismaTakvimiData = [
  {
    baslangic: "13 Eki 2020",
    bitis: null,
    sure: "4 yıl 5 ay 16 gün",
    calismaTakvimi: "Genel çalışma tablosu",
    atamaTarihi: "16 Oca 2023",
    guncel: true,
  },
  {
    baslangic: "2 Ağu 2019",
    bitis: "12 Eki 2020",
    sure: "1 yıl 2 ay 13 gün",
    calismaTakvimi: "Genel çalışma tablosu",
    atamaTarihi: "7 Şub 2023",
    guncel: false,
  },
];
</script>

<template>
  <div class="mx-10 my-5">
    <ul
      class="flex flex-wrap text-sm font-medium text-center border-b border-gray-200 dark:border-gray-700"
    >
      <li class="me-2">
        <button
          @click="setActiveTab2('pozisyon')"
          :class="
            activeTab2 === 'pozisyon'
              ? 'text-neutral-100 bg-sky-600  hover:bg-sky-500 dark:text-neutral-50'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Pozisyon
        </button>
      </li>
      <li class="me-2">
        <button
          @click="setActiveTab2('maas')"
          :class="
            activeTab2 === 'maas'
              ? 'bg-sky-600  hover:bg-sky-500 text-neutral-100 dark:text-neutral-50'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Maaş
        </button>
      </li>
      <li class="me-2">
        <button
          @click="setActiveTab2('calismaTakvimi')"
          :class="
            activeTab2 === 'calismaTakvimi'
              ? 'bg-sky-600 hover:bg-sky-500 text-neutral-100'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Çalışma Takvimi
        </button>
      </li>
      <li class="me-2">
        <button
          @click="setActiveTab2('performans')"
          :class="
            activeTab2 === 'performans'
              ? 'bg-sky-600 hover:bg-sky-500 text-neutral-100'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Performans
        </button>
      </li>
    </ul>

    <div v-if="activeTab2 === 'pozisyon'" class="">
      <div class="overflow-x-auto mt-5">
        <TableLayout
          :table-headers="[
            'Başlangıç',
            'Bitiş',
            'Çalışma Şekli',
            'Yönetici',
            'Şirket',
            'Şube',
            'Departman',
            'Unvan',
            'Durum',
          ]"
          :table-content="pozisyonData"
          :islemler="['edit']"
        />
      </div>
    </div>

    <div v-if="activeTab2 === 'maas'" class="space-y-6">
      <div class="flex justify-center items-center mt-10 w-full">
        <div
          class="border-2 border-neutral-200 dark:border-neutral-500 p-6 bg-transparent rounded-lg w-full"
        >
          <div class="text-center my-5">
            <i class="fa-solid fa-wallet fa-2xl" style="color: #3562b1"></i>

            <br />
            <br />
            <p class="text-gray-800 dark:text-neutral-300 text-l mb-4">Maaş bilgisi bulunamadı</p>

            <p class="text-gray-700 dark:text-neutral-400 text-sm">
              Bordro işlemleri için bir maaş bilgisi ekleyin
            </p>
          </div>
        </div>
      </div>
    </div>

    <div v-if="activeTab2 === 'calismaTakvimi'" class="space-y-6">
      <div class="overflow-x-auto mt-10">
        <TableLayout
          :table-headers="[
            'Başlangıç',
            'Bitiş',
            'Süre',
            'Çalışma Takvimi',
            'Atanma Tarihi',
            'Durum',
          ]"
          :table-content="calismaTakvimiData"
          :islemler="['detaylar']"
        />
      </div>
    </div>

    <div v-if="activeTab2 === 'performans'" class="space-y-6">
      <div class="flex justify-center items-center mt-10 w-full">
        <div
          class="border-2 border-neutral-200 dark:border-neutral-500 p-6 bg-transparent rounded-lg w-full"
        >
          <div class="text-center my-5">
            <i class="fa-solid fa-circle-exclamation fa-2xl" style="color: #3562b1"></i>

            <br />
            <br />
            <p class="text-gray-800 dark:text-neutral-300 text-l mb-4">
              Girilen kriterlere uygun sonuç bulunamadı
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
