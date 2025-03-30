<script setup lang="ts">
import TableLayout from "@/components/TableLayout.vue";
import type { IzinTurModel } from "@/models/entity-models/izin/IzinTurModel";
import IzinService from "@/services/IzinService";
import { onMounted, computed, type Ref, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();

const izinTurler: Ref<IzinTurModel[] | undefined> = ref([]);

const filteredIzinTurler = computed<Record<string, unknown>[]>(() => {
  return (izinTurler.value || []).map(
    ({ ad, ucretliMi, limitTipi, limitGunSayisi, createUserName, createdAt, isActive }) => ({
      ad,
      ucretliMi,
      limitTipi,
      limitGunSayisi,
      createUserName,
      createdAt: new Date(createdAt).toDateString(),
      isActive: isActive ? "Aktif" : "Pasif",
    })
  );
});

const getIzinTurler = async () => {
  try {
    const res = await IzinService.getIzinTurler();
    console.log(res);
    izinTurler.value = res?.IzinTurler;
  } catch (error) {
    console.error("Veri çekme hatası:", error);
  }
};

// Geri dön
const goBack = () => {
  router.push({ name: 'IzinKurallariKurallar' });
};

onMounted(() => {
  getIzinTurler();
});
</script>

<template>
  <div class="flex-1 transition-all duration-300">
    <div class="w-full mt-2 ml-5">
      <div class="flex items-center justify-start">
        <i
          class="fa-solid fa-arrow-left mr-4 cursor-pointer text-xl dark:text-neutral-300 dark:hover:text-neutral-100"
          @click="goBack"
        ></i>
        <h3 class="text-xl">Yeni Kural Oluştur</h3>
      </div>
      <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">Yeni kural oluşturabilirsiniz.</p>
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
    <div class="mx-10 mt-5">
      <h1 class="text-xl mb-5">Türler</h1>
      <div class="mb-2 w-11/12 justify-center">
        <TableLayout
          :table-headers="['Ad', 'Ücretli/Ücretsiz', 'Hak Ediş', 'Durum']"
          :table-content="[
            { ad: 'tür1', ücretliMi: 'Ücretli', hakEdis: 'Yılda 14 gün', durum: 'Aktif' },
          ]"
          :islemler="['edit', 'detaylar']"
        />
      </div>
      <div class="mb-2 flex flex-col">
        <label for="ad" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
          >Tür Ekle</label
        >
        <div class="flex items-start">
          <select
            id="countries"
            class="bg-gray-50 border border-gray-300 text-neutral-900 text-sm rounded-lg block w-1/3 p-2.5 dark:bg-neutral-700 dark:border-neutral-600 dark:placeholder-gray-400 dark:text-white focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none"
          >
            <option class="text-neutral-800 dark:text-neutral-200" selected>
              Kurala eklemek için tür seçin
            </option>
            <option class="text-neutral-800 dark:text-neutral-200">Yıllık izin</option>
            <option class="text-neutral-800 dark:text-neutral-200">Mazeret İzni</option>
          </select>
          <button
            type="button"
            class="ml-5 text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
          >
            Ekle
          </button>
        </div>
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
