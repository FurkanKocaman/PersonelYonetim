<script setup lang="ts">
import { onMounted } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();

onMounted(() => {
  console.log(route.path);
});
</script>

<template>
  <div class="border-b border-gray-200 dark:border-gray-700 my-2">
    <ul class="flex flex-wrap">
      <li class="mr-2">
        <RouterLink
          to="/dashboard/profile"
          class="inline-block py-1 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
          :class="
            route.path == '/dashboard/profile'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-user"></i> Profilim
        </RouterLink>
      </li>
      <li class="mr-2">
        <RouterLink
          to="/dashboard/profile/kisisel-bilgiler"
          class="inline-block py-1 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
          :class="
            route.path == '/dashboard/profile/kisisel-bilgiler'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-person"></i> Kişisel Bilgilerim
        </RouterLink>
      </li>
      <li class="mr-2">
        <RouterLink
          to="/dashboard/profile/kariyerim"
          class="inline-block py-1 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
          :class="
            route.path == '/dashboard/profile/kariyerim'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-user-tie"></i> Kariyerim
        </RouterLink>
      </li>
      <li class="mr-2">
        <RouterLink
          to="/dashboard/profile/izinlerim"
          class="inline-block py-1 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
          :class="
            route.path == '/dashboard/profile/izinlerim'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-mug-hot"></i> İzinlerim
        </RouterLink>
      </li>

      <li class="mr-2">
        <RouterLink
          to="/dashboard/profile/odemelerim"
          class="inline-block py-1 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
          :class="
            route.path == '/dashboard/profile/odemelerim'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-money-bill"></i> Ödemelerim
        </RouterLink>
      </li>

      <li class="mr-2">
        <RouterLink
          to="/dashboard/profile/mesailerim"
          class="inline-block py-1 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
          :class="
            route.path == '/dashboard/profile/mesailerim'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-briefcase"></i> Mesailerim
        </RouterLink>
      </li>

      <li class="mr-2">
        <RouterLink
          to="/dashboard/profile/diger"
          class="inline-block py-1 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
          :class="
            route.path == '/dashboard/profile/diger'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-square-poll-horizontal"></i> Diğer
        </RouterLink>
      </li>
    </ul>
  </div>

  <RouterView></RouterView>
</template>
