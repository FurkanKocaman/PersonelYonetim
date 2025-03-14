<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();

// Hızlı erişim butonları için veriler
const quickAccessButtons = ref([
  { 
    id: 1, 
    title: "Personel Ekle", 
    icon: "user-plus", 
    color: "bg-blue-500 hover:bg-blue-600",
    route: "/dashboard/personel"
  },
  {
    id: 2,
    title: "Bordro Oluştur",
    icon: "file-invoice-dollar",
    color: "bg-green-500 hover:bg-green-600",
    route: "/dashboard"
  },
  {
    id: 3,
    title: "İzin Talebi",
    icon: "calendar-plus",
    color: "bg-purple-500 hover:bg-purple-600",
    route: "/dashboard/izin/talep"
  },
  { 
    id: 4, 
    title: "Rapor Oluştur", 
    icon: "chart-line", 
    color: "bg-amber-500 hover:bg-amber-600",
    route: "/dashboard"
  },
  { 
    id: 5, 
    title: "Mesai Kaydet", 
    icon: "clock", 
    color: "bg-red-500 hover:bg-red-600",
    route: "/dashboard"
  },
  {
    id: 6,
    title: "İzin Yönetimi",
    icon: "calendar-alt",
    color: "bg-indigo-500 hover:bg-indigo-600",
    route: "/dashboard/izin"
  }
]);

// Butona tıklandığında yönlendirme
const navigateTo = (route: string) => {
  router.push(route);
};
</script>

<template>
  <div class="mt-6">
    <div class="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-4">
      <div
        v-for="button in quickAccessButtons"
        :key="button.id"
        :class="`${button.color} rounded-lg shadow-md overflow-hidden hover-button cursor-pointer`"
        @click="navigateTo(button.route)"
      >
        <div class="p-4 flex flex-col items-center justify-center text-white h-full min-h-[100px]">
          <div class="text-2xl mb-2">
            <i :class="`fas fa-${button.icon}`"></i>
          </div>
          <span class="text-sm font-medium text-center">{{ button.title }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Buton hover efekti */
.hover-button {
  transition: all 0.3s ease;
}

.hover-button:hover {
  transform: translateY(-3px);
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
}
</style>
