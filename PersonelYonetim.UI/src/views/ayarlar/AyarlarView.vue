<script setup lang="ts">
import { ref, onMounted } from "vue";

// Bileşen durumu
const isLoading = ref(true);
const activeTab = ref("genel");
const savedMessage = ref("");

// Kullanıcı ayarları
const userSettings = ref({
  name: "Ahmet Yılmaz",
  email: "ahmet.yilmaz@example.com",
  role: "Yönetici",
  language: "tr",
  darkMode: false,
  notifications: {
    email: true,
    browser: true,
    mobile: false,
  },
  security: {
    twoFactorAuth: false,
    sessionTimeout: 30,
  },
});

// Sistem ayarları
const systemSettings = ref({
  companyName: "ABC Şirketi",
  address: "İstanbul, Türkiye",
  phone: "+90 212 123 4567",
  email: "info@abcsirketi.com",
  logo: "/logo.png",
  defaultLanguage: "tr",
  dateFormat: "DD.MM.YYYY",
  timeFormat: "24",
});

// Ayarları yükle
const loadSettings = async () => {
  try {
    // API çağrısını simüle et
    isLoading.value = true;

    // Gerçek bir uygulamada, bu bir API çağrısı olurdu
    // const response = await axios.get('/api/settings');
    // userSettings.value = response.data.user;
    // systemSettings.value = response.data.system;

    // Şimdilik sahte veri kullanıyoruz
    await new Promise((resolve) => setTimeout(resolve, 1000));

    isLoading.value = false;
  } catch (err) {
    console.error("Ayarlar yüklenirken hata oluştu:", err);
    isLoading.value = false;
  }
};

// Ayarları kaydet
const saveSettings = async () => {
  try {
    // API çağrısını simüle et
    isLoading.value = true;

    // Gerçek bir uygulamada, bu bir API çağrısı olurdu
    // await axios.post('/api/settings', {
    //   user: userSettings.value,
    //   system: systemSettings.value
    // });

    // API gecikmesini simüle et
    await new Promise((resolve) => setTimeout(resolve, 1000));

    // Başarı mesajını göster
    savedMessage.value = "Ayarlar başarıyla kaydedildi!";
    setTimeout(() => {
      savedMessage.value = "";
    }, 3000);

    isLoading.value = false;
  } catch (err) {
    console.error("Ayarlar kaydedilirken hata oluştu:", err);
    isLoading.value = false;
  }
};

// Aktif sekmeyi değiştir
const setActiveTab = (tab: string) => {
  activeTab.value = tab;
};

// Bileşeni başlat
onMounted(() => {
  loadSettings();
});
</script>

<template>
  <div class="container mx-auto px-4 py-2">
    <div class="mb-3">
      <h1 class="text-2xl font-bold text-gray-800 dark:text-white">Ayarlar</h1>
      <p class="text-gray-600 dark:text-gray-400 text-sm">Sistem ve kullanıcı ayarlarını yönetin</p>
    </div>

    <!-- Yükleniyor -->
    <div v-if="isLoading" class="flex justify-center items-center h-64">
      <div
        class="w-16 h-16 border-4 border-sky-600 border-t-transparent rounded-full animate-spin"
      ></div>
    </div>

    <!-- İçerik -->
    <div v-else>
      <!-- Başarı Mesajı -->
      <div
        v-if="savedMessage"
        class="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded relative mb-6"
      >
        <span class="block sm:inline">{{ savedMessage }}</span>
      </div>

      <!-- Sekmeler -->
      <div class="mb-6 border-b border-gray-200 dark:border-gray-700">
        <ul class="flex flex-wrap -mb-px">
          <li class="mr-2">
            <button
              @click="setActiveTab('genel')"
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="
                activeTab === 'genel'
                  ? 'text-sky-600 border-sky-600'
                  : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
              "
            >
              <i class="fas fa-cog mr-2"></i> Genel Ayarlar
            </button>
          </li>
          <li class="mr-2">
            <button
              @click="setActiveTab('kullanici')"
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="
                activeTab === 'kullanici'
                  ? 'text-sky-600 border-sky-600'
                  : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
              "
            >
              <i class="fas fa-user mr-2"></i> Kullanıcı Ayarları
            </button>
          </li>
          <li class="mr-2">
            <button
              @click="setActiveTab('guvenlik')"
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="
                activeTab === 'guvenlik'
                  ? 'text-sky-600 border-sky-600'
                  : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
              "
            >
              <i class="fas fa-shield-alt mr-2"></i> Güvenlik
            </button>
          </li>
          <li class="mr-2">
            <button
              @click="setActiveTab('bildirimler')"
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="
                activeTab === 'bildirimler'
                  ? 'text-sky-600 border-sky-600'
                  : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
              "
            >
              <i class="fas fa-bell mr-2"></i> Bildirimler
            </button>
          </li>
        </ul>
      </div>

      <!-- Sekme İçeriği -->
      <div class="bg-white dark:bg-neutral-800 rounded-lg shadow p-6">
        <!-- Genel Ayarlar -->
        <div v-if="activeTab === 'genel'" class="space-y-6">
          <h2 class="text-xl font-semibold text-gray-800 dark:text-white">Genel Ayarlar</h2>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Şirket Adı</label
              >
              <input
                v-model="systemSettings.companyName"
                type="text"
                class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500 dark:bg-neutral-700 dark:text-white"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >E-posta Adresi</label
              >
              <input
                v-model="systemSettings.email"
                type="email"
                class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500 dark:bg-neutral-700 dark:text-white"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Telefon</label
              >
              <input
                v-model="systemSettings.phone"
                type="text"
                class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500 dark:bg-neutral-700 dark:text-white"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Adres</label
              >
              <input
                v-model="systemSettings.address"
                type="text"
                class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500 dark:bg-neutral-700 dark:text-white"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Varsayılan Dil</label
              >
              <select
                v-model="systemSettings.defaultLanguage"
                class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500 dark:bg-neutral-700 dark:text-white"
              >
                <option value="tr">Türkçe</option>
                <option value="en">İngilizce</option>
                <option value="de">Almanca</option>
                <option value="fr">Fransızca</option>
              </select>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Tarih Formatı</label
              >
              <select
                v-model="systemSettings.dateFormat"
                class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500 dark:bg-neutral-700 dark:text-white"
              >
                <option value="DD.MM.YYYY">DD.MM.YYYY</option>
                <option value="MM/DD/YYYY">MM/DD/YYYY</option>
                <option value="YYYY-MM-DD">YYYY-MM-DD</option>
              </select>
            </div>
          </div>
        </div>

        <!-- Kullanıcı Ayarları -->
        <div v-if="activeTab === 'kullanici'" class="space-y-6">
          <h2 class="text-xl font-semibold text-gray-800 dark:text-white">Kullanıcı Ayarları</h2>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Ad Soyad</label
              >
              <input
                v-model="userSettings.name"
                type="text"
                class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500 dark:bg-neutral-700 dark:text-white"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >E-posta</label
              >
              <input
                v-model="userSettings.email"
                type="email"
                class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500 dark:bg-neutral-700 dark:text-white"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Dil</label
              >
              <select
                v-model="userSettings.language"
                class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500 dark:bg-neutral-700 dark:text-white"
              >
                <option value="tr">Türkçe</option>
                <option value="en">İngilizce</option>
                <option value="de">Almanca</option>
                <option value="fr">Fransızca</option>
              </select>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Tema</label
              >
              <div class="mt-2">
                <label class="inline-flex items-center">
                  <input
                    v-model="userSettings.darkMode"
                    type="checkbox"
                    class="rounded border-gray-300 text-sky-600 shadow-sm focus:border-sky-300 focus:ring focus:ring-sky-200 focus:ring-opacity-50"
                  />
                  <span class="ml-2 text-gray-700 dark:text-gray-300">Karanlık Mod</span>
                </label>
              </div>
            </div>
          </div>
        </div>

        <!-- Güvenlik Ayarları -->
        <div v-if="activeTab === 'guvenlik'" class="space-y-6">
          <h2 class="text-xl font-semibold text-gray-800 dark:text-white">Güvenlik Ayarları</h2>

          <div class="space-y-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >İki Faktörlü Doğrulama</label
              >
              <div class="mt-2">
                <label class="inline-flex items-center">
                  <input
                    v-model="userSettings.security.twoFactorAuth"
                    type="checkbox"
                    class="rounded border-gray-300 text-sky-600 shadow-sm focus:border-sky-300 focus:ring focus:ring-sky-200 focus:ring-opacity-50"
                  />
                  <span class="ml-2 text-gray-700 dark:text-gray-300"
                    >İki faktörlü doğrulamayı etkinleştir</span
                  >
                </label>
              </div>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Oturum Zaman Aşımı (dakika)</label
              >
              <input
                v-model="userSettings.security.sessionTimeout"
                type="number"
                min="5"
                max="120"
                class="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md shadow-sm focus:outline-none focus:ring-sky-500 focus:border-sky-500 dark:bg-neutral-700 dark:text-white"
              />
            </div>

            <div>
              <button class="bg-sky-600 hover:bg-sky-700 text-white font-bold py-2 px-4 rounded">
                Şifremi Değiştir
              </button>
            </div>
          </div>
        </div>

        <!-- Bildirim Ayarları -->
        <div v-if="activeTab === 'bildirimler'" class="space-y-6">
          <h2 class="text-xl font-semibold text-gray-800 dark:text-white">Bildirim Ayarları</h2>

          <div class="space-y-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >E-posta Bildirimleri</label
              >
              <div class="mt-2">
                <label class="inline-flex items-center">
                  <input
                    v-model="userSettings.notifications.email"
                    type="checkbox"
                    class="rounded border-gray-300 text-sky-600 shadow-sm focus:border-sky-300 focus:ring focus:ring-sky-200 focus:ring-opacity-50"
                  />
                  <span class="ml-2 text-gray-700 dark:text-gray-300"
                    >E-posta bildirimlerini etkinleştir</span
                  >
                </label>
              </div>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Tarayıcı Bildirimleri</label
              >
              <div class="mt-2">
                <label class="inline-flex items-center">
                  <input
                    v-model="userSettings.notifications.browser"
                    type="checkbox"
                    class="rounded border-gray-300 text-sky-600 shadow-sm focus:border-sky-300 focus:ring focus:ring-sky-200 focus:ring-opacity-50"
                  />
                  <span class="ml-2 text-gray-700 dark:text-gray-300"
                    >Tarayıcı bildirimlerini etkinleştir</span
                  >
                </label>
              </div>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Mobil Bildirimleri</label
              >
              <div class="mt-2">
                <label class="inline-flex items-center">
                  <input
                    v-model="userSettings.notifications.mobile"
                    type="checkbox"
                    class="rounded border-gray-300 text-sky-600 shadow-sm focus:border-sky-300 focus:ring focus:ring-sky-200 focus:ring-opacity-50"
                  />
                  <span class="ml-2 text-gray-700 dark:text-gray-300"
                    >Mobil bildirimlerini etkinleştir</span
                  >
                </label>
              </div>
            </div>
          </div>
        </div>
        <!-- İzin kurallar sekme -->
        <IzinKuralComponent />
        <!-- <div v-if="activeTab === 'izinkurallar'" class="space-y-6"></div> -->

        <!-- Kaydet Düğmesi -->
        <div class="mt-8 flex justify-end">
          <button
            @click="saveSettings"
            class="bg-sky-600 hover:bg-sky-700 text-white font-bold py-2 px-6 rounded flex items-center"
          >
            <i class="fas fa-save mr-2"></i> Kaydet
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.animate-spin {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
