<script setup lang="ts">
import { ref, type Ref } from "vue";
import "@vuepic/vue-datepicker/dist/main.css";
import Datepicker from "@vuepic/vue-datepicker";
import { TakvimService } from "@/services/TakvimService";
import type { TakvimEtkinlikCreateCommand } from "@/models/request-models/TakvimEtkinlikCreateCommand";

const props = defineProps<{
  editMode?: boolean;
  event?: TakvimEtkinlikCreateCommand;
}>();
const takvimEtkinlikRequest: Ref<TakvimEtkinlikCreateCommand> = ref({
  etkinlikId: "",
  baslik: "",
  aciklama: undefined,
  baslangicTarihi: new Date(),
  bitisTarihi: undefined,
  isPublic: true,
});
const emit = defineEmits(["closeModal", "refresh"]);

const createEtkinlik = async () => {
  await TakvimService.createEtkinlik(takvimEtkinlikRequest.value);
  emit("refresh");
  emit("closeModal", false);
};
const guncelleEtkinlik = async () => {
  await TakvimService.updateEtkinlik(takvimEtkinlikRequest.value);
  emit("refresh");
  emit("closeModal", false);
};

const silEtkinlik = async () => {
  await TakvimService.deleteEtkinlik(takvimEtkinlikRequest.value.etkinlikId);
  emit("refresh");
  emit("closeModal", false);
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
            {{ takvimEtkinlikRequest.etkinlikId == "" ? "Etkinlik Oluştur" : "Etkinlik Düzenle" }}
          </h3>
          <button
            type="button"
            class="end-2.5 text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
            @click="
              () => {
                emit('closeModal', false);
              }
            "
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
          <form
            class="space-y-4 w-full"
            @submit.prevent="props.editMode ? guncelleEtkinlik() : createEtkinlik()"
          >
            <div class="flex">
              <div class="flex flex-col mr-2 w-full">
                <div class="mb-2 flex flex-col">
                  <div class="flex justify-between">
                    <label
                      for="baslik"
                      class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                      >Etkinlik Baslığı</label
                    >
                    <button
                      v-if="props.editMode"
                      type="button"
                      class="text-red-700 hover:text-white border border-red-700 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 text-center me-2 mb-2 dark:border-red-500 dark:text-red-500 dark:hover:text-white dark:hover:bg-red-600 dark:focus:ring-red-900"
                      @click="silEtkinlik()"
                    >
                      Etkinliği sil
                    </button>
                  </div>
                  <input
                    type="text"
                    name="baslik"
                    id="baslik"
                    v-model="takvimEtkinlikRequest.baslik"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="Toplantı"
                    required
                  />
                </div>
                <div>
                  <label
                    for="aciklama"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Açıklama <span class="text-neutral-400">(Opsiyonel)</span></label
                  >
                  <textarea
                    type="text"
                    id="aciklama"
                    v-model="takvimEtkinlikRequest.aciklama"
                    class="bg-gray-50 border max-h-20 min-h-20 border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder=""
                  ></textarea>
                </div>
                <div class="flex">
                  <div class="w-full mr-2">
                    <label for="baslangicTarih" class="block text-sm/5 font-semibold my-2"
                      >Başlangıç Tarihi</label
                    >
                    <Datepicker
                      id="baslangicTarih"
                      locale="TR"
                      v-model="takvimEtkinlikRequest.baslangicTarihi"
                      :enable-time-picker="true"
                      :format="'dd-MM-yyyy'"
                    />
                  </div>
                  <div class="w-full mr-2">
                    <label for="baslangicTarih" class="block text-sm/5 font-semibold my-2"
                      >Bitiş Tarihi</label
                    >
                    <Datepicker
                      id="baslangicTarih"
                      locale="TR"
                      v-model="takvimEtkinlikRequest.bitisTarihi"
                      :enable-time-picker="true"
                      :format="'dd-MM-yyyy'"
                    />
                  </div>
                </div>
              </div>
            </div>
            <div>
              <label class="inline-flex items-center cursor-pointer">
                <input
                  type="checkbox"
                  v-model="takvimEtkinlikRequest.isPublic"
                  class="sr-only peer"
                  checked
                />
                <div
                  class="relative w-11 h-6 bg-gray-200 rounded-full peer peer-focus:ring-4 peer-focus:ring-blue-300 dark:peer-focus:ring-blue-800 dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-0.5 after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                ></div>
                <span class="ms-3 text-sm font-medium text-gray-900 dark:text-gray-300"
                  >Herkese açık</span
                >
              </label>
            </div>
            <div v-if="!takvimEtkinlikRequest.isPublic">
              <label
                for="aciklama"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Katılımcılar <span class="text-neutral-400">(Opsiyonel)</span></label
              >
              <textarea
                type="text"
                name="aciklama"
                id="aciklama"
                class="bg-gray-50 border max-h-20 min-h-20 border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                placeholder=""
              ></textarea>
            </div>

            <button
              type="submit"
              class="w-full text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
            >
              {{ takvimEtkinlikRequest.etkinlikId == "" ? "Oluştur" : "Güncelle" }}
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
