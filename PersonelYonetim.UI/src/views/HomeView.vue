<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import NavBar from "../components/home/NavBar.vue";

const router = useRouter();
const isLoaded = ref(false);
const activeTestimonial = ref(0);

// Sayfa yüklendiğinde
onMounted(() => {
  // Sayfa yüklendi olarak işaretle
  setTimeout(() => {
    isLoaded.value = true;
  }, 500);

  // Otomatik testimonial değiştirme
  setInterval(() => {
    activeTestimonial.value = (activeTestimonial.value + 1) % testimonials.length;
  }, 5000);
});

// Kayıt ol butonuna tıklama
const handleRegister = () => {
  router.push("/register");
};

// Demo butonuna tıklama
const handleDemo = () => {
  router.push("/dashboard");
};

// Özellikler listesi
const features = [
  {
    title: "Personel Yönetimi",
    description: "Tüm çalışanlarınızın bilgilerini tek bir yerden yönetin ve takip edin.",
    icon: "users",
  },
  {
    title: "Bordro İşlemleri",
    description: "Maaş, prim ve diğer ödemeleri otomatik olarak hesaplayın ve raporlayın.",
    icon: "file-invoice-dollar",
  },
  {
    title: "İzin Takibi",
    description: "Çalışanlarınızın izin taleplerini onaylayın ve takip edin.",
    icon: "calendar-alt",
  },
  {
    title: "Performans Değerlendirme",
    description: "Çalışanlarınızın performansını ölçün ve değerlendirin.",
    icon: "chart-line",
  },
  {
    title: "Raporlama",
    description: "Detaylı raporlar ile işletmenizin insan kaynakları verilerini analiz edin.",
    icon: "chart-bar",
  },
  {
    title: "Mobil Erişim",
    description: "Her yerden ve her cihazdan sisteme erişim sağlayın.",
    icon: "mobile-alt",
  },
];

// Referanslar
const testimonials = [
  {
    name: "Ahmet Yılmaz",
    position: "İK Müdürü",
    company: "ABC Teknoloji",
    text: "Personel Yönetim sistemi sayesinde insan kaynakları süreçlerimizi çok daha verimli hale getirdik. Artık tüm personel bilgilerimiz tek bir yerde ve kolayca erişilebilir durumda.",
    avatar: "https://randomuser.me/api/portraits/men/1.jpg",
  },
  {
    name: "Ayşe Demir",
    position: "CEO",
    company: "XYZ Holding",
    text: "Kullanımı kolay ve kapsamlı özellikleri ile tüm ihtiyaçlarımızı karşılıyor. Bordro işlemlerimiz artık çok daha hızlı ve hatasız bir şekilde tamamlanıyor.",
    avatar: "https://randomuser.me/api/portraits/women/2.jpg",
  },
];

// Müşteriler
const clients = [
  {
    name: "Migros",
    logo: "https://upload.wikimedia.org/wikipedia/commons/thumb/0/07/MiGROS_Logo.svg/2560px-MiGROS_Logo.svg.png",
  },
  {
    name: "Teknosa",
    logo: "https://upload.wikimedia.org/wikipedia/commons/thumb/8/85/Teknosa_logo.svg/152px-Teknosa_logo.svg.png",
  },
  {
    name: "Nike",
    logo: "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a6/Logo_NIKE.svg/1200px-Logo_NIKE.svg.png",
  },
  { name: "MAC", logo: "https://cdn-icons-png.flaticon.com/512/2/2235.png" },
  { name: "Penti", logo: "https://upload.wikimedia.org/wikipedia/commons/3/33/Penti_logo.png" },
];

// Uygulama modülleri
const modules = [
  {
    title: "Personel Bilgi Yönetimi",
    description: "Tüm çalışan bilgilerini tek bir yerde toplayın ve yönetin",
    icon: "id-card",
  },
  {
    title: "Bordro ve Maaş Yönetimi",
    description: "Maaş hesaplamalarını otomatikleştirin ve bordro süreçlerini kolaylaştırın",
    icon: "file-invoice-dollar",
  },
  {
    title: "İzin Yönetimi",
    description: "İzin taleplerini onaylayın ve takip edin",
    icon: "calendar-alt",
  },
  {
    title: "Performans Takibi",
    description: "Çalışan performansını ölçün ve değerlendirin",
    icon: "chart-line",
  },
];

// Form verisi
const formData = ref({
  name: "",
  email: "",
  message: "",
});

// Form gönderme
const submitForm = () => {
  alert("Form gönderildi!");
  formData.value = {
    name: "",
    email: "",
    message: "",
  };
};
</script>

<template>
  <div
    class="min-h-screen bg-white dark:bg-neutral-900 transition-all duration-300"
    :class="{ 'opacity-100': isLoaded, 'opacity-0': !isLoaded }"
  >
    <!-- Üst Menü -->
    <NavBar transparent />

    <!-- Hero Bölümü -->
    <section class="pt-32 pb-20 bg-gradient-to-r from-green-400 to-sky-500 text-white">
      <div class="container mx-auto px-4">
        <div class="flex flex-col md:flex-row items-center">
          <div class="md:w-1/2 mb-10 md:mb-0">
            <h2 class="text-4xl font-bold mb-6">Bordronun artık kolay var!</h2>
            <p class="text-xl mb-8">
              İşletmenizin insan kaynakları süreçlerini dijitalleştirin, verimliliği artırın.
              Personel yönetimi artık çok daha kolay ve etkili.
            </p>
            <div class="flex space-x-4">
              <button
                @click="handleDemo"
                class="px-6 py-3 bg-white text-sky-600 font-medium rounded-lg hover:bg-gray-100 transition-colors"
              >
                Hemen Deneyin
              </button>
              <button
                class="px-6 py-3 border border-white text-white font-medium rounded-lg hover:bg-sky-700 transition-colors"
              >
                Daha Fazla Bilgi
              </button>
            </div>
          </div>
          <div class="md:w-1/2 relative">
            <div class="bg-white p-4 rounded-lg shadow-xl">
              <img
                src="https://via.placeholder.com/600x400?text=Dashboard+Preview"
                alt="Dashboard Preview"
                class="rounded-lg"
              />
            </div>
            <div
              class="absolute -bottom-10 -left-10 w-20 h-20 bg-green-300 rounded-lg hidden md:block"
            ></div>
            <div
              class="absolute -top-10 -right-10 w-20 h-20 bg-sky-300 rounded-lg hidden md:block"
            ></div>
          </div>
        </div>
      </div>
    </section>

    <!-- Müşteriler Bölümü -->
    <section class="py-12 bg-gray-50 dark:bg-neutral-800">
      <div class="container mx-auto px-4">
        <div class="flex flex-wrap justify-center items-center gap-8 md:gap-16">
          <img
            v-for="client in clients"
            :key="client.name"
            :src="client.logo"
            :alt="client.name"
            class="h-12 md:h-16 object-contain opacity-80 hover:opacity-100 transition-opacity"
          />
        </div>
      </div>
    </section>

    <!-- Uygulama Modülleri Bölümü -->
    <section id="modules" class="py-20">
      <div class="container mx-auto px-4">
        <div class="text-center mb-16">
          <h2 class="text-3xl font-bold text-gray-800 dark:text-gray-200 mb-4">Uygulamalar</h2>
          <p class="text-xl text-gray-600 dark:text-gray-400 max-w-3xl mx-auto">
            Personel Yönetim sistemi ile tüm insan kaynakları süreçlerinizi tek bir platformda
            yönetin.
          </p>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
          <div
            v-for="module in modules"
            :key="module.title"
            class="bg-white dark:bg-neutral-800 p-6 rounded-lg shadow-sm hover:shadow-md transition-shadow border border-gray-100 dark:border-neutral-700"
          >
            <div
              class="w-12 h-12 bg-sky-100 dark:bg-sky-900 rounded-full flex items-center justify-center mb-4"
            >
              <i :class="`fas fa-${module.icon} text-sky-600 dark:text-sky-400 text-xl`"></i>
            </div>
            <h3 class="text-xl font-semibold text-gray-800 dark:text-gray-200 mb-2">
              {{ module.title }}
            </h3>
            <p class="text-gray-600 dark:text-gray-400">{{ module.description }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Özellikler Bölümü -->
    <section id="features" class="py-20 bg-gray-50 dark:bg-neutral-800">
      <div class="container mx-auto px-4">
        <div class="text-center mb-16">
          <h2 class="text-3xl font-bold text-gray-800 dark:text-gray-200 mb-4">
            Neden Bizi Seçmelisiniz?
          </h2>
          <p class="text-xl text-gray-600 dark:text-gray-400 max-w-3xl mx-auto">
            Modern, kullanıcı dostu ve kapsamlı bir personel yönetim sistemi.
          </p>
        </div>

        <div class="flex flex-col md:flex-row">
          <div class="md:w-1/2 mb-10 md:mb-0 md:pr-10">
            <img
              src="https://via.placeholder.com/600x400?text=Features"
              alt="Features"
              class="rounded-lg shadow-xl"
            />
          </div>
          <div class="md:w-1/2">
            <div class="space-y-8">
              <div v-for="feature in features.slice(0, 3)" :key="feature.title" class="flex">
                <div class="flex-shrink-0 mr-4">
                  <div
                    class="w-10 h-10 bg-sky-100 dark:bg-sky-900 rounded-full flex items-center justify-center"
                  >
                    <i :class="`fas fa-${feature.icon} text-sky-600 dark:text-sky-400`"></i>
                  </div>
                </div>
                <div>
                  <h3 class="text-lg font-semibold text-gray-800 dark:text-gray-200 mb-1">
                    {{ feature.title }}
                  </h3>
                  <p class="text-gray-600 dark:text-gray-400">{{ feature.description }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Referanslar Bölümü -->
    <section id="testimonials" class="py-20 bg-white dark:bg-neutral-900">
      <div class="container mx-auto px-4">
        <div class="text-center mb-16">
          <h2 class="text-3xl font-bold text-gray-800 dark:text-gray-200 mb-4">
            Kullanıcı Hikayeleri
          </h2>
          <p class="text-xl text-gray-600 dark:text-gray-400 max-w-3xl mx-auto">
            Müşterilerimizin deneyimlerini dinleyin.
          </p>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
          <div
            v-for="(testimonial, index) in testimonials"
            :key="testimonial.name"
            class="bg-gray-50 dark:bg-neutral-800 p-8 rounded-lg shadow-sm relative transition-all duration-300"
            :class="{ 'ring-2 ring-sky-500 scale-105': index === activeTestimonial }"
          >
            <div class="flex items-center mb-6">
              <img
                :src="testimonial.avatar"
                alt="Avatar"
                class="w-16 h-16 rounded-full mr-4 border-2 border-sky-500"
              />
              <div>
                <h4 class="font-semibold text-gray-800 dark:text-gray-200 text-lg">
                  {{ testimonial.name }}
                </h4>
                <p class="text-gray-600 dark:text-gray-400">
                  {{ testimonial.position }}, {{ testimonial.company }}
                </p>
              </div>
            </div>
            <p class="text-gray-700 dark:text-gray-300 italic text-lg">{{ testimonial.text }}</p>
            <div class="absolute top-4 right-4 text-sky-200 dark:text-sky-800">
              <i class="fas fa-quote-right text-4xl"></i>
            </div>
          </div>
        </div>

        <div class="flex justify-center mt-8 space-x-2">
          <button
            v-for="(_, index) in testimonials"
            :key="index"
            @click="activeTestimonial = index"
            class="w-3 h-3 rounded-full transition-colors"
            :class="index === activeTestimonial ? 'bg-sky-600' : 'bg-gray-300 hover:bg-sky-400'"
          ></button>
        </div>
      </div>
    </section>

    <!-- CTA Bölümü -->
    <section class="py-20 bg-gradient-to-r from-sky-500 to-blue-600 text-white">
      <div class="container mx-auto px-4 text-center">
        <h2 class="text-3xl font-bold mb-6">Personel Yönetimini Kolaylaştırmaya Hazır mısınız?</h2>
        <p class="text-xl mb-8 max-w-3xl mx-auto">
          Hemen kaydolun ve personel yönetim süreçlerinizi dijitalleştirmeye başlayın.
        </p>
        <div class="flex flex-col md:flex-row justify-center space-y-4 md:space-y-0 md:space-x-4">
          <button
            @click="handleRegister"
            class="px-8 py-4 bg-white text-sky-600 font-medium rounded-lg hover:bg-gray-100 transition-colors"
          >
            Ücretsiz Başlayın
          </button>
          <button
            @click="handleDemo"
            class="px-8 py-4 border border-white text-white font-medium rounded-lg hover:bg-sky-700 transition-colors"
          >
            Demo Talep Edin
          </button>
        </div>
      </div>
    </section>

    <!-- İletişim Bölümü -->
    <section id="contact" class="py-20">
      <div class="container mx-auto px-4">
        <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-md p-8 max-w-3xl mx-auto">
          <h2 class="text-3xl font-bold text-center text-gray-800 dark:text-gray-200 mb-8">
            Bizimle İletişime Geçin
          </h2>
          <form class="space-y-6" @submit.prevent="submitForm">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label
                  for="name"
                  class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                  >Adınız</label
                >
                <input
                  type="text"
                  id="name"
                  v-model="formData.name"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-sky-500 dark:bg-neutral-700 dark:text-white"
                />
              </div>
              <div>
                <label
                  for="email"
                  class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                  >E-posta</label
                >
                <input
                  type="email"
                  id="email"
                  v-model="formData.email"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-sky-500 dark:bg-neutral-700 dark:text-white"
                />
              </div>
            </div>
            <div>
              <label
                for="message"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Mesajınız</label
              >
              <textarea
                id="message"
                v-model="formData.message"
                rows="4"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-sky-500 dark:bg-neutral-700 dark:text-white"
              ></textarea>
            </div>
            <div class="flex justify-center">
              <button
                type="submit"
                class="px-6 py-3 bg-sky-600 text-white font-medium rounded-lg hover:bg-sky-700 transition-colors"
              >
                Gönder
              </button>
            </div>
          </form>
        </div>
      </div>
    </section>

    <!-- Alt Bilgi -->
    <footer class="bg-gray-800 dark:bg-black text-white py-16">
      <div class="container mx-auto px-4">
        <div class="grid grid-cols-1 md:grid-cols-4 gap-8 mb-8">
          <div>
            <div class="flex items-center mb-4">
              <i class="fas fa-users-cog text-sky-400 text-2xl mr-2"></i>
              <h2 class="text-xl font-bold">Personel Yönetim</h2>
            </div>
            <p class="text-gray-400 mb-4">Modern İnsan Kaynakları Çözümü</p>
            <div class="flex space-x-4">
              <a href="#" class="text-gray-400 hover:text-white transition-colors">
                <i class="fab fa-facebook-f"></i>
              </a>
              <a href="#" class="text-gray-400 hover:text-white transition-colors">
                <i class="fab fa-twitter"></i>
              </a>
              <a href="#" class="text-gray-400 hover:text-white transition-colors">
                <i class="fab fa-linkedin-in"></i>
              </a>
              <a href="#" class="text-gray-400 hover:text-white transition-colors">
                <i class="fab fa-instagram"></i>
              </a>
            </div>
          </div>

          <div>
            <h3 class="text-lg font-semibold mb-4">Ürün</h3>
            <ul class="space-y-2">
              <li>
                <a href="#" class="text-gray-400 hover:text-white transition-colors">Özellikler</a>
              </li>
              <li>
                <a href="#" class="text-gray-400 hover:text-white transition-colors"
                  >Fiyatlandırma</a
                >
              </li>
              <li>
                <a href="#" class="text-gray-400 hover:text-white transition-colors"
                  >Entegrasyonlar</a
                >
              </li>
              <li>
                <a href="#" class="text-gray-400 hover:text-white transition-colors">Güvenlik</a>
              </li>
            </ul>
          </div>

          <div>
            <h3 class="text-lg font-semibold mb-4">Şirket</h3>
            <ul class="space-y-2">
              <li>
                <a href="#" class="text-gray-400 hover:text-white transition-colors">Hakkımızda</a>
              </li>
              <li>
                <a href="#" class="text-gray-400 hover:text-white transition-colors">Kariyer</a>
              </li>
              <li><a href="#" class="text-gray-400 hover:text-white transition-colors">Blog</a></li>
              <li>
                <a href="#" class="text-gray-400 hover:text-white transition-colors">İletişim</a>
              </li>
            </ul>
          </div>

          <div>
            <h3 class="text-lg font-semibold mb-4">Destek</h3>
            <ul class="space-y-2">
              <li>
                <a href="#" class="text-gray-400 hover:text-white transition-colors"
                  >Yardım Merkezi</a
                >
              </li>
              <li>
                <a href="#" class="text-gray-400 hover:text-white transition-colors"
                  >Dokümantasyon</a
                >
              </li>
              <li><a href="#" class="text-gray-400 hover:text-white transition-colors">SSS</a></li>
              <li>
                <a href="#" class="text-gray-400 hover:text-white transition-colors">Topluluk</a>
              </li>
            </ul>
          </div>
        </div>

        <div
          class="border-t border-gray-700 pt-8 flex flex-col md:flex-row justify-between items-center"
        >
          <p class="text-gray-400">&copy; 2025 Personel Yönetim. Tüm hakları saklıdır.</p>
          <div class="flex space-x-6 mt-4 md:mt-0">
            <a href="#" class="text-gray-400 hover:text-white transition-colors"
              >Gizlilik Politikası</a
            >
            <a href="#" class="text-gray-400 hover:text-white transition-colors"
              >Kullanım Şartları</a
            >
            <a href="#" class="text-gray-400 hover:text-white transition-colors"
              >Çerez Politikası</a
            >
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>

<style scoped>
/* Smooth scroll behavior */
html {
  scroll-behavior: smooth;
}

/* Hover animations */
.hover\:transform {
  transition: transform 0.3s ease;
}

/* Gradient animations */
.bg-gradient-to-r {
  background-size: 200% 200%;
  animation: gradient 15s ease infinite;
}

@keyframes gradient {
  0% {
    background-position: 0% 50%;
  }
  50% {
    background-position: 100% 50%;
  }
  100% {
    background-position: 0% 50%;
  }
}
</style>
