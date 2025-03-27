<script setup lang="ts">
import { ref } from "vue";
import PayrollTable from "../../components/dashboard/PayrollTable.vue";
import AnnouncementsCard from "../../components/dashboard/AnnouncementsCard.vue";
import QuickAccessButtons from "../../components/dashboard/QuickAccessButtons.vue";
import StatisticsCard from "@/components/dashboard/StatisticsCard.vue";

const payrollItems = ref([
  { id: 1, name: "Didem Deniz", department: "Pazarlama", status: "Onaylandı", date: "15.03.2025" },
  { id: 2, name: "Mehmet Kaya", department: "Finans", status: "Beklemede", date: "16.03.2025" },
  { id: 3, name: "Ayşe Demir", department: "İK", status: "Onaylandı", date: "14.03.2025" },
  { id: 4, name: "Can Yılmaz", department: "Satış", status: "İşleniyor", date: "17.03.2025" },
]);

// İstatistikler için örnek veriler
const stats = ref([
  { title: "Toplam Personel", value: 124, icon: "users", color: "bg-blue-500" },
  { title: "Aktif Projeler", value: 8, icon: "briefcase", color: "bg-green-500" },
  { title: "Bu Ay İzinler", value: 12, icon: "calendar", color: "bg-purple-500" },
  { title: "Bekleyen Onaylar", value: 5, icon: "clock", color: "bg-amber-500" },
]);

// Duyurular için örnek veriler
const announcements = ref([
  {
    id: 1,
    title: "Yeni Personel Yönetim Sistemi",
    date: "12.03.2025",
    content:
      "Yeni personel yönetim sistemimiz kullanıma açılmıştır. Tüm personelimize hayırlı olsun.",
  },
  {
    id: 2,
    title: "Yıllık İzin Planlaması",
    date: "10.03.2025",
    content: "Yaz dönemi izin planlamaları için son başvuru tarihi 30 Mart 2025'tir.",
  },
]);
</script>

<template>
  <!-- Ana İçerik Alanı -->
  <div class="flex-1 transition-all duration-300">
    <!-- Üst Başlık Alanı -->
    <header class="bg-white dark:bg-neutral-800 shadow-sm">
      <div class="py-4 px-6">
        <h1 class="text-2xl font-bold text-gray-800 dark:text-gray-200">Yönetim Paneli</h1>
      </div>
    </header>

    <!-- İçerik Alanı -->
    <main class="p-6">
      <!-- İstatistik Kartları -->
      <StatisticsCard :stats="stats" />

      <!-- Ana İçerik Grid -->
      <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <!-- Sol Kolon: Bordro İşlemleri -->
        <div class="lg:col-span-2">
          <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 mb-6">
            <div class="flex justify-between items-center mb-4">
              <h2 class="text-xl font-bold text-gray-800 dark:text-gray-200">Bordro İşlemleri</h2>
              <button
                class="text-sky-600 dark:text-sky-400 hover:text-sky-700 dark:hover:text-sky-300"
              >
                Tümünü Gör
              </button>
            </div>
            <PayrollTable :payrollItems="payrollItems" />
          </div>

          <!-- Hızlı Erişim Butonları -->
          <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6">
            <h2 class="text-xl font-bold text-gray-800 dark:text-gray-200 mb-4">Hızlı Erişim</h2>
            <QuickAccessButtons />
          </div>
        </div>

        <!-- Sağ Kolon: Duyurular -->
        <div class="lg:col-span-1">
          <AnnouncementsCard :announcements="announcements" />
        </div>
      </div>
    </main>
  </div>
</template>
