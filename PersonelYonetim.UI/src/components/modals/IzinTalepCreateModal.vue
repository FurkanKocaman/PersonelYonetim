<script setup lang="ts">
import type { IzinTalepCreateCommand } from "@/models/request-models/IzinTalepCreateCommand";
import IzinService from "@/services/IzinService";
import "@vuepic/vue-datepicker/dist/main.css";
import Datepicker from "@vuepic/vue-datepicker";
import { onMounted, reactive, ref, watch, type Ref } from "vue";
import type { IzinTurResponse } from "@/models/entity-models/izin/IzinKuralModel";

const request: IzinTalepCreateCommand = reactive({
  izinTurId: "",
  baslangicTarihi: new Date(),
  bitisTarihi: new Date(),
  aciklama: undefined,
});
// const mesaiBaslangic = ref(new Date());
// const toplamGun: number = ref(0);
const izinTurler: Ref<IzinTurResponse[] | undefined> = ref([]);
const emit = defineEmits(["closeModal"]);
const handleIzinTalepCreate = async () => {
  await IzinService.createIzinTalep(request);

  emit("closeModal", false);
};
onMounted(() => {
  getIzinKural();
});

const getIzinKural = async () => {
  const response = await IzinService.getIzinKural();
  izinTurler.value = response?.IzinKurallar[0].izinTurler;
};

const mesaiBaslangicHesapla = () => {
  if (!request.baslangicTarihi || !request.bitisTarihi) return;

  const diffTime = request.bitisTarihi.getTime() - request.baslangicTarihi.getTime();
  console.log("DiffTime:", diffTime);

  const diffDays = (diffTime / (1000 * 60 * 60 * 24)).toFixed(2); // Gün farkını 2 basamaklı al
  console.log("DiffDays:", diffDays);
};

watch(() => request.baslangicTarihi, mesaiBaslangicHesapla);
watch(() => request.bitisTarihi, mesaiBaslangicHesapla);
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
          <h3 class="text-xl font-semibold text-gray-900 dark:text-white">İzin Talebi Oluştur</h3>
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
          <form class="space-y-4 w-full" @submit.prevent="handleIzinTalepCreate()">
            <div class="flex">
              <div class="flex flex-col mr-2 w-full">
                <div class="mb-2 flex flex-col">
                  <label
                    for="ad"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >İzin Türü</label
                  >
                  <select
                    id="countries"
                    class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
                    v-model="request.izinTurId"
                  >
                    <option
                      class="text-neutral-800 dark:text-neutral-200"
                      :value="undefined"
                      selected
                    >
                      İzin türü seçin
                    </option>
                    <option
                      v-for="izinTur in izinTurler"
                      :key="izinTur.id"
                      :value="izinTur.id"
                      class="text-neutral-800 dark:text-neutral-200 flex justify-between"
                    >
                      <span>{{ izinTur.ad }}</span>
                      <span class="dark:text-red-400 ml-10">
                        Kalan:{{ izinTur.kalanGunSayisi + " gün" }}</span
                      >
                    </option>
                  </select>
                </div>
                <div class="flex">
                  <div class="w-full mr-2">
                    <label for="baslangicTarih" class="block text-sm/5 font-semibold my-2"
                      >Başlangıç Tarihi</label
                    >
                    <Datepicker
                      id="baslangicTarih"
                      locale="TR"
                      v-model="request.baslangicTarihi"
                      :enable-time-picker="true"
                      :format="'dd-MM-yyyy'"
                    />
                  </div>
                  <div class="w-full ml-2">
                    <label for="bitisTarih" class="block text-sm/5 font-semibold my-2"
                      >Bitiş Tarihi</label
                    >
                    <Datepicker
                      id="bitisTarih"
                      locale="TR"
                      v-model="request.bitisTarihi"
                      :enable-time-picker="true"
                      :format="'dd-MM-yyyy'"
                    />
                  </div>
                </div>
                <!-- <div class="flex">
                  <div class="w-full mr-2">
                    <label for="mesaiBaslangicTarih" class="block text-sm/5 font-semibold my-2"
                      >Mesai Başlangıç Tarihi</label
                    >
                    <Datepicker
                      id="mesaiBaslangicTarih"
                      locale="TR"
                      :enable-time-picker="true"
                      :format="'dd-MM-yyyy'"
                      :disabled="true"
                    />
                  </div>
                  <div class="w-full ml-2 flex flex-col justify-center">
                    <label for="isebaslamatarih" class="block text-sm/5 font-semibold my-2"
                      >Toplam Süre</label
                    >
                    <span
                      class="border w-fit px-3 py-1.5 rounded-sm border-neutral-300 dark:border-neutral-600 bg-neutral-200 dark:bg-neutral-700"
                      >5 gün</span
                    >
                  </div>
                </div> -->
              </div>
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
              Oluştur
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
