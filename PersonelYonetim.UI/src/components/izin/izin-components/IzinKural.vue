<script setup lang="ts">
import { computed, onMounted, ref, type Ref } from "vue";
import TableLayout from "../../TableLayout.vue";
import IzinService from "@/services/IzinService";
import type { IzinKuralModel } from "@/models/entity-models/izin/IzinKuralModel";
import type { PaginationParams } from "@/models/request-models/PaginationParams";

const izinKurallar: Ref<IzinKuralModel[] | undefined> = ref([]);

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
    ({ ad, izinTurler, aciklama, createUserName, createdAt, isActive }) => ({
      ad,
      izinTur: izinTurler.length,
      aciklama,
      createUserName,
      createdAt: new Date(createdAt),
      isActive: isActive ? "Aktif" : "Pasif",
    })
  );
});
</script>

<template>
  <div class="space-y-6">
    <div class="mx-10">
      <TableLayout
        :table-headers="[
          'Ad',
          'Izin Türleri',
          'Açıklama',
          'Oluşturan',
          'Oluşturma Tarihi',
          'Durum',
        ]"
        :table-content="filteredIzinKurallar"
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
