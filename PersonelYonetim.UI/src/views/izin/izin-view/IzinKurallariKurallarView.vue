<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import TableLayout from "@/components/TableLayout.vue";
import IzinKurallariService from "@/services/IzinKurallariService";

// İzin kuralı tipi tanımlama
interface IzinKurali {
  id: number;
  ad: string;
  aciklama: string;
  turSayisi: number;
  olusturan: string;
  olusturmaTarihi: string;
  durum: string;
}

const router = useRouter();
const loading = ref(true);
const error = ref(false);
const izinKurallari = ref<IzinKurali[]>([]);

// İzin kurallarını getir
const getIzinKurallari = async () => {
  loading.value = true;
  error.value = false;
  
  try {
    // API'den verileri al (mock veri kullanılıyor şimdilik)
    // const response = await IzinKurallariService.getIzinKurallari();
    // izinKurallari.value = response.data;
    
    // Mock veri
    izinKurallari.value = [
      { id: 1, ad: 'Yıllık İzin Kuralı', aciklama: 'Yıllık izin hakları', turSayisi: 1, olusturan: 'Admin', olusturmaTarihi: '01.01.2023', durum: 'Aktif' },
      { id: 2, ad: 'Mazeret İzni Kuralı', aciklama: 'Mazeret izinleri', turSayisi: 3, olusturan: 'Admin', olusturmaTarihi: '15.02.2023', durum: 'Aktif' },
      { id: 3, ad: 'Hastalık İzni Kuralı', aciklama: 'Hastalık izinleri', turSayisi: 2, olusturan: 'Admin', olusturmaTarihi: '10.03.2023', durum: 'Pasif' },
    ];
    
    loading.value = false;
  } catch (err) {
    console.error("İzin kuralları yüklenirken hata oluştu:", err);
    error.value = true;
    loading.value = false;
  }
};

// Kural detaylarını görüntüle
const viewRuleDetails = (id: number) => {
  console.log(`Kural detayları görüntüleniyor: ${id}`);
  // router.push({ name: 'IzinKuralDetay', params: { id } });
};

// Kuralı düzenle
const editRule = (id: number) => {
  console.log(`Kural düzenleniyor: ${id}`);
  // router.push({ name: 'IzinKuralDuzenle', params: { id } });
};

// Kuralı sil
const deleteRule = (id: number) => {
  console.log(`Kural siliniyor: ${id}`);
  // Silme işlemi için onay alınabilir ve ardından API çağrısı yapılabilir
  // await IzinKurallariService.deleteIzinKurali(id);
  // Başarılı olursa listeyi güncelle
  // getIzinKurallari();
};

// Yeni kural oluştur
const createNewRule = () => {
  router.push({ name: 'IzinKuralOlustur' });
};

// Sayfa yüklendiğinde verileri getir
onMounted(() => {
  getIzinKurallari();
});
</script>

<template>
  <div>
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

    <!-- İzin Kuralları Tablosu -->
    <div v-else>
      <!-- Filtre ve Arama Alanı -->
      <div class="mb-6 flex justify-between items-center">
        <div class="relative w-64">
          <input 
            type="text" 
            placeholder="Kural ara..." 
            class="w-full pl-10 pr-4 py-2 border border-gray-300 dark:border-neutral-600 rounded-lg bg-white dark:bg-neutral-700 text-gray-900 dark:text-gray-100"
          />
          <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <i class="fas fa-search text-gray-400 dark:text-gray-500"></i>
          </div>
        </div>
        
        <div class="flex space-x-2">
          <select class="border border-gray-300 dark:border-neutral-600 rounded-lg bg-white dark:bg-neutral-700 text-gray-900 dark:text-gray-100 px-4 py-2">
            <option value="all">Tüm Durumlar</option>
            <option value="active">Aktif</option>
            <option value="passive">Pasif</option>
          </select>
        </div>
      </div>

      <!-- Tablo -->
      <TableLayout
        :table-headers="['Ad', 'Açıklama', 'Tür Sayısı', 'Oluşturan', 'Oluşturma Tarihi', 'Durum']"
        :table-content="izinKurallari"
        :islemler="['edit', 'detaylar', 'sil']"
        @edit="editRule"
        @detaylar="viewRuleDetails"
        @sil="deleteRule"
      />
    </div>
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
