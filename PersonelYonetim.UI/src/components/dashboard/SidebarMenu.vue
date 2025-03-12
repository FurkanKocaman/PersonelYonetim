<script setup lang="ts">
import { defineProps, defineEmits, ref, onMounted } from 'vue';

// Menü öğesi için tip tanımı
interface MenuItem {
  name: string;
  icon: string;
  active: boolean;
  path: string;
}

// Kullanıcı için tip tanımı
interface User {
  name: string;
  role: string;
  avatar: string;
}

// Komponent prop'ları
const props = defineProps({
  menuItems: {
    type: Array as () => MenuItem[],
    required: true
  },
  user: {
    type: Object as () => User,
    required: true
  },
  sidebarOpen: {
    type: Boolean,
    default: true
  }
});

// Komponent event'leri
const emit = defineEmits(['menu-click', 'toggle-sidebar']);

// Menü öğesi tıklama işlevi
const handleMenuClick = (item: MenuItem) => {
  emit('menu-click', item);
};

// Sidebar toggle işlevi
const toggleSidebar = () => {
  emit('toggle-sidebar');
};

// Ekran genişliği için ref
const isMobile = ref(false);

// Komponent yüklendiğinde
onMounted(() => {
  // İlk yüklemede ekran genişliğini kontrol et
  checkScreenWidth();

  // Ekran boyutu değiştiğinde kontrol et
  window.addEventListener('resize', checkScreenWidth);
});

// Ekran genişliğini kontrol et
const checkScreenWidth = () => {
  isMobile.value = window.innerWidth < 1024;
};
</script>

<template>
  <!-- Sidebar - Mobil için kaydırılabilir, masaüstü için sabit -->
  <div>
    <!-- Mobil görünüm için overlay -->
    <div v-if="sidebarOpen && isMobile" class="lg:hidden fixed inset-0 z-20 bg-black bg-opacity-50 sidebar-overlay"
         @click="toggleSidebar"></div>
         
    <!-- Sidebar Toggle Button (Outside sidebar) -->
    <button 
      @click="toggleSidebar" 
      class="fixed z-50 flex items-center justify-center w-10 h-10 rounded-full bg-sky-600 dark:bg-sky-700 shadow-md text-white hover:bg-sky-700 dark:hover:bg-sky-800 transition-all duration-200"
      :class="{'left-[16rem]': sidebarOpen, 'left-[4rem]': !sidebarOpen}"
      style="top: 5rem;"
    >
      <div class="flex items-center justify-center">
        <i class="fas fa-bars text-lg"></i>
        <i v-if="sidebarOpen" class="fas fa-angle-left ml-1 text-lg"></i>
        <i v-else class="fas fa-angle-right ml-1 text-lg"></i>
      </div>
    </button>

    <!-- Sidebar Container -->
    <div 
      class="fixed inset-y-0 left-0 z-30 bg-white dark:bg-neutral-800 shadow-lg transition-all duration-300 transform h-screen overflow-hidden"
      :class="{
        'w-64': sidebarOpen,
        'w-16': !sidebarOpen,
        '-translate-x-full lg:translate-x-0': !sidebarOpen && isMobile,
        'translate-x-0': sidebarOpen || !isMobile
      }"
    >
      <div class="h-full flex flex-col overflow-y-auto overflow-x-hidden">
        <!-- Logo ve Başlık -->
        <div class="flex items-center justify-between h-16 bg-sky-600 dark:bg-sky-700 px-4">
          <span class="text-white text-xl font-bold whitespace-nowrap overflow-hidden transition-all duration-300"
                :class="{'opacity-100': sidebarOpen, 'opacity-0 w-0': !sidebarOpen}">
            Personel Yönetim
          </span>
          <!-- Logo Icon for Collapsed State -->
          <div class="flex items-center justify-center" :class="{'hidden': sidebarOpen, 'block': !sidebarOpen}">
            <i class="fas fa-users-cog text-white text-xl"></i>
          </div>
          <!-- Removed redundant toggle button as requested -->
        </div>

        <!-- Kullanıcı Profili -->
        <div class="flex flex-col items-center mt-6 -mx-2" :class="{'px-2': !sidebarOpen}">
          <img class="object-cover mx-2 rounded-full border-2 border-sky-500" 
               :class="{'w-16 h-16': sidebarOpen, 'w-10 h-10': !sidebarOpen}"
               :src="user.avatar" alt="Avatar">
          <div :class="{'hidden': !sidebarOpen}">
            <h4 class="mx-2 mt-2 font-medium text-gray-800 dark:text-gray-200">{{ user.name }}</h4>
            <p class="mx-2 mt-1 text-sm font-medium text-gray-600 dark:text-gray-400">{{ user.role }}</p>
          </div>
        </div>

        <!-- Menü Öğeleri -->
        <div class="mt-6 flex-grow">
          <div v-for="item in menuItems" :key="item.name"
               @click="handleMenuClick(item)"
               class="flex items-center mt-2 text-gray-700 dark:text-gray-300 rounded-lg cursor-pointer transition-colors duration-200 hover:bg-gray-100 dark:hover:bg-neutral-700 relative group"
               :class="{
                 'px-4 py-3 mx-3': sidebarOpen,
                 'px-0 py-3 justify-center mx-3': !sidebarOpen,
                 'bg-sky-100 dark:bg-sky-900 text-sky-600 dark:text-sky-400': item.active
               }">
            <!-- Icon Container - Always Visible -->
            <div class="w-8 h-8 flex items-center justify-center text-lg" 
                 :class="{'mr-3': sidebarOpen, 'mx-auto': !sidebarOpen}">
              <i :class="`fas fa-${item.icon}`"></i>
            </div>
            <!-- Text - Only Visible When Sidebar is Open -->
            <span class="font-medium transition-all duration-300 whitespace-nowrap overflow-hidden"
                  :class="{'opacity-100 w-auto': sidebarOpen, 'opacity-0 w-0': !sidebarOpen}">
              {{ item.name }}
            </span>
            <!-- Tooltip for Collapsed State -->
            <div v-if="!sidebarOpen" class="absolute left-16 bg-gray-800 text-white text-sm px-2 py-1 rounded opacity-0 group-hover:opacity-100 pointer-events-none transition-opacity duration-200 whitespace-nowrap z-50">
              {{ item.name }}
            </div>
          </div>
        </div>

        <!-- Çıkış Butonu -->
        <div class="w-full py-4">
          <div class="flex items-center text-gray-700 dark:text-gray-300 cursor-pointer hover:bg-gray-100 dark:hover:bg-neutral-700 relative group"
               :class="{'px-7': sidebarOpen, 'justify-center': !sidebarOpen}">
            <!-- Icon Container - Always Visible -->
            <div class="w-8 h-8 flex items-center justify-center text-lg"
                 :class="{'mr-3': sidebarOpen, 'mx-auto': !sidebarOpen}">
              <i class="fas fa-sign-out-alt"></i>
            </div>
            <!-- Text - Only Visible When Sidebar is Open -->
            <span class="font-medium transition-all duration-300 whitespace-nowrap overflow-hidden"
                  :class="{'opacity-100 w-auto': sidebarOpen, 'opacity-0 w-0': !sidebarOpen}">
              Çıkış Yap
            </span>
            <!-- Tooltip for Collapsed State -->
            <div v-if="!sidebarOpen" class="absolute left-16 bg-gray-800 text-white text-sm px-2 py-1 rounded opacity-0 group-hover:opacity-100 pointer-events-none transition-opacity duration-200 whitespace-nowrap z-50">
              Çıkış Yap
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
/* Mobil görünüm için ek stiller */
@media (max-width: 1024px) {
  .sidebar-overlay {
    background-color: rgba(0, 0, 0, 0.5);
    position: fixed;
    inset: 0;
    z-index: 20;
  }
}

/* Tooltip Styles */
.group:hover .group-hover\:opacity-100 {
  opacity: 1;
}
</style>