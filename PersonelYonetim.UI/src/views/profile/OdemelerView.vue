<script setup lang="ts">
import TableLayout from "@/components/TableLayout.vue";
import { ref } from "vue";

const activeTab4 = ref("harcama");

const setActiveTab4 = (tab: string) => {
  activeTab4.value = tab;
};

// fazla mesai kısmı
const data = ref([
  {
    date: "2024-01-31",
    description: "OCAK AYI MESAİ ÜCRETİ",
    status: "Onaylandı",
    amount: 738.89,
    created_at: "2024-02-26T17:14",
    payroll: "Dahil Değil",
    paid: false,
  },
  {
    date: "2023-12-31",
    description: "ARALIK AYI MESAİ ÜCRETİ",
    status: "Onaylandı",
    amount: 4375.0,
    created_at: "2024-02-26T17:13",
    payroll: "Dahil Değil",
    paid: true,
  },
]);
</script>

<template>
  <!-- ödemelerim kısmı -->
  <div class="mx-10 my-5">
    <ul
      class="flex flex-wrap text-sm font-medium text-center border-b border-gray-200 dark:border-gray-700"
    >
      <li class="me-2">
        <button
          @click="setActiveTab4('harcama')"
          :class="
            activeTab4 === 'harcama'
              ? 'bg-sky-600 hover:bg-sky-500 text-neutral-100'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Harcama
        </button>
      </li>
      <li class="me-2">
        <button
          @click="setActiveTab4('fazlaMesai')"
          :class="
            activeTab4 === 'fazlaMesai'
              ? 'bg-sky-600 hover:bg-sky-500 text-neutral-100'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Fazla Mesai
        </button>
      </li>
    </ul>

    <div v-if="activeTab4 === 'harcama'" class="space-y-6 mt-10">
      <div class="flex justify-center items-center w-full">
        <div
          class="border-2 border-neutral-200 dark:border-neutral-500 p-6 bg-transparent rounded-lg w-full"
        >
          <div class="text-center my-5">
            <i class="fa-solid fa-wallet fa-2xl" style="color: #3562b1"></i>

            <br />
            <br />
            <p class="text-gray-800 dark:text-neutral-300 text-l mb-4">Kayıtlı ödeme bulunamad</p>
          </div>
        </div>
      </div>
    </div>

    <!-- fazla mesai -->
    <div v-if="activeTab4 === 'fazlaMesai'" class="space-y-6 mt-10">
      <TableLayout
        :table-headers="[
          'Tarih',
          'Açıklama',
          'Durum',
          'Ücret',
          'Oluşturulmu Tarihi',
          'Bordro',
          'Ödendi',
        ]"
        :table-content="data"
        :islemler="['detaylar']"
      />
    </div>
  </div>
</template>
