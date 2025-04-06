<script setup lang="ts">
import { ref } from "vue";

const activeTab4 = ref("egitimlerim");

const setActiveTab4 = (tab: string) => {
  activeTab4.value = tab;
};
</script>

<template>
  <!-- diğer -->
  <div class="space-y-6 mt-5 mx-10">
    <ul
      class="flex flex-wrap text-sm font-medium text-center border-b border-gray-200 dark:border-gray-700"
    >
      <li class="me-2">
        <button
          @click="setActiveTab4('egitimlerim')"
          :class="
            activeTab4 === 'egitimlerim'
              ? 'bg-sky-600 hover:bg-sky-500 text-neutral-100'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Eğitimlerim
        </button>
      </li>
      <li class="me-2">
        <button
          @click="setActiveTab4('zimmetlerim')"
          :class="
            activeTab4 === 'zimmetlerim'
              ? 'bg-sky-600 hover:bg-sky-500 text-neutral-100'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Zimmetlerim
        </button>
      </li>
      <li class="me-2">
        <button
          @click="setActiveTab4('vizeBelgesiTaleplerim')"
          :class="
            activeTab4 === 'vizeBelgesiTaleplerim'
              ? 'bg-sky-600 hover:bg-sky-500 text-neutral-100'
              : 'text-neutral-700 dark:text-neutral-300'
          "
          class="inline-block px-5 py-2 rounded-t-lg"
        >
          Vize Belgesi Taleplerim
        </button>
      </li>
    </ul>

    <div v-if="activeTab4 === 'egitimlerim'" class="space-y-6">
      <div class="flex justify-center items-center w-full">
        <div
          class="border-2 border-neutral-200 dark:border-neutral-500 p-6 bg-transparent rounded-lg w-full"
        >
          <div class="text-center my-5">
            <i class="fa-regular fa-chart-bar fa-rotate-270 fa-2xl" style="color: #3562b1"></i>

            <br />
            <br />
            <p class="text-gray-800 dark:text-neutral-300 text-l mb-4">
              Kayıtlı eğitim bilgisi bulunamadı
            </p>
          </div>
        </div>
      </div>
    </div>

    <div v-if="activeTab4 === 'vizeBelgesiTaleplerim'" class="space-y-6">
      <div class="flex justify-center items-center w-full">
        <div
          class="border-2 border-neutral-200 dark:border-neutral-500 p-6 bg-transparent rounded-lg w-full"
        >
          <div class="text-center my-5">
            <i class="fa-solid fa-file fa-2xl" style="color: #3562b1"></i>

            <br />
            <br />
            <p class="text-gray-800 dark:text-neutral-300 text-l mb-4">
              Kayıtlı vize belgesi süreci bulunamadı
            </p>
          </div>
        </div>
      </div>
    </div>

    <div v-if="activeTab4 === 'zimmetlerim'" class="space-y-6">
      <div class="flex justify-center items-center w-full">
        <div
          class="border-2 border-neutral-200 dark:border-neutral-500 p-6 bg-transparent rounded-lg w-full"
        >
          <div class="text-center my-5">
            <i class="fa-solid fa-wallet fa-2xl" style="color: #3562b1"></i>

            <br />
            <br />
            <p class="text-gray-800 dark:text-neutral-300 text-l mb-4">Kayıtlı zimmet bulunamadı</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
