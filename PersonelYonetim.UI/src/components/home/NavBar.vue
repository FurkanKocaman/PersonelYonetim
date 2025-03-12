<script setup lang="ts">
import { ref, onMounted, onUnmounted, computed } from 'vue';
import { useRouter } from 'vue-router';

const props = defineProps({
  transparent: {
    type: Boolean,
    default: false
  }
});

const router = useRouter();
const isScrolled = ref(false);
const activeDropdown = ref('');
const isMobileMenuOpen = ref(false);

// Dropdown menüler
const dropdowns = [
  {
    id: 'applications',
    title: 'Uygulamalar',
    items: [
      { name: 'Personel Yönetimi', icon: 'users', link: '#' },
      { name: 'Bordro İşlemleri', icon: 'file-invoice-dollar', link: '#' },
      { name: 'İzin Takibi', icon: 'calendar-alt', link: '#' },
      { name: 'Performans Değerlendirme', icon: 'chart-line', link: '#' }
    ]
  },
  {
    id: 'resources',
    title: 'Kaynaklar',
    items: [
      { name: 'Blog', icon: 'blog', link: '#' },
      { name: 'Destek', icon: 'headset', link: '#' },
      { name: 'Eğitimler', icon: 'graduation-cap', link: '#' },
      { name: 'SSS', icon: 'question-circle', link: '#' }
    ]
  }
];

// Sayfa yüklendiğinde
onMounted(() => {
  // Scroll event listener ekle
  window.addEventListener('scroll', handleScroll);
  // Click outside listener
  window.addEventListener('click', handleClickOutside);
});

// Component unmount olduğunda
onUnmounted(() => {
  // Scroll event listener'ı kaldır
  window.removeEventListener('scroll', handleScroll);
  // Click outside listener
  window.removeEventListener('click', handleClickOutside);
});

// Scroll durumunu kontrol et
const handleScroll = () => {
  isScrolled.value = window.scrollY > 50;
};

// Dropdown menüyü aç/kapat
const toggleDropdown = (menu, event) => {
  event.stopPropagation();
  if (activeDropdown.value === menu) {
    activeDropdown.value = '';
  } else {
    activeDropdown.value = menu;
  }
};

// Dropdown dışına tıklandığında kapat
const handleClickOutside = (event) => {
  const dropdownMenus = document.querySelectorAll('.dropdown-menu');
  let isClickInside = false;

  dropdownMenus.forEach(menu => {
    if (menu.contains(event.target)) {
      isClickInside = true;
    }
  });

  if (!isClickInside) {
    activeDropdown.value = '';
  }
};

// Mobil menüyü aç/kapat
const toggleMobileMenu = () => {
  isMobileMenuOpen.value = !isMobileMenuOpen.value;
};

// Giriş yap butonuna tıklama
const handleLogin = () => {
  router.push('/login');
};

// Kayıt ol butonuna tıklama
const handleRegister = () => {
  router.push('/register');
};

// Navbar background class
const navbarClass = computed(() => {
  if (props.transparent) {
    return isScrolled.value
      ? 'bg-white dark:bg-neutral-800 shadow-md transition-all duration-300'
      : 'bg-transparent transition-all duration-300';
  }
  return 'bg-white dark:bg-neutral-800 shadow-md';
});

// Text color class
const textColorClass = computed(() => {
  if (props.transparent && !isScrolled.value) {
    return 'text-white';
  }
  return 'text-gray-800 dark:text-gray-200';
});

// Link color class
const linkColorClass = computed(() => {
  if (props.transparent && !isScrolled.value) {
    return 'text-white hover:text-gray-200';
  }
  return 'text-gray-600 hover:text-sky-600 dark:text-gray-300 dark:hover:text-sky-400';
});
</script>

<template>
  <header :class="[navbarClass, 'fixed w-full z-50']">
    <div class="container mx-auto px-4 py-4">
      <div class="flex justify-between items-center">
        <!-- Logo -->
        <div class="flex items-center">
          <i class="fas fa-users-cog text-sky-600 text-2xl mr-2"></i>
          <h1 :class="[textColorClass, 'text-xl font-bold']">Personel Yönetim</h1>
        </div>

        <!-- Desktop Navigation -->
        <nav class="hidden lg:flex items-center space-x-8">
          <!-- Regular Links -->
          <a href="#features" :class="linkColorClass">Özellikler</a>

          <!-- Dropdown Menus -->
          <div v-for="dropdown in dropdowns" :key="dropdown.id" class="relative dropdown-menu">
            <button
              @click="(e) => toggleDropdown(dropdown.id, e)"
              :class="[linkColorClass, 'flex items-center space-x-1 focus:outline-none']"
            >
              <span>{{ dropdown.title }}</span>
              <i class="fas fa-chevron-down text-xs transition-transform"
                 :class="{'transform rotate-180': activeDropdown === dropdown.id}"></i>
            </button>

            <!-- Dropdown Content -->
            <div
              v-show="activeDropdown === dropdown.id"
              class="absolute top-full left-0 mt-2 w-64 bg-white dark:bg-neutral-700 rounded-lg shadow-lg overflow-hidden transition-all duration-200 ease-in-out"
            >
              <div class="p-4 grid gap-3">
                <a
                  v-for="item in dropdown.items"
                  :key="item.name"
                  :href="item.link"
                  class="flex items-center p-2 rounded-md hover:bg-gray-100 dark:hover:bg-neutral-600 transition-colors"
                >
                  <div class="w-8 h-8 bg-sky-100 dark:bg-sky-900 rounded-full flex items-center justify-center mr-3">
                    <i :class="`fas fa-${item.icon} text-sky-600 dark:text-sky-400`"></i>
                  </div>
                  <span class="text-gray-800 dark:text-gray-200">{{ item.name }}</span>
                </a>
              </div>
            </div>
          </div>

          <a href="#testimonials" :class="linkColorClass">Referanslar</a>
          <a href="#contact" :class="linkColorClass">İletişim</a>
        </nav>

        <!-- Action Buttons -->
        <div class="hidden md:flex items-center space-x-4">
          <button @click="handleLogin" class="px-5 py-2.5 bg-gradient-to-r from-sky-500 to-blue-600 text-white rounded-lg hover:from-sky-600 hover:to-blue-700 shadow-md hover:shadow-lg transition-all duration-300 font-medium flex items-center">
            <i class="fas fa-sign-in-alt mr-2"></i>
            Giriş Yap
          </button>
          <button @click="handleRegister" class="px-5 py-2.5 bg-white dark:bg-neutral-800 border-2 border-sky-500 text-sky-600 dark:text-sky-400 rounded-lg hover:bg-sky-50 dark:hover:bg-sky-900/30 shadow-md hover:shadow-lg transition-all duration-300 font-medium flex items-center">
            <i class="fas fa-user-plus mr-2"></i>
            Kayıt Ol
          </button>
        </div>

        <!-- Mobile Menu Button -->
        <button @click="toggleMobileMenu" class="lg:hidden text-gray-800 dark:text-gray-200 focus:outline-none">
          <i class="fas" :class="isMobileMenuOpen ? 'fa-times' : 'fa-bars'"></i>
        </button>
      </div>

      <!-- Mobile Menu -->
      <div
        v-show="isMobileMenuOpen"
        class="lg:hidden mt-4 py-4 border-t border-gray-200 dark:border-neutral-700"
      >
        <nav class="flex flex-col space-y-4">
          <a href="#features" class="text-gray-600 dark:text-gray-300 hover:text-sky-600 dark:hover:text-sky-400">Özellikler</a>

          <!-- Mobile Dropdown Accordions -->
          <div v-for="dropdown in dropdowns" :key="`mobile-${dropdown.id}`" class="space-y-2">
            <button
              @click="(e) => toggleDropdown(dropdown.id + '-mobile', e)"
              class="flex items-center justify-between w-full text-left text-gray-600 dark:text-gray-300 hover:text-sky-600 dark:hover:text-sky-400"
            >
              <span>{{ dropdown.title }}</span>
              <i class="fas fa-chevron-down text-xs transition-transform"
                 :class="{'transform rotate-180': activeDropdown === dropdown.id + '-mobile'}"></i>
            </button>

            <div
              v-show="activeDropdown === dropdown.id + '-mobile'"
              class="pl-4 space-y-2 border-l-2 border-gray-200 dark:border-neutral-700"
            >
              <a
                v-for="item in dropdown.items"
                :key="`mobile-${item.name}`"
                :href="item.link"
                class="flex items-center py-1 text-gray-600 dark:text-gray-400 hover:text-sky-600 dark:hover:text-sky-400"
              >
                <i :class="`fas fa-${item.icon} w-5 text-sky-600 dark:text-sky-400 mr-2`"></i>
                <span>{{ item.name }}</span>
              </a>
            </div>
          </div>

          <a href="#testimonials" class="text-gray-600 dark:text-gray-300 hover:text-sky-600 dark:hover:text-sky-400">Referanslar</a>
          <a href="#contact" class="text-gray-600 dark:text-gray-300 hover:text-sky-600 dark:hover:text-sky-400">İletişim</a>

          <div class="flex space-x-4 pt-2">
            <button @click="handleLogin" class="flex-1 px-4 py-3 bg-gradient-to-r from-sky-500 to-blue-600 text-white rounded-lg hover:from-sky-600 hover:to-blue-700 shadow-md hover:shadow-lg transition-all duration-300 font-medium flex items-center justify-center">
              <i class="fas fa-sign-in-alt mr-2"></i>
              Giriş Yap
            </button>
            <button @click="handleRegister" class="flex-1 px-4 py-3 bg-white dark:bg-neutral-800 border-2 border-sky-500 text-sky-600 dark:text-sky-400 rounded-lg hover:bg-sky-50 dark:hover:bg-sky-900/30 shadow-md hover:shadow-lg transition-all duration-300 font-medium flex items-center justify-center">
              <i class="fas fa-user-plus mr-2"></i>
              Kayıt Ol
            </button>
          </div>
        </nav>
      </div>
    </div>
  </header>
</template>
