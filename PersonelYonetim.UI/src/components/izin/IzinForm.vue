<template>
  <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-md p-6">
    <h2 class="text-xl font-bold text-gray-800 dark:text-white mb-4">
      {{ isEdit ? 'İzin Kaydını Düzenle' : 'Yeni İzin Kaydı Ekle' }}
    </h2>
    
    <form @submit.prevent="saveIzin">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div class="mb-4">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="personelAdi">
            Personel Adı *
          </label>
          <input 
            id="personelAdi" 
            v-model="formData.personelAdi" 
            type="text" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="Personel adını girin"
            required
          >
        </div>
        
        <div class="mb-4">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="izinTipi">
            İzin Tipi *
          </label>
          <select 
            id="izinTipi" 
            v-model="formData.izinTipi" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            required
          >
            <option value="" disabled>İzin tipi seçin</option>
            <option v-for="tip in izinTipleri" :key="tip" :value="tip">{{ tip }}</option>
          </select>
        </div>
        
        <div class="mb-4">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="baslangicTarihi">
            Başlangıç Tarihi *
          </label>
          <input 
            id="baslangicTarihi" 
            v-model="formData.baslangicTarihi" 
            type="date" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            required
            @change="hesaplaGunSayisi"
          >
        </div>
        
        <div class="mb-4">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="bitisTarihi">
            Bitiş Tarihi *
          </label>
          <input 
            id="bitisTarihi" 
            v-model="formData.bitisTarihi" 
            type="date" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            required
            @change="hesaplaGunSayisi"
          >
        </div>
        
        <div class="mb-4">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="gun">
            Gün Sayısı *
          </label>
          <input 
            id="gun" 
            v-model.number="formData.gun" 
            type="number" 
            min="1"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            readonly
          >
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
        
        <div class="mb-4 md:col-span-2">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="aciklama">
            Açıklama
          </label>
          <textarea 
            id="aciklama" 
            v-model="formData.aciklama" 
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="İzin açıklaması girin"
            rows="3"
          ></textarea>
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
import { ref, defineProps, defineEmits, onMounted, watch } from 'vue';
import type { IzinRequest } from '@/models/IzinModels';

const props = defineProps<{
  initialData?: Partial<IzinRequest>;
  isEdit: boolean;
}>();

const emit = defineEmits<{
  (e: 'save', data: IzinRequest): void;
  (e: 'cancel'): void;
}>();

// İzin tipleri listesi
const izinTipleri = [
  'Yıllık İzin',
  'Hastalık İzni',
  'Mazeret İzni',
  'Doğum İzni',
  'Babalık İzni',
  'Evlilik İzni',
  'Ölüm İzni',
  'Ücretsiz İzin'
];

// Durum listesi
const durumList = [
  'Onaylandı',
  'Beklemede',
  'Reddedildi'
];

const formData = ref<IzinRequest>({
  personelAdi: '',
  izinTipi: '',
  baslangicTarihi: new Date().toISOString().split('T')[0],
  bitisTarihi: new Date().toISOString().split('T')[0],
  gun: 1,
  aciklama: '',
  durum: 'Beklemede'
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

// Başlangıç ve bitiş tarihleri değiştiğinde gün sayısını hesapla
watch([() => formData.value.baslangicTarihi, () => formData.value.bitisTarihi], () => {
  hesaplaGunSayisi();
});

// Gün sayısını hesapla
const hesaplaGunSayisi = () => {
  if (formData.value.baslangicTarihi && formData.value.bitisTarihi) {
    const baslangic = new Date(formData.value.baslangicTarihi);
    const bitis = new Date(formData.value.bitisTarihi);
    
    // Bitiş tarihi başlangıç tarihinden önce ise düzelt
    if (bitis < baslangic) {
      formData.value.bitisTarihi = formData.value.baslangicTarihi;
      formData.value.gun = 1;
      return;
    }
    
    // İki tarih arasındaki gün farkını hesapla
    const farkMs = bitis.getTime() - baslangic.getTime();
    const gunFarki = Math.floor(farkMs / (1000 * 60 * 60 * 24)) + 1; // +1 ile başlangıç günü de dahil edilir
    
    formData.value.gun = gunFarki;
  }
};

const saveIzin = () => {
  emit('save', formData.value);
};
</script>
