<template>
  <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-md p-6">
    <h2 class="text-xl font-bold text-gray-800 dark:text-white mb-4">
      {{ isEdit ? "Maaş Kaydını Düzenle" : "Yeni Maaş Kaydı Ekle" }}
    </h2>

    <form @submit.prevent="saveMaas">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div class="mb-4">
          <label
            class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2"
            for="personelAdi"
          >
            Personel Adı *
          </label>
          <input
            id="personelAdi"
            v-model="formData.personelAdi"
            type="text"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="Personel adını girin"
            required
          />
        </div>

        <div class="mb-4">
          <label
            class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2"
            for="departman"
          >
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
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="maas">
            Maaş (TL) *
          </label>
          <input
            id="maas"
            v-model.number="formData.maas"
            type="number"
            min="0"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="Maaş miktarını girin"
            required
          />
        </div>

        <div class="mb-4">
          <label
            class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2"
            for="odenmeTarihi"
          >
            Ödenme Tarihi *
          </label>
          <input
            id="odenmeTarihi"
            v-model="formData.odenmeTarihi"
            type="date"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            required
          />
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
import { ref, defineProps, defineEmits, onMounted } from "vue";
import type { MaasRequest } from "@/models/entity-models/MaasModels";
import { MaasService } from "@/services/MaasService";

const props = defineProps<{
  initialData?: Partial<MaasRequest>;
  isEdit: boolean;
}>();

const emit = defineEmits<{
  (e: "save", data: MaasRequest): void;
  (e: "cancel"): void;
}>();

// Departman listesi
const departmanList = [
  "Bilgi İşlem",
  "İnsan Kaynakları",
  "Muhasebe",
  "Satış",
  "Pazarlama",
  "Yönetim",
];

// Durum listesi
const durumList = ["Ödendi", "Beklemede", "İptal Edildi"];

// Form verisi
const formData = ref<MaasRequest>({
  personelAdi: "",
  departman: "",
  maas: 0,
  odenmeTarihi: "",
  durum: "Beklemede",
});

// Form verilerini başlangıç verileriyle doldur
onMounted(() => {
  if (props.initialData) {
    formData.value = {
      ...formData.value,
      ...props.initialData,
    };
  }
});

// Maaş kaydını kaydet
const saveMaas = () => {
  emit("save", formData.value);
};
</script>
