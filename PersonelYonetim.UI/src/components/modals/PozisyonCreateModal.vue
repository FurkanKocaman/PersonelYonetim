<script setup lang="ts">
import type { SirketModel } from "@/models/entity-models/SirketModel";
import type { PozisyonCreateRequest } from "@/models/request-models/PozisyonlarCreateRequest";
import PozisyonService from "@/services/PozisyonService";
import { defineProps, type PropType } from "vue";
import { onMounted } from "vue";
import type { PozisyonModel } from "@/models/entity-models/PozisyonModel";

const props = defineProps({
  sirketler: {
    type: Array as PropType<SirketModel[]>,
    required: true,
  },
  editMode: {
    type: Boolean,
    default: false,
  },
  pozisyon: {
    type: Object as PropType<PozisyonModel>,
    default: null,
  },
});

const emit = defineEmits(["closeModal", "pozisyonCreated", "pozisyonUpdated"]);

const request: PozisyonCreateRequest = {
  ad: "",
  aciklama: null,
  sirketId: "",
};

// Düzenleme modunda ise mevcut pozisyon bilgilerini forma dolduruyoruz
onMounted(() => {
  if (props.editMode && props.pozisyon) {
    request.ad = props.pozisyon.ad;
    request.aciklama = props.pozisyon.aciklama || null;
    request.sirketId = props.pozisyon.sirketId.toString();
  }
});

const handlePozisyonCreate = async () => {
  try {
    if (props.editMode && props.pozisyon) {
      await PozisyonService.pozisyonlarUpdate(Number(props.pozisyon.id), request);
      emit("pozisyonUpdated", true);
    } else {
      await PozisyonService.pozisyonlarCreate(request);
      emit("pozisyonCreated", true);
    }

    emit("closeModal", false);
  } catch (error) {
    console.error(error);
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
            {{ props.editMode ? "Pozisyon Düzenle" : "Pozisyon Oluştur" }}
          </h3>
          <button
            type="button"
            class="end-2.5 text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
            @click="emit('closeModal', false)"
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
            <span class="sr-only">Close modal</span>
          </button>
        </div>

        <div class="p-4 md:p-5 w-full">
          <form class="space-y-4 w-full" @submit.prevent="handlePozisyonCreate()">
            <div class="flex">
              <div class="flex flex-col mr-2 w-full">
                <div class="mb-2 flex flex-col">
                  <label
                    for="ad"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Ad</label
                  >
                  <input
                    type="text"
                    name="ad"
                    id="ad"
                    v-model="request.ad"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg min-w-sm block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="Pozisyon1"
                    required
                  />
                </div>
              </div>
            </div>
            <div>
              <label
                for="countries"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Şirket</label
              >
              <select
                id="countries"
                class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                v-model="request.sirketId"
              >
                <option selected>Pozisyonun ekleneceği şirketi seçin</option>
                <option
                  v-for="sirket in props.sirketler"
                  :key="sirket.id"
                  :value="sirket.id"
                  class="text-neutral-800 dark:text-neutral-200"
                >
                  {{ sirket.ad }}
                </option>
              </select>
            </div>

            <div>
              <label
                for="aciklama"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Açıklama <span class="text-neutral-400">(Opsiyonel)</span></label
              >
              <textarea
                type="text"
                name="aciklama"
                id="aciklama"
                v-model="request.aciklama"
                class="bg-gray-50 border max-h-20 min-h-20 border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                placeholder=""
              ></textarea>
            </div>

            <button
              type="submit"
              class="w-full text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
            >
              {{ props.editMode ? "Güncelle" : "Oluştur" }}
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
