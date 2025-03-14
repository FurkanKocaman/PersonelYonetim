<template>
  <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-md p-6">
    <h2 class="text-xl font-bold text-gray-800 dark:text-white mb-4">
      {{ isEdit ? 'Personel Düzenle' : 'Yeni Personel Ekle' }}
    </h2>
    
    <form @submit.prevent="savePersonel">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
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
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="departman">
            Departman *
          </label>
          <select 
            id="departman" 
            v-model="formData.departman" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            required
          >
            <option value="" disabled>Departman seçin</option>
            <option v-for="dept in departmanList" :key="dept" :value="dept">{{ dept }}</option>
          </select>
        </div>
        
        <div class="mb-4">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="pozisyon">
            Pozisyon *
          </label>
          <input 
            id="pozisyon" 
            v-model="formData.pozisyon" 
            type="text" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="Pozisyon girin"
            required
          >
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
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="email">
            E-posta *
          </label>
          <input 
            id="email" 
            v-model="formData.email" 
            type="email" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="E-posta adresini girin"
            required
          >
        </div>
        
        <div class="mb-4">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="telefon">
            Telefon
          </label>
          <input 
            id="telefon" 
            v-model="formData.telefon" 
            type="tel" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="Telefon numarasını girin"
          >
        </div>
        
        <div class="mb-4 md:col-span-2">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="adres">
            Adres
          </label>
          <textarea 
            id="adres" 
            v-model="formData.adres" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="Adres girin"
            rows="3"
          ></textarea>
        </div>
        
        <div class="mb-4">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="durum">
            Durum *
          </label>
          <select 
            id="durum" 
            v-model="formData.durum" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            required
          >
            <option v-for="status in durumList" :key="status" :value="status">{{ status }}</option>
          </select>
        </div>
      </div>
      
      <div class="flex justify-end mt-6 space-x-2">
        <button 
          type="button"
          @click="$emit('cancel')" 
          class="bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
        >
          İptal
        </button>
        <button 
          type="submit" 
          class="bg-sky-600 hover:bg-sky-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
        >
          Kaydet
        </button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, defineProps, defineEmits, onMounted } from 'vue';
import type { PersonelRequest } from '@/models/PersonelModels';
import { PersonelService } from '@/services/PersonelService';

const props = defineProps<{
  initialData?: Partial<PersonelRequest>;
  isEdit: boolean;
}>();

const emit = defineEmits<{
  (e: 'save', data: PersonelRequest): void;
  (e: 'cancel'): void;
}>();

// Departman listesi
const departmanList = [
  'Bilgi İşlem',
  'İnsan Kaynakları',
  'Muhasebe',
  'Satış',
  'Pazarlama',
  'Yönetim'
];

// Durum listesi
const durumList = [
  'Aktif',
  'İzinli',
  'Pasif'
];

// Form verisi
const formData = ref<PersonelRequest>({
  ad: '',
  soyad: '',
  departman: '',
  pozisyon: '',
  iseGirisTarihi: '',
  email: '',
  telefon: '',
  adres: '',
  durum: 'Aktif'
});

// Form verilerini başlangıç verileriyle doldur
onMounted(() => {
  if (props.initialData) {
    formData.value = {
      ...formData.value,
      ...props.initialData
    };
  }
});

// Personel kaydını kaydet
const savePersonel = () => {
  emit('save', formData.value);
};
</script>
