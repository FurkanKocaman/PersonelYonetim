<script setup lang="ts">
import { ref, reactive, onMounted, computed } from "vue";
import { useRouter, useRoute } from "vue-router";
import IzinService from "@/services/IzinService";
import { PersonelService } from "@/services/PersonelService";
import type { IzinRequest, IzinItem } from "@/models/IzinModels";
import type PersonelItem from "@/models/PersonelModels";

const router = useRouter();
const route = useRoute();

// Create an instance of PersonelService
const personelService = new PersonelService();

// Düzenleme modu kontrolü
const isEditMode = computed(() => {
  return route.query.id !== undefined;
});

// Düzenlenen izin ID'si
const editId = computed(() => {
  return route.query.id ? Number(route.query.id) : undefined;
});

// İzin türleri
const izinTurleri = ["Yıllık İzin", "Hastalık İzni", "Mazeret İzni", "Ücretsiz İzin"];

// Personel listesi
const personelList = ref<PersonelItem[]>([]);
const loadingPersonel = ref(true);

// Form verisi
const formData = reactive<IzinRequest & { dosya: File | null }>({
  izinTipi: "",
  baslangicTarihi: "",
  bitisTarihi: "",
  aciklama: "",
  personelAdi: "",
  gun: 0,
  durum: "Beklemede",
  dosya: null,
});

// Form durumu
const formStatus = reactive({
  loading: false,
  submitting: false,
  success: false,
  error: false,
  errorMessage: "",
});

// Form doğrulama hataları
const formErrors = reactive({
  izinTipi: "",
  baslangicTarihi: "",
  bitisTarihi: "",
  aciklama: "",
  personelAdi: "",
});

// Dosya seçimi için referans
const fileInput = ref<HTMLInputElement | null>(null);

// Sayfa başlığı
const pageTitle = computed(() => {
  return isEditMode.value ? "İzin Talebini Düzenle" : "Yeni İzin Talebi";
});

const submitButtonText = computed(() => {
  return isEditMode.value ? "Güncelle" : "Gönder";
});

// Mevcut izin verisini yükle (düzenleme modu için)
const loadExistingIzin = async () => {
  if (!editId.value) return;

  formStatus.loading = true;
  formStatus.error = false;

  try {
    const izinData = await IzinService.getIzinById(editId.value);

    // Tarih formatını düzelt (DD.MM.YYYY -> YYYY-MM-DD)
    const formatDate = (dateStr: string) => {
      if (!dateStr) return "";
      const parts = dateStr.split(".");
      if (parts.length !== 3) return dateStr;
      return `${parts[2]}-${parts[1]}-${parts[0]}`;
    };

    // Form verilerini doldur
    formData.izinTipi = izinData.izinTipi;
    formData.baslangicTarihi = formatDate(izinData.baslangicTarihi);
    formData.bitisTarihi = formatDate(izinData.bitisTarihi);
    formData.aciklama = izinData.aciklama || "";
    formData.personelAdi = izinData.personelAdi;
    formData.gun = izinData.gun;
    formData.durum = izinData.durum;
  } catch (error) {
    console.error("İzin verisi yüklenirken hata oluştu:", error);
    formStatus.error = true;
    formStatus.errorMessage = "İzin verisi yüklenirken bir hata oluştu. Lütfen tekrar deneyin.";
  } finally {
    formStatus.loading = false;
  }
};

// Personel listesini yükle
const loadPersonelList = async () => {
  try {
    loadingPersonel.value = true;
    const response = await personelService.getPersonelList();
    personelList.value = response.items;
  } catch (error) {
    console.error("Personel listesi yüklenirken hata oluştu:", error);
  } finally {
    loadingPersonel.value = false;
  }
};

// Komponent yüklendiğinde
onMounted(async () => {
  await loadPersonelList();

  if (isEditMode.value) {
    await loadExistingIzin();
  }
});

// Dosya seçme işlemi
const handleFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement;
  if (target.files && target.files.length > 0) {
    formData.dosya = target.files[0];
  }
};

// Dosya seçimini temizleme
const clearFileSelection = () => {
  formData.dosya = null;
  if (fileInput.value) {
    fileInput.value.value = "";
  }
};

// Form doğrulama
const validateForm = () => {
  // Hata mesajlarını temizle
  Object.keys(formErrors).forEach((key) => {
    (formErrors as any)[key] = "";
  });

  let isValid = true;

  // İzin türü doğrulama
  if (!formData.izinTipi) {
    formErrors.izinTipi = "İzin türü seçilmelidir";
    isValid = false;
  } else {
    formErrors.izinTipi = "";
  }

  // Personel doğrulama
  if (!formData.personelAdi) {
    formErrors.personelAdi = "Personel seçilmelidir";
    isValid = false;
  } else {
    formErrors.personelAdi = "";
  }

  // Başlangıç tarihi doğrulama
  if (!formData.baslangicTarihi) {
    formErrors.baslangicTarihi = "Başlangıç tarihi seçilmelidir";
    isValid = false;
  } else {
    formErrors.baslangicTarihi = "";
  }

  // Bitiş tarihi doğrulama
  if (!formData.bitisTarihi) {
    formErrors.bitisTarihi = "Bitiş tarihi seçilmelidir";
    isValid = false;
  } else {
    // Bitiş tarihi başlangıç tarihinden sonra olmalı
    const baslangic = new Date(formData.baslangicTarihi);
    const bitis = new Date(formData.bitisTarihi);

    if (bitis < baslangic) {
      formErrors.bitisTarihi = "Bitiş tarihi başlangıç tarihinden sonra olmalıdır";
      isValid = false;
    } else {
      formErrors.bitisTarihi = "";
    }
  }

  return isValid;
};

// Form gönderme
const submitForm = async () => {
  if (!validateForm()) {
    return;
  }

  formStatus.submitting = true;
  formStatus.error = false;

  try {
    // Form verilerini hazırla
    const izinData: IzinRequest = {
      personelAdi: formData.personelAdi,
      izinTipi: formData.izinTipi,
      baslangicTarihi: formData.baslangicTarihi,
      bitisTarihi: formData.bitisTarihi,
      gun: formData.gun,
      aciklama: formData.aciklama,
      durum: formData.durum,
    };

    // Dosya varsa yükle

    // İzin talebini kaydet
    if (isEditMode.value && editId.value) {
      await IzinService.updateIzin(editId.value, izinData);
    } else {
      await IzinService.createIzin(izinData);
    }

    // Başarılı mesajı göster
    formStatus.success = true;

    // 2 saniye sonra izin listesine yönlendir
    setTimeout(() => {
      router.push({ name: "izinler" });
    }, 2000);
  } catch (error) {
    console.error("İzin talebi kaydedilirken hata oluştu:", error);
    formStatus.error = true;
    formStatus.errorMessage = "İzin talebi kaydedilirken bir hata oluştu. Lütfen tekrar deneyin.";
  } finally {
    formStatus.submitting = false;
  }
};

// Formu iptal etme
const cancelForm = () => {
  router.push({ name: "izinler" });
};
</script>

<template>
  <div class="flex-1 transition-all duration-300">
    <!-- Üst Başlık Alanı -->
    <header class="bg-white dark:bg-neutral-800 shadow-sm">
      <div class="py-4 px-6">
        <h1 class="text-2xl font-bold text-gray-800 dark:text-gray-200">{{ pageTitle }}</h1>
        <p class="text-gray-600 dark:text-gray-400">İzin talebinizi oluşturun veya düzenleyin.</p>
      </div>
    </header>

    <main class="p-6">
      <!-- Form Alanı -->
      <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6">
        <!-- Yükleniyor Göstergesi -->
        <div v-if="formStatus.loading" class="flex justify-center items-center py-12">
          <svg class="animate-spin h-12 w-12 border-b-2 border-blue-600"></svg>
        </div>

        <template v-else>
          <!-- Başarı Mesajı -->
          <div
            v-if="formStatus.success"
            class="mb-6 bg-green-100 border border-green-200 text-green-700 dark:bg-green-900 dark:border-green-800 dark:text-green-300 rounded-lg p-4"
          >
            <div class="flex">
              <svg class="h-5 w-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
                <path
                  fill-rule="evenodd"
                  d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
                  clip-rule="evenodd"
                />
              </svg>
              <span>İzin talebiniz başarıyla kaydedildi! Yönlendiriliyorsunuz...</span>
            </div>
          </div>

          <!-- Hata Mesajı -->
          <div
            v-if="formStatus.error"
            class="mb-6 bg-red-100 border border-red-200 text-red-700 dark:bg-red-900 dark:border-red-800 dark:text-red-300 rounded-lg p-4"
          >
            <div class="flex">
              <svg class="h-5 w-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
                <path
                  fill-rule="evenodd"
                  d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zm-1-9a1 1 0 00-1 1v4a1 1 0 102 0V6a1 1 0 00-1-1z"
                  clip-rule="evenodd"
                />
              </svg>
              <span>{{ formStatus.errorMessage }}</span>
            </div>
          </div>

          <form @submit.prevent="submitForm" class="space-y-6">
            <!-- Personel Seçimi -->
            <div>
              <label
                for="personel"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Personel</label
              >
              <div
                v-if="loadingPersonel"
                class="flex items-center text-gray-500 dark:text-gray-400"
              >
                <svg class="animate-spin h-5 w-5 mr-2" fill="none" viewBox="0 0 24 24">
                  <circle
                    class="opacity-25"
                    cx="12"
                    cy="12"
                    r="10"
                    stroke="currentColor"
                    stroke-width="4"
                  ></circle>
                  <path
                    class="opacity-75"
                    fill="currentColor"
                    d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                  ></path>
                </svg>
                <span>Personel listesi yükleniyor...</span>
              </div>
              <div v-else>
                <select
                  id="personel"
                  v-model="formData.personelAdi"
                  class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                  :class="{ 'border-red-500 dark:border-red-500': formErrors.personelAdi }"
                >
                  <option value="">Personel Seçiniz</option>
                  <option
                    v-for="personel in personelList"
                    :key="personel.id"
                    :value="personel.ad + ' ' + personel.soyad"
                  >
                    {{ personel.ad + " " + personel.soyad }} ({{ personel.departman }})
                  </option>
                </select>
                <p
                  v-if="formErrors.personelAdi"
                  class="mt-1 text-sm text-red-600 dark:text-red-400"
                >
                  {{ formErrors.personelAdi }}
                </p>
              </div>
            </div>

            <!-- İzin Türü -->
            <div>
              <label
                for="izinTuru"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >İzin Türü</label
              >
              <select
                id="izinTuru"
                v-model="formData.izinTipi"
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                :class="{ 'border-red-500 dark:border-red-500': formErrors.izinTipi }"
              >
                <option value="">İzin Türü Seçiniz</option>
                <option v-for="tur in izinTurleri" :key="tur" :value="tur">{{ tur }}</option>
              </select>
              <p v-if="formErrors.izinTipi" class="mt-1 text-sm text-red-600 dark:text-red-400">
                {{ formErrors.izinTipi }}
              </p>
            </div>

            <!-- Tarih Seçimleri -->
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <!-- Başlangıç Tarihi -->
              <div>
                <label
                  for="baslangicTarihi"
                  class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                  >Başlangıç Tarihi</label
                >
                <input
                  type="date"
                  id="baslangicTarihi"
                  v-model="formData.baslangicTarihi"
                  class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                  :class="{ 'border-red-500 dark:border-red-500': formErrors.baslangicTarihi }"
                />
                <p
                  v-if="formErrors.baslangicTarihi"
                  class="mt-1 text-sm text-red-600 dark:text-red-400"
                >
                  {{ formErrors.baslangicTarihi }}
                </p>
              </div>

              <!-- Bitiş Tarihi -->
              <div>
                <label
                  for="bitisTarihi"
                  class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                  >Bitiş Tarihi</label
                >
                <input
                  type="date"
                  id="bitisTarihi"
                  v-model="formData.bitisTarihi"
                  class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                  :class="{ 'border-red-500 dark:border-red-500': formErrors.bitisTarihi }"
                />
                <p
                  v-if="formErrors.bitisTarihi"
                  class="mt-1 text-sm text-red-600 dark:text-red-400"
                >
                  {{ formErrors.bitisTarihi }}
                </p>
              </div>
            </div>

            <!-- Açıklama -->
            <div>
              <label
                for="aciklama"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Açıklama</label
              >
              <textarea
                id="aciklama"
                v-model="formData.aciklama"
                rows="4"
                class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                :class="{ 'border-red-500 dark:border-red-500': formErrors.aciklama }"
                placeholder="İzin talebiniz hakkında açıklama yazınız..."
              ></textarea>
              <p v-if="formErrors.aciklama" class="mt-1 text-sm text-red-600 dark:text-red-400">
                {{ formErrors.aciklama }}
              </p>
            </div>

            <!-- Dosya Yükleme -->
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Belge Yükleme (İsteğe Bağlı)</label
              >
              <div class="mt-1 flex items-center">
                <input
                  type="file"
                  ref="fileInput"
                  @change="handleFileChange"
                  class="hidden"
                  accept=".pdf,.doc,.docx,.jpg,.jpeg,.png"
                />
                <button
                  type="button"
                  @click="fileInput?.click()"
                  class="px-4 py-2 bg-gray-200 hover:bg-gray-300 text-gray-700 dark:bg-neutral-700 dark:hover:bg-neutral-600 dark:text-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
                >
                  Dosya Seç
                </button>
                <span v-if="formData.dosya" class="ml-3 text-gray-700 dark:text-gray-300">
                  {{ formData.dosya.name }}
                  <button
                    type="button"
                    @click="clearFileSelection"
                    class="ml-2 text-red-600 hover:text-red-800 dark:text-red-400 dark:hover:text-red-300 focus:outline-none"
                  >
                    <svg class="h-4 w-4" fill="currentColor" viewBox="0 0 20 20">
                      <path
                        fill-rule="evenodd"
                        d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                        clip-rule="evenodd"
                      />
                    </svg>
                  </button>
                </span>
                <span v-else class="ml-3 text-gray-500 dark:text-gray-400"
                  >Henüz dosya seçilmedi</span
                >
              </div>
              <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">
                PDF, Word veya resim dosyaları yükleyebilirsiniz (max. 5MB)
              </p>
            </div>

            <!-- Form Butonları -->
            <div class="flex justify-end space-x-3 pt-4">
              <button
                type="button"
                @click="cancelForm"
                class="px-4 py-2 bg-gray-200 hover:bg-gray-300 text-gray-700 dark:bg-neutral-700 dark:hover:bg-neutral-600 dark:text-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500"
              >
                İptal
              </button>
              <button
                type="submit"
                :disabled="formStatus.submitting"
                class="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                <span v-if="formStatus.submitting" class="flex items-center">
                  <svg
                    class="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <circle
                      class="opacity-25"
                      cx="12"
                      cy="12"
                      r="10"
                      stroke="currentColor"
                      stroke-width="4"
                    ></circle>
                    <path
                      class="opacity-75"
                      fill="currentColor"
                      d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                    ></path>
                  </svg>
                  Kaydediliyor...
                </span>
                <span v-else>{{ submitButtonText }}</span>
              </button>
            </div>
          </form>
        </template>
      </div>
    </main>
  </div>
</template>

<style scoped>
/* İzin talep formu için özel stiller */
</style>
