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

const filterOptions = ref<FilterOptions>({
  durum: "",
  izinTipi: "",
});

const izinDurumlari: IzinDurumu[] = ["Beklemede", "Onaylandı", "Reddedildi"];
const izinTurleri = ["Yıllık İzin", "Hastalık İzni", "Mazeret İzni"];
const statusColors: Record<IzinDurumu, string> = {
  Beklemede: "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300",
  Onaylandı: "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300",
  Reddedildi: "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300",
};

const route = useRoute();
const router = useRouter();

const activeTab = computed(() => {
  if (route.name === "Izinler") return "izinler";
  if (route.name === "IzinKurallar") return "izinkurallar";
  return "";
});

const filteredIzinList = computed(() => {
  return izinList.value.filter((izin) => {
    const durumMatch = !filterOptions.value.durum || izin.durum === filterOptions.value.durum;
    const tipMatch =
      !filterOptions.value.izinTipi || izin.izinTipi === filterOptions.value.izinTipi;
    return durumMatch && tipMatch;
  });
});

const isKurallarRoute = computed(() => {
  return (
    route.name === "IzinKurallar" ||
    route.name === "IzinKurallariKurallar" ||
    route.name === "IzinKurallariRaporlar" ||
    route.name === "IzinKurallariOrnekSablonlar"
  );
});

const viewIzinDetails = (izin: IzinItem) => {
  selectedIzin.value = izin;
  showDetailModal.value = true;
};

const editIzin = (izin: IzinItem) => {
  console.log("Edit izin:", izin);
};

const closeDetailModal = () => {
  showDetailModal.value = false;
  selectedIzin.value = null;
};

const goToRules = () => {
  router.push({ name: "IzinKurallari" });
};

const createLeaveRequest = () => {
  router.push({ name: "IzinTalep" });
};

onMounted(() => {
  console.log(activeTab.value);
});
const closeIzinTalep = (res: boolean) => {
  izinTalepCreate.value = res;
};

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
          <RouterLink
            :to="{ name: 'Izinler' }"
            class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
            :class="
              route.name === 'Izinler'
                ? 'text-sky-600 border-sky-600'
                : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
            "
          >
            <i class="fas fa-calendar-alt mr-2"></i> İzin Talepleri
          </RouterLink>
        </li>
        <li class="mr-2">
          <Router-link
            :to="{ name: 'IzinKurallar' }"
            class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg cursor-pointer"
            :class="
              isKurallarRoute
                ? 'text-sky-600 border-sky-600'
                : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'
            "
          >
            <i class="fas fa-book mr-2"></i> İzin Kuralları
          </Router-link>
        </li>
      </ul>
      <div class="flex space-x-2">
        <button
          v-if="route.path == '/dashboard/izin/izinler'"
          @click="
            () => {
              izinTalepCreate = true;
            }
          "
          class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
        >
          <i class="fas fa-plus mr-2"></i> İzin Talebi Oluştur
        </button>
        <RouterLink
          v-if="route.path == '/dashboard/izin/izin-kurallar'"
          to="/dashboard/izin/kural-create"
          class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
        >
          <i class="fas fa-plus mr-2"></i> İzin Kural Oluştur
        </RouterLink>
      </div>
    </div>

    <RouterView></RouterView>
  </div>
  <IzinTalepCreateModal v-if="izinTalepCreate" @closeModal="closeIzinTalep" />
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
