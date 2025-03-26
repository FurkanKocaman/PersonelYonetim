<script setup lang="ts">
import { onMounted, computed } from "vue";
import { useRoute } from "vue-router";

const route = useRoute();
const activeTab = computed(() => {
  if (route.path.includes("/izin-kurallar")) return "izinkurallar";
  if (route.path.includes("/izinler")) return "izinler";
  return "";
});
onMounted(() => {
  console.log(activeTab);
});
</script>

<template>
  <div class="flex-1 transition-all duration-300">
    <div class="w-full mt-2 ml-5">
      <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">
        İzinleri görüntüleyip yeni türler ve kurallar oluşturabilirsiniz.
      </p>
    </div>
    <!-- Sekmeler -->
    <div
      class="mb-6 mt-2 border-b border-gray-200 dark:border-gray-700 flex justify-between items-center"
    >
      <ul class="flex flex-wrap -mb-px">
        <li class="mr-2">
          <router-link
            to="/dashboard/izin/izinler"
            class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
            :class="
              activeTab === 'izinler'
                ? 'text-sky-600 border-sky-600'
                : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
            "
          >
            <i class="fas fa-cog mr-2"></i> İzinler
          </router-link>
        </li>
        <li class="mr-2">
          <router-link
            to="/dashboard/izin/izin-kurallar"
            class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
            :class="
              activeTab === 'izinkurallar'
                ? 'text-sky-600 border-sky-600'
                : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
            "
          >
            <i class="fas fa-book mr-2"></i> Kurallar
          </router-link>
        </li>
        <li class="mr-2">
          <router-link
            to="/dashboard/izin/onay-sürecleri"
            class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
            :class="
              activeTab === ''
                ? 'text-sky-600 border-sky-600'
                : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
            "
          >
            <i class="fas fa-shield-alt mr-2"></i> Onay Süreçleri
          </router-link>
        </li>
      </ul>
      <router-link
        to="/dashboard/izin-kural-olustur"
        type="button"
        v-if="activeTab === 'izinkurallar'"
        class="cursor-pointer text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
      >
        Kural Ekle
      </router-link>
    </div>
    <!-- Sekmeler end -->
    <RouterView></RouterView>
  </div>
</template>

<style scoped>
.animate-spin {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}
</style>
