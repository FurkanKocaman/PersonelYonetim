<script setup lang="ts">
import { onMounted, ref, type Ref } from "vue";
import AnnouncementsCard from "../../components/dashboard/AnnouncementsCard.vue";
import QuickAccessButtons from "../../components/dashboard/QuickAccessButtons.vue";
// import StatisticsCard from "@/components/dashboard/StatisticsCard.vue";
import DuyuruService from "@/services/DuyuruService";
import type { DuyuruModel } from "@/models/entity-models/DuyuruModel";
import PersonelService from "@/services/PersonelService";
import { useUserStore } from "@/stores/user";
import IzinBilgilerimCard from "@/components/dashboard/IzinBilgilerimCard.vue";
import PersonelCard from "@/components/dashboard/PersonelCard.vue";

const duyurular: Ref<DuyuruModel[]> = ref([]);
const personelCount = ref(0);

onMounted(() => {
  getDuyurular();
  getPersoneller();
});

const getDuyurular = async () => {
  const res = await DuyuruService.getDuyurular();
  duyurular.value = res!.items;
};

const getPersoneller = async () => {
  const response = await PersonelService.getPersonelList(
    useUserStore().user.sirketId,
    undefined,
    undefined
  );
  if (response) {
    personelCount.value = response.count;
  }
};

// const stats = computed(() => [
//   { title: "Toplam Personel", value: personelCount.value, icon: "users", color: "bg-blue-500" },
//   { title: "Aktif Projeler", value: 8, icon: "briefcase", color: "bg-green-500" },
//   { title: "Bu Ay İzinler", value: 12, icon: "calendar", color: "bg-purple-500" },
//   { title: "Bekleyen Onaylar", value: 5, icon: "clock", color: "bg-amber-500" },
// ]);
</script>

<template>
  <!-- Ana İçerik Alanı -->
  <div class="flex-1 transition-all duration-300">
    <!-- Üst Başlık Alanı -->
    <!-- <header class="bg-white dark:bg-neutral-800 shadow-sm">
      <div class="py-4 px-6">
        <h1 class="text-2xl font-bold text-gray-800 dark:text-gray-200">Yönetim Paneli</h1>
      </div>
    </header> -->

    <main class="p-6">
      <!-- <StatisticsCard :stats="stats" /> -->

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div class="lg:col-span-2">
          <PersonelCard />
          <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 mb-6">
            <div class="flex justify-between items-center mb-4">
              <h2 class="text-xl font-bold text-gray-800 dark:text-gray-200">İzin Bilgilerim</h2>
              <RouterLink
                to="/dashboard/profile/izinlerim"
                class="text-sky-600 dark:text-sky-400 hover:text-sky-700 dark:hover:text-sky-300"
              >
                Tümünü Gör
              </RouterLink>
            </div>

            <IzinBilgilerimCard />
          </div>

          <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6">
            <h2 class="text-xl font-bold text-gray-800 dark:text-gray-200 mb-4">Hızlı Erişim</h2>
            <QuickAccessButtons />
          </div>
        </div>

        <div class="lg:col-span-1">
          <AnnouncementsCard :duyurular="duyurular" />
        </div>
      </div>
    </main>
  </div>
</template>
