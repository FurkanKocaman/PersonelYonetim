<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import SidebarMenu from '../components/dashboard/SidebarMenu.vue';
import StatisticsCard from '../components/dashboard/StatisticsCard.vue';
import PayrollTable from '../components/dashboard/PayrollTable.vue';
import AnnouncementsCard from '../components/dashboard/AnnouncementsCard.vue';
import QuickAccessButtons from '../components/dashboard/QuickAccessButtons.vue';

// Yönlendirme için router tanımlama
const router = useRouter();

// Kullanıcı bilgileri için örnek veri
const user = ref({
  name: 'Ahmet Yılmaz',
  role: 'İnsan Kaynakları Yöneticisi',
  avatar: 'https://randomuser.me/api/portraits/men/32.jpg'
});

// Bordro işlemleri için örnek veriler
const payrollItems = ref([
  { id: 1, name: 'Didem Deniz', department: 'Pazarlama', status: 'Onaylandı', date: '15.03.2025' },
  { id: 2, name: 'Mehmet Kaya', department: 'Finans', status: 'Beklemede', date: '16.03.2025' },
  { id: 3, name: 'Ayşe Demir', department: 'İK', status: 'Onaylandı', date: '14.03.2025' },
  { id: 4, name: 'Can Yılmaz', department: 'Satış', status: 'İşleniyor', date: '17.03.2025' }
]);

// İstatistikler için örnek veriler
const stats = ref([
  { title: 'Toplam Personel', value: 124, icon: 'users', color: 'bg-blue-500' },
  { title: 'Aktif Projeler', value: 8, icon: 'briefcase', color: 'bg-green-500' },
  { title: 'Bu Ay İzinler', value: 12, icon: 'calendar', color: 'bg-purple-500' },
  { title: 'Bekleyen Onaylar', value: 5, icon: 'clock', color: 'bg-amber-500' }
]);

// Duyurular için örnek veriler
const announcements = ref([
  { id: 1, title: 'Yeni Personel Yönetim Sistemi', date: '12.03.2025', content: 'Yeni personel yönetim sistemimiz kullanıma açılmıştır. Tüm personelimize hayırlı olsun.' },
  { id: 2, title: 'Yıllık İzin Planlaması', date: '10.03.2025', content: 'Yaz dönemi izin planlamaları için son başvuru tarihi 30 Mart 2025\'tir.' }
]);

// Sidebar menüsü için örnek veriler
const menuItems = ref([
  { name: 'Ana Sayfa', icon: 'home', active: true, path: '/dashboard' },
  { name: 'Personel', icon: 'users', active: false, path: '/personel' },
  { name: 'Bordro', icon: 'file-invoice-dollar', active: false, path: '/bordro' },
  { name: 'İzin Yönetimi', icon: 'calendar-alt', active: false, path: '/izin' },
  { name: 'Raporlar', icon: 'chart-bar', active: false, path: '/raporlar' },
  { name: 'Ayarlar', icon: 'cog', active: false, path: '/ayarlar' }
]);

// Sayfa yükleme durumu
const isPageLoaded = ref(false);

// Sidebar durumu
const sidebarOpen = ref(true);

// İçerik alanı sınıfı
const contentClass = ref('lg:ml-64');

// Sayfa yüklendiğinde
onMounted(() => {
  // Sayfa yüklendi olarak işaretle
  setTimeout(() => {
    isPageLoaded.value = true;
  }, 500);

  // Ekran boyutuna göre sidebar durumunu ayarla
  handleResize();

  // Ekran boyutu değiştiğinde sidebar durumunu güncelle
  window.addEventListener('resize', handleResize);
});

// Ekran boyutuna göre sidebar durumunu ayarla
const handleResize = () => {
  if (window.innerWidth < 1024) {
    sidebarOpen.value = false;
    contentClass.value = '';
  } else {
    sidebarOpen.value = true;
    contentClass.value = 'lg:ml-64';
  }
};

// Sidebar toggle fonksiyonu
const toggleSidebar = () => {
  sidebarOpen.value = !sidebarOpen.value;
  contentClass.value = sidebarOpen.value ? 'lg:ml-64' : 'lg:ml-16';
};

// Menü öğesi tıklama işlevi
const handleMenuClick = (item: any) => {
  // Tüm menü öğelerini pasif yap
  menuItems.value.forEach(menuItem => {
    menuItem.active = false;
  });

  // Tıklanan öğeyi aktif yap
  item.active = true;

  // Yönlendirme yap
  router.push(item.path);
};
</script>

<template>
  <div class="flex h-screen bg-gray-100 dark:bg-neutral-900 overflow-hidden transition-all duration-300"
       :class="{'opacity-100': isPageLoaded, 'opacity-0': !isPageLoaded}">
    
    <!-- Sidebar Bileşeni -->
    <SidebarMenu 
      :menuItems="menuItems" 
      :user="user" 
      :sidebarOpen="sidebarOpen"
      @menu-click="handleMenuClick"
      @toggle-sidebar="toggleSidebar"
    />
    
    <!-- Ana İçerik Alanı -->
    <div class="flex-1 overflow-x-hidden overflow-y-auto transition-all duration-300" :class="contentClass">
      <!-- Üst Başlık Alanı -->
      <header class="bg-white dark:bg-neutral-800 shadow-sm">
        <div class="py-4 px-6">
          <h1 class="text-2xl font-bold text-gray-800 dark:text-gray-200">Yönetim Paneli</h1>
        </div>
      </header>
      
      <!-- İçerik Alanı -->
      <main class="p-6">
        <!-- İstatistik Kartları -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-6">
          <!-- Each stat card is rendered individually for proper layout -->
          <div v-for="stat in stats" :key="stat.title" class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-4 flex items-center">
            <div class="rounded-full w-12 h-12 flex items-center justify-center mr-4" :class="stat.color">
              <i class="fas text-white text-xl" :class="'fa-' + stat.icon"></i>
            </div>
            <div>
              <h3 class="text-lg font-semibold text-gray-800 dark:text-gray-200">{{ stat.value }}</h3>
              <p class="text-sm text-gray-600 dark:text-gray-400">{{ stat.title }}</p>
            </div>
          </div>
        </div>
        
        <!-- Ana İçerik Grid -->
        <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <!-- Sol Kolon: Bordro İşlemleri -->
          <div class="lg:col-span-2">
            <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 mb-6">
              <div class="flex justify-between items-center mb-4">
                <h2 class="text-xl font-bold text-gray-800 dark:text-gray-200">Bordro İşlemleri</h2>
                <button class="text-sky-600 dark:text-sky-400 hover:text-sky-700 dark:hover:text-sky-300">
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
  </div>
</template>
