<script setup lang="ts">
import type { SubeCreateRequest } from "@/models/request-models/SubeCreateRequest";
import type { SirketModel } from "@/models/entity-models/SirketModel";
import type { SubeModel } from "@/models/entity-models/SubeModel";

import SubeService from "@/services/SubeService";
import { reactive, onMounted, type PropType, ref } from "vue";

const props = defineProps({
  sirketler: {
    type: Array as PropType<SirketModel[]>,
    required: true,
  },
  editMode: {
    type: Boolean,
    default: false,
  },
  sube: {
    type: Object as PropType<SubeModel>,
    default: null,
  },
});

const deleteSubeModal = ref(false);

const emit = defineEmits(["closeModal", "refresh"]);

const request: SubeCreateRequest = reactive({
  ad: "",
  aciklama: null,
  sirketId: "",
  adres: {
    ulke: "Türkiye",
    sehir: "",
    ilce: "",
    tamAdres: "",
  },
  iletisim: {
    eposta: "",
    telefon: "",
  },
});

onMounted(() => {
  if (props.editMode && props.sube) {
    request.ad = props.sube.ad;
    request.aciklama = props.sube.aciklama || null;
    request.sirketId = props.sube.sirketId.toString();
    request.adres.ulke = props.sube.adres.ulke;
    request.adres.sehir = props.sube.adres.sehir;
    request.adres.ilce = props.sube.adres.ilce;
    request.adres.tamAdres = props.sube.adres.tamAdres;
    request.iletisim.eposta = props.sube.iletisim.eposta;
    request.iletisim.telefon = props.sube.iletisim.telefon;
  }
});

const handleSubeCreate = async () => {
  try {
    if (props.editMode && props.sube) {
      await SubeService.subelerUpdate(props.sube.id, request);
    } else {
      await SubeService.subelerCreate(request);
    }

    emit("refresh");
    emit("closeModal", false);
  } catch (error) {
    console.error("Şube işlemi sırasında hata oluştu:", error);
  }
};
const handleSubeDelete = async () => {
  if (props.sube) {
    await SubeService.subelerDelete(props.sube.id);
    emit("refresh");
    emit("closeModal", false);
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
            {{ props.editMode ? "Şube Düzenle" : "Şube Oluştur" }}
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
          <form class="space-y-4 w-full" @submit.prevent="handleSubeCreate()">
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
                    class="bg-gray-50 border border-neutral-900 text-gray-900 text-sm rounded-lg min-w-sm block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="sube1"
                    required
                  />
                </div>
                <div class="mb-2">
                  <label
                    for="email"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Şube E-posta</label
                  >
                  <input
                    type="email"
                    name="email"
                    id="email"
                    v-model="request.iletisim.eposta"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="mail@examle.com"
                    required
                  />
                </div>
                <div class="mb-2">
                  <label
                    for="telefon"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Şube Telefon</label
                  >
                  <input
                    type="phone"
                    name="telefon"
                    id="telefon"
                    v-model="request.iletisim.telefon"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="0850 000 00 00"
                    required
                  />
                </div>
              </div>

              <div class="flex flex-col ml-2 w-full">
                <div class="mb-2 flex flex-col">
                  <label
                    for="sehir"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Sehir</label
                  >
                  <input
                    type="text"
                    name="sehir"
                    id="sehir"
                    v-model="request.adres.sehir"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg min-w-sm block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="Trabzon"
                    required
                  />
                </div>
                <div class="mb-2">
                  <label
                    for="ilce"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >İlçe</label
                  >
                  <input
                    type="text"
                    name="ilce"
                    id="ilce"
                    v-model="request.adres.ilce"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="Ortahisar"
                    required
                  />
                </div>
                <div class="mb-2">
                  <label
                    for="tam-adres"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Tam Adres</label
                  >
                  <input
                    type="text"
                    name="tam-adres"
                    id="tam-adres"
                    v-model="request.adres.tamAdres"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
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
                <option class="text-neutral-800 dark:text-neutral-200" selected>
                  Departmanın bulunduğu şubeyi seçin
                </option>
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

            <div
              class="flex items-center"
              :class="props.editMode ? 'justify-between' : 'justify-end'"
            >
              <button
                type="button"
                v-if="props.editMode"
                class="text-red-700 hover:text-white border border-red-700 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-red-500 dark:text-red-500 dark:hover:text-white dark:hover:bg-red-600 dark:focus:ring-red-900"
                @click.self="
                  () => {
                    deleteSubeModal = true;
                  }
                "
              >
                Şubeyi sil
              </button>

              <button
                type="submit"
                class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
              >
                {{ props.editMode ? "Güncelle" : "Oluştur" }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>

  <div class="relative z-10" v-if="deleteSubeModal">
    <div class="fixed inset-0 bg-gray-500/75 transition-opacity"></div>

    <div class="fixed inset-0 z-10 w-screen overflow-y-auto">
      <div class="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
        <div
          class="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg"
        >
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="sm:flex sm:items-start">
              <div
                class="mx-auto flex size-12 shrink-0 items-center justify-center rounded-full bg-red-100 sm:mx-0 sm:size-10"
              >
                <svg
                  class="size-6 text-red-600"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke-width="1.5"
                  stroke="currentColor"
                  aria-hidden="true"
                  data-slot="icon"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126ZM12 15.75h.007v.008H12v-.008Z"
                  />
                </svg>
              </div>
              <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left">
                <h3 class="text-base font-semibold text-gray-900" id="modal-title">Şubeyi Sil</h3>
                <div class="mt-2">
                  <p class="text-sm text-gray-500">
                    {{
                      props.sube.ad +
                      " isimli şubeyi silmek istediğinize emin misiniz? Bu şubeye bağlı departmanlar da silinecektir."
                    }}
                  </p>
                </div>
              </div>
            </div>
          </div>
          <div class="bg-gray-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6">
            <button
              type="button"
              class="inline-flex w-full justify-center rounded-md bg-red-600 px-10 py-2 text-sm font-semibold text-white shadow-xs hover:bg-red-500 sm:ml-3 sm:w-auto"
              @click.stop="handleSubeDelete()"
            >
              Sil
            </button>
            <button
              type="button"
              class="mt-3 inline-flex w-full justify-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 ring-1 shadow-xs ring-gray-300 ring-inset hover:bg-gray-50 sm:mt-0 sm:w-auto"
              @click="deleteSubeModal = false"
            >
              İptal
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
