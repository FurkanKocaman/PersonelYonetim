<script setup lang="ts">
import { ref, computed, onMounted, onActivated, defineComponent } from "vue";
import { useRouter } from "vue-router";
import IzinService, { mockIzinList } from "@/services/IzinService";
import type { IzinItem } from "@/models/IzinModels";

const router = useRouter();

// İzin listesi için veri
const izinList = ref<IzinItem[]>([]);
const loading = ref(true);
const error = ref(false);

// Seçilen izin detayları için modal
const showDetailModal = ref(false);
const selectedIzin = ref<IzinItem | null>(null);

// Durum renklerini belirleme
const statusColors: Record<string, string> = {
  "Onaylandı": "text-green-600 bg-green-100 dark:bg-green-900 dark:text-green-300",
  "Beklemede": "text-amber-600 bg-amber-100 dark:bg-amber-900 dark:text-amber-300",
  "Reddedildi": "text-red-600 bg-red-100 dark:bg-red-900 dark:text-red-300"
};

// Filtre seçenekleri
const filterOptions = ref({
  durum: "",
  izinTipi: ""
});

// Filtrelenmiş izin listesi
const filteredIzinList = computed(() => {
  return izinList.value.filter(izin => {
    const durumMatch = !filterOptions.value.durum || izin.durum === filterOptions.value.durum;
    const izinTipiMatch = !filterOptions.value.izinTipi || izin.izinTipi === filterOptions.value.izinTipi;
    return durumMatch && izinTipiMatch;
  });
});

// İzin türleri
const izinTurleri = ["Yıllık İzin", "Hastalık İzni", "Mazeret İzni", "Ücretsiz İzin"];

// İzin durumları
const izinDurumlari = ["Onaylandı", "Beklemede", "Reddedildi"];

// Verileri yükle
const loadIzinList = async () => {
  console.log("İzin listesi yükleniyor...");
  loading.value = true;
  error.value = false;
  
  try {
    // API'den verileri al
    const response = await IzinService.getIzinList();
    console.log("Yüklenen izin verileri:", response);
    
    if (response && response.items && response.items.length > 0) {
      izinList.value = response.items;
    } else {
      console.warn("API'den boş veri döndü, örnek veriler kullanılıyor");
      // Veri yoksa örnek veriler kullan
      izinList.value = [
        { id: 1, personelAdi: "Ahmet Yılmaz", izinTipi: "Yıllık İzin", baslangicTarihi: "15.03.2025", bitisTarihi: "20.03.2025", gun: 5, durum: "Onaylandı", aciklama: "Yıllık izin talebi" },
        { id: 2, personelAdi: "Ayşe Demir", izinTipi: "Mazeret İzni", baslangicTarihi: "22.03.2025", bitisTarihi: "24.03.2025", gun: 2, durum: "Beklemede", aciklama: "Aile ziyareti için izin" },
        { id: 3, personelAdi: "Mehmet Kaya", izinTipi: "Yıllık İzin", baslangicTarihi: "01.04.2025", bitisTarihi: "10.04.2025", gun: 9, durum: "Reddedildi", aciklama: "Yoğun iş dönemi nedeniyle reddedildi" },
        { id: 4, personelAdi: "Zeynep Öztürk", izinTipi: "Hastalık İzni", baslangicTarihi: "05.04.2025", bitisTarihi: "07.04.2025", gun: 2, durum: "Onaylandı", aciklama: "Doktor raporu ile onaylandı" },
        { id: 5, personelAdi: "Can Yılmaz", izinTipi: "Yıllık İzin", baslangicTarihi: "12.04.2025", bitisTarihi: "16.04.2025", gun: 4, durum: "Beklemede", aciklama: "Tatil planı için izin" },
      ];
    }
  } catch (err) {
    console.error("İzin listesi yüklenirken hata oluştu:", err);
    error.value = true;
    
    // Hata durumunda örnek veriler kullan
    izinList.value = [
      { id: 1, personelAdi: "Ahmet Yılmaz", izinTipi: "Yıllık İzin", baslangicTarihi: "15.03.2025", bitisTarihi: "20.03.2025", gun: 5, durum: "Onaylandı", aciklama: "Yıllık izin talebi" },
      { id: 2, personelAdi: "Ayşe Demir", izinTipi: "Mazeret İzni", baslangicTarihi: "22.03.2025", bitisTarihi: "24.03.2025", gun: 2, durum: "Beklemede", aciklama: "Aile ziyareti için izin" },
      { id: 3, personelAdi: "Mehmet Kaya", izinTipi: "Yıllık İzin", baslangicTarihi: "01.04.2025", bitisTarihi: "10.04.2025", gun: 9, durum: "Reddedildi", aciklama: "Yoğun iş dönemi nedeniyle reddedildi" },
      { id: 4, personelAdi: "Zeynep Öztürk", izinTipi: "Hastalık İzni", baslangicTarihi: "05.04.2025", bitisTarihi: "07.04.2025", gun: 2, durum: "Onaylandı", aciklama: "Doktor raporu ile onaylandı" },
      { id: 5, personelAdi: "Can Yılmaz", izinTipi: "Yıllık İzin", baslangicTarihi: "12.04.2025", bitisTarihi: "16.04.2025", gun: 4, durum: "Beklemede", aciklama: "Tatil planı için izin" },
    ];
  } finally {
    loading.value = false;
  }
};

// İzin detaylarını görüntüle
const viewIzinDetails = (izin: IzinItem) => {
  selectedIzin.value = izin;
  showDetailModal.value = true;
};

// İzin düzenleme sayfasına git
const editIzin = (izin: IzinItem) => {
  // İzin düzenleme sayfasına yönlendir
  router.push({
    name: 'IzinTalep',
    query: { id: izin.id.toString() }
  });
};

// Detay modalını kapat
const closeDetailModal = () => {
  showDetailModal.value = false;
  selectedIzin.value = null;
};

// Sayfa yüklendiğinde verileri getir
onMounted(() => {
  console.log("IzinView mounted");
  loadIzinList();
});

// Vue Router'ın keep-alive özelliği ile kullanıldığında, sayfa her aktif olduğunda verileri yenile
onActivated(() => {
  console.log("IzinView activated");
  loadIzinList();
});
</script>

<template>
  <div class="flex-1 transition-all duration-300">
    <!-- Üst Başlık Alanı -->
    <header class="bg-white dark:bg-neutral-800 shadow-sm">
      <div class="py-4 px-6">
        <h1 class="text-2xl font-bold text-gray-800 dark:text-gray-200">İzin Yönetimi</h1>
      </div>
    </header>

    <!-- İçerik Alanı -->
    <main class="p-6">
      <!-- Üst Kontroller -->
      <div class="mb-6 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <!-- Filtreler ve Sekmeler -->
        <div class="flex flex-col gap-4 w-full md:w-auto">
          <!-- Sekmeler -->
          <div class="flex space-x-2 mb-2">
            <router-link 
              :to="{ name: 'Izin' }" 
              class="px-4 py-2 bg-blue-600 text-white rounded-lg"
            >
              İzin Talepleri
            </router-link>
            <router-link 
              :to="{ name: 'IzinKurallari' }" 
              class="px-4 py-2 bg-gray-200 text-gray-700 dark:bg-neutral-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-neutral-600 rounded-lg transition-colors duration-300"
            >
              İzin Kuralları
            </router-link>
          </div>
          
          <!-- Filtreler -->
          <div class="flex flex-wrap gap-4">
            <div class="w-full sm:w-auto">
              <select 
                v-model="filterOptions.durum" 
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
              >
                <option value="">Tüm Durumlar</option>
                <option v-for="durum in izinDurumlari" :key="durum" :value="durum">{{ durum }}</option>
              </select>
            </div>
            <div class="w-full sm:w-auto">
              <select 
                v-model="filterOptions.izinTipi" 
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
              >
                <option value="">Tüm İzin Türleri</option>
                <option v-for="izinTipi in izinTurleri" :key="izinTipi" :value="izinTipi">{{ izinTipi }}</option>
              </select>
            </div>
          </div>
        </div>
        
        <!-- Yeni İzin Talebi Butonu -->
        <router-link 
          :to="{ name: 'IzinTalep' }" 
          class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-300 flex items-center"
        >
          <i class="fas fa-plus mr-2"></i>
          Yeni İzin Talebi
        </router-link>
      </div>

      <!-- Yükleniyor Göstergesi -->
      <div v-if="loading" class="flex justify-center items-center py-12">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>

      <!-- Hata Mesajı -->
      <div v-else-if="error" class="bg-red-100 border border-red-200 text-red-700 dark:bg-red-900 dark:border-red-800 dark:text-red-300 rounded-lg p-4 mb-6">
        <div class="flex items-center">
          <i class="fas fa-exclamation-circle mr-2"></i>
          <span>İzin listesi yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.</span>
        </div>
      </div>

      <!-- İzin Listesi Tablosu -->
      <div v-else-if="izinList.length > 0" class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm overflow-hidden">
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200 dark:divide-neutral-700">
            <thead class="bg-gray-50 dark:bg-neutral-700">
              <tr>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">ID</th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">Personel</th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">Başlangıç</th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">Bitiş</th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">İzin Tipi</th>
                <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">Durum</th>
                <th scope="col" class="px-6 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider">İşlemler</th>
              </tr>
            </thead>
            <tbody class="bg-white dark:bg-neutral-800 divide-y divide-gray-200 dark:divide-neutral-700">
              <tr v-for="izin in filteredIzinList" :key="izin.id" class="hover:bg-gray-50 dark:hover:bg-neutral-700">
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200">{{ izin.id }}</td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <div class="flex items-center">
                    <div class="ml-4">
                      <div class="text-sm font-medium text-gray-900 dark:text-gray-200">{{ izin.personelAdi }}</div>
                      <!-- Departman bilgisi gösterilmiyor çünkü IzinItem arayüzünde yok -->
                    </div>
                  </div>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200">{{ izin.baslangicTarihi }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200">{{ izin.bitisTarihi }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-200">{{ izin.izinTipi }}</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm">
                  <span class="px-2 py-1 rounded-full text-xs font-medium" :class="statusColors[izin.durum]">
                    {{ izin.durum }}
                  </span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                  <button 
                    @click="viewIzinDetails(izin)" 
                    class="text-sky-600 dark:text-sky-400 hover:text-sky-800 dark:hover:text-sky-300 mr-3"
                  >
                    <i class="fas fa-eye"></i>
                  </button>
                  <button 
                    @click="editIzin(izin)" 
                    class="text-amber-600 dark:text-amber-400 hover:text-amber-800 dark:hover:text-amber-300"
                  >
                    <i class="fas fa-edit"></i>
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      
      <!-- Veri Yoksa -->
      <div v-else class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 text-center">
        <div class="flex flex-col items-center justify-center py-12">
          <i class="fas fa-calendar-times text-4xl text-gray-400 dark:text-gray-600 mb-4"></i>
          <h3 class="text-lg font-medium text-gray-900 dark:text-gray-200 mb-2">İzin Kaydı Bulunamadı</h3>
          <p class="text-gray-500 dark:text-gray-400 mb-6">Henüz hiç izin talebi oluşturulmamış.</p>
          <router-link 
            :to="{ name: 'IzinTalep' }" 
            class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-300 flex items-center"
          >
            <i class="fas fa-plus mr-2"></i>
            Yeni İzin Talebi Oluştur
          </router-link>
        </div>
      </div>
    </main>

    <!-- İzin Detay Modalı -->
    <div v-if="showDetailModal" class="fixed inset-0 z-50 overflow-y-auto">
      <div class="flex items-center justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <!-- Arkaplan Overlay -->
        <div class="fixed inset-0 transition-opacity" @click="closeDetailModal">
          <div class="absolute inset-0 bg-gray-500 dark:bg-neutral-900 opacity-75"></div>
        </div>

        <!-- Modal İçeriği -->
        <div class="inline-block align-bottom bg-white dark:bg-neutral-800 rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
          <div class="bg-white dark:bg-neutral-800 px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="sm:flex sm:items-start">
              <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left w-full">
                <h3 class="text-lg leading-6 font-medium text-gray-900 dark:text-gray-200 mb-4">
                  İzin Detayları
                </h3>
                <div class="mt-2 space-y-4">
                  <div v-if="selectedIzin" class="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <div>
                      <div class="text-sm font-medium text-gray-500 dark:text-gray-400">Personel</div>
                      <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">{{ selectedIzin.personelAdi }}</div>
                      <!-- Departman bilgisi gösterilmiyor çünkü IzinItem arayüzünde yok -->
                    </div>
                    <div>
                      <div class="text-sm font-medium text-gray-500 dark:text-gray-400">İzin Tipi</div>
                      <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">{{ selectedIzin.izinTipi }}</div>
                    </div>
                    <div>
                      <div class="text-sm font-medium text-gray-500 dark:text-gray-400">Durum</div>
                      <div class="mt-1">
                        <span class="px-2 py-1 rounded-full text-xs font-medium" :class="statusColors[selectedIzin.durum]">
                          {{ selectedIzin.durum }}
                        </span>
                      </div>
                    </div>
                    <div>
                      <div class="text-sm font-medium text-gray-500 dark:text-gray-400">Başlangıç Tarihi</div>
                      <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">{{ selectedIzin.baslangicTarihi }}</div>
                    </div>
                    <div>
                      <div class="text-sm font-medium text-gray-500 dark:text-gray-400">Bitiş Tarihi</div>
                      <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">{{ selectedIzin.bitisTarihi }}</div>
                    </div>
                    <div class="sm:col-span-2">
                      <div class="text-sm font-medium text-gray-500 dark:text-gray-400">Açıklama</div>
                      <div class="mt-1 text-sm text-gray-900 dark:text-gray-200">{{ selectedIzin.aciklama }}</div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="bg-gray-50 dark:bg-neutral-700 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <button 
              @click="closeDetailModal" 
              class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none sm:ml-3 sm:w-auto sm:text-sm"
            >
              Kapat
            </button>
            <button 
              @click="editIzin(selectedIzin!)" 
              class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 dark:border-neutral-600 shadow-sm px-4 py-2 bg-white dark:bg-neutral-800 text-base font-medium text-gray-700 dark:text-gray-200 hover:bg-gray-50 dark:hover:bg-neutral-700 focus:outline-none sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
            >
              Düzenle
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* İzin yönetimi sayfası için özel stiller */
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
