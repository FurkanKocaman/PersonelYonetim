<template>
  <div class="flex-1 transition-all duration-300">
    <!-- Üst Başlık Alanı -->
    <header class="bg-white dark:bg-neutral-800 shadow-sm">
      <div class="py-4 px-6">
        <h1 class="text-2xl font-bold text-gray-800 dark:text-gray-200">İzin Kuralları</h1>
      </div>
    </header>

    <!-- İçerik Alanı -->
    <main class="p-6">
      <!-- Üst Kontroller -->
      <div class="mb-6 flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div class="flex items-center">
          <ul
            class="flex flex-wrap text-sm font-medium text-center text-gray-500 border-b border-gray-200 dark:border-gray-700 dark:text-gray-400"
          >
            <li class="mr-2">
              <router-link
                :to="{ name: 'IzinKurallari' }"
                class="inline-block p-4 rounded-t-lg hover:text-gray-600 hover:bg-gray-50 dark:hover:bg-gray-800 dark:hover:text-gray-300"
              >
                İzinler
              </router-link>
            </li>
            <li class="mr-2">
              <router-link
                :to="{ name: 'IzinKurallariRaporlar' }"
                class="inline-block p-4 rounded-t-lg hover:text-gray-600 hover:bg-gray-50 dark:hover:bg-gray-800 dark:hover:text-gray-300"
              >
                Raporlar
              </router-link>
            </li>
            <li class="mr-2">
              <router-link
                :to="{ name: 'IzinKurallariKurallar' }"
                class="inline-block p-4 rounded-t-lg hover:text-gray-600 hover:bg-gray-50 dark:hover:bg-gray-800 dark:hover:text-gray-300"
              >
                Kurallar
              </router-link>
            </li>
            <li class="mr-2">
              <router-link
                :to="{ name: 'IzinKurallariOrnekSablonlar' }"
                class="inline-block p-4 text-blue-600 bg-gray-100 rounded-t-lg active dark:bg-gray-800 dark:text-blue-500"
              >
                Örnek Şablonlar
              </router-link>
            </li>
          </ul>
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
            <div class="flex justify-between items-start mb-4">
              <h3 class="text-lg font-semibold text-gray-800 dark:text-gray-200">
                {{ template.name }}
              </h3>
              <span
                class="px-2 py-1 text-xs font-semibold rounded-full"
                :class="
                  template.paid
                    ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300'
                    : 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300'
                "
              >
                {{ template.paid ? "Ücretli" : "Ücretsiz" }}
              </span>
            </div>

            <p class="text-gray-600 dark:text-gray-400 mb-4">{{ template.description }}</p>

            <div class="grid grid-cols-2 gap-4 mb-4">
              <div>
                <span class="block text-sm font-medium text-gray-500 dark:text-gray-400"
                  >Hak Ediş</span
                >
                <span class="block text-gray-800 dark:text-gray-200">{{
                  template.entitlement
                }}</span>
              </div>
              <div>
                <span class="block text-sm font-medium text-gray-500 dark:text-gray-400">Gün</span>
                <span class="block text-gray-800 dark:text-gray-200">{{ template.days }}</span>
              </div>
            </div>

            <div class="flex space-x-2">
              <button
                @click="useTemplate(template)"
                class="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-300 flex items-center justify-center"
              >
                <i class="fas fa-check mr-2"></i>
                Şablonu Kullan
              </button>
              <button
                @click="copyTemplate(template)"
                class="px-4 py-2 bg-gray-200 text-gray-800 dark:bg-gray-700 dark:text-gray-200 rounded-lg hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors duration-300"
              >
                <i class="fas fa-copy"></i>
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Veri Yok Mesajı -->
      <div v-else class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 text-center">
        <p class="text-gray-500 dark:text-gray-400">Henüz örnek şablon bulunmamaktadır.</p>
      </div>

      <!-- Şablon Detay Modalı -->
      <div
        v-if="selectedTemplate"
        class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-10"
      >
        <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-lg max-w-2xl w-full">
          <div class="p-6">
            <div class="flex justify-between items-center mb-4">
              <h3 class="text-xl font-semibold text-gray-800 dark:text-gray-200">
                {{ selectedTemplate.name }}
              </h3>
              <button
                @click="selectedTemplate = null"
                class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200"
              >
                <i class="fas fa-times"></i>
              </button>
            </div>

            <div class="mb-4">
              <span
                class="inline-block px-2 py-1 text-xs font-semibold rounded-full mb-2"
                :class="
                  selectedTemplate.paid
                    ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300'
                    : 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300'
                "
              >
                {{ selectedTemplate.paid ? "Ücretli" : "Ücretsiz" }}
              </span>
              <p class="text-gray-600 dark:text-gray-400">{{ selectedTemplate.description }}</p>
            </div>

            <div class="grid grid-cols-2 gap-4 mb-6">
              <div>
                <span class="block text-sm font-medium text-gray-500 dark:text-gray-400"
                  >Hak Ediş</span
                >
                <span class="block text-gray-800 dark:text-gray-200">{{
                  selectedTemplate.entitlement
                }}</span>
              </div>
              <div>
                <span class="block text-sm font-medium text-gray-500 dark:text-gray-400">Gün</span>
                <span class="block text-gray-800 dark:text-gray-200">{{
                  selectedTemplate.days
                }}</span>
              </div>
            </div>

            <div class="flex justify-end space-x-2">
              <button
                @click="copyTemplate(selectedTemplate)"
                class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-300 flex items-center"
              >
                <i class="fas fa-copy mr-2"></i>
                Kopyala ve Ekle
              </button>
              <button
                @click="selectedTemplate = null"
                class="px-4 py-2 bg-gray-200 text-gray-800 dark:bg-gray-700 dark:text-gray-200 rounded-lg hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors duration-300"
              >
                Kapat
              </button>
            </div>
          </div>
        </div>
      </div>
    </main>

    <!-- Şablon Uygulama Modalı -->
    <div v-if="showTemplateModal" class="fixed inset-0 z-50 overflow-y-auto">
      <div
        class="flex items-center justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0"
      >
        <div class="fixed inset-0 transition-opacity" aria-hidden="true">
          <div class="absolute inset-0 bg-gray-500 dark:bg-gray-900 opacity-75"></div>
        </div>

        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true"
          >&#8203;</span
        >

        <div
          class="inline-block align-bottom bg-white dark:bg-neutral-800 rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full"
        >
          <div class="bg-white dark:bg-neutral-800 px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="sm:flex sm:items-start">
              <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left w-full">
                <h3 class="text-lg leading-6 font-medium text-gray-900 dark:text-gray-100">
                  {{ selectedTemplate.title }} Şablonunu Uygula
                </h3>
                <div class="mt-2">
                  <p class="text-sm text-gray-500 dark:text-gray-400">
                    Bu şablonu uygulamak, mevcut izin kurallarınızı değiştirecektir. Devam etmek
                    istediğinizden emin misiniz?
                  </p>
                </div>
                <div class="mt-4">
                  <div class="bg-gray-50 dark:bg-neutral-700 p-3 rounded-md">
                    <h4 class="text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                      Şablon İçeriği:
                    </h4>
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
              @click="confirmApplyTemplate"
              class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:ml-3 sm:w-auto sm:text-sm"
            >
              Şablonu Uygula
            </button>
            <button
              type="button"
              @click="closeTemplateModal"
              class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm dark:bg-neutral-800 dark:text-gray-200 dark:border-neutral-600 dark:hover:bg-neutral-700"
            >
              İptal
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import IzinKurallariService, { type IzinKurali } from "@/services/IzinKurallariService";

// Şablon verileri
const templates = ref<IzinKurali[]>([]);
const loading = ref(true);
const error = ref(false);

// Seçili şablon
const selectedTemplate = ref<IzinKurali | null>(null);

// Şablonları yükle
const loadTemplates = async () => {
  loading.value = true;
  error.value = false;

  try {
    // API'den şablonları al
    const response = await IzinKurallariService.getIzinSablonlari();
    templates.value = response.items;
  } catch (err) {
    console.error("İzin şablonları yüklenirken hata oluştu:", err);
    error.value = true;
  } finally {
    loading.value = false;
  }
};

// Şablonu kullan
const useTemplate = (template: IzinKurali) => {
  selectedTemplate.value = template;
};

// Şablonu kopyala
const copyTemplate = async (template: IzinKurali) => {
  try {
    // Şablonu kopyala ve yeni bir kural olarak ekle
    const newRule = { ...template };
    const { id, ...ruleWithoutId } = newRule;
    ruleWithoutId.name = `${ruleWithoutId.name} (Kopya)`;

    await IzinKurallariService.createIzinKurali(ruleWithoutId);
    alert("Şablon başarıyla kopyalandı ve izin kurallarına eklendi.");
  } catch (err) {
    console.error("Şablon kopyalanırken hata oluştu:", err);
    alert("Şablon kopyalanırken bir hata oluştu.");
  }
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
