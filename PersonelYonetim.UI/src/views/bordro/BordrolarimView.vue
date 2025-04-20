<script setup lang="ts">
import type { BordroGetByPersonelModel } from "@/models/response-models/BordroGetByPersonelModel";
import BordroService from "@/services/BordroService";
import { onMounted, ref, watch, type Ref } from "vue";

const isYilMenuOpen = ref(false);

const bordro: Ref<BordroGetByPersonelModel[] | undefined> = ref([]);
const bordroCount = ref(0);

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
const yillar: { id: number; yil: number }[] = [];
const suankiYil = new Date().getFullYear();

for (let i = 0; i <= 10; i++) {
  yillar.push({ id: i, yil: suankiYil - i });
}

onMounted(() => {
  getAllBordro();
});

const getAllBordro = async () => {
  const res = await BordroService.bordroGetByPersonel(selectedYil.value);
  bordro.value = res?.items;
  bordroCount.value = res!.count;
};

watch(selectedYil, () => getAllBordro());
</script>

<template>
  <div class="flex flex-col w-full h-full pt-10 px-5">
    <div class="flex">
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
    </div>
    <div
      v-if="bordroCount == 0"
      class="w-full flex flex-col items-center justify-center mt-3 border py-5 rounded-md bg-neutral-300/50 dark:bg-neutral-800/50 shadow-xl"
    >
      <svg class="size-20" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path
          fill-rule="evenodd"
          clip-rule="evenodd"
          d="M12.8984 3.61441C12.5328 2.86669 11.4672 2.86669 11.1016 3.61441L3.30562 19.5608C2.98083 20.2251 3.46451 21 4.204 21H19.796C20.5355 21 21.0192 20.2251 20.6944 19.5608L12.8984 3.61441ZM9.30485 2.73599C10.4015 0.492834 13.5985 0.492825 14.6952 2.73599L22.4912 18.6824C23.4655 20.6754 22.0145 23 19.796 23H4.204C1.98555 23 0.534479 20.6754 1.50885 18.6824L9.30485 2.73599Z"
          class="fill-blue-600/50"
        />
        <path
          d="M11 8.49999C11 7.94771 11.4477 7.49999 12 7.49999C12.5523 7.49999 13 7.94771 13 8.49999V14C13 14.5523 12.5523 15 12 15C11.4477 15 11 14.5523 11 14V8.49999Z"
          class="fill-blue-600/50"
        />
        <path
          d="M13.5 18C13.5 18.8284 12.8285 19.5 12 19.5C11.1716 19.5 10.5 18.8284 10.5 18C10.5 17.1716 11.1716 16.5 12 16.5C12.8285 16.5 13.5 17.1716 13.5 18Z"
          class="fill-blue-600/50"
        />
      </svg>
      <h1 class="py-3 font-semibold text-xl">Belirtilen yıla ait bordro kaydı bulunamadı</h1>
    </div>
    <div v-else class="relative overflow-x-auto mt-5">
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
        <div class="flex border-b border-neutral-400 w-full">
          <div
            class="flex justify-between w-4/12 px-4 py-5 rounded-tl-md bg-neutral-200 dark:bg-neutral-800 text-sm font-semibold"
          >
            <div class="flex-1 flex items-center justify-start h-full">
              <input type="checkbox" class="w-4 h-4" />
            </div>
            <div class="flex-8 flex justify-start">Tarih</div>
            <div class="flex-1 flex justify-end">Durum</div>
          </div>
          <div class="flex justify-between w-8/12 border-l border-neutral-500">
            <div
              v-for="item in selectedTableGroup.items"
              :key="item"
              class="border-r flex-1 border-neutral-300 flex flex-col justify-center items-start px-5 text-sm font-semibold"
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
          class="flex bg-green-100/30 border-b border-neutral-400"
        >
          <div class="flex justify-between w-4/12 pl-4 py-2 rounded-tl-md text-base font-semibold">
            <div class="flex-1 flex items-center justify-center h-full">
              <input type="checkbox" class="w-4 h-4" />
            </div>
            <div class="flex-10 flex items-center text-sm mx-2 truncate">
              <span>{{ maaPusula.yil + "\t" + aylar.find((x) => x.id == maaPusula.ay)!.ad }}</span>
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
            class="flex justify-between w-8/12 border-l border-neutral-500"
          >
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.brutUcret.toFixed(2) }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.sgkGun }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ekOdemelerToplam }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ozelKesintiler }}
            </div>
          </div>
          <!-- Girdiler End -->
          <!-- Kazançlar start -->
          <div
            v-if="selectedTableGroup.header === 'Kazançlar'"
            class="flex justify-between w-8/12 border-l border-neutral-500"
          >
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.gunlukUcret.toFixed(2) }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.odemeyeEsasGunSayisi }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.brutUcret.toFixed(2) }}
            </div>
          </div>
          <!-- Kazançlar end -->
          <!-- SGK start -->
          <div
            v-if="selectedTableGroup.header === 'SGK'"
            class="flex justify-between w-8/12 border-l border-neutral-500"
          >
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.sgkGun }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.fiiliCalisma }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ucretliIzin }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.raporlu }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ucretsizIzin }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.digerEksikGun }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ekOdemeIstisnaToplam }}
            </div>
          </div>
          <!-- SGK End -->
          <!-- Gelir Vergisi start -->
          <div
            v-if="selectedTableGroup.header === 'Gelir Vergisi'"
            class="flex justify-between w-8/12 border-l border-neutral-500"
          >
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.kumulatifToplam }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ekOdemeIstisna }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.gvAylikMatrah }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.gvOdemesi }}
            </div>
          </div>
          <!-- Gelir Vergisi end -->
          <!-- Damga Vergisi start -->
          <div
            v-if="selectedTableGroup.header === 'Damga Vergisi'"
            class="flex justify-between w-8/12 border-l border-neutral-500"
          >
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ekOdemeIstisnaToplam }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.dvAylikMatrah }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.dvOdemesi }}
            </div>
          </div>
          <!-- Damga Vergisi end -->
          <!-- Kesintiler start -->
          <div
            v-if="selectedTableGroup.header === 'Kesintiler'"
            class="flex justify-between w-8/12 border-l border-neutral-500"
          >
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.yasalKesintiler }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.ozelKesintiler }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              0
              <!--   Ayni yardım -->
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.tumKesintiler }}
            </div>
          </div>
          <!-- Kesintiler end -->
          <!-- Maliyet start -->
          <div
            v-if="selectedTableGroup.header === 'Maliyet'"
            class="flex justify-between w-8/12 border-l border-neutral-500"
          >
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.eleGecenUcret }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.tesvikler }}
            </div>
            <div
              class="border-r flex-1 border-neutral-300 flex items-center px-5 justify-start text-sm font-semibold"
            >
              {{ maaPusula.tesvikliMaliyet }}
            </div>
          </div>
          <!-- Maliyet end -->
        </div>
      </div>
    </div>
  </div>
</template>
