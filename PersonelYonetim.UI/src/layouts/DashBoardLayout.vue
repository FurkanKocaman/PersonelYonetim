<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import SidebarMenu from "../components/dashboard/SidebarMenu.vue";
import { RouterView } from "vue-router";

// Yönlendirme için router tanımlama
const router = useRouter();
const route = useRoute();

// Kullanıcı bilgileri için örnek veri
const user = ref({
  name: "Ahmet Yılmaz",
  role: "İnsan Kaynakları Yöneticisi",
  avatar: "https://randomuser.me/api/portraits/men/32.jpg",
});

// Sidebar menüsü için örnek veriler
const menuItems = ref([
  { name: "Ana Sayfa", icon: "home", active: false, path: "/dashboard" },
  { name: "Personel", icon: "users", active: false, path: "/dashboard/personel" },
  { name: "Bordro", icon: "file-invoice-dollar", active: false, path: "/bordro" },
  { name: "İzin Yönetimi", icon: "calendar-alt", active: false, path: "/izin" },
  { name: "Raporlar", icon: "chart-bar", active: false, path: "/raporlar" },
  { name: "Ayarlar", icon: "cog", active: false, path: "/ayarlar" },
]);
interface MenuItem {
  name: string;
  icon: string;
  active: boolean;
  path: string;
}

// Sayfa yükleme durumu
const isPageLoaded = ref(false);

// Sidebar durumu
const sidebarOpen = ref(true);

// Sayfa yüklendiğinde
onMounted(() => {
  // Sayfa yüklendi olarak işaretle
  setTimeout(() => {
    isPageLoaded.value = true;
  }, 100);

  menuItems.value
    .filter((item) => item.path === route.path || item.path + "/" === route.path)
    .forEach((item) => {
      item.active = true;
    });
  console.log(route.path);
});

// Sidebar toggle fonksiyonu
const toggleSidebar = () => {
  sidebarOpen.value = !sidebarOpen.value;
};

// Menü öğesi tıklama işlevi
const handleMenuClick = (item: MenuItem) => {
  // Tüm menü öğelerini pasif yap
  menuItems.value.forEach((menuItem) => {
    menuItem.active = false;
  });

  // Tıklanan öğeyi aktif yap
  item.active = true;

  // Yönlendirme yap
  router.push(item.path);
};
</script>

<template>
  <div
    class="flex h-screen overflow-hidden transition-all duration-100"
    :class="{ 'opacity-100': isPageLoaded, 'opacity-0': !isPageLoaded }"
  >
    <!-- Sidebar Bileşeni -->
    <SidebarMenu
      :menuItems="menuItems"
      :user="user"
      :sidebarOpen="sidebarOpen"
      @menu-click="handleMenuClick"
      @toggle-sidebar="toggleSidebar"
    />

    <!-- Ana İçerik Alanı -->
    <RouterView class="overflow-auto"></RouterView>
  </div>
</template>
