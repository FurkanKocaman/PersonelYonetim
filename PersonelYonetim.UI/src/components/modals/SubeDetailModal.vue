<script setup lang="ts">
import type { SubeModel } from "@/models/entity-models/SubeModel";
import { ref } from "vue";

// Seçilen şube bilgilerini props olarak alıyoruz
const props = defineProps<{
  sube: SubeModel;
}>();

// Modal'ı kapatmak ve şube bilgilerini düzenlemek için emit tanımlıyoruz
defineEmits<{
  (event: "closeModal", closeModal: boolean): void;
  (event: "editSube", sube: SubeModel): void;
}>();
</script>

<template>
  <div
    class="overflow-y-auto overflow-x-hidden fixed flex justify-center items-center top-0 right-0 left-0 z-50 backdrop-blur-sm bg-black/30 w-full h-full"
  >
    <div class="relative p-4 max-w-4xl w-full max-h-full">
      <div class="relative bg-white rounded-lg shadow-sm dark:bg-neutral-800 w-full">
        <div
          class="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600 border-gray-200"
        >
          <h3 class="text-xl font-semibold text-gray-900 dark:text-white">Şube Detayları</h3>
          <button
            type="button"
            class="end-2.5 text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
            @click="$emit('closeModal', false)"
          >
            <svg
              class="w-3 h-3"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 14 14"
            >
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
              />
            </svg>
            <span class="sr-only">Kapat</span>
          </button>
        </div>
        
        <div class="p-4 md:p-5 w-full">
          <div class="space-y-4 w-full">
            <!-- Şube Bilgileri -->
            <div class="flex">
              <div class="flex flex-col mr-2 w-full">
                <div class="mb-4">
                  <h4 class="text-lg font-medium text-gray-900 dark:text-white mb-2">Genel Bilgiler</h4>
                  <div class="grid grid-cols-2 gap-4">
                    <div>
                      <p class="text-sm font-medium text-gray-500 dark:text-gray-400">Şube Adı</p>
                      <p class="text-base text-gray-900 dark:text-white">{{ props.sube.ad }}</p>
                    </div>
                    <div>
                      <p class="text-sm font-medium text-gray-500 dark:text-gray-400">Şirket</p>
                      <p class="text-base text-gray-900 dark:text-white">{{ props.sube.sirketAd }}</p>
                    </div>
                    <div>
                      <p class="text-sm font-medium text-gray-500 dark:text-gray-400">Durum</p>
                      <p class="text-base text-gray-900 dark:text-white">
                        <span 
                          :class="props.sube.isActive ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300' : 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300'" 
                          class="px-2 py-1 rounded-full text-xs font-medium"
                        >
                          {{ props.sube.isActive ? 'Aktif' : 'Pasif' }}
                        </span>
                      </p>
                    </div>
                    <div>
                      <p class="text-sm font-medium text-gray-500 dark:text-gray-400">Oluşturulma Tarihi</p>
                      <p class="text-base text-gray-900 dark:text-white">{{ new Date(props.sube.createdAt).toLocaleDateString('tr-TR') }}</p>
                    </div>
                    <div>
                      <p class="text-sm font-medium text-gray-500 dark:text-gray-400">Oluşturan</p>
                      <p class="text-base text-gray-900 dark:text-white">{{ props.sube.createUserName }}</p>
                    </div>
                  </div>
                </div>
                
                <div class="mb-4">
                  <h4 class="text-lg font-medium text-gray-900 dark:text-white mb-2">İletişim Bilgileri</h4>
                  <div class="grid grid-cols-2 gap-4">
                    <div>
                      <p class="text-sm font-medium text-gray-500 dark:text-gray-400">E-posta</p>
                      <p class="text-base text-gray-900 dark:text-white">{{ props.sube.iletisim.eposta }}</p>
                    </div>
                    <div>
                      <p class="text-sm font-medium text-gray-500 dark:text-gray-400">Telefon</p>
                      <p class="text-base text-gray-900 dark:text-white">{{ props.sube.iletisim.telefon }}</p>
                    </div>
                  </div>
                </div>
              </div>
              
              <div class="flex flex-col ml-2 w-full">
                <div class="mb-4">
                  <h4 class="text-lg font-medium text-gray-900 dark:text-white mb-2">Adres Bilgileri</h4>
                  <div class="grid grid-cols-2 gap-4">
                    <div>
                      <p class="text-sm font-medium text-gray-500 dark:text-gray-400">Ülke</p>
                      <p class="text-base text-gray-900 dark:text-white">{{ props.sube.adres.ulke }}</p>
                    </div>
                    <div>
                      <p class="text-sm font-medium text-gray-500 dark:text-gray-400">Şehir</p>
                      <p class="text-base text-gray-900 dark:text-white">{{ props.sube.adres.sehir }}</p>
                    </div>
                    <div>
                      <p class="text-sm font-medium text-gray-500 dark:text-gray-400">İlçe</p>
                      <p class="text-base text-gray-900 dark:text-white">{{ props.sube.adres.ilce }}</p>
                    </div>
                  </div>
                  <div class="mt-2">
                    <p class="text-sm font-medium text-gray-500 dark:text-gray-400">Tam Adres</p>
                    <p class="text-base text-gray-900 dark:text-white">{{ props.sube.adres.tamAdres }}</p>
                  </div>
                </div>
                
                <div v-if="props.sube.aciklama" class="mb-4">
                  <h4 class="text-lg font-medium text-gray-900 dark:text-white mb-2">Açıklama</h4>
                  <p class="text-base text-gray-900 dark:text-white">{{ props.sube.aciklama }}</p>
                </div>
              </div>
            </div>
            
            <!-- Düğmeler -->
            <div class="flex justify-end space-x-2 pt-4 border-t dark:border-gray-600 border-gray-200">
              <button
                type="button"
                class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                @click="$emit('editSube', props.sube)"
              >
                Düzenle
              </button>
              <button
                type="button"
                class="text-gray-500 bg-white hover:bg-gray-100 focus:ring-4 focus:outline-none focus:ring-gray-200 rounded-lg border border-gray-200 text-sm font-medium px-5 py-2.5 hover:text-gray-900 focus:z-10 dark:bg-gray-700 dark:text-gray-300 dark:border-gray-500 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-600"
                @click="$emit('closeModal', false)"
              >
                Kapat
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
