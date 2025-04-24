<script setup lang="ts">
import type { BordroKazancEkleRequest } from "@/models/request-models/BordroKazancEkleRequest";
import type { BordroKesintiEkleRequest } from "@/models/request-models/BordroKesintiEkleRequest";
import type { BordroGetAllModel } from "@/models/response-models/BordroGetAllModel";
import BordroService from "@/services/BordroService";
import MaasPusulaService from "@/services/MaasPusulaService";
import { onMounted, ref, watch, type Ref } from "vue";

const isYilMenuOpen = ref(false);
const isAyMenuOpen = ref(false);

const isIslemlerMenuOpen = ref(false);
const isKazancEkleModalOpen = ref(false);
const isKesintiEkleModalOpen = ref(false);

const selectedPusulaId: Ref<string> = ref("");

const bordro: Ref<BordroGetAllModel[] | undefined> = ref([]);
const bordroCount = ref(0);

const kazancEkleRequest: Ref<BordroKazancEkleRequest> = ref({
  maasPusulaId: "",
  kazancTuru: "",
  aciklama: undefined,
  tutar: 0,
  sgkMatrahinaDahil: true,
  gelirVergisiMatrahinaDahil: true,
  damgaVergisiMatrahinaDahil: true,
});
const kesintiEkleRequest: Ref<BordroKesintiEkleRequest> = ref({
  maasPusulaId: "",
  kesintiTuru: "",
  aciklama: undefined,
  tutar: 0,
  sgkMatrahinaDahil: true,
  gelirVergisiMatrahinaDahil: true,
  damgaVergisiMatrahinaDahil: true,
});

const apiUrl = import.meta.env.VITE_API_URL;

const pusulaIdler: Ref<string[]> = ref([]);
const selectAllChecked = ref(false);

const selectedTableGroup = ref<{
  header: string;
  bgColor: string;
  borderColor: string;
  items: string[];
}>({
  header: "Girdiler",
  bgColor: "bg-slate-600/10",
  borderColor: "border-slate-600",
  items: ["Baz Ücret", "SGK", "Ek Ödemeler", "Özel Kesintiler"],
});
const statusColors: Record<string, string> = {
  Onaylandi: "text-green-600 bg-green-100 dark:bg-green-900 dark:text-green-300",
  Beklemede: "text-amber-600 bg-amber-100 dark:bg-amber-900 dark:text-amber-300",
  Reddedildi: "text-red-600 bg-red-100 dark:bg-red-900 dark:text-red-300",
  Iptal: "text-red-600 bg-red-100 dark:bg-red-900 dark:text-red-300",
};
const selectedYil = ref(2025);
const selectedAy = ref(2);

const tableGroupHeaders = [
  {
    header: "Girdiler",
    bgColor: "bg-slate-400/10",
    borderColor: "border-slate-600",
    items: ["Baz Ücret", "SGK", "Ek Ödemeler", "Özel Kesintiler"],
  },
  {
    header: "Kazançlar",
    bgColor: "bg-green-200/10",
    borderColor: "border-green-600",
    items: ["Günlük Ücret", "Ödemeye Esas", "Normal Kazanç"], // eklenebilir
  },
  {
    header: "SGK",
    bgColor: "bg-yellow-200/10",
    borderColor: "border-yellow-500",
    items: [
      "SGK Günü",
      "Fiili Çalışma",
      "Ücretli İzin",
      "Raporlu",
      "Ücretsiz İzin",
      "Diğer Eksik",
      "Ek Odeme İstisna",
    ], // eklenebilir
  },
  {
    header: "Gelir Vergisi",
    bgColor: "bg-blue-200/10",
    borderColor: "border-blue-500",
    items: ["Kümülatif Matrah", "Ek Ödeme İstisna", "GV Aylık Matrah", "GV ödemesi"],
  },
  {
    header: "Damga Vergisi",
    bgColor: "bg-yellow-950/10",
    borderColor: "border-yellow-950",
    items: ["Ek Ödeme İstisna", "DV Aylık Matrah", "DV Ödemesi"],
  },
  {
    header: "Kesintiler",
    bgColor: "bg-red-200/10",
    borderColor: "border-red-500",
    items: ["Yasal Kesintiler", "Özel Kesintiler", "Ayni Yardımlar", "Tüm Kesintiler"],
  },
  {
    header: "Maliyet",
    bgColor: "bg-red-500/10",
    borderColor: "border-red-700",
    items: ["Ele Geçen Ücret", "Teşvikler", "Teşvikli Maliyet"],
  },
];

const yillar: { id: number; yil: number }[] = [];
const suankiYil = new Date().getFullYear();

for (let i = 0; i <= 10; i++) {
  yillar.push({ id: i, yil: suankiYil - i });
}

const aylar = [
  { id: 1, ad: "Ocak" },
  { id: 2, ad: "Şubat" },
  { id: 3, ad: "Mart" },
  { id: 4, ad: "Nisan" },
  { id: 5, ad: "Mayıs" },
  { id: 6, ad: "Haziran" },
  { id: 7, ad: "Temmuz" },
  { id: 8, ad: "Ağustos" },
  { id: 9, ad: "Eylül" },
  { id: 10, ad: "Ekim" },
  { id: 11, ad: "Kasım" },
  { id: 12, ad: "Aralık" },
];
onMounted(() => {
  const today = new Date();
  selectedYil.value = today.getFullYear();
  selectedAy.value = today.getMonth() + 1;
});

const getAllBordro = async () => {
  const res = await BordroService.bordroGetAll(selectedYil.value, selectedAy.value);
  bordro.value = res?.items;
  bordroCount.value = res!.count;
};

const getMaasPusulaPdf = async (personelId: string, yil: number, ay: number) => {
  await MaasPusulaService.getMaasPusulaPdf(personelId, yil, ay);
};

const selectAll = () => {
  if (selectAllChecked.value) {
    pusulaIdler.value = bordro.value!.map((x) => x.personelId);
  } else {
    pusulaIdler.value = [];
  }
};

const toggleSinglePusula = (pusulaId: string) => {
  if (pusulaIdler.value.includes(pusulaId)) {
    pusulaIdler.value = pusulaIdler.value.filter((personelId) => personelId !== pusulaId);
  } else {
    pusulaIdler.value.push(pusulaId);
  }
  selectAllChecked.value =
    bordro.value?.every((x) => pusulaIdler.value.includes(x.personelId)) ?? false;
};

const getPusulaPdfler = async () => {
  for (const personelId of pusulaIdler.value) {
    await MaasPusulaService.getMaasPusulaPdf(personelId, selectedYil.value, selectedAy.value);
  }
};

const kazancEkle = async () => {
  kazancEkleRequest.value.maasPusulaId = selectedPusulaId.value;
  const res = await BordroService.bordroKazancEkle(kazancEkleRequest.value);
  if (res) {
    isKazancEkleModalOpen.value = false;
  }
};

const kesintiEkle = async () => {
  kesintiEkleRequest.value.maasPusulaId = selectedPusulaId.value;
  const res = await BordroService.bordroKesintiEkle(kesintiEkleRequest.value);
  if (res) {
    isKesintiEkleModalOpen.value = false;
  }
};

watch(selectedYil, () => getAllBordro());
watch(selectedAy, () => getAllBordro());
</script>

<template>
  <div class="flex flex-col w-full h-full px-5">
    <div class="flex justify-between">
      <div class="flex flex-1">
        <div class="relative">
          <button
            id="dropdownBgHoverButton"
            class="w-[10rem] text-neutral-700 border flex justify-end border-neutral-300 dark:border-gray-600 bg-neutral-50 cursor-pointer outline-none focus:outline-none font-medium rounded-md text-sm px-5 py-2.5 text-start items-center dark:bg-neutral-700 dark:hover:bg-neutral-600 dark:text-neutral-200"
            type="button"
            @click="() => (isYilMenuOpen = !isYilMenuOpen)"
          >
            <span class="flex-1 pr-10">{{ yillar.find((x) => x.yil == selectedYil)!.yil }}</span>
            <svg
              class="w-2.5 h-2.5 ms-3"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 10 6"
            >
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m1 1 4 4 4-4"
              />
            </svg>
          </button>

          <div
            v-if="isYilMenuOpen"
            class="absolute w-[10rem] z-50 bg-neutral-300 dark:bg-neutral-700 rounded-md"
          >
            <ul
              class="p-3 space-y-1 text-sm text-gray-700 dark:text-gray-200 select-none max-h-[40dvh] overflow-y-auto"
            >
              <li
                v-for="yil in yillar"
                :key="yil.id"
                @click="
                  () => {
                    selectedYil = yil.yil;
                    isYilMenuOpen = !isYilMenuOpen;
                  }
                "
              >
                <div
                  class="flex items-center p-2 rounded-sm hover:bg-gray-100 dark:hover:bg-gray-600"
                >
                  {{ yil.yil }}
                </div>
              </li>
            </ul>
          </div>
        </div>

        <div class="relative ml-5">
          <button
            id="dropdownBgHoverButton"
            class="w-[10rem] text-neutral-700 border border-neutral-300 dark:border-gray-600 bg-neutral-50 cursor-pointer outline-none focus:outline-none font-medium rounded-md text-sm px-8 py-2.5 flex items-center dark:bg-neutral-700 dark:hover:bg-neutral-600 dark:text-neutral-200"
            type="button"
            @click="() => (isAyMenuOpen = !isAyMenuOpen)"
          >
            <span class="pr-5 flex-1 flex justify-start">{{
              aylar.find((x) => x.id == selectedAy)!.ad
            }}</span>
            <svg
              class="size-3"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 10 6"
            >
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m1 1 4 4 4-4"
              />
            </svg>
          </button>

          <div
            v-if="isAyMenuOpen"
            class="w-[10rem] absolute z-50 bg-neutral-300 dark:bg-neutral-700 rounded-md"
          >
            <ul
              class="p-3 space-y-1 text-sm text-gray-700 dark:text-gray-200 select-none max-h-[40dvh] overflow-y-auto"
            >
              <li
                v-for="ay in aylar"
                :key="ay.id"
                @click="
                  () => {
                    selectedAy = ay.id;
                    isAyMenuOpen = !isAyMenuOpen;
                  }
                "
              >
                <div
                  class="flex items-center p-2 rounded-sm hover:bg-gray-100 dark:hover:bg-gray-600"
                >
                  {{ ay.ad }}
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div class="relative">
        <button
          class="flex justify-center items-center text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-md text-sm px-4 py-1.5 text-center me-2 mb-2 group dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
          @click="
            () => {
              isIslemlerMenuOpen = !isIslemlerMenuOpen;
            }
          "
        >
          İşlemler
          <svg class="size-7 fill-none" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path
              d="M7 10L12 15L17 10"
              class="stroke-blue-700 group-hover:stroke-white"
              stroke-width="1.5"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
          </svg>
        </button>
        <div
          v-if="isIslemlerMenuOpen"
          class="absolute bg-neutral-50 dark:bg-neutral-800 z-20 rounded-md border border-blue-600"
        >
          <button
            class="font-semibold text-sm hover:bg-blue-600 hover:text-neutral-100 px-3 py-1 rounded-md"
            @click="getPusulaPdfler()"
          >
            PDF olarak indir
          </button>
        </div>
      </div>
    </div>
    <div class="relative overflow-x-auto mt-5">
      <div class="w-full flex flex-col bg-neutral-50 dark:bg-neutral-900">
        <div class="flex w-full">
          <div class="flex w-4/12">
            <span
              class="h-fit bg-gray-100 text-gray-800 text-xs font-medium me-2 px-2.5 py-0.5 rounded-sm dark:bg-gray-700 dark:text-gray-300"
              >Toplam : {{ bordroCount }}</span
            >
          </div>
          <div class="flex w-8/12 justify-between items-center">
            <div
              v-for="groupHeader in tableGroupHeaders"
              :key="groupHeader.header"
              class="flex-1 pt-2 rounded-tr-md border-t border-l cursor-pointer"
              :class="[
                selectedTableGroup.header === groupHeader.header
                  ? 'rounded-tr-md'
                  : 'rounded-t-md ',
                groupHeader.borderColor,
                groupHeader.bgColor,
              ]"
              @click="selectedTableGroup = groupHeader"
            >
              <span class="px-2 font-semibold truncate">{{ groupHeader.header }}</span>
              <div
                class="mt-2 border-b-4"
                :class="
                  selectedTableGroup.header !== groupHeader.header
                    ? selectedTableGroup.borderColor
                    : ' border-transparent'
                "
              ></div>
            </div>
          </div>
        </div>
        <div class="flex border-b border-neutral-400 w-full dark:border-neutral-600">
          <div
            class="flex justify-between w-4/12 px-4 py-5 rounded-tl-md bg-neutral-200 dark:bg-neutral-800 text-sm font-semibold"
          >
            <div class="flex-1 flex items-center justify-start h-full">
              <input
                type="checkbox"
                class="w-4 h-4"
                v-model="selectAllChecked"
                @change="selectAll"
              />
            </div>
            <div class="flex-8 flex justify-start">Adı</div>
            <div class="flex-1 flex justify-end">Durum</div>
          </div>
          <div
            class="flex justify-between w-8/12 border-l border-neutral-500 dark:border-neutral-600"
          >
            <div
              v-for="item in selectedTableGroup.items"
              :key="item"
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex flex-col justify-center items-start px-5 text-sm font-semibold"
              :class="selectedTableGroup.bgColor"
            >
              {{ item }}
            </div>
          </div>
        </div>
        <!-- Maas Pusulalar -->
        <div
          v-for="maaPusula in bordro"
          :key="maaPusula.id"
          class="flex border-b border-neutral-400 dark:border-neutral-600 hover:bg-neutral-200 dark:hover:bg-neutral-800"
        >
          <div class="flex justify-between w-4/12 pl-4 py-2 rounded-tl-md text-base font-semibold">
            <div class="flex-1 flex items-center justify-center h-full">
              <input
                type="checkbox"
                class="w-4 h-4"
                :checked="pusulaIdler.includes(maaPusula.personelId)"
                @change="toggleSinglePusula(maaPusula.personelId)"
              />
            </div>

            <div
              class="flex-10 flex items-center text-sm mx-2 truncate"
              @click="getMaasPusulaPdf(maaPusula.personelId, maaPusula.yil, maaPusula.ay)"
            >
              <img
                v-if="maaPusula.avatarUrl"
                class="size-8 rounded-md object-cover mr-2"
                width="100"
                height="100"
                :src="apiUrl + maaPusula.avatarUrl"
                alt=""
              />
              <div
                v-else
                class="text-xl font-semibold text-sky-600 transition-all duration-300 ease-in-out mr-2 rounded-md border-1 border-sky-500 size-8 flex items-center justify-center"
              >
                {{ maaPusula.fullName[0] }}
              </div>
              <span>{{ maaPusula.fullName }}</span>
            </div>
            <div class="flex-1 flex items-center">
              <span
                class="text-xs font-medium me-2 px-2.5 py-0.5 rounded-sm"
                :class="statusColors[maaPusula.durum]"
                >{{ maaPusula.durum }}</span
              >
            </div>
          </div>
          <!-- Girdiler start -->
          <div
            v-if="selectedTableGroup.header === 'Girdiler'"
            class="flex justify-between w-8/12 border-l border-neutral-500 dark:border-neutral-600"
          >
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.brutUcret.toFixed(2) }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-end text-sm font-semibold"
            >
              {{ maaPusula.sgkGun }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-end text-sm font-semibold group hover:bg-neutral-800/20 dark:hover:bg-neutral-100/30 cursor-pointer"
              @click="
                () => {
                  selectedPusulaId = maaPusula.id;
                  isKazancEkleModalOpen = true;
                }
              "
            >
              {{ maaPusula.ekOdemelerToplam }}

              <svg
                class="size-5 ml-3"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M22 10.5V12C22 16.714 22 19.0711 20.5355 20.5355C19.0711 22 16.714 22 12 22C7.28595 22 4.92893 22 3.46447 20.5355C2 19.0711 2 16.714 2 12C2 7.28595 2 4.92893 3.46447 3.46447C4.92893 2 7.28595 2 12 2H13.5"
                  class="stroke-neutral-600 dark:stroke-neutral-200"
                  stroke-width="1.5"
                  stroke-linecap="round"
                />
                <path
                  d="M16.652 3.45506L17.3009 2.80624C18.3759 1.73125 20.1188 1.73125 21.1938 2.80624C22.2687 3.88124 22.2687 5.62415 21.1938 6.69914L20.5449 7.34795M16.652 3.45506C16.652 3.45506 16.7331 4.83379 17.9497 6.05032C19.1662 7.26685 20.5449 7.34795 20.5449 7.34795M16.652 3.45506L10.6872 9.41993C10.2832 9.82394 10.0812 10.0259 9.90743 10.2487C9.70249 10.5114 9.52679 10.7957 9.38344 11.0965C9.26191 11.3515 9.17157 11.6225 8.99089 12.1646L8.41242 13.9M20.5449 7.34795L14.5801 13.3128C14.1761 13.7168 13.9741 13.9188 13.7513 14.0926C13.4886 14.2975 13.2043 14.4732 12.9035 14.6166C12.6485 14.7381 12.3775 14.8284 11.8354 15.0091L10.1 15.5876M10.1 15.5876L8.97709 15.9619C8.71035 16.0508 8.41626 15.9814 8.21744 15.7826C8.01862 15.5837 7.9492 15.2897 8.03811 15.0229L8.41242 13.9M10.1 15.5876L8.41242 13.9"
                  class="stroke-neutral-600 dark:stroke-neutral-200"
                  stroke-width="1.5"
                />
              </svg>
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-end text-sm font-semibold group hover:bg-neutral-800/20 dark:hover:bg-neutral-100/30 cursor-pointer"
              @click="
                () => {
                  selectedPusulaId = maaPusula.id;
                  isKesintiEkleModalOpen = true;
                }
              "
            >
              {{ maaPusula.ozelKesintiler }}
              <svg
                class="size-5 ml-3"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M22 10.5V12C22 16.714 22 19.0711 20.5355 20.5355C19.0711 22 16.714 22 12 22C7.28595 22 4.92893 22 3.46447 20.5355C2 19.0711 2 16.714 2 12C2 7.28595 2 4.92893 3.46447 3.46447C4.92893 2 7.28595 2 12 2H13.5"
                  class="stroke-neutral-600 dark:stroke-neutral-200"
                  stroke-width="1.5"
                  stroke-linecap="round"
                />
                <path
                  d="M16.652 3.45506L17.3009 2.80624C18.3759 1.73125 20.1188 1.73125 21.1938 2.80624C22.2687 3.88124 22.2687 5.62415 21.1938 6.69914L20.5449 7.34795M16.652 3.45506C16.652 3.45506 16.7331 4.83379 17.9497 6.05032C19.1662 7.26685 20.5449 7.34795 20.5449 7.34795M16.652 3.45506L10.6872 9.41993C10.2832 9.82394 10.0812 10.0259 9.90743 10.2487C9.70249 10.5114 9.52679 10.7957 9.38344 11.0965C9.26191 11.3515 9.17157 11.6225 8.99089 12.1646L8.41242 13.9M20.5449 7.34795L14.5801 13.3128C14.1761 13.7168 13.9741 13.9188 13.7513 14.0926C13.4886 14.2975 13.2043 14.4732 12.9035 14.6166C12.6485 14.7381 12.3775 14.8284 11.8354 15.0091L10.1 15.5876M10.1 15.5876L8.97709 15.9619C8.71035 16.0508 8.41626 15.9814 8.21744 15.7826C8.01862 15.5837 7.9492 15.2897 8.03811 15.0229L8.41242 13.9M10.1 15.5876L8.41242 13.9"
                  class="stroke-neutral-600 dark:stroke-neutral-200"
                  stroke-width="1.5"
                />
              </svg>
            </div>
          </div>
          <!-- Girdiler End -->
          <!-- Kazançlar start -->
          <div
            v-if="selectedTableGroup.header === 'Kazançlar'"
            class="flex justify-between w-8/12 border-l border-neutral-500 dark:border-neutral-600"
          >
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.gunlukUcret.toFixed(2) }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.odemeyeEsasGunSayisi }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.brutUcret }}
            </div>
          </div>
          <!-- Kazançlar end -->
          <!-- SGK start -->
          <div
            v-if="selectedTableGroup.header === 'SGK'"
            class="flex justify-between w-8/12 border-l border-neutral-500 dark:border-neutral-600"
          >
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.sgkGun }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.fiiliCalisma }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ucretliIzin }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.raporlu }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ucretsizIzin }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.digerEksikGun }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ekOdemeIstisnaToplam }}
            </div>
          </div>
          <!-- SGK End -->
          <!-- Gelir Vergisi start -->
          <div
            v-if="selectedTableGroup.header === 'Gelir Vergisi'"
            class="flex justify-between w-8/12 border-l border-neutral-500 dark:border-neutral-600"
          >
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.kumulatifToplam }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ekOdemeIstisnaToplam }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.gvAylikMatrah }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.gvOdemesi }}
            </div>
          </div>
          <!-- Gelir Vergisi end -->
          <!-- Damga Vergisi start -->
          <div
            v-if="selectedTableGroup.header === 'Damga Vergisi'"
            class="flex justify-between w-8/12 border-l border-neutral-500 dark:border-neutral-600"
          >
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ekOdemeIstisna }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.dvAylikMatrah }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.dvOdemesi }}
            </div>
          </div>
          <!-- Damga Vergisi end -->
          <!-- Kesintiler start -->
          <div
            v-if="selectedTableGroup.header === 'Kesintiler'"
            class="flex justify-between w-8/12 border-l border-neutral-500 dark:border-neutral-600"
          >
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.yasalKesintiler }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ozelKesintiler }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              0
              <!--   Ayni yardım -->
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.tumKesintiler }}
            </div>
          </div>
          <!-- Kesintiler end -->
          <!-- Maliyet start -->
          <div
            v-if="selectedTableGroup.header === 'Maliyet'"
            class="flex justify-between w-8/12 border-l border-neutral-500 dark:border-neutral-600"
          >
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.eleGecenUcret }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.tesvikler }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 dark:border-neutral-600 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.tesvikliMaliyet }}
            </div>
          </div>
          <!-- Maliyet end -->
        </div>
      </div>
    </div>
  </div>
  <!-- Modal start -->
  <div
    v-if="isKazancEkleModalOpen"
    id="hs-focus-management-modal"
    class="size-full fixed top-0 start-0 z-50 overflow-x-hidden overflow-y-auto flex justify-center items-center backdrop-blur-xl"
  >
    <div
      class="-open:mt-7 -open:opacity-100 -open:duration-500 mt-0 ease-out transition-all sm:max-w-lg sm:w-full m-3 sm:mx-auto"
    >
      <div
        class="flex flex-col bg-white border border-gray-200 shadow-2xs rounded-xl pointer-events-auto dark:bg-neutral-800 dark:border-neutral-700 dark:shadow-neutral-700/70"
      >
        <div
          class="flex justify-between items-center py-3 px-4 border-b border-gray-200 dark:border-neutral-700"
        >
          <h3 id="hs-focus-management-modal-label" class="font-bold text-gray-800 dark:text-white">
            Kazanç Ekle
          </h3>
          <button
            type="button"
            class="size-8 inline-flex justify-center items-center gap-x-2 rounded-full border border-transparent bg-gray-100 text-gray-800 hover:bg-gray-200 focus:outline-hidden focus:bg-gray-200 disabled:opacity-50 disabled:pointer-events-none dark:bg-neutral-700 dark:hover:bg-neutral-600 dark:text-neutral-400 dark:focus:bg-neutral-600 cursor-pointer"
            @click="isKazancEkleModalOpen = false"
          >
            <span class="sr-only">Close</span>
            <svg
              class="shrink-0 size-4"
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <path d="M18 6 6 18"></path>
              <path d="m6 6 12 12"></path>
            </svg>
          </button>
        </div>
        <div class="p-4 overflow-y-auto">
          <label for="ad" class="block text-sm font-medium mb-2 dark:text-white">Kazanç Türü</label>
          <input
            type="text"
            id="ad"
            v-model="kazancEkleRequest.kazancTuru"
            class="py-3 px-4 block w-full border border-neutral-400 rounded-lg text-sm focus:outline-none focus:border-blue-600 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-200"
            placeholder="Prim"
          />
        </div>
        <div class="p-4 overflow-y-auto">
          <label for="kod" class="block text-sm font-medium mb-2 dark:text-white">Tutar</label>
          <input
            type="number"
            id="kod"
            step="any"
            v-model="kazancEkleRequest.tutar"
            class="py-3 px-4 block w-full border border-neutral-400 rounded-lg text-sm focus:outline-none focus:border-blue-600 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-200"
            placeholder="1000"
          />
        </div>

        <div class="p-4 overflow-y-auto">
          <label for="aciklama" class="block text-sm font-medium mb-2 dark:text-white"
            >Aciklama<span class="dark:text-neutral-400 text-neutral-500">(Opsiyonel)</span>
          </label>
          <input
            type="text"
            id="aciklama"
            v-model="kazancEkleRequest.aciklama"
            class="py-3 px-4 block w-full border border-neutral-400 rounded-lg text-sm focus:outline-none focus:border-blue-600 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-200"
            placeholder=""
          />
        </div>
        <div
          class="flex justify-end items-center gap-x-2 py-3 px-4 border-t border-gray-200 dark:border-neutral-700"
        >
          <button
            type="button"
            class="py-2 px-3 inline-flex items-center gap-x-2 text-sm font-medium rounded-lg border border-gray-200 bg-white text-gray-800 shadow-2xs hover:bg-gray-50 focus:outline-hidden focus:bg-gray-50 disabled:opacity-50 disabled:pointer-events-none dark:bg-neutral-800 dark:border-neutral-700 dark:text-white dark:hover:bg-neutral-700 dark:focus:bg-neutral-700"
            @click="isKazancEkleModalOpen = false"
          >
            İptal
          </button>
          <button
            type="button"
            class="py-2 px-3 inline-flex items-center gap-x-2 text-sm font-medium rounded-lg border border-transparent bg-blue-600 text-white hover:bg-blue-700 focus:outline-hidden focus:bg-blue-700 disabled:opacity-50 disabled:pointer-events-none"
            @click="kazancEkle()"
          >
            Kaydet
          </button>
        </div>
      </div>
    </div>
  </div>
  <!-- Modal end -->
  <!-- Modal start -->
  <div
    v-if="isKesintiEkleModalOpen"
    id="hs-focus-management-modal"
    class="size-full fixed top-0 start-0 z-50 overflow-x-hidden overflow-y-auto flex justify-center items-center backdrop-blur-xl"
  >
    <div
      class="-open:mt-7 -open:opacity-100 -open:duration-500 mt-0 ease-out transition-all sm:max-w-lg sm:w-full m-3 sm:mx-auto"
    >
      <div
        class="flex flex-col bg-white border border-gray-200 shadow-2xs rounded-xl pointer-events-auto dark:bg-neutral-800 dark:border-neutral-700 dark:shadow-neutral-700/70"
      >
        <div
          class="flex justify-between items-center py-3 px-4 border-b border-gray-200 dark:border-neutral-700"
        >
          <h3 id="hs-focus-management-modal-label" class="font-bold text-gray-800 dark:text-white">
            Kesinti Ekle
          </h3>
          <button
            type="button"
            class="size-8 inline-flex justify-center items-center gap-x-2 rounded-full border border-transparent bg-gray-100 text-gray-800 hover:bg-gray-200 focus:outline-hidden focus:bg-gray-200 disabled:opacity-50 disabled:pointer-events-none dark:bg-neutral-700 dark:hover:bg-neutral-600 dark:text-neutral-400 dark:focus:bg-neutral-600 cursor-pointer"
            @click="isKesintiEkleModalOpen = false"
          >
            <span class="sr-only">Close</span>
            <svg
              class="shrink-0 size-4"
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            >
              <path d="M18 6 6 18"></path>
              <path d="m6 6 12 12"></path>
            </svg>
          </button>
        </div>
        <div class="p-4 overflow-y-auto">
          <label for="ad" class="block text-sm font-medium mb-2 dark:text-white"
            >Kesinti Türü</label
          >
          <input
            type="text"
            id="ad"
            v-model="kesintiEkleRequest.kesintiTuru"
            class="py-3 px-4 block w-full border border-neutral-400 rounded-lg text-sm focus:outline-none focus:border-blue-600 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-200"
            placeholder="Avans"
          />
        </div>
        <div class="p-4 overflow-y-auto">
          <label for="kod" class="block text-sm font-medium mb-2 dark:text-white">Tutar</label>
          <input
            type="number"
            id="kod"
            step="any"
            v-model="kesintiEkleRequest.tutar"
            class="py-3 px-4 block w-full border border-neutral-400 rounded-lg text-sm focus:outline-none focus:border-blue-600 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-200"
            placeholder="1000"
          />
        </div>

        <div class="p-4 overflow-y-auto">
          <label for="aciklama" class="block text-sm font-medium mb-2 dark:text-white"
            >Aciklama<span class="dark:text-neutral-400 text-neutral-500">(Opsiyonel)</span>
          </label>
          <input
            type="text"
            id="aciklama"
            v-model="kesintiEkleRequest.aciklama"
            class="py-3 px-4 block w-full border border-neutral-400 rounded-lg text-sm focus:outline-none focus:border-blue-600 dark:bg-neutral-900 dark:border-neutral-700 dark:placeholder-neutral-500 dark:text-neutral-200"
            placeholder=""
          />
        </div>
        <div
          class="flex justify-end items-center gap-x-2 py-3 px-4 border-t border-gray-200 dark:border-neutral-700"
        >
          <button
            type="button"
            class="py-2 px-3 inline-flex items-center gap-x-2 text-sm font-medium rounded-lg border border-gray-200 bg-white text-gray-800 shadow-2xs hover:bg-gray-50 focus:outline-hidden focus:bg-gray-50 disabled:opacity-50 disabled:pointer-events-none dark:bg-neutral-800 dark:border-neutral-700 dark:text-white dark:hover:bg-neutral-700 dark:focus:bg-neutral-700"
            @click="isKesintiEkleModalOpen = false"
          >
            İptal
          </button>
          <button
            type="button"
            class="py-2 px-3 inline-flex items-center gap-x-2 text-sm font-medium rounded-lg border border-transparent bg-blue-600 text-white hover:bg-blue-700 focus:outline-hidden focus:bg-blue-700 disabled:opacity-50 disabled:pointer-events-none"
            @click="kesintiEkle()"
          >
            Kaydet
          </button>
        </div>
      </div>
    </div>
  </div>
  <!-- Modal end -->
</template>
