<script setup lang="ts">
import { useRoute } from "vue-router";

const route = useRoute();
</script>

<template>
  <div class="flex flex-col h-full">
    <div class="ml-5">
      <ul class="flex flex-wrap -mb-px">
        <li class="me-2">
          <RouterLink
            to="sirket-yapisi"
            class="inline-block p-4 border-b-2 rounded-t-lg"
            :class="
              route.fullPath.includes('sirket-yapisi')
                ? 'text-blue-600 border-blue-600 dark:text-blue-500 dark:border-blue-500 active'
                : 'hover:text-gray-600 hover:border-gray-300 dark:hover:text-gray-300 border-transparent'
            "
            >Şirket Yapısı</RouterLink
          >
        </li>
        <li class="me-2">
          <RouterLink
            to="sirket-ayarlari"
            class="inline-block p-4 border-b-2 rounded-t-lg"
            :class="
              route.fullPath.includes('sirket-ayarlari')
                ? 'text-blue-600 border-blue-600 dark:text-blue-500 dark:border-blue-500 active'
                : 'hover:text-gray-600 hover:border-gray-300 dark:hover:text-gray-300 border-transparent'
            "
            aria-current="page"
            >Şirket Ayarları</RouterLink
          >
        </li>
        <li class="me-2">
          <RouterLink
            to="pozisyonlar"
            class="inline-block p-4 border-b-2 rounded-t-lg"
            :class="
              route.fullPath.includes('pozisyonlar')
                ? 'text-blue-600 border-blue-600 dark:text-blue-500 dark:border-blue-500 active'
                : 'hover:text-gray-600 hover:border-gray-300 dark:hover:text-gray-300 border-transparent'
            "
            >Pozisyonlar</RouterLink
          >
        </li>
        <li class="me-2">
          <RouterLink
            to="rol-ayarlari"
            class="inline-block p-4 border-b-2 rounded-t-lg"
            :class="
              route.fullPath.includes('rol-ayarlari')
                ? 'text-blue-600 border-blue-600 dark:text-blue-500 dark:border-blue-500 active'
                : 'hover:text-gray-600 hover:border-gray-300 dark:hover:text-gray-300 border-transparent'
            "
            >Rol Ayarları</RouterLink
          >
        </li>
      </ul>
    </div>
    <RouterView></RouterView>
  </div>
</template>

<style scoped></style>
