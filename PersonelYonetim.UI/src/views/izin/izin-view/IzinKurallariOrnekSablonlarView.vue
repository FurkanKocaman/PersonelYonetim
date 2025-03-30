<template>
  <div class="flex-1 transition-all duration-300">
    <!-- İçerik Alanı -->
    <main class="p-6">
      <!-- Üst Kontroller -->
      <div class="mb-6 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div class="flex items-center">
          <h2 class="text-xl font-semibold text-gray-800 dark:text-gray-200">İzin Şablonları</h2>
        </div>
      </div>

      <!-- Yükleniyor Göstergesi -->
      <div v-if="loading" class="flex justify-center items-center py-12">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>

      <!-- Hata Mesajı -->
      <div
        v-else-if="error"
        class="bg-red-100 border border-red-200 text-red-700 dark:bg-red-900 dark:border-red-800 dark:text-red-300 rounded-lg p-4 mb-6"
      >
        <div class="flex items-center">
          <i class="fas fa-exclamation-circle mr-2"></i>
          <span
            >İzin şablonları yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.</span
          >
        </div>
      </div>

      <!-- Şablonlar -->
      <div
        v-else-if="templates.length > 0"
        class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6"
      >
        <div
          v-for="template in templates"
          :key="template.id"
          class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm overflow-hidden"
        >
          <div class="p-6">
            <h3 class="text-lg font-semibold text-gray-800 dark:text-gray-200 mb-2">{{ template.name }}</h3>
            <p class="text-gray-600 dark:text-gray-400 mb-4">{{ template.description }}</p>

            <div class="flex items-center text-sm text-gray-500 dark:text-gray-400 mb-4">
              <span class="px-2 py-1 bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-300 rounded-full text-xs mr-2">
                {{ template.entitlement }}
              </span>
              <span class="px-2 py-1 bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300 rounded-full text-xs">
                {{ template.paid ? 'Ücretli' : 'Ücretsiz' }}
              </span>
            </div>

            <div class="flex justify-end space-x-2">
              <button
                @click="useTemplate(template)"
                class="bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg text-sm px-4 py-2 text-center transition-colors duration-300"
              >
                Şablonu Kullan
              </button>
              <button
                @click="copyTemplate(template)"
                class="bg-gray-200 hover:bg-gray-300 text-gray-700 dark:bg-neutral-700 dark:text-gray-300 dark:hover:bg-neutral-600 font-medium rounded-lg text-sm px-4 py-2 text-center transition-colors duration-300"
              >
                Kopyala
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Veri Yok Mesajı -->
      <div v-else class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 text-center">
        <p class="text-gray-500 dark:text-gray-400">Henüz izin şablonu bulunmamaktadır.</p>
      </div>

      <!-- Şablon Uygulama Modalı -->
      <div v-if="showTemplateModal && selectedTemplate" class="fixed inset-0 z-50 overflow-y-auto">
        <div class="flex items-center justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
          <div class="fixed inset-0 transition-opacity" aria-hidden="true">
            <div class="absolute inset-0 bg-gray-500 dark:bg-gray-900 opacity-75"></div>
          </div>

          <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

          <div class="inline-block align-bottom bg-white dark:bg-neutral-800 rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
            <div class="bg-white dark:bg-neutral-800 px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
              <div class="sm:flex sm:items-start">
                <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left w-full">
                  <h3 class="text-lg leading-6 font-medium text-gray-900 dark:text-gray-100">
                    {{ selectedTemplate.name }} Şablonunu Uygula
                  </h3>
                  <div class="mt-2">
                    <p class="text-sm text-gray-500 dark:text-gray-400">
                      Bu şablonu uygulamak, mevcut izin kurallarınızı değiştirecektir. Devam etmek istediğinizden emin misiniz?
                    </p>
                  </div>
                  <div class="mt-4">
                    <div class="bg-gray-50 dark:bg-neutral-700 p-3 rounded-md">
                      <h4 class="text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Şablon İçeriği:</h4>
                      <ul class="text-sm text-gray-600 dark:text-gray-400 space-y-1">
                        <li v-for="(rule, index) in selectedTemplate.rules" :key="index">
                          • {{ rule.name }}: {{ rule.description }}
                        </li>
                      </ul>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="bg-gray-50 dark:bg-neutral-700 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
              <button
                type="button"
                @click="applyTemplate"
                class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:ml-3 sm:w-auto sm:text-sm"
              >
                Şablonu Uygula
              </button>
              <button
                type="button"
                @click="showTemplateModal = false"
                class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm dark:bg-neutral-700 dark:border-neutral-600 dark:text-gray-200 dark:hover:bg-neutral-600"
              >
                İptal
              </button>
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import IzinKurallariService, { type IzinKurali } from "@/services/IzinKurallariService";

// Şablon için genişletilmiş tip tanımı
interface IzinSablonu extends IzinKurali {
  rules?: Array<{name: string, description: string}>;
}

// Şablon verileri
const templates = ref<IzinSablonu[]>([]);
const loading = ref(true);
const error = ref(false);
const selectedTemplate = ref<IzinSablonu | null>(null);
const showTemplateModal = ref(false);

// Şablonları yükle
const loadTemplates = async () => {
  loading.value = true;
  error.value = false;

  try {
    // API'den verileri al (mock veri kullanılıyor şimdilik)
    // const response = await IzinKurallariService.getTemplates();
    // templates.value = response.data;

    // Mock veri
    templates.value = [
      { id: 1, name: 'Standart İzin Paketi', description: 'Temel izin hakları içeren standart paket', paid: true, entitlement: 'Yıllık', frequency: 'Yıllık', minDays: 14, maxDays: 14, days: 14, active: true, rules: [{ name: 'Yıllık İzin', description: 'Yılda 14 gün ücretli izin' }] },
      { id: 2, name: 'Genişletilmiş İzin Paketi', description: 'Daha fazla izin hakkı içeren genişletilmiş paket', paid: true, entitlement: 'Yıllık', frequency: 'Yıllık', minDays: 21, maxDays: 21, days: 21, active: true, rules: [{ name: 'Yıllık İzin', description: 'Yılda 21 gün ücretli izin' }] },
      { id: 3, name: 'Mazeret İzinleri Paketi', description: 'Çeşitli mazeret izinlerini içeren paket', paid: true, entitlement: 'Olay Bazlı', frequency: 'Gerektiğinde', minDays: 1, maxDays: 5, days: 5, active: true, rules: [{ name: 'Mazeret İzni', description: 'Yılda 5 gün ücretli mazeret izni' }] },
    ];

    loading.value = false;
  } catch (err) {
    console.error("Şablonlar yüklenirken hata oluştu:", err);
    error.value = true;
    loading.value = false;
  }
};

// Şablonu kullan
const useTemplate = (template: IzinSablonu) => {
  selectedTemplate.value = template;
  showTemplateModal.value = true;
};

// Şablonu kopyala
const copyTemplate = (template: IzinSablonu) => {
  // Şablonu kopyala ve yeni kural oluştur
  console.log("Şablon kopyalanıyor:", template);

  // Burada şablonu kopyalayarak yeni bir kural oluşturma işlemi yapılabilir
  // Örneğin:
  // const newRule = { ...template };
  // delete newRule.id;
  // newRule.name = `${template.name} (Kopya)`;
  // IzinKurallariService.createRule(newRule);

  // Başarılı mesajı göster
  alert("Şablon başarıyla kopyalandı ve yeni kural oluşturuldu!");
};

// Şablonu uygula
const applyTemplate = () => {
  // Şablonu uygula
  console.log("Şablon uygulanıyor:", selectedTemplate.value);

  // Burada şablonu uygulama işlemi yapılabilir
  // Örneğin:
  // IzinKurallariService.applyTemplate(selectedTemplate.value.id);

  // Modalı kapat
  showTemplateModal.value = false;

  // Başarılı mesajı göster
  alert("Şablon başarıyla uygulandı!");
};

// Komponent yüklendiğinde
onMounted(() => {
  loadTemplates();
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
