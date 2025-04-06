<script setup lang="ts">
import TableLayout from "@/components/TableLayout.vue";
import type { IzinTurModel } from "@/models/entity-models/izin/IzinTurModel";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import IzinService from "@/services/IzinService";
import { onMounted, computed, type Ref, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();

const selectedIzinTur: Ref<IzinTurModel | null> = ref(null);
const izinTurler: Ref<IzinTurModel[] | undefined> = ref([]);
const selectedIzinTurler: Ref<IzinTurModel[] | undefined> = ref([]);

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

const filteredIzinTurler = computed<Record<string, unknown>[]>(() => {
  return (selectedIzinTurler.value || []).map(
    ({ id, ad, ucretliMi, limitTipi, createdAt, isActive }) => ({
      id,
      ad,
      ucretliMi: ucretliMi ? "Ücretli" : "Ücretsiz",
      limitTipi,
      createdAt: new Date(createdAt),
      isActive: isActive ? "Aktif" : "Pasif",
    })
  );
});

const getIzinTurler = async () => {
  try {
    const res = await IzinService.getIzinTurler(paginationParams.value);
    izinTurler.value = res!.items;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  }
};

// Geri dön
const goBack = () => {
  router.push({ name: "IzinKurallar" });
};

const addIzinTur = () => {
  if (selectedIzinTur.value && !selectedIzinTurler.value?.includes(selectedIzinTur.value)) {
    selectedIzinTurler.value?.push(selectedIzinTur.value);
  }
};

const removeIzinTur = (item: IzinTurModel) => {
  selectedIzinTurler.value = selectedIzinTurler.value?.filter((p) => p.id != item.id);
};

onMounted(() => {
  getIzinTurler();
});

const setPageNumber = (pageNumber: number) => {
  paginationParams.value.pageNumber = pageNumber;
  getIzinTurler();
};
</script>

<template>
  <div class="flex-1 transition-all duration-300">
    <div class="w-full mt-2 ml-5">
      <div class="flex items-center justify-between">
        <div class="flex">
          <i
            class="fa-solid fa-arrow-left mr-4 cursor-pointer text-xl dark:text-neutral-300 dark:hover:text-neutral-100"
            @click="goBack"
          ></i>
          <h3 class="text-xl">Yeni İzin Kuralı Oluştur</h3>
        </div>
        <button
          type="button"
          class="mr-10 text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
          @click="addIzinTur()"
        >
          Kural Oluştur
        </button>
      </div>
      <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">
        Yeni izin kuralı oluşturabilirsiniz.
      </p>
    </div>

    <div class="mx-10 mt-5 w-1/3">
      <h1 class="text-xl mb-5">Detaylar</h1>
      <div class="mb-2 flex flex-col">
        <label for="ad" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
          >Ad</label
        >
        <input
          type="text"
          name="ad"
          id="ad"
          class="bg-gray-50 border border-neutral-900 text-gray-900 text-sm rounded-lg w-full block p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
          placeholder="Kural1"
          required
        />
      </div>
      <div>
        <label for="aciklama" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
          >Açıklama <span class="text-neutral-400">(Opsiyonel)</span></label
        >
        <textarea
          type="text"
          id="aciklama"
          class="bg-gray-50 border max-h-20 min-h-20 border-gray-300 text-gray-900 w-full text-sm rounded-lg block p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
          placeholder=""
        ></textarea>
      </div>
    </div>
    <div class="mx-10 my-4 flex flex-col">
      <label for="ad" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
        >Tür Ekle</label
      >
      <div class="flex items-start">
        <select
          id="countries"
          class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-1/3 p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
          v-model="selectedIzinTur"
        >
          <option class="text-neutral-800 dark:text-neutral-200" selected>
            Kurala eklemek için tür seçin
          </option>

          <option
            v-for="izinTur in izinTurler"
            :key="izinTur.id"
            :value="izinTur"
            class="text-neutral-800 dark:text-neutral-200"
          >
            {{ izinTur.ad }}
          </option>
        </select>
        <button
          type="button"
          class="ml-5 text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
          @click="addIzinTur()"
        >
          Ekle
        </button>
      </div>
    </div>
    <div class="mx-10 mt-5">
      <h1 class="text-xl mb-5">Türler</h1>
      <div class="mb-2 w-11/12 justify-center">
        <TableLayout
          :table-headers="['Ad', 'Ücret', 'Hak Ediş', 'Oluşturulma Tarihi', 'Durum']"
          :table-content="filteredIzinTurler"
          :islemler="['remove', 'edit']"
          @remove-click="removeIzinTur"
          :page-count="
            Math.ceil(paginationParams.count / paginationParams.pageSize) == 0
              ? 1
              : Math.ceil(paginationParams.count / paginationParams.pageSize)
          "
          :count="paginationParams.count"
          :page-size="
            paginationParams.pageSize > paginationParams.count
              ? paginationParams.count
              : paginationParams.pageSize
          "
          :current-page="paginationParams.pageNumber"
          @set-page="setPageNumber"
        />
      </div>

      <!-- Kaydet ve İptal Butonları -->
      <div class="flex justify-end mt-8 mb-4">
        <button
          @click="goBack"
          class="px-6 py-2.5 bg-gray-200 text-gray-700 dark:bg-neutral-700 dark:text-gray-300 hover:bg-gray-300 dark:hover:bg-neutral-600 rounded-lg transition-colors duration-300 mr-4"
        >
          İptal
        </button>
        <button
          type="button"
          class="px-6 py-2.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-300"
        >
          Kaydet
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
