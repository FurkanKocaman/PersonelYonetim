<template>
  <div class="flex-1 transition-all duration-300">
    <!-- İçerik Alanı -->
    <main class="p-6">
      <!-- Üst Kontroller -->
      <div class="mb-6 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div class="flex items-center">
          <h2 class="text-xl font-semibold text-gray-800 dark:text-gray-200">İzin Kuralları Yönetimi</h2>
        </div>
      </div>

      <!-- Sekmeler -->
      <div class="border-b border-gray-200 dark:border-gray-700 mb-6">
        <ul class="flex flex-wrap -mb-px">
          <li class="mr-2">
            <router-link
              :to="{ name: 'IzinKurallariKurallar' }"
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="
                $route.name === 'IzinKurallariKurallar'
                  ? 'text-sky-600 border-sky-600'
                  : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
              "
            >
              <i class="fas fa-book mr-2"></i> Kurallar
            </router-link>
          </li>
          <li class="mr-2">
            <router-link
              :to="{ name: 'IzinKurallariRaporlar' }"
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="
                $route.name === 'IzinKurallariRaporlar'
                  ? 'text-sky-600 border-sky-600'
                  : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
              "
            >
              <i class="fas fa-chart-bar mr-2"></i> Raporlar
            </router-link>
          </li>
          <li class="mr-2">
            <router-link
              :to="{ name: 'IzinKurallariOrnekSablonlar' }"
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="
                $route.name === 'IzinKurallariOrnekSablonlar'
                  ? 'text-sky-600 border-sky-600'
                  : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
              "
            >
              <i class="fas fa-copy mr-2"></i> Örnek Şablonlar
            </router-link>
          </li>
        </ul>
      </div>

      <!-- İçerik -->
      <router-view></router-view>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import IzinKurallariService from "@/services/IzinKurallariService";

const router = useRouter();
const route = useRoute();

// İzin kuralları için veri
const loading = ref(true);
const error = ref(false);

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
  // Eğer doğrudan IzinKurallari sayfasına gelinmişse, varsayılan olarak Kurallar sekmesine yönlendir
  if (route.name === 'IzinKurallari') {
    router.replace({ name: 'IzinKurallariKurallar' });
  }
  loadData();
});

// Yeni kural oluşturma sayfasına git
const createNewRule = () => {
  router.push({ name: 'IzinKuralOlustur' });
};
</script>

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
