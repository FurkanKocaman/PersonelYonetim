<script setup lang="ts">
import TableLayout from "@/components/TableLayout.vue";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { IzinTalepGetResponse } from "@/models/response-models/izinler/IzinTalepGetResponse";
import IzinService from "@/services/IzinService";
import { computed, onMounted, ref, type Ref } from "vue";

const izinBakiyesi = ref(42.51);
const hakEdisBaslangic = ref("2 Ağu 2024");
const hakEdisBitis = ref("1 Ağu 2025");
const kullanilanIzin = ref(2.61);
const ileriTarihli = ref(0);

const toplamIzin = computed(() => izinBakiyesi.value + kullanilanIzin.value);
const kullanilanOran = computed(() => (kullanilanIzin.value / toplamIzin.value) * 100);
const kalanOran = computed(() => (izinBakiyesi.value / toplamIzin.value) * 100);

const izinList = ref<IzinTalepGetResponse[]>([]);
const selectedIzin = ref<IzinTalepGetResponse | undefined>(undefined);

const loading = ref(true);
const error = ref(false);

const showDetailModal = ref(false);

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

onMounted(() => {
  personelGetIzinTalepler();
});

const personelGetIzinTalepler = async () => {
  const res = await IzinService.getPersonelIzinTalepler(paginationParams.value);
  izinList.value = res!.items;
  console.log(res);
  paginationParams.value.count = res!.count;
};

const setPageNumber = (pageNumber: number) => {
  paginationParams.value.pageNumber = pageNumber;
  personelGetIzinTalepler();
};

const filterOptions = ref({
  durum: "",
  izinTipi: "",
});

const filteredIzinList = computed(() => {
  return izinList.value.filter((izin) => {
    const durumMatch =
      !filterOptions.value.durum || izin.degerlendirmeDurumu === filterOptions.value.durum;
    const izinTipiMatch =
      !filterOptions.value.izinTipi || izin.izinTuru === filterOptions.value.izinTipi;
    return durumMatch && izinTipiMatch;
  });
});

const filteredIzinTalepler = computed<Record<string, unknown>[]>(() => {
  return (filteredIzinList.value || []).map(
    ({
      id,
      personelFullName,
      baslangicTarihi,
      bitisTarihi,
      mesaiBaslangicTarihi,
      toplamSure,
      izinTuru,
      degerlendirmeDurumu,
    }) => ({
      id,
      personelFullName,
      baslangicTarihi: new Date(baslangicTarihi),
      bitisTarihi: new Date(bitisTarihi),
      mesaiBaslangicTarihi: new Date(mesaiBaslangicTarihi),
      toplamSure,
      izinTuru,
      degerlendirmeDurumu,
    })
  );
});

const izinler = ref([
  {
    baslangic: "5 Mar 2025 09:00",
    bitis: "5 Mar 2025 12:30",
    mesaiBaslangic: "5 Mar 2025 12:30",
    sure: "0.39 gün",
    izinTuru: "Yıllık İzin",
    aciklama: "5 mart öğleden önce hastane işlerim vardı",
    olusturmaTarihi: "20 Mar 2025 10:51",
    durum: "Onaylandı",
  },
  {
    baslangic: "14 Oca 2025 09:00",
    bitis: "14 Oca 2025 18:00",
    mesaiBaslangic: "15 Oca 2025 09:00",
    sure: "1 gün",
    izinTuru: "Yıllık İzin",
    aciklama: "Gribal rahatsızlığımdan dolayı izin talebimdir",
    olusturmaTarihi: "14 Oca 2025 12:51",
    durum: "Onaylandı",
  },
  {
    baslangic: "27 Kas 2024 14:00",
    bitis: "27 Kas 2024 16:30",
    mesaiBaslangic: "27 Kas 2024 16:30",
    sure: "0.28 gün",
    izinTuru: "Yıllık İzin",
    aciklama: "14.30 Diş randevumdan dolayı izin talebimdir",
    olusturmaTarihi: "26 Kas 2024 18:12",
    durum: "Onaylandı",
  },
  {
    baslangic: "27 Kas 2024 14:00",
    bitis: "27 Kas 2024 16:30",
    mesaiBaslangic: "27 Kas 2024 16:30",
    sure: "0.28 gün",
    izinTuru: "Yıllık İzin",
    aciklama: "14.30 Diş randevumdan dolayı izin talebimdir",
    olusturmaTarihi: "26 Kas 2024 18:12",
    durum: "Onaylandı",
  },
  {
    baslangic: "27 Kas 2024 14:00",
    bitis: "27 Kas 2024 16:30",
    mesaiBaslangic: "27 Kas 2024 16:30",
    sure: "0.28 gün",
    izinTuru: "Yıllık İzin",
    aciklama: "14.30 Diş randevumdan dolayı izin talebimdir",
    olusturmaTarihi: "26 Kas 2024 18:12",
    durum: "Onaylandı",
  },
]);
</script>

<template>
  <div class="space-y-6 w-full flex flex-col itmes-center justify-center">
    <div class="p-4 rounded-lg shadow-md bg-gray mx-5 bg-neutral-100 dark:bg-neutral-800">
      <!-- İzin Bakiyesi Bilgisi -->
      <div class="mb-4 flex flex-col">
        <div class="w-full justify-start">
          <h2 class="text-md font-semibold" style="font-size: 15px">
            Kullanılabilir İzin Bakiyesi / Yıllık İzin
          </h2>
        </div>
        <div class="flex justify-end">
          <p class="text-sm">
            Güncel Hak Ediş Dönemi: <strong>{{ hakEdisBaslangic }} – {{ hakEdisBitis }}</strong>
          </p>
        </div>
        <div class="flex justify-start">
          <p class="text-xl text-green-600" style="font-size: 30px">{{ izinBakiyesi }} gün</p>
        </div>
      </div>

      <!-- İzin Progress Bar -->
      <div class="relative w-full h-2 bg-gray-200 rounded-full overflow-hidden">
        <div class="absolute h-full bg-green-600" :style="{ width: kalanOran + '%' }"></div>
        <div
          class="absolute h-full bg-red-500"
          :style="{ width: kullanilanOran + '%', left: kalanOran + '%' }"
        ></div>
      </div>

      <div class="flex justify-between text-sm mt-2">
        <div class="flex items-center">
          <span class="w-3 h-3 bg-yellow-400 inline-block rounded-full mr-1"></span>
          Dönemde İleri Tarihli {{ ileriTarihli }}
        </div>
        <div class="flex items-center">
          <span class="w-3 h-3 bg-red-500 inline-block rounded-full mr-1"></span>
          Dönemde Kullanılan {{ kullanilanIzin }}
        </div>
      </div>
    </div>

    <div class="overflow-x-auto w-full flex flex-col items-center justify-center px-5">
      <TableLayout
        :table-headers="[
          { key: 'personelFullName', value: 'Personel Adı' },
          { key: 'baslangicTarihi', value: 'Başlangıç' },
          { key: 'bitisTarihi', value: 'Bitiş' },
          { key: 'mesaiBaslangicTarihi', value: 'Mesai Başlangıç' },
          { key: 'toplamSure', value: 'Toplam Süre' },
          { key: 'izinTuru', value: 'İzin Tipi' },
          { key: 'degerlendirmeDurumu', value: 'Durum' },
        ]"
        :table-content="filteredIzinTalepler"
        :islemler="['detaylar']"
        :page-count="
          Math.ceil(paginationParams.count / paginationParams.pageSize) == 0
            ? 1
            : Math.ceil(paginationParams.count / paginationParams.pageSize)
        "
        :count="paginationParams.count"
        :page-size="
          paginationParams.pageSize > paginationParams.count
            ? paginationParams.count
            : paginationParams.pageSize
        "
        :current-page="paginationParams.pageNumber"
        @set-page="setPageNumber"
      />
    </div>
  </div>
</template>
