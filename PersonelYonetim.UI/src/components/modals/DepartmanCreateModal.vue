<script setup lang="ts">
import type { DepartmanCreateRequest } from "@/models/request-models/DepartmanCreateCommand";
import type { SubeModel } from "@/models/entity-models/SubeModel";
import DepartmanService from "@/services/DepartmanService";
import type { PropType } from "vue";
import { onMounted, reactive } from "vue";
import type { DepartmanModel } from "@/models/entity-models/DepartmanModel";

const props = defineProps({
  subeler: {
    type: Array as PropType<SubeModel[]>,
    required: true,
  },
  editMode: {
    type: Boolean,
    default: false,
  },
  departman: {
    type: Object as PropType<DepartmanModel>,
    default: null,
  },
});

const emit = defineEmits(["closeModal", "departmanCreated", "departmanUpdated"]);

const request: DepartmanCreateRequest = reactive({
  ad: "",
  aciklama: null,
  subeId: "",
  iletisim: undefined,
});

onMounted(() => {
  if (props.editMode && props.departman) {
    request.ad = props.departman.ad;
    request.aciklama = props.departman.aciklama || null;
    request.subeId = props.departman.subeId.toString();
  }
});

const handleDepartmanCreate = async () => {
  try {
    if (props.editMode && props.departman) {
      await DepartmanService.departmanlarUpdate(props.departman.id, request);
    } else {
      await DepartmanService.departmanlarCreate(request);
    }

    emit("closeModal", false);
  } catch (error) {
    console.error(error);
  }
};
</script>
<template>
  <div
    class="overflow-y-auto overflow-x-hidden fixed flex justify-center items-center top-0 right-0 left-0 z-10 backdrop-blur-sm bg-black/30 w-full h-full"
  >
    <div class="relative p-4 max-w-4xl w-full max-h-full">
      <div class="relative bg-white rounded-lg shadow-sm dark:bg-neutral-800 w-full">
        <div
          class="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600 border-gray-200"
        >
          <h3 class="text-xl font-semibold text-gray-900 dark:text-white">
            {{ props.editMode ? "Departman Düzenle" : "Departman Oluştur" }}
          </h3>
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
            <span class="sr-only">Close modal</span>
          </button>
        </div>

        <div class="p-4 md:p-5 w-full">
          <form class="space-y-4 w-full" @submit.prevent="handleDepartmanCreate()">
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
                    placeholder="Departman1"
                    required
                  />
                </div>
                <div class="mb-2">
                  <label
                    for="email"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Departman E-posta <span class="text-neutral-400">(Opsiyonel)</span></label
                  >
                  <input
                    type="email"
                    name="email"
                    id="email"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="info@sirket1.com"
                  />
                </div>
                <div class="mb-2">
                  <label
                    for="telefon"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Departman Telefon <span class="text-neutral-400">(Opsiyonel)</span></label
                  >
                  <input
                    type="phone"
                    name="telefon"
                    id="telefon"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="0850 000 00 00"
                  />
                </div>
              </div>
            </div>
            <div>
              <label
                for="countries"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Şube</label
              >
              <select
                id="countries"
                class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                v-model="request.subeId"
              >
                <option class="text-neutral-800 dark:text-neutral-200" selected>
                  Departmanın bulunduğu şubeyi seçin
                </option>
                <option
                  v-for="sube in props.subeler"
                  :key="sube.id"
                  :value="sube.id"
                  class="text-neutral-800 dark:text-neutral-200"
                >
                  {{ sube.ad }}
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
