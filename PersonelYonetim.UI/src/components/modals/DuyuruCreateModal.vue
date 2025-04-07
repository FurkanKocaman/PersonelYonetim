<script setup lang="ts">
import { defineProps, reactive, type PropType, onMounted, type Ref, ref } from "vue";
import type { SirketModel } from "@/models/entity-models/SirketModel";
import type { DuyuruCreateRequest } from "@/models/request-models/DuyuruCreateRequest";
import type { DuyuruModel } from "@/models/entity-models/DuyuruModel";
import DuyuruService from "@/services/DuyuruService";
import SirketService from "@/services/SirketService";

const props = defineProps({
  duyuru: {
    type: Object as PropType<DuyuruModel>,
    default: null,
  },
});
const sirketler: Ref<SirketModel[] | undefined> = ref([]);

const emit = defineEmits(["closeModal", "duyuruCreated", "duyuruUpdated"]);

const request: DuyuruCreateRequest = reactive(
  props.duyuru
    ? JSON.parse(JSON.stringify(props.duyuru))
    : {
        baslik: "",
        aciklama: "",
        sirketId: "",
        aliciTipiValue: 5,
        aliciId: undefined,
        aliciIdler: [],
      }
);

onMounted(() => {
  getSirketler();
});

const handleSubmit = async () => {
  try {
    if (props.duyuru) {
      // await DuyuruService.updateDuyuru(props.duyuru.id, request);
      emit("duyuruUpdated", true);
    } else {
      await DuyuruService.createDuyuru(request);
      emit("duyuruCreated", true);
    }

    emit("closeModal", false);
  } catch (error) {
    console.error(error);
  }
};
const getSirketler = async () => {
  try {
    const res = await SirketService.sirketlerGet();
    sirketler.value = res?.items;
    request.sirketId = sirketler.value![0].id;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  }
};
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
          <h3 class="text-xl font-semibold text-gray-900 dark:text-white">
            {{ props.duyuru ? "Duyuru Düzenle" : "Duyuru Oluştur" }}
          </h3>
          <button
            @click="emit('closeModal', false)"
            type="button"
            class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
          >
            <span class="sr-only">Kapat</span>
            <svg class="w-3 h-3" fill="none" viewBox="0 0 14 14">
              <path
                stroke="currentColor"
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
              />
            </svg>
          </button>
        </div>

        <div class="p-4 md:p-5">
          <form @submit.prevent="handleSubmit" class="space-y-4">
            <div>
              <label class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Başlık</label
              >
              <input
                v-model="request.baslik"
                required
                type="text"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                placeholder="Duyuru başlığı"
              />
            </div>

            <div>
              <label class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Açıklama</label
              >
              <textarea
                v-model="request.aciklama"
                rows="3"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                placeholder="Opsiyonel açıklama"
              ></textarea>
            </div>

            <div>
              <label class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Şirket</label
              >
              <select
                v-model="request.sirketId"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
              >
                <option disabled value="">Şirket seçiniz</option>
                <option v-for="sirket in sirketler" :key="sirket.id" :value="sirket.id">
                  {{ sirket.ad }}
                </option>
              </select>
            </div>

            <div>
              <label class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Alıcı Tipi</label
              >
              <select
                v-model="request.aliciTipiValue"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
              >
                <option :value="0">Personel</option>
                <option :value="1">Departman</option>
                <option :value="2">Şube</option>
                <option :value="3">Şirket</option>
                <option :value="4">Personeller</option>
                <option :value="5">Herkes</option>
              </select>
            </div>

            <div v-if="request.aliciTipiValue === 1">
              <label class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Alıcı (ID)</label
              >
              <input
                v-model="request.aliciId"
                type="text"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                placeholder="Alıcı ID giriniz"
              />
            </div>

            <div v-if="request.aliciTipiValue === 2">
              <label class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Alıcılar (ID)</label
              >
              <input
                v-model="request.aliciIdler"
                type="text"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                placeholder="Virgül ile ID giriniz örn: 1,2,3"
              />
            </div>

            <button
              type="submit"
              class="w-full text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700"
            >
              {{ props.duyuru ? "Güncelle" : "Oluştur" }}
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
