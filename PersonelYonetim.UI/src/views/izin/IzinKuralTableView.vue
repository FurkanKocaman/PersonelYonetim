<script setup lang="ts">
import { computed, onMounted, ref, type Ref } from "vue";
import TableLayout from "../../components/TableLayout.vue";
import IzinService from "@/services/IzinService";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { IzinKuralGetResponse } from "@/models/response-models/izinler/IzinKuralGetResponse";

const izinKurallar: Ref<IzinKuralGetResponse[] | undefined> = ref([]);

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

onMounted(async () => {
  getIzinKurallar();
});
const setPageNumber = (pageNumber: number) => {
  paginationParams.value.pageNumber = pageNumber;
  getIzinKurallar();
};

const getIzinKurallar = async () => {
  const response = await IzinService.getIzinKural(paginationParams.value);
  paginationParams.value.count = response!.count;
  izinKurallar.value = response?.items;
};

const filteredIzinKurallar = computed<Record<string, unknown>[]>(() => {
  return (izinKurallar.value || []).map(
    ({ id, ad, izinTurler, aciklama, personelCount, createUserName, createdAt, isActive }) => ({
      id,
      ad,
      izinTur: izinTurler.length,
      aciklama,
      personelCount,
      createUserName,
      createdAt: new Date(createdAt),
      isActive: isActive ? "Aktif" : "Pasif",
    })
  );
});
// const orderBy = (order: string) => {
//   paginationParams.value.orderBy = order;
//   getIzinKurallar();
// };

const kuralDetay = (izinKural: IzinKuralGetResponse) => {
  console.log(izinKural);
};
</script>

<template>
  <div class="space-y-6">
    <div class="mx-2">
      <TableLayout
        :table-headers="[
          { key: 'ad', value: 'Ad' },
          { key: 'izinTur', value: 'Izin Türleri' },
          { key: 'aciklama', value: 'Açıklama' },
          { key: 'personelCount', value: 'Personel' },
          // { key: 'createUserName', value: 'Oluşturan' },
          { key: 'createdAt', value: 'Oluşturma Tarihi' },
          { key: 'isActive', value: 'Durum' },
        ]"
        :table-content="filteredIzinKurallar"
        :islemler="['detaylar']"
        @detail-click="kuralDetay"
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
