<script setup lang="ts">
import IzinDetaylarComponent from "@/components/izinler/IzinDetaylarComponent.vue";
import TableLayout from "@/components/TableLayout.vue";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { IzinTalepGetResponse } from "@/models/response-models/izinler/IzinTalepGetResponse";
import type { IzinlerKalanResponse } from "@/models/response-models/izinler/İzinlerKalanResponse";
import IzinService from "@/services/IzinService";
import { computed, onMounted, ref, type Ref } from "vue";

const izinList = ref<IzinTalepGetResponse[]>([]);
const showDetailModal = ref(false);
const selectedIzin = ref<IzinTalepGetResponse | null>(null);

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

const kalanIzin: Ref<IzinlerKalanResponse> = ref({
  izinTurAd: "",
  kullanilan: 0,
  kalan: 0,
  guncelHakEdisDonem: "",
});

onMounted(() => {
  personelGetIzinTalepler();
  getKalanIzin();
});

const viewIzinDetails = (izin: IzinTalepGetResponse) => {
  selectedIzin.value = izin;
  showDetailModal.value = true;
};

const closeDetailModal = () => {
  showDetailModal.value = false;
  selectedIzin.value = null;
};

const personelGetIzinTalepler = async () => {
  const res = await IzinService.getPersonelIzinTalepler(paginationParams.value);
  izinList.value = res!.items;
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

const getKalanIzin = async () => {
  const res = await IzinService.getIzinlerKalan();
  kalanIzin.value = res!;
};

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
</script>

<template>
  <div class="space-y-6 w-full flex flex-col itmes-center justify-center p-3">
    <div class="p-4 rounded-lg shadow-md bg-gray bg-neutral-100 dark:bg-neutral-800">
      <!-- İzin Bakiyesi Bilgisi -->
      <div class="mb-4 flex flex-col">
        <div class="w-full justify-start">
          <h2 class="text-md font-semibold" style="font-size: 15px">
            Kullanılabilir İzin Bakiyesi / Yıllık İzin
          </h2>
        </div>
        <div class="flex justify-end">
          <p class="text-sm">
            Güncel Hak Ediş Dönemi: <strong>{{ kalanIzin.guncelHakEdisDonem }}</strong>
          </p>
        </div>
        <div class="flex justify-start">
          <p class="text-xl text-green-600" style="font-size: 30px">{{ kalanIzin.kalan }} gün</p>
        </div>
      </div>

      <!-- İzin Progress Bar -->
      <div class="relative w-full h-2 bg-gray-200 rounded-full overflow-hidden">
        <div
          class="absolute h-full bg-green-600"
          :style="{
            width: (kalanIzin.kalan / (kalanIzin.kalan + kalanIzin.kullanilan)) * 100 + '%',
          }"
        ></div>
        <div
          class="absolute h-full bg-red-500"
          :style="{
            width: (kalanIzin.kullanilan / (kalanIzin.kalan + kalanIzin.kullanilan)) * 100 + '%',
            left: (kalanIzin.kalan / (kalanIzin.kalan + kalanIzin.kullanilan)) * 100 + '%',
          }"
        ></div>
      </div>

      <div class="flex justify-end text-sm mt-2">
        <div class="flex items-center">
          <span class="w-3 h-3 bg-red-500 inline-block rounded-full mr-1"></span>
          Dönemde Kullanılan {{ kalanIzin.kullanilan }}
        </div>
      </div>
    </div>

    <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
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
        @detail-click="viewIzinDetails"
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
  <IzinDetaylarComponent
    v-if="showDetailModal"
    :selected-izin="selectedIzin"
    @closeDetailModal="closeDetailModal"
  />
</template>
