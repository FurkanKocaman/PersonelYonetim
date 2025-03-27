<script setup lang="ts">
import { useRouter } from "vue-router";

const router = useRouter();

const goBack = () => {
  router.back();
};
</script>

<template>
  <div class="flex justify-center items-center h-full bg-neutral-50 dark:bg-neutral-800">
    <div
      class="bg-neutral-200 dark:bg-neutral-700 p-6 rounded-lg shadow-lg w-full sm:w-96 text-center"
    >
      <h1 class="text-2xl font-bold text-red-600 mb-4">Yetkisiz Giriş</h1>
      <p class="text-gray-600 dark:text-neutral-300 mb-6">
        Bu sayfaya erişim izniniz bulunmamaktadır.
      </p>
      <button
        @click="goBack"
        class="px-6 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition"
      >
        Geri Dön
      </button>
    </div>
  </div>
</template>

<style scoped>
/* Webkit Autofill styling */
input:-webkit-autofill,
input:-webkit-autofill:hover,
input:-webkit-autofill:focus,
input:-webkit-autofill:active {
  -webkit-box-shadow: 0 0 0px 1000px white inset !important;
  box-shadow: 0 0 0px 1000px white inset !important;
  background-color: red !important;
  color: black !important;
}
</style>
