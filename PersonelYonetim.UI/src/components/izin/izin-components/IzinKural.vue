<script setup lang="ts">
import { computed, onMounted, ref, type Ref } from "vue";
import TableLayout from "../TableLayout.vue";
import IzinService from "@/services/IzinService";
import type { IzinKuralModel } from "@/models/entity-models/izin/IzinKuralModel";

const izinKurallar: Ref<IzinKuralModel[] | undefined> = ref([]);

onMounted(async () => {
  const response = await IzinService.getIzinKurallar();
  izinKurallar.value = response?.IzinKurallar;
});

const filteredIzinKurallar = computed<Record<string, unknown>[]>(() => {
  return (izinKurallar.value || []).map(
    ({ ad, izinTur, aciklama, createUserName, createdAt, isActive }) => ({
      ad,
      izinTur: izinTur.length,
      aciklama,
      createUserName,
      createdAt: new Date(createdAt).toDateString(),
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
      />
    </div>
  </div>
</template>
