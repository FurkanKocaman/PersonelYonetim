<template>
  <div class="fixed inset-0 flex justify-center items-center z-50">
    <div class="absolute inset-0 bg-gray-900 opacity-50 backdrop-blur-sm" @click="$emit('close')"></div>

    <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-md p-6 w-full max-w-4xl max-h-[90vh] overflow-y-auto relative z-10">
      <h2 class="text-xl font-bold text-gray-800 dark:text-white mb-4">
        {{ isEdit ? 'Personel Düzenle' : 'Yeni Personel Ekle' }}
      </h2>

      <form @submit.prevent="savePersonel">
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
          <!-- Kişisel Bilgiler -->
          <div class="col-span-1 md:col-span-3">
            <h3 class="text-lg font-semibold text-gray-700 dark:text-gray-300 mb-2">Kişisel Bilgiler</h3>
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="ad">
              Ad *
            </label>
            <input
              id="ad"
              v-model="formData.ad"
              type="text"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              placeholder="Adı girin"
              required
            >
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="soyad">
              Soyad *
            </label>
            <input
              id="soyad"
              v-model="formData.soyad"
              type="text"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              placeholder="Soyadı girin"
              required
            >
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="dogumTarihi">
              Doğum Tarihi *
            </label>
            <input
              id="dogumTarihi"
              v-model="formData.dogumTarihi"
              type="date"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              required
            >
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2">
              Cinsiyet *
            </label>
            <div class="flex space-x-4">
              <label class="inline-flex items-center">
                <input type="radio" v-model="formData.cinsiyet" :value="true" class="form-radio" required>
                <span class="ml-2 text-gray-700 dark:text-gray-300">Erkek</span>
              </label>
              <label class="inline-flex items-center">
                <input type="radio" v-model="formData.cinsiyet" :value="false" class="form-radio" required>
                <span class="ml-2 text-gray-700 dark:text-gray-300">Kadın</span>
              </label>
            </div>
          </div>

          <!-- İş Bilgileri -->
          <div class="col-span-1 md:col-span-3 mt-4">
            <h3 class="text-lg font-semibold text-gray-700 dark:text-gray-300 mb-2">İş Bilgileri</h3>
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="sirket">
              Şirket *
            </label>
            <select
              id="sirket"
              v-model="selectedSirket"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              required
              @change="onSirketChange"
            >
              <option value="" disabled>Şirket seçin</option>
              <option v-for="sirket in sirketler" :key="sirket.id" :value="sirket.id">{{ sirket.ad }}</option>
            </select>
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="sube">
              Şube *
            </label>
            <select
              id="sube"
              v-model="selectedSube"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              required
              @change="onSubeChange"
              :disabled="!selectedSirket"
            >
              <option value="" disabled>Şube seçin</option>
              <option v-for="sube in subeler" :key="sube.id" :value="sube.id">{{ sube.ad }}</option>
            </select>
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="departman">
              Departman *
            </label>
            <select
              id="departman"
              v-model="formData.departmanId"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              required
              :disabled="!selectedSube"
            >
              <option value="" disabled>Departman seçin</option>
              <option v-for="departman in departmanlar" :key="departman.id" :value="departman.id">{{ departman.ad }}</option>
            </select>
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="pozisyon">
              Pozisyon *
            </label>
            <select
              id="pozisyon"
              v-model="formData.pozisyonId"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              required
            >
              <option value="" disabled>Pozisyon seçin</option>
              <option v-for="pozisyon in pozisyonlar" :key="pozisyon.id" :value="pozisyon.id">{{ pozisyon.ad }}</option>
            </select>
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="iseGirisTarihi">
              İşe Giriş Tarihi *
            </label>
            <input
              id="iseGirisTarihi"
              v-model="formData.iseGirisTarihi"
              type="date"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              required
            >
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="durum">
              Durum *
            </label>
            <select
              id="durum"
              v-model="formData.isActive"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              required
            >
              <option :value="true">Aktif</option>
              <option :value="false">Pasif</option>
            </select>
          </div>

          <!-- İletişim Bilgileri -->
          <div class="col-span-1 md:col-span-3 mt-4">
            <h3 class="text-lg font-semibold text-gray-700 dark:text-gray-300 mb-2">İletişim Bilgileri</h3>
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="eposta">
              E-posta *
            </label>
            <input
              id="eposta"
              v-model="formData.eposta"
              type="email"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              placeholder="E-posta adresini girin"
              required
            >
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="telefon">
              Telefon *
            </label>
            <input
              id="telefon"
              v-model="formData.telefon"
              type="tel"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              placeholder="Telefon numarasını girin"
              required
            >
          </div>

          <!-- Adres Bilgileri -->
          <div class="col-span-1 md:col-span-3 mt-4">
            <h3 class="text-lg font-semibold text-gray-700 dark:text-gray-300 mb-2">Adres Bilgileri</h3>
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="ulke">
              Ülke *
            </label>
            <input
              id="ulke"
              v-model="formData.ulke"
              type="text"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              placeholder="Ülke girin"
              required
            >
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="sehir">
              Şehir *
            </label>
            <input
              id="sehir"
              v-model="formData.sehir"
              type="text"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              placeholder="Şehir girin"
              required
            >
          </div>

          <div class="mb-4">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="ilce">
              İlçe *
            </label>
            <input
              id="ilce"
              v-model="formData.ilce"
              type="text"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              placeholder="İlçe girin"
              required
            >
          </div>

          <div class="mb-4 md:col-span-3">
            <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="tamAdres">
              Tam Adres *
            </label>
            <textarea
              id="tamAdres"
              v-model="formData.tamAdres"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
              placeholder="Tam adresi girin"
              rows="3"
              required
            ></textarea>
          </div>
        </div>

        <div class="flex justify-end mt-6 space-x-2">
          <button
            type="button"
            @click="$emit('close')"
            class="bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
          >
            İptal
          </button>
          <button
            type="submit"
            class="bg-sky-600 hover:bg-sky-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            :disabled="isSubmitting"
          >
            {{ isSubmitting ? 'Kaydediliyor...' : 'Kaydet' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, defineProps, defineEmits } from 'vue';
import type { PersonelCreateRequest } from '@/models/request-models/PersonelCreateRequest';
import type { SirketModel } from '@/models/entity-models/SirketModel';
import type { SubeModel } from '@/models/entity-models/SubeModel';
import type { DepartmanModel } from '@/models/entity-models/DepartmanModel';
import type { PozisyonModel } from '@/models/entity-models/PozisyonModel';
import PersonelService from '@/services/PersonelService';
import SirketService from '@/services/SirketService';
import SubeService from '@/services/SubeService';
import DepartmanService from '@/services/DepartmanService';
import PozisyonService from '@/services/PozisyonService';

const props = defineProps<{
  personelId?: string;
  isEdit: boolean;
}>();

const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'saved'): void;
}>();

// Form verisi
const formData = ref<PersonelCreateRequest>({
  ad: '',
  soyad: '',
  dogumTarihi: '',
  cinsiyet: true,
  departmanId: '',
  pozisyonId: '',
  iseGirisTarihi: '',
  eposta: '',
  telefon: '',
  ulke: 'Türkiye',
  sehir: '',
  ilce: '',
  tamAdres: '',
  isActive: true
});

// Seçim listeleri
const sirketler = ref<SirketModel[]>([]);
const subeler = ref<SubeModel[]>([]);
const departmanlar = ref<DepartmanModel[]>([]);
const pozisyonlar = ref<PozisyonModel[]>([]);

// Seçilen değerler
const selectedSirket = ref('');
const selectedSube = ref('');
const isSubmitting = ref(false);

// Sayfa yüklendiğinde
onMounted(async () => {
  try {
    // Şirketleri yükle
    const sirketResponse = await SirketService.sirketlerGet();
    if (sirketResponse?.Sirketler) {
      sirketler.value = sirketResponse.Sirketler;
    }

    // Eğer düzenleme modundaysa, mevcut personel verilerini getir
    if (props.isEdit && props.personelId) {
      // Burada personel verilerini getirme API'si eklenecek
      // const personel = await PersonelService.getPersonelById(props.personelId);
      // formData.value = { ...personel };
      // selectedSirket.value = personel.sirketId;
      // selectedSube.value = personel.subeId;
      // await onSirketChange();
      // await onSubeChange();
    }
  } catch (error) {
    console.error('Veriler yüklenirken hata oluştu:', error);
  }
});

// Şirket değiştiğinde
const onSirketChange = async () => {
  if (!selectedSirket.value) return;

  try {
    // Şubeleri yükle
    const subeResponse = await SubeService.subelerGet(selectedSirket.value);
    if (subeResponse?.Subeler) {
      subeler.value = subeResponse.Subeler;
    }

    // Şube seçimini sıfırla
    selectedSube.value = '';
    formData.value.departmanId = '';
    departmanlar.value = [];
  } catch (error) {
    console.error('Şubeler yüklenirken hata oluştu:', error);
  }
};

// Şube değiştiğinde
const onSubeChange = async () => {
  if (!selectedSube.value) return;

  try {
    // Departmanları yükle
    const departmanResponse = await DepartmanService.departmanlarGet(selectedSube.value);
    if (departmanResponse?.Departmanlar) {
      departmanlar.value = departmanResponse.Departmanlar;
    }

    // Departman seçimini sıfırla
    formData.value.departmanId = '';

    // Pozisyonları yükle
    await loadPozisyonlar();
  } catch (error) {
    console.error('Departmanlar yüklenirken hata oluştu:', error);
  }
};

// Pozisyonları yükle
const loadPozisyonlar = async () => {
  try {
    if (!selectedSirket.value) return;

    // Pozisyonları API'den getir
    const pozisyonResponse = await PozisyonService.pozisyonlarGet(selectedSirket.value);
    if (pozisyonResponse?.Pozisyonlar) {
      pozisyonlar.value = pozisyonResponse.Pozisyonlar;
    } else {
      // Eğer API yanıt vermezse örnek pozisyonlar
      const currentDate = new Date();
      pozisyonlar.value = [
        {
          id: '1',
          ad: 'Müdür',
          aciklama: undefined,
          sirketId: selectedSirket.value,
          sirketAd: 'Şirket',
          isActive: true,
          createdAt: currentDate,
          createUserId: '1',
          createUserName: 'Admin',
          updateAt: currentDate,
          updateUserId: '1',
          updateuserName: 'Admin',
          isDeleted: false,
          deleteAt: currentDate
        },
        {
          id: '2',
          ad: 'Uzman',
          aciklama: undefined,
          sirketId: selectedSirket.value,
          sirketAd: 'Şirket',
          isActive: true,
          createdAt: currentDate,
          createUserId: '1',
          createUserName: 'Admin',
          updateAt: currentDate,
          updateUserId: '1',
          updateuserName: 'Admin',
          isDeleted: false,
          deleteAt: currentDate
        },
        {
          id: '3',
          ad: 'Asistan',
          aciklama: undefined,
          sirketId: selectedSirket.value,
          sirketAd: 'Şirket',
          isActive: true,
          createdAt: currentDate,
          createUserId: '1',
          createUserName: 'Admin',
          updateAt: currentDate,
          updateUserId: '1',
          updateuserName: 'Admin',
          isDeleted: false,
          deleteAt: currentDate
        },
        {
          id: '4',
          ad: 'Stajyer',
          aciklama: undefined,
          sirketId: selectedSirket.value,
          sirketAd: 'Şirket',
          isActive: true,
          createdAt: currentDate,
          createUserId: '1',
          createUserName: 'Admin',
          updateAt: currentDate,
          updateUserId: '1',
          updateuserName: 'Admin',
          isDeleted: false,
          deleteAt: currentDate
        }
      ];
    }
  } catch (error) {
    console.error('Pozisyonlar yüklenirken hata oluştu:', error);
    // Hata durumunda örnek pozisyonlar
    const currentDate = new Date();
    pozisyonlar.value = [
      {
        id: '1',
        ad: 'Müdür',
        aciklama: undefined,
        sirketId: selectedSirket.value || '1',
        sirketAd: 'Şirket',
        isActive: true,
        createdAt: currentDate,
        createUserId: '1',
        createUserName: 'Admin',
        updateAt: currentDate,
        updateUserId: '1',
        updateuserName: 'Admin',
        isDeleted: false,
        deleteAt: currentDate
      },
      {
        id: '2',
        ad: 'Uzman',
        aciklama: undefined,
        sirketId: selectedSirket.value || '1',
        sirketAd: 'Şirket',
        isActive: true,
        createdAt: currentDate,
        createUserId: '1',
        createUserName: 'Admin',
        updateAt: currentDate,
        updateUserId: '1',
        updateuserName: 'Admin',
        isDeleted: false,
        deleteAt: currentDate
      },
      {
        id: '3',
        ad: 'Asistan',
        aciklama: undefined,
        sirketId: selectedSirket.value || '1',
        sirketAd: 'Şirket',
        isActive: true,
        createdAt: currentDate,
        createUserId: '1',
        createUserName: 'Admin',
        updateAt: currentDate,
        updateUserId: '1',
        updateuserName: 'Admin',
        isDeleted: false,
        deleteAt: currentDate
      },
      {
        id: '4',
        ad: 'Stajyer',
        aciklama: undefined,
        sirketId: selectedSirket.value || '1',
        sirketAd: 'Şirket',
        isActive: true,
        createdAt: currentDate,
        createUserId: '1',
        createUserName: 'Admin',
        updateAt: currentDate,
        updateUserId: '1',
        updateuserName: 'Admin',
        isDeleted: false,
        deleteAt: currentDate
      }
    ];
  }
};

// Personel kaydetme
const savePersonel = async () => {
  try {
    isSubmitting.value = true;

    if (props.isEdit && props.personelId) {
      // Mevcut personeli güncelle
      await PersonelService.updatePersonel(props.personelId, formData.value);
    } else {
      // Yeni personel oluştur
      await PersonelService.createPersonel(formData.value);
    }

    // Başarılı olduğunda
    emit('saved');
  } catch (error) {
    console.error('Personel kaydedilirken hata oluştu:', error);
    alert('Personel kaydedilirken bir hata oluştu. Lütfen tekrar deneyin.');
  } finally {
    isSubmitting.value = false;
  }
};
</script>
