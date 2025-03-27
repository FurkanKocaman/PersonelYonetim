<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import IzinKurallariService from "@/services/IzinKurallariService";

const router = useRouter();

// İzin kuralları için veri
const loading = ref(true);
const error = ref(false);
const activeTab = ref('kurallar'); // Default active tab

// Verileri yükle
const loadData = async () => {
  loading.value = true;
  error.value = false;
  
  try {
    // API'den verileri al
    await IzinKurallariService.getIzinKurallari();
    // Başarılı yükleme durumunda herhangi bir işlem yapılmıyor
  } catch (err) {
    console.error("İzin kuralları yüklenirken hata oluştu:", err);
    error.value = true;
  } finally {
    loading.value = false;
  }
};

// Sayfa yüklendiğinde verileri getir
onMounted(() => {
  loadData();
});

// Sekme değiştirme işlevi
const changeTab = (tab: string) => {
  if (tab === activeTab.value) return;
  
  switch(tab) {
    case 'kurallar':
      router.push({ name: 'IzinKurallariKurallar' });
      break;
    case 'raporlar':
      router.push({ name: 'IzinKurallariRaporlar' });
      break;
    case 'orneksablonlar':
      router.push({ name: 'IzinKurallariOrnekSablonlar' });
      break;
    default:
      router.push({ name: 'IzinKurallariKurallar' });
  }
};
</script>

<template>
  <div class="flex-1 transition-all duration-300">
    <!-- Üst Başlık Alanı -->
    <header class="bg-white dark:bg-neutral-800 shadow-sm">
      <div class="py-4 px-6">
        <div class="flex items-center justify-between">
          <h1 class="text-2xl font-bold text-gray-800 dark:text-gray-200">İzin Kuralları</h1>
          <div class="flex space-x-2">
            <router-link 
              :to="{ name: 'Izin' }" 
              class="px-4 py-2 bg-gray-200 text-gray-700 dark:bg-neutral-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-neutral-600 rounded-lg transition-colors duration-300"
            >
              İzin Talepleri
            </router-link>
          </div>
        </div>
      </div>
    </header>

    <!-- İçerik Alanı -->
    <main class="p-6">
      <!-- Yükleniyor Göstergesi -->
      <div v-if="loading" class="flex justify-center items-center py-12">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>

      <!-- Hata Mesajı -->
      <div v-else-if="error" class="bg-red-100 border border-red-200 text-red-700 dark:bg-red-900 dark:border-red-800 dark:text-red-300 rounded-lg p-4 mb-6">
        <div class="flex items-center">
          <i class="fas fa-exclamation-circle mr-2"></i>
          <span>İzin kuralları yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.</span>
        </div>
      </div>

      <!-- Ana İçerik -->
      <div v-else class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm overflow-hidden">
        <!-- Sekmeler -->
        <div class="flex border-b border-gray-200 dark:border-neutral-700">
          <button 
            @click="changeTab('kurallar')" 
            class="px-6 py-3 text-sm font-medium border-b-2 transition-colors duration-300"
            :class="{ 
              'border-blue-600 text-blue-600 dark:border-blue-500 dark:text-blue-500': $route.name === 'IzinKurallariKurallar', 
              'border-transparent text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300': $route.name !== 'IzinKurallariKurallar' 
            }"
          >
            Kurallar
          </button>
          <button 
            @click="changeTab('raporlar')" 
            class="px-6 py-3 text-sm font-medium border-b-2 transition-colors duration-300"
            :class="{ 
              'border-blue-600 text-blue-600 dark:border-blue-500 dark:text-blue-500': $route.name === 'IzinKurallariRaporlar', 
              'border-transparent text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300': $route.name !== 'IzinKurallariRaporlar' 
            }"
          >
            Raporlar
          </button>
          <button 
            @click="changeTab('orneksablonlar')" 
            class="px-6 py-3 text-sm font-medium border-b-2 transition-colors duration-300"
            :class="{ 
              'border-blue-600 text-blue-600 dark:border-blue-500 dark:text-blue-500': $route.name === 'IzinKurallariOrnekSablonlar', 
              'border-transparent text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300': $route.name !== 'IzinKurallariOrnekSablonlar' 
            }"
          >
            Örnek Şablonlar
          </button>
        </div>

        <!-- Sekme İçeriği -->
        <div class="p-6">
          <router-view></router-view>
        </div>
      </div>
    </main>
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
