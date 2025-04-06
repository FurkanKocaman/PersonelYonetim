<script setup lang="ts">
import IzinTalepCreateModal from "@/components/modals/IzinTalepCreateModal.vue";
import { onMounted, ref, type Ref } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();
const isMenuOpen = ref(false);

const selectedMenu: Ref<{ title: string; icon: string }> = ref({
  title: "",
  icon: "",
});

onMounted(() => {
  switch (route.path) {
    case "/dashboard/profile":
      setSelectedMenu("Profilim", "fa-user");
      break;
    case "/dashboard/profile/kisisel-bilgiler":
      setSelectedMenu("Kişisel Bilgilerim", "fa-person");
      break;
    case "/dashboard/profile/kariyerim":
      setSelectedMenu("Kariyerim", "fa-user-tie");
      break;
    case "/dashboard/profile/izinlerim":
      setSelectedMenu("İzinlerim", "fa-mug-hot");
      break;
    case "/dashboard/profile/odemelerim":
      setSelectedMenu("Ödemelerim", "fa-money-bill");
      break;
    case "/dashboard/profile/mesailerim":
      setSelectedMenu("Mesailerim", "fa-briefcase");
      break;
    case "/dashboard/profile/diger":
      setSelectedMenu("Diğer", "fa-square-poll-horizontal");
      break;
    default:
      setSelectedMenu("Profilim", "fa-user");
      break;
  }
});
const izinTalepCreate = ref(false);
const closeIzinTalep = (res: boolean) => {
  izinTalepCreate.value = res;
};

const setSelectedMenu = (title: string, icon: string) => {
  selectedMenu.value = { title, icon };
};
</script>

<template>
  <div class="border-b border-gray-200 dark:border-gray-700 my-2 flex justify-between">
    <ul class="flex md:flex-row flex-col flex-wrap relative">
      <li class="mr-2 md:hidden block">
        <button
          class="inline-block py-2 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
          @click="
            () => {
              isMenuOpen = !isMenuOpen;
            }
          "
        >
          <i class="fa-solid" :class="selectedMenu.icon"></i> {{ selectedMenu.title }}
        </button>
      </li>
      <ul
        v-if="isMenuOpen"
        class="absolute top-10 left-2 bg-neutral-200 dark:bg-neutral-900 px-3 py-2 rounded-lg w-50 z-10 md:hidden"
      >
        <li class="my-2 py-1">
          <RouterLink to="/dashboard/profile">
            <i class="fa-solid fa-user"></i>
            Profilim
          </RouterLink>
        </li>
        <li class="my-2 py-1">
          <RouterLink to="/dashboard/profile/kisisel-bilgiler">
            <i class="fa-solid fa-person"></i>
            Kişisel Bilgilerim</RouterLink
          >
        </li>
        <li class="my-2 py-1">
          <RouterLink to="/dashboard/profile/kariyerim">
            <i class="fa-solid fa-user-tie"></i>
            Kariyerim</RouterLink
          >
        </li>
        <li class="my-2 py-1">
          <RouterLink to="/dashboard/profile/izinlerim">
            <i class="fa-solid fa-mug-hot"></i>
            İzinlerim</RouterLink
          >
        </li>
        <li class="my-2 py-1">
          <RouterLink to="/dashboard/profile/odemelerim">
            <i class="fa-solid fa-money-bill"></i>
            Ödemelerim</RouterLink
          >
        </li>
        <li class="my-2 py-1">
          <RouterLink to="/dashboard/profile/mesailerim">
            <i class="fa-solid fa-briefcase"></i>
            Mesailerim</RouterLink
          >
        </li>
        <li class="my-2 py-1">
          <RouterLink to="/dashboard/profile/diger">
            <i class="fa-solid fa-square-poll-horizontal"></i>
            Diğer</RouterLink
          >
        </li>
      </ul>
      <li class="mr-2 md:block hidden">
        <RouterLink
          to="/dashboard/profile"
          class="inline-block py-2 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
          :class="
            route.path == '/dashboard/profile'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-user"></i> Profilim
        </RouterLink>
      </li>
      <li class="mr-2 md:block hidden">
        <RouterLink
          to="/dashboard/profile/kisisel-bilgiler"
          class="inline-block py-2 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
          :class="
            route.path == '/dashboard/profile/kisisel-bilgiler'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-person"></i> Kişisel Bilgilerim
        </RouterLink>
      </li>
      <li class="mr-2 md:block hidden">
        <RouterLink
          to="/dashboard/profile/kariyerim"
          class="inline-block py-2 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
          :class="
            route.path == '/dashboard/profile/kariyerim'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-user-tie"></i> Kariyerim
        </RouterLink>
      </li>
      <li class="mr-2 md:block hidden">
        <RouterLink
          to="/dashboard/profile/izinlerim"
          class="inline-block py-2 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
          :class="
            route.path == '/dashboard/profile/izinlerim'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-mug-hot"></i> İzinlerim
        </RouterLink>
      </li>

      <li class="mr-2 md:block hidden">
        <RouterLink
          to="/dashboard/profile/odemelerim"
          class="inline-block py-2 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
          :class="
            route.path == '/dashboard/profile/odemelerim'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-money-bill"></i> Ödemelerim
        </RouterLink>
      </li>

      <li class="mr-2 md:block hidden">
        <RouterLink
          to="/dashboard/profile/mesailerim"
          class="inline-block py-2 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
          :class="
            route.path == '/dashboard/profile/mesailerim'
              ? 'text-sky-600 border-sky-600'
              : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
          "
        >
          <i class="fa-solid fa-briefcase"></i> Mesailerim
        </RouterLink>
      </li>

      <li class="mr-2 md:block hidden">
        <RouterLink
          to="/dashboard/profile/diger"
          class="inline-block py-2 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
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
    <button
      v-if="route.path == '/dashboard/profile/izinlerim'"
      @click="
        () => {
          izinTalepCreate = true;
        }
      "
      class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
    >
      <i class="fas fa-plus mr-2"></i> İzin Talebi Oluştur
    </button>
  </div>

  <RouterView></RouterView>
  <IzinTalepCreateModal v-if="izinTalepCreate" @closeModal="closeIzinTalep" />
</template>
