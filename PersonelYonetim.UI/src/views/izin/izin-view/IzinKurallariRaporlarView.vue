<template>
  <div class="flex-1 transition-all duration-300">
    <!-- Üst Başlık Alanı -->
    <header class="bg-white dark:bg-neutral-800 shadow-sm">
      <div class="py-4 px-6">
        <h1 class="text-2xl font-bold text-gray-800 dark:text-gray-200">İzin Kuralları Raporları</h1>
      </div>
    </header>

    <!-- İçerik Alanı -->
    <main class="p-6">
      <!-- Üst Kontroller -->
      <div class="mb-6 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div class="flex flex-col w-full">
          <h2 class="text-lg font-semibold text-gray-800 dark:text-gray-200 mb-4">İzin Raporları</h2>
          
          <!-- Filtreler -->
          <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">
            <!-- İzin Türü Filtresi -->
            <div>
              <label for="izinTipi" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                İzin Türü
              </label>
              <select 
                id="izinTipi"
                v-model="filterOptions.izinTipi" 
                class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
              >
                <option v-for="tur in izinTurleri" :key="tur" :value="tur">{{ tur }}</option>
              </select>
            </div>
            
            <!-- Ücret Durumu Filtresi -->
            <div>
              <label for="ucretDurumu" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Ücret Durumu
              </label>
              <select 
                id="ucretDurumu"
                v-model="filterOptions.ucretDurumu" 
                class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
              >
                <option value="">Tümü</option>
                <option value="Ücretli">Ücretli</option>
                <option value="Ücretsiz">Ücretsiz</option>
              </select>
            </div>
            
            <!-- Tarih Aralığı Filtresi -->
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Tarih Aralığı
              </label>
              <div class="flex space-x-2">
                <input 
                  type="date" 
                  v-model="filterOptions.dateRange.start"
                  class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                />
                <input 
                  type="date" 
                  v-model="filterOptions.dateRange.end"
                  class="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                />
              </div>
            </div>
          </div>
          
          <!-- Rapor İşlemleri -->
          <div class="flex space-x-2">
            <button 
              @click="exportReport"
              class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-300 flex items-center"
            >
              <i class="fas fa-file-export mr-2"></i>
              Dışa Aktar
            </button>
            <button 
              @click="printReport"
              class="px-4 py-2 bg-gray-600 text-white rounded-lg hover:bg-gray-700 transition-colors duration-300 flex items-center"
            >
              <i class="fas fa-print mr-2"></i>
              Yazdır
            </button>
          </div>
        </div>
      </div>

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

      <!-- Rapor Tablosu -->
      <div v-else-if="filteredRules.length > 0" class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm overflow-hidden">
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200 dark:divide-neutral-700">
            <thead class="bg-gray-50 dark:bg-neutral-700">
              <tr>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                  İzin Türü
                </th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                  Açıklama
                </th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                  Ücret Durumu
                </th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                  Hak Ediş
                </th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">
                  Gün
                </th>
              </tr>
            </thead>
            <tbody class="bg-white dark:bg-neutral-800 divide-y divide-gray-200 dark:divide-neutral-700">
              <tr v-for="rule in filteredRules" :key="rule.id" class="hover:bg-gray-50 dark:hover:bg-neutral-700">
                <td class="px-6 py-4 whitespace-nowrap">
                  <div class="text-sm font-medium text-gray-900 dark:text-gray-100">{{ rule.name }}</div>
                  <div v-if="rule.code" class="text-xs text-gray-500 dark:text-gray-400">Kod: {{ rule.code }}</div>
                </td>
                <td class="px-6 py-4">
                  <div class="text-sm text-gray-500 dark:text-gray-400">{{ rule.description }}</div>
                </td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <span class="px-2 py-1 inline-flex text-xs leading-5 font-semibold rounded-full" :class="rule.paid ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300' : 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300'">
                    {{ rule.paid ? 'Ücretli' : 'Ücretsiz' }}
                  </span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400">
                  {{ rule.entitlement }}
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400">
                  {{ rule.days }}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Veri Yok Mesajı -->
      <div v-else class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 text-center">
        <p class="text-gray-500 dark:text-gray-400">Seçilen filtrelere uygun izin kuralı bulunamadı.</p>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue';
import IzinKurallariService, { type IzinKurali } from "@/services/IzinKurallariService";

// Rapor verileri
const izinRules = ref<IzinKurali[]>([]);
const loading = ref(true);
const error = ref(false);

// Filtre seçenekleri
const filterOptions = reactive({
  izinTipi: '',
  ucretDurumu: '',
  dateRange: {
    start: '',
    end: ''
  }
});

// İzin türleri listesi
const izinTurleri = computed(() => {
  return ['Tüm İzin Türleri', ...new Set(izinRules.value.map(rule => rule.name))];
});

// Filtrelenmiş izin kuralları
const filteredRules = computed(() => {
  return izinRules.value.filter(rule => {
    const izinTipiMatch = !filterOptions.izinTipi || filterOptions.izinTipi === 'Tüm İzin Türleri' || rule.name === filterOptions.izinTipi;
    const ucretDurumuMatch = !filterOptions.ucretDurumu || 
      (filterOptions.ucretDurumu === 'Ücretli' && rule.paid) || 
      (filterOptions.ucretDurumu === 'Ücretsiz' && !rule.paid);
    
    return izinTipiMatch && ucretDurumuMatch;
  });
});

// Verileri yükle
const loadIzinKurallari = async () => {
  loading.value = true;
  error.value = false;
  
  try {
    // API'den verileri al
    const response = await IzinKurallariService.getIzinKurallari();
    izinRules.value = response.items;
  } catch (err) {
    console.error("İzin kuralları yüklenirken hata oluştu:", err);
    error.value = true;
  } finally {
    loading.value = false;
  }
};

// Raporu dışa aktar
const exportReport = () => {
  // CSV veya Excel formatında dışa aktarma işlemi burada yapılabilir
  alert('Rapor dışa aktarma özelliği henüz uygulanmadı.');
};

// Raporu yazdır
const printReport = () => {
  window.print();
};

// Komponent yüklendiğinde
onMounted(() => {
  loadIzinKurallari();
});
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

@media print {
  button {
    display: none;
  }
}
</style>
