<script setup lang="ts">
import IzinTalepCreateModal from "@/components/modals/IzinTalepCreateModal.vue";
import { onMounted, computed, ref } from "vue";
import { useRoute, useRouter } from "vue-router";

// Define valid status types
type IzinDurumu = "Beklemede" | "Onaylandı" | "Reddedildi";

interface IzinItem {
  id: number;
  personelAdi: string;
  baslangicTarihi: string;
  bitisTarihi: string;
  izinTipi: string;
  durum: IzinDurumu;
  aciklama?: string; // Optional field for leave description
}

interface FilterOptions {
  durum: string;
  izinTipi: string;
}

const izinTalepCreate = ref(false);

// Component state
const loading = ref(false);
const error = ref<string | null>(null);
const showDetailModal = ref(false);
const selectedIzin = ref<IzinItem | null>(null);
const izinList = ref<IzinItem[]>([]);

// Filter options
const filterOptions = ref<FilterOptions>({
  durum: "",
  izinTipi: "",
});

// Static data
const izinDurumlari: IzinDurumu[] = ["Beklemede", "Onaylandı", "Reddedildi"];
const izinTurleri = ["Yıllık İzin", "Hastalık İzni", "Mazeret İzni"];
const statusColors: Record<IzinDurumu, string> = {
  Beklemede: "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300",
  Onaylandı: "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300",
  Reddedildi: "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300",
};

// Route and active tab
const route = useRoute();
const router = useRouter();

const activeTab = computed(() => {
  if (route.name === 'Izinler') return "izinler";
  if (route.name === 'IzinKurallar') return "izinkurallar";
  return "";
});

// Computed properties
const filteredIzinList = computed(() => {
  return izinList.value.filter((izin) => {
    const durumMatch = !filterOptions.value.durum || izin.durum === filterOptions.value.durum;
    const tipMatch =
      !filterOptions.value.izinTipi || izin.izinTipi === filterOptions.value.izinTipi;
    return durumMatch && tipMatch;
  });
});

// Kurallar sayfasına ait mi kontrolü
const isKurallarRoute = computed(() => {
  return route.name === 'IzinKurallari' ||
         route.name === 'IzinKurallariKurallar' ||
         route.name === 'IzinKurallariRaporlar' ||
         route.name === 'IzinKurallariOrnekSablonlar';
});

// Methods
const viewIzinDetails = (izin: IzinItem) => {
  selectedIzin.value = izin;
  showDetailModal.value = true;
};

const editIzin = (izin: IzinItem) => {
  // Implement edit functionality
  console.log("Edit izin:", izin);
};

const closeDetailModal = () => {
  showDetailModal.value = false;
  selectedIzin.value = null;
};

// Kurallar sayfasına yönlendir
const goToRules = () => {
  router.push({ name: 'IzinKurallari' });
};

// İzin talebi oluştur
const createLeaveRequest = () => {
  router.push({ name: 'IzinTalep' });
};

onMounted(() => {
  // Implement API call to fetch izin list
  console.log(activeTab.value);
});

// Expose necessary variables to template
defineExpose({
  loading,
  error,
  showDetailModal,
  selectedIzin,
  izinList,
  filterOptions,
  izinDurumlari,
  izinTurleri,
  statusColors,
  activeTab,
  filteredIzinList,
  viewIzinDetails,
  editIzin,
  closeDetailModal,
  goToRules,
  createLeaveRequest,
  isKurallarRoute,
});
</script>

<template>
  <div class="flex-1 transition-all duration-300">
    <div
      class="mb-6 mt-2 border-b border-gray-200 dark:border-gray-700 flex justify-between items-center"
    >
      <ul class="flex flex-wrap -mb-px">
        <li class="mr-2">
          <router-link
            :to="{ name: 'Izinler' }"
            class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
            :class="
              route.name === 'Izinler'
                ? 'text-sky-600 border-sky-600'
                : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
            "
          >
            <i class="fas fa-calendar-alt mr-2"></i> İzin Talepleri
          </router-link>
        </li>
        <li class="mr-2">
          <a
            @click="goToRules"
            class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
            :class="
              isKurallarRoute
                ? 'text-sky-600 border-sky-600'
                : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
            "
          >
            <i class="fas fa-book mr-2"></i> İzin Kuralları
          </a>
        </li>
      </ul>
      <div class="flex space-x-2">
        <button
          @click="createLeaveRequest"
          class="cursor-pointer bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 transition-colors duration-300"
        >
          <i class="fas fa-plus mr-2"></i> İzin Talebi Oluştur
        </button>
      </div>
    </div>

    <RouterView></RouterView>
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
