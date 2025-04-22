<script setup lang="ts">
import { reactive, onMounted, type Ref, ref } from "vue";
import type { KurumsalBirimModel } from "@/models/entity-models/KurumsalBirimModel";
import type { KurumsalBirimCreateCommand } from "@/models/request-models/KurumsalBirimCreateCommand";
import KurumsalBirimService from "@/services/KurumsalBirimService";
import type { UstBirimDto } from "@/models/entity-models/UstBirimDto";

const props = defineProps<{
  editMode?: boolean;
  birim: KurumsalBirimModel | undefined;
  selectedKurumsalBirimTip: string;
}>();

const emit = defineEmits(["closeModal", "refresh"]);

const request: KurumsalBirimCreateCommand = reactive(
  props.birim
    ? JSON.parse(JSON.stringify(props.birim))
    : {
        ad: "",
        kod: undefined,
        birimTipiId: props.selectedKurumsalBirimTip,
        ustBirimId: undefined,
        tenantId: undefined,
      }
);

const ustBirimler: Ref<UstBirimDto[]> = ref([]);

onMounted(() => {
  if (props.editMode && props.birim) {
    console.log(props.birim);
    request.id = props.birim.id;
    request.ad = props.birim.ad;
    request.logoUrl = props.birim.logoUrl || undefined;
    request.kod = props.birim.kod;
    request.birimTipiId = props.birim.birimTipiId;
    request.ustBirimId = props.birim.ustBirimId;
  }

  getUstBirimler();
});

const getUstBirimler = async () => {
  const res = await KurumsalBirimService.getUstBirimler(props.selectedKurumsalBirimTip);
  ustBirimler.value = res;
  if (ustBirimler.value && !props.editMode) request.ustBirimId = ustBirimler.value[0].birimId;
};

const handleBirimCreate = async () => {
  try {
    if (props.editMode && props.birim) {
      await KurumsalBirimService.birimUpdate(
        props.birim.id,
        request.ad,
        request.kod,
        request.ustBirimId
      );
    } else {
      await KurumsalBirimService.birimCreate(request);
    }
    emit("refresh");
    emit("closeModal", false);
  } catch (error) {
    console.error("Şirket işlemi sırasında hata oluştu:", error);
  }
};

const handleBirimDelete = async () => {
  if (props.birim) {
    await KurumsalBirimService.birimDelete(props.birim.id);
    emit("refresh");
    emit("closeModal", false);
  }
};
</script>
<template>
  <div
    class="overflow-y-auto overflow-x-hidden fixed flex justify-center items-center top-0 right-0 left-0 z-20 backdrop-blur-sm bg-black/30 w-full h-full"
  >
    <div class="relative p-4 max-w-4xl w-full max-h-full">
      <div class="relative bg-neutral-100 rounded-lg dark:bg-neutral-800 w-full">
        <div
          class="flex items-center justify-between p-3 md:p-3 border-b rounded-t dark:border-gray-600 border-gray-200"
        >
          <h3
            class="text-base font-semibold text-neutral-700 dark:text-neutral-300 flex items-center"
          >
            <svg
              class="size-5 mx-2"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M4 12H20M12 4V20"
                class="stroke-neutral-600 dark:stroke-neutral-300 stroke-2"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
            </svg>
            {{ props.editMode ? "Birim Düzenle" : " Yeni Birim Ekle" }}
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
            <span class="sr-only">Kapat</span>
          </button>
        </div>

        <div class="p-4 md:p-5 w-full">
          <form class="space-y-4 w-full" @submit.prevent="handleBirimCreate()">
            <div class="flex">
              <div class="flex flex-col mr-2 w-full">
                <div class="mb-2 flex flex-col">
                  <label
                    for="ad"
                    class="block mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300"
                    >Birim Adı</label
                  >
                  <input
                    type="text"
                    name="ad"
                    id="ad"
                    v-model="request.ad"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg min-w-sm block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="yeni birim"
                    required
                  />
                </div>
                <!-- <div class="mb-2">
                  <label
                    for="email"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Şirket E-posta</label
                  >
                  <input
                    type="email"
                    name="email"
                    id="email"
                    v-model="request.iletisim.eposta"
                    class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                    placeholder="info@sirket1.com"
                    required
                  />
                </div> -->
                <!-- <div class="mb-2">
                  <label
                    for="telefon"
                    class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                    >Şirket Telefon</label
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
                </div> -->
              </div>

              <div class="flex flex-col ml-2 w-full">
                <div class="mb-2 flex flex-col">
                  <label
                    for="ustBirimId"
                    class="block mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300"
                    >Bağlı Olduğu üst birim (Opsiyonel)</label
                  >
                  <select
                    v-model="request.ustBirimId"
                    class="w-full px-4 py-2.5 border border-gray-300 rounded-lg focus:ring-blue-500 focus:border-blue-500 dark:bg-neutral-700 dark:border-neutral-600 dark:text-white"
                  >
                    <option
                      v-for="birim in ustBirimler"
                      :key="birim.birimId"
                      :value="birim.birimId"
                    >
                      {{ birim.birimAd }}
                    </option>
                  </select>
                </div>
                <!-- <div class="mb-2 flex flex-col">
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
                </div> -->
              </div>
            </div>

            <div>
              <label
                class="block mb-2 text-sm font-medium text-neutral-700 dark:text-neutral-300"
                for="file_input"
                >Birim Logo</label
              >
              <input
                class="block w-full text-sm text-neutral-700 dark:text-neutral-300 border border-gray-300 rounded-lg cursor-pointer p-3 bg-gray-50 focus:outline-none dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400"
                id="file_input"
                type="file"
              />
              <p class="mt-1 text-sm text-gray-500 dark:text-gray-300" id="file_input_help">
                SVG, PNG, JPG or GIF (MAX 4MB).
              </p>
            </div>

            <!-- <div>
              <label
                for="aciklama"
                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >Açıklama <span class="text-neutral-400">(Opsiyonel)</span></label
              >
              <textarea
                type="text"
                v-model="request.aciklama"
                id="aciklama"
                class="bg-gray-50 border max-h-20 min-h-20 border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                placeholder=""
              ></textarea>
            </div> -->
            <div
              class="flex items-center"
              :class="props.editMode ? 'justify-between' : 'justify-end'"
            >
              <button
                v-if="props.editMode"
                class="text-red-700 hover:text-white border border-red-700 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-red-500 dark:text-red-500 dark:hover:text-white dark:hover:bg-red-600 dark:focus:ring-red-900"
                @click.stop="handleBirimDelete()"
              >
                Birimi sil
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
</template>
