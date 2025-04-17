<script setup lang="ts">
import { useThemeStore } from "@/stores/ThemeStore";
import { defineEmits, onMounted, ref, computed, type Ref } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useUserStore } from "@/stores/user";
import type { BildirimModel } from "@/models/entity-models/BildirimModel";
import BildirimService from "@/services/BildirimService";

const route = useRoute();
const activeTab = computed(() => {
  if (route.path.includes("/sirket")) return "Sirket Yönetimi";
  if (route.path.includes("/personel")) return "Personel Yönetimi";
  if (route.path.includes("/izin")) return "İzin Yönetimi";
  if (route.path.includes("/maas")) return "Maaş Yönetimi";
  if (route.path.includes("/takvim")) return "Takvim";
  if (route.path.includes("/ayarlar")) return "Ayarlar";
  if (route.path.includes("/profile")) return "Profil";
  if (route.path.includes("/dashboard")) return "Ana Sayfa";
  return "";
});

const router = useRouter();
const themeStore = useThemeStore();
const userStore = useUserStore();
const apiUrl = import.meta.env.VITE_API_URL;

const bildirimler: Ref<BildirimModel[] | undefined> = ref([]);

// Dropdown durumları
const notificationsOpen = ref(false);
const messagesOpen = ref(false);
const userMenuOpen = ref(false);

onMounted(() => {
  document.addEventListener("click", closeDropdowns);

  getBildirimler();
});

const getBildirimler = async () => {
  const res = await BildirimService.getBildirimler();
  bildirimler.value = res;
};

// Event'ler
const emit = defineEmits(["toggle-sidebar"]);

const toggleSidebar = () => {
  emit("toggle-sidebar");
};

// Bildirimleri aç/kapat
const toggleNotifications = (event: Event) => {
  event.stopPropagation();
  notificationsOpen.value = !notificationsOpen.value;
  messagesOpen.value = false;
  userMenuOpen.value = false;
};

// Kullanıcı menüsünü aç/kapat
const toggleUserMenu = (event: Event) => {
  event.stopPropagation();
  userMenuOpen.value = !userMenuOpen.value;
  notificationsOpen.value = false;
  messagesOpen.value = false;
};

// Tüm açılır menüleri kapat
const closeDropdowns = (event: Event) => {
  const target = event.target as HTMLElement;
  if (!target.closest(".dropdown-container")) {
    notificationsOpen.value = false;
    messagesOpen.value = false;
    userMenuOpen.value = false;
  }
};

const markNotificationAsRead = async (id: string) => {
  const bildirim = bildirimler.value!.find((n) => n.id == id);
  if (bildirim) {
    const res = await BildirimService.setBildirimOkundu(id);
    console.log("Bildirim okundu", res);
    bildirim.okunduMu = true;
  }
};

// Çıkış yap
const logout = () => {
  userStore.logout();
  router.push("/login");
};
</script>

<template>
  <header class="w-full bg-white dark:bg-neutral-800/60 shadow-sm h-16 flex items-center">
    <div class="container mx-auto px-4 flex justify-between items-center">
      <!-- Sol taraf - Mobil için açma/kapama düğmesi -->
      <div class="flex items-center">
        <button
          @click="toggleSidebar"
          class="xl:hidden p-2 rounded-md text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 focus:outline-none"
        >
          <i class="fas fa-bars"></i>
        </button>
        <span class="ml-2 text-gray-700 dark:text-gray-200 font-medium text-xl">{{
          activeTab
        }}</span>
      </div>

      <!-- Sağ taraf - Kullanıcı işlemleri -->
      <div class="flex items-center space-x-4">
        <!-- Tema değiştirme -->
        <div class="cursor-pointer">
          <svg
            v-if="themeStore.theme == 'light'"
            class="size-7 fill-none"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
            v-on:click="themeStore.toggleTheme()"
          >
            <path
              d="M3.32031 11.6835C3.32031 16.6541 7.34975 20.6835 12.3203 20.6835C16.1075 20.6835 19.3483 18.3443 20.6768 15.032C19.6402 15.4486 18.5059 15.6834 17.3203 15.6834C12.3497 15.6834 8.32031 11.654 8.32031 6.68342C8.32031 5.50338 8.55165 4.36259 8.96453 3.32996C5.65605 4.66028 3.32031 7.89912 3.32031 11.6835Z"
              class="stroke-black stroke-2"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
          </svg>
          <svg
            v-if="themeStore.theme == 'dark'"
            class="size-7 fill-none"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
            v-on:click="themeStore.toggleTheme()"
          >
            <path
              d="M12 3V4M12 20V21M4 12H3M6.31412 6.31412L5.5 5.5M17.6859 6.31412L18.5 5.5M6.31412 17.69L5.5 18.5001M17.6859 17.69L18.5 18.5001M21 12H20M16 12C16 14.2091 14.2091 16 12 16C9.79086 16 8 14.2091 8 12C8 9.79086 9.79086 8 12 8C14.2091 8 16 9.79086 16 12Z"
              class="stroke-white stroke-2"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
          </svg>
        </div>

        <!-- Bildirimler -->
        <div class="relative dropdown-container">
          <button
            @click="toggleNotifications"
            class="p-2 rounded-full text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 focus:outline-none"
          >
            <i class="fas fa-bell"></i>
            <span
              v-if="bildirimler!.filter((n) => !n.okunduMu).length > 0"
              class="absolute top-0 right-0 bg-red-500 text-white rounded-full w-4 h-4 flex items-center justify-center text-xs"
              >{{ bildirimler!.filter((n) => !n.okunduMu).length }}</span
            >
          </button>

          <!-- Bildirimler Dropdown -->
          <div
            v-if="notificationsOpen"
            class="absolute right-0 mt-2 w-80 bg-white dark:bg-neutral-800 rounded-lg shadow-xl z-10 overflow-hidden border border-gray-200 dark:border-neutral-700 animate-fadeIn"
          >
            <div
              class="p-3 border-b border-gray-200 dark:border-neutral-700 flex justify-between items-center"
            >
              <h3 class="text-sm font-semibold text-gray-700 dark:text-gray-200">Bildirimler</h3>
              <span
                class="text-xs px-2 py-1 bg-blue-100 dark:bg-blue-900/30 text-blue-800 dark:text-blue-300 rounded-full"
              >
                {{ bildirimler!.filter((n) => !n.okunduMu).length }} yeni
              </span>
            </div>
            <div class="max-h-96 overflow-y-auto">
              <div
                v-for="bildirim in bildirimler"
                :key="bildirim.id"
                @click="markNotificationAsRead(bildirim.id)"
                class="p-3 border-b border-gray-200 dark:border-neutral-700 hover:bg-gray-50 dark:hover:bg-neutral-700 cursor-pointer transition-colors duration-150"
                :class="{ 'bg-blue-50 dark:bg-blue-900/20': !bildirim.okunduMu }"
              >
                <div class="flex items-start">
                  <div class="flex-shrink-0 mr-3">
                    <div
                      class="w-10 h-10 rounded-full bg-blue-100 dark:bg-blue-900/30 flex items-center justify-center"
                    >
                      <i class="fas fa-bell text-blue-500 dark:text-blue-400"></i>
                    </div>
                  </div>
                  <div class="flex-1">
                    <p class="text-sm font-medium text-gray-800 dark:text-gray-200">
                      {{ bildirim.baslik }}
                    </p>
                    <p class="text-xs text-gray-600 dark:text-gray-400 mt-1">
                      {{ bildirim.aciklama }}
                    </p>
                    <div class="flex items-center mt-1">
                      <i class="fas fa-clock text-xs text-gray-500 dark:text-gray-500 mr-1"></i>
                      <p class="text-xs text-gray-500 dark:text-gray-500">
                        {{
                          new Date(bildirim.createdAt).toLocaleDateString("tr-TR", {
                            day: "2-digit",
                            month: "2-digit",
                            year: "numeric",
                            hour: "2-digit",
                            minute: "2-digit",
                            hour12: false,
                          })
                        }}
                      </p>
                    </div>
                  </div>
                  <div v-if="!bildirim.okunduMu" class="w-2 h-2 bg-blue-500 rounded-full"></div>
                </div>
              </div>
              <div
                v-if="bildirimler!.length === 0"
                class="p-3 text-center text-gray-500 dark:text-gray-400"
              >
                Bildirim bulunmuyor
              </div>
            </div>
            <div
              class="p-2 border-t border-gray-200 dark:border-neutral-700 text-center bg-gray-50 dark:bg-neutral-700/50"
            >
              <button class="text-xs text-blue-600 dark:text-blue-400 hover:underline font-medium">
                Tümünü Gör
              </button>
            </div>
          </div>
        </div>

        <!-- Kullanıcı menüsü -->
        <div class="relative dropdown-container z-20">
          <button
            @click="toggleUserMenu"
            class="flex items-center justify-center space-x-2 text-gray-600 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 rounded-full p-1 focus:outline-none"
          >
            <img
              v-if="userStore.user?.avatarUrl"
              :src="apiUrl + userStore.user?.avatarUrl"
              alt="Kullanıcı"
              class="w-8 h-8 rounded-full border border-gray-200 dark:border-neutral-600"
            />
            <div
              v-else
              class="text-3xl font-semibold text-sky-600 transition-all duration-300 ease-in-out rounded-full border-1 border-sky-500 w-10 h-10 flex items-center justify-center"
            >
              {{ userStore.user.ad[0] }}
            </div>
          </button>

          <!-- Kullanıcı Menüsü Dropdown -->
          <div
            v-if="userMenuOpen"
            class="absolute right-0 mt-2 w-60 bg-white dark:bg-neutral-800 rounded-lg shadow-xl z-10 overflow-hidden border border-gray-200 dark:border-neutral-700 animate-fadeIn"
          >
            <div class="p-4 border-b border-gray-200 dark:border-neutral-700 text-center">
              <div class="relative mx-auto w-16 h-16 mb-3">
                <img
                  v-if="userStore.user?.avatarUrl"
                  :src="apiUrl + userStore.user?.avatarUrl"
                  alt="Kullanıcı"
                  class="w-16 h-16 rounded-full border-2 border-white dark:border-neutral-600 shadow-md object-cover"
                />
                <div
                  v-else
                  class="text-4xl font-semibold text-sky-600 transition-all duration-300 ease-in-out rounded-full border-1 border-sky-500 w-16 h-16 flex items-center justify-center"
                >
                  {{ userStore.user.ad[0] }}
                </div>

                <div
                  class="absolute bottom-0 right-0 w-4 h-4 bg-green-500 rounded-full border-2 border-white dark:border-neutral-800"
                ></div>
              </div>
              <h3 class="text-sm font-semibold text-gray-800 dark:text-gray-200">
                {{ userStore.user?.ad + " " + userStore.user?.soyad || "Kullanıcı" }}
              </h3>
              <p class="text-xs text-gray-500 dark:text-gray-400 mt-1">
                {{ userStore.user?.iletisim.eposta || "kullanici@ornek.com" }}
              </p>
              <div
                class="mt-2 text-xs bg-blue-100 dark:bg-blue-900/30 text-blue-800 dark:text-blue-300 py-1 px-2 rounded-full inline-block"
              >
                {{ userStore.user.pozisyonAd }}
              </div>
            </div>
            <div class="py-1">
              <router-link
                to="/dashboard/profile"
                class="flex items-center px-4 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 transition-colors duration-150"
              >
                <i
                  class="fas fa-user-cog mr-3 text-gray-500 dark:text-gray-400 w-5 text-center"
                ></i>
                Profil Ayarları
              </router-link>
              <router-link
                to="/dashboard/profile/izinlerim"
                class="flex items-center px-4 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 transition-colors duration-150"
              >
                <i
                  class="fas fa-calendar-alt mr-3 text-gray-500 dark:text-gray-400 w-5 text-center"
                ></i>
                İzin Talebi
              </router-link>
              <router-link
                to="/dashboard/takvim"
                class="flex items-center px-4 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-neutral-700 transition-colors duration-150"
              >
                <i
                  class="fas fa-calendar-week mr-3 text-gray-500 dark:text-gray-400 w-5 text-center"
                ></i>
                Takvim
              </router-link>
            </div>
            <div class="border-t border-gray-200 dark:border-neutral-700 py-1">
              <button
                @click="logout"
                class="flex items-center w-full px-4 py-2 text-sm text-red-600 dark:text-red-400 hover:bg-gray-100 dark:hover:bg-neutral-700 transition-colors duration-150"
              >
                <i class="fas fa-sign-out-alt mr-3 w-5 text-center"></i> Çıkış Yap
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </header>
</template>

<style scoped>
.animate-fadeIn {
  animation: fadeIn 0.2s ease-in-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  line-clamp: 2;
}
</style>
