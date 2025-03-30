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
                class="inline-block p-4 text-blue-600 bg-gray-100 rounded-t-lg active dark:bg-gray-800 dark:text-blue-500"
              >
                Kurallar
              </router-link>
            </li>
            <li class="mr-2">
              <router-link
                :to="{ name: 'IzinKurallariOrnekSablonlar' }"
                class="inline-block p-4 rounded-t-lg hover:text-gray-600 hover:bg-gray-50 dark:hover:bg-gray-800 dark:hover:text-gray-300"
              >
                Örnek Şablonlar
              </router-link>
            </li>
          </ul>
        </div>
      </div>

      <!-- Kurallar İçeriği -->
      <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm overflow-hidden">
        <div class="p-6">
          <h2 class="text-xl font-semibold text-gray-800 dark:text-gray-200 mb-4">
            Örnek Türk İş Kanunu Kuralı
          </h2>

          <div class="mb-6">
            <h3 class="text-lg font-medium text-gray-700 dark:text-gray-300 mb-2">Detaylar</h3>

            <div class="space-y-4">
              <div>
                <h4 class="text-sm font-medium text-gray-600 dark:text-gray-400">Adı</h4>
                <p class="text-base text-gray-800 dark:text-gray-200">
                  Örnek Türk İş Kanunu Kuralı
                </p>
              </div>

              <div>
                <h4 class="text-sm font-medium text-gray-600 dark:text-gray-400">Açıklama</h4>
                <p class="text-base text-gray-800 dark:text-gray-200">
                  İzin kuralı ve türleri Türk İş Kanunu'na göre tanımlanmıştır.
                </p>
              </div>

              <div>
                <button
                  class="text-blue-600 dark:text-blue-400 text-sm font-medium hover:underline"
                >
                  Detayları Düzenleyin
                </button>
              </div>
            </div>
          </div>

          <div>
            <h3 class="text-lg font-medium text-gray-700 dark:text-gray-300 mb-2">
              Türler ve Kurallar
            </h3>
            <p class="text-sm text-gray-600 dark:text-gray-400 mb-4">
              Kural, çalışanların talep edebileceği 13 izin türüne sahip.
            </p>

            <div class="overflow-x-auto">
              <table class="min-w-full divide-y divide-gray-200 dark:divide-neutral-700">
                <thead class="bg-gray-50 dark:bg-neutral-700">
                  <tr>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      <div class="flex items-center">
                        Tür
                        <button class="ml-1 text-gray-400">
                          <i class="fas fa-sort"></i>
                        </button>
                      </div>
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Durum
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Ücretli / Ücretsiz
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Hak Ediş
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      Hak Ediş Sıklığı
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      En az
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      En çok
                    </th>
                    <th
                      scope="col"
                      class="px-6 py-3 text-center text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
                    >
                      İşlemler
                    </th>
                  </tr>
                </thead>
                <tbody
                  class="bg-white dark:bg-neutral-800 divide-y divide-gray-200 dark:divide-neutral-700"
                >
                  <tr
                    v-for="rule in izinRules"
                    :key="rule.id"
                    class="hover:bg-gray-50 dark:hover:bg-neutral-700"
                  >
                    <td class="px-6 py-4 whitespace-nowrap">
                      <div
                        class="text-sm font-medium text-blue-600 dark:text-blue-400 hover:underline cursor-pointer"
                      >
                        {{ rule.name }}
                      </div>
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap">
                      <span
                        class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full"
                        :class="
                          rule.active
                            ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300'
                            : 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-300'
                        "
                      >
                        {{ rule.active ? "Ücretli" : "Ücretsiz" }}
                      </span>
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400"
                    >
                      {{ rule.paid ? "Ücretli" : "Ücretsiz" }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400"
                    >
                      {{ rule.entitlement }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400"
                    >
                      {{ rule.frequency }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400"
                    >
                      {{ rule.minDays === 0 ? "—" : rule.minDays }}
                    </td>
                    <td
                      class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400"
                    >
                      {{ rule.maxDays === 0 ? "—" : rule.maxDays }}
                    </td>
                    <td class="px-6 py-4 whitespace-nowrap text-sm text-right">
                      <div class="flex justify-center space-x-2">
                        <button
                          class="text-gray-500 hover:text-blue-600 dark:text-gray-400 dark:hover:text-blue-400"
                        >
                          <i class="fas fa-ellipsis-v"></i>
                        </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
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

// İzin kuralları için veri
const izinRules = ref<IzinKurali[]>([]);
const loading = ref(true);
const error = ref(false);

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
</style>
