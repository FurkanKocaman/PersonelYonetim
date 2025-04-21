<script setup lang="ts">
import { onMounted } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();

onMounted(() => {
  console.log(route.name);
});
</script>

<template>
  <div class="flex-1 transition-all duration-300">
    <div
      class="mb-6 mt-2 border-b border-gray-200 dark:border-gray-700 flex justify-between items-center"
    >
      <ul class="flex flex-wrap -mb-px">
        <li class="mr-2">
          <RouterLink
            :to="{ name: 'Calisanlar' }"
            class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
            :class="
              route.name == 'Calisanlar'
                ? 'text-sky-600 border-sky-600'
                : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
            "
          >
            <i class="fas fa-calendar-alt mr-2"></i> Çalışanlar
          </RouterLink>
        </li>
        <li class="mr-2">
          <Router-link
            to="/dashboard/bordro"
            class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
            :class="
              route.name == ''
                ? 'text-sky-600 border-sky-600'
                : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
            "
          >
            <i class="fas fa-book mr-2"></i> Bordro
          </Router-link>
        </li>
      </ul>
      <div class="flex space-x-2">
        <!-- <button
          v-if="route.path == '/dashboard/izin/izinler'"
          @click="
            () => {
              izinTalepCreate = true;
            }
          "
          class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
        > -->
        <!-- <i class="fas fa-plus mr-2"></i> İzin Talebi Oluştur
        </button> -->
      </div>
    </div>

    <RouterView></RouterView>

    <div v-if="false" class="flex justify-center items-center h-64">
      <div
        class="w-16 h-16 border-4 border-sky-600 border-t-transparent rounded-full animate-spin"
      ></div>
    </div>
  </div>
</template>

<style scoped>
.animate-spin {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
