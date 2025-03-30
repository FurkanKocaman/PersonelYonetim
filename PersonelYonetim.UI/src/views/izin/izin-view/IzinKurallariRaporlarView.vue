<template>
  <div class="flex-1 transition-all duration-300">
    <!-- İçerik Alanı -->
    <main class="p-6">
      <!-- Üst Kontroller -->
      <div class="mb-6 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div class="flex items-center">
          <h2 class="text-xl font-semibold text-gray-800 dark:text-gray-200">İzin Raporları</h2>
        </div>
        <div class="flex space-x-2">
          <button
            @click="exportReport"
            class="cursor-pointer bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg text-sm px-5 py-2.5 text-center transition-colors duration-300 flex items-center"
          >
            <i class="fas fa-file-export mr-2"></i> Raporu Dışa Aktar
          </button>
        </div>
      </div>

      <!-- Filtreler -->
      <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 mb-6">
        <h3 class="text-lg font-semibold text-gray-800 dark:text-gray-200 mb-4">Rapor Filtreleri</h3>
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
          <div>
            <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Departman</label>
            <select v-model="filters.department" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white">
              <option value="">Tümü</option>
              <option v-for="dept in departments" :key="dept" :value="dept">{{ dept }}</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">İzin Tipi</label>
            <select v-model="filters.leaveType" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white">
              <option value="">Tümü</option>
              <option v-for="type in leaveTypes" :key="type" :value="type">{{ type }}</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Tarih Aralığı</label>
            <div class="flex space-x-2">
              <input 
                type="date" 
                v-model="filters.startDate" 
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white"
              />
              <input 
                type="date" 
                v-model="filters.endDate" 
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white"
              />
            </div>
          </div>
        </div>
        <div class="mt-6 flex justify-end">
          <button 
            @click="applyFilters"
            class="bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg text-sm px-5 py-2.5 text-center transition-colors duration-300"
          >
            Filtreleri Uygula
          </button>
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
          <span>İzin raporları yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.</span>
        </div>
      </div>

      <!-- Rapor Tablosu -->
      <div v-else-if="reports.length > 0" class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm overflow-hidden">
        <div class="overflow-x-auto">
          <table class="w-full text-sm text-left text-gray-500 dark:text-gray-400">
            <thead class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-neutral-700 dark:text-gray-400">
              <tr>
                <th scope="col" class="px-6 py-3">Personel</th>
                <th scope="col" class="px-6 py-3">Departman</th>
                <th scope="col" class="px-6 py-3">İzin Tipi</th>
                <th scope="col" class="px-6 py-3">Başlangıç Tarihi</th>
                <th scope="col" class="px-6 py-3">Bitiş Tarihi</th>
                <th scope="col" class="px-6 py-3">Gün Sayısı</th>
                <th scope="col" class="px-6 py-3">Durum</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="report in reports" :key="report.id" class="bg-white border-b dark:bg-neutral-800 dark:border-neutral-700 hover:bg-gray-50 dark:hover:bg-neutral-700">
                <td class="px-6 py-4 font-medium text-gray-900 dark:text-white">{{ report.personel }}</td>
                <td class="px-6 py-4">{{ report.department }}</td>
                <td class="px-6 py-4">{{ report.leaveType }}</td>
                <td class="px-6 py-4">{{ formatDate(report.startDate) }}</td>
                <td class="px-6 py-4">{{ formatDate(report.endDate) }}</td>
                <td class="px-6 py-4">{{ report.days }}</td>
                <td class="px-6 py-4">
                  <span class="px-2 py-1 text-xs font-semibold rounded-full" :class="getStatusClass(report.status)">
                    {{ report.status }}
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        
        <!-- Özet Bilgileri -->
        <div class="p-6 border-t border-gray-200 dark:border-neutral-700">
          <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div class="bg-gray-50 dark:bg-neutral-700 p-4 rounded-lg">
              <h4 class="text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">Toplam İzin Günü</h4>
              <p class="text-xl font-bold text-gray-800 dark:text-gray-200">{{ summary.totalDays }} gün</p>
            </div>
            <div class="bg-gray-50 dark:bg-neutral-700 p-4 rounded-lg">
              <h4 class="text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">Onaylanan İzinler</h4>
              <p class="text-xl font-bold text-green-600 dark:text-green-400">{{ summary.approved }} izin</p>
            </div>
            <div class="bg-gray-50 dark:bg-neutral-700 p-4 rounded-lg">
              <h4 class="text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">Bekleyen İzinler</h4>
              <p class="text-xl font-bold text-yellow-600 dark:text-yellow-400">{{ summary.pending }} izin</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Veri Yok Mesajı -->
      <div v-else class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 text-center">
        <p class="text-gray-500 dark:text-gray-400">Seçilen kriterlere uygun izin raporu bulunamadı.</p>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import IzinKurallariService from "@/services/IzinKurallariService";

// Rapor verileri
interface IzinReport {
  id: number;
  personel: string;
  department: string;
  leaveType: string;
  startDate: string;
  endDate: string;
  days: number;
  status: string;
}

interface ReportFilters {
  department: string;
  leaveType: string;
  startDate: string;
  endDate: string;
}

// Komponent state
const reports = ref<IzinReport[]>([]);
const loading = ref(true);
const error = ref(false);

// Filtreler
const filters = ref<ReportFilters>({
  department: '',
  leaveType: '',
  startDate: '',
  endDate: ''
});

// Statik veriler
const departments = ['İnsan Kaynakları', 'Muhasebe', 'Yazılım', 'Pazarlama', 'Satış'];
const leaveTypes = ['Yıllık İzin', 'Hastalık İzni', 'Mazeret İzni', 'Ücretsiz İzin'];

// Özet bilgileri
const summary = computed(() => {
  return {
    totalDays: reports.value.reduce((total, report) => total + report.days, 0),
    approved: reports.value.filter(report => report.status === 'Onaylandı').length,
    pending: reports.value.filter(report => report.status === 'Beklemede').length
  };
});

// Raporları yükle
const loadReports = async () => {
  loading.value = true;
  error.value = false;
  
  try {
    // API'den verileri al (mock veri kullanılıyor şimdilik)
    // const response = await IzinKurallariService.getReports(filters.value);
    // reports.value = response.data;
    
    // Mock veri
    reports.value = [
      { id: 1, personel: 'Ahmet Yılmaz', department: 'İnsan Kaynakları', leaveType: 'Yıllık İzin', startDate: '2023-06-01', endDate: '2023-06-15', days: 15, status: 'Onaylandı' },
      { id: 2, personel: 'Ayşe Demir', department: 'Muhasebe', leaveType: 'Hastalık İzni', startDate: '2023-07-10', endDate: '2023-07-12', days: 3, status: 'Onaylandı' },
      { id: 3, personel: 'Mehmet Kaya', department: 'Yazılım', leaveType: 'Mazeret İzni', startDate: '2023-08-05', endDate: '2023-08-05', days: 1, status: 'Beklemede' },
      { id: 4, personel: 'Zeynep Şahin', department: 'Pazarlama', leaveType: 'Yıllık İzin', startDate: '2023-09-20', endDate: '2023-09-30', days: 11, status: 'Onaylandı' },
      { id: 5, personel: 'Ali Yıldız', department: 'Satış', leaveType: 'Ücretsiz İzin', startDate: '2023-10-15', endDate: '2023-10-20', days: 6, status: 'Reddedildi' },
    ];
    
    loading.value = false;
  } catch (err) {
    console.error("İzin raporları yüklenirken hata oluştu:", err);
    error.value = true;
    loading.value = false;
  }
};

// Filtreleri uygula
const applyFilters = () => {
  loadReports();
};

// Raporu dışa aktar
const exportReport = () => {
  // Raporu dışa aktar
  console.log("Rapor dışa aktarılıyor...");
  alert("Rapor başarıyla dışa aktarıldı!");
};

// Tarih formatla
const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('tr-TR');
};

// Durum sınıfı al
const getStatusClass = (status: string) => {
  switch (status) {
    case 'Onaylandı':
      return 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300';
    case 'Beklemede':
      return 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300';
    case 'Reddedildi':
      return 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300';
    default:
      return 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300';
  }
};

// Komponent yüklendiğinde
onMounted(() => {
  loadReports();
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
</style>
