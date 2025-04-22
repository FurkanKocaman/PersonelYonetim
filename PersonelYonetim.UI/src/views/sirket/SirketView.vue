<script setup lang="ts">
import { onMounted, ref, type Ref } from "vue";
import TableLayout from "@/components/TableLayout.vue";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { KurumsalBirimTipiGetModel } from "@/models/response-models/KurumsalBirimTipleriGetModel";
import type { KurumsalBirimGetModel } from "@/models/response-models/KurumsalBirimGetModel";
import KurumsalBirimCreateModal from "@/components/modals/KurumsalBirimCreateModal.vue";
import KurumsalBirimTipiService from "@/services/KurumsalBirimTipiService";
import KurumsalBirimTipiCreateModal from "@/components/modals/KurumsalBirimTipiCreateModal.vue";

const expand = ref<Record<string, boolean>>({});
const loading = ref<Record<string, boolean>>({});
const count = ref<Record<string, number>>({});
const showModal = ref<Record<string, boolean>>({});
const showBirimTipiOption = ref<Record<string, boolean>>({});
const editMode = ref(false);

const yeniBirimTipHiyerarsiSeviyesi = ref(0);
const showYeniBirimTipiCreateModal = ref(false);

const kurumsalBirimTipleri: Ref<KurumsalBirimTipiGetModel[] | undefined> = ref([]);
const selectedKurumsalBirimTipi: Ref<KurumsalBirimTipiGetModel | undefined> = ref(undefined);
const selectedKurumsalBirim: Ref<KurumsalBirimGetModel | undefined> = ref(undefined);

const paginationParamsSirket: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

onMounted(async () => {
  getKurumsalBirimTipleri();
});

const getKurumsalBirimTipleri = async () => {
  const res = await KurumsalBirimTipiService.kurumsalBirimTipleriGet();
  kurumsalBirimTipleri.value = res?.items;
  for (const item of res!.items) {
    const key = item.id;
    expand.value[key] = false;
    loading.value[key] = false;
    count.value[key] = 0;
    showModal.value[key] = false;
    showBirimTipiOption.value[key] = false;
  }
};

function filteredkurumsalBirimler<T extends KurumsalBirimGetModel>(items: T[]) {
  return items.map(({ id, ad, kod, personelCount, createdAt, isActive }) => ({
    id,
    ad,
    kod,
    personelCount,
    createdAt: new Date(createdAt),
    isActive: isActive ? "Aktif" : "Pasif",
  }));
}

const openBirimModal = (birim: KurumsalBirimGetModel) => {
  editMode.value = true;
  selectedKurumsalBirim.value = kurumsalBirimTipleri.value
    ?.find((p) => p.kurumsalBirimler.find((x) => x.id == birim.id))
    ?.kurumsalBirimler.find((p) => p.id == birim.id);
  if (selectedKurumsalBirim.value) {
    showModal.value[selectedKurumsalBirim.value.birimTipiId] = true;
  }
};
// const setPageNumber = (pageNumber: number) => {
//   if (pageNumber != paginationParamsSirket.value.pageNumber) {
//     paginationParamsSirket.value.pageNumber = pageNumber;
//   }
// };

const deleteKurumsalBirimTipi = async (id: string) => {
  const res = await KurumsalBirimTipiService.kurumsalBirimlerDelete(id);
  console.log(res);
  getKurumsalBirimTipleri();
};

const closeAllBirimTipiOptions = () => {
  for (const item of kurumsalBirimTipleri.value!) {
    const key = item.id;
    showBirimTipiOption.value[key] = false;
  }
};
</script>

<template>
  <div
    class="flex flex-col h-full"
    @click="
      () => {
        closeAllBirimTipiOptions();
      }
    "
  >
    <div class="w-full mt-2 ml-5">
      <p class="text-sm text-gray-500 dark:text-gray-400 mt-2">
        Şirketteki birimleri buradan düzenleyebilirsiniz.
      </p>
    </div>
    <div class="flex w-full h-full divide-x dark:divide-neutral-700 divide-neutral-300">
      <div class="flex-1 flex flex-col mt-5">
        <h1 class="mx-2 w-full text-base dark:text-neutral-300">Şirket Yapısı</h1>
        <div v-for="kurumsalBirimTipi in kurumsalBirimTipleri" :key="kurumsalBirimTipi.id">
          <div
            class="mx-2 my-2 p-1 bg-neutral-200/30 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300 select-none"
          >
            <div class="flex justify-between items-center p-2">
              <div class="flex items-center">
                <span class="text-base font-medium ml-2 dark:text-neutral-300 text-neutral-700">{{
                  kurumsalBirimTipi.ad
                }}</span>
              </div>
              <div class="relative flex items-center">
                <svg
                  class="size-4 group hover:cursor-pointer"
                  viewBox="0 0 16 16"
                  xmlns="http://www.w3.org/2000/svg"
                  @click.stop="
                    showBirimTipiOption[kurumsalBirimTipi.id] =
                      !showBirimTipiOption[kurumsalBirimTipi.id]
                  "
                >
                  <path
                    d="M8 12C9.10457 12 10 12.8954 10 14C10 15.1046 9.10457 16 8 16C6.89543 16 6 15.1046 6 14C6 12.8954 6.89543 12 8 12Z"
                    class="dark:fill-neutral-500 fill-neutral-800 dark:group-hover:fill-neutral-200 group-hover:fill-neutral-500"
                  />
                  <path
                    d="M8 6C9.10457 6 10 6.89543 10 8C10 9.10457 9.10457 10 8 10C6.89543 10 6 9.10457 6 8C6 6.89543 6.89543 6 8 6Z"
                    class="dark:fill-neutral-500 fill-neutral-800 dark:group-hover:fill-neutral-200 group-hover:fill-neutral-500"
                  />
                  <path
                    d="M10 2C10 0.89543 9.10457 -4.82823e-08 8 0C6.89543 4.82823e-08 6 0.895431 6 2C6 3.10457 6.89543 4 8 4C9.10457 4 10 3.10457 10 2Z"
                    class="dark:fill-neutral-500 fill-neutral-800 dark:group-hover:fill-neutral-200 group-hover:fill-neutral-500"
                  />
                </svg>
                <div
                  v-if="showBirimTipiOption[kurumsalBirimTipi.id]"
                  class="absolute mt-30 flex flex-col items-start bg-neutral-200 dark:bg-neutral-700 rounded-md p-2 z-20"
                >
                  <button
                    class="hover:bg-neutral-400 dark:hover:bg-neutral-600 px-2 py-1 cursor-pointer rounded-md"
                  >
                    Düzenle
                  </button>
                  <button
                    class="hover:bg-red-600 hover:text-white w-full text-start px-2 py-1 cursor-pointer rounded-md"
                    @click="deleteKurumsalBirimTipi(kurumsalBirimTipi.id)"
                  >
                    Sil
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div
            class="flex items-center gap-2 group hover:cursor-pointer mx-20"
            @click="
              () => {
                yeniBirimTipHiyerarsiSeviyesi = kurumsalBirimTipi.hiyerarsiSeviyesi + 1;
                showYeniBirimTipiCreateModal = true;
              }
            "
          >
            <hr class="flex-grow border-t border-gray-400 group-hover:border-blue-600" />
            <span class="text-gray-600 group-hover:text-blue-600">+</span>
            <hr class="flex-grow border-t border-gray-400 group-hover:border-blue-600" />
          </div>
        </div>
      </div>
      <!-- sağ taraf -->
      <div class="flex-2 flex flex-col mt-5">
        <h1 class="mx-2 w-full text-base dark:text-neutral-300">Şirket Birimleri</h1>
        <div
          class="mx-2 my-2 p-1 bg-neutral-200/30 dark:bg-neutral-800 rounded-lg shadow-sm hover:shadow-md hover:shadow-neutral-400 dark:hover:shadow-neutral-700 transition-shadow duration-300 select-none"
          v-for="kurumsalBirimTipi in kurumsalBirimTipleri"
          :key="kurumsalBirimTipi.id"
        >
          <div
            class="flex justify-between items-center p-2"
            @click="
              () => {
                expand[kurumsalBirimTipi.ad] = !expand[kurumsalBirimTipi.ad];
              }
            "
          >
            <div class="flex items-center">
              <svg
                viewBox="0 0 1024 1024"
                class="size-5 cursor-pointer select-none transform transition-transform duration-200"
                :class="{ 'rotate-90': expand[kurumsalBirimTipi.ad] }"
                xmlns="http://www.w3.org/2000/svg"
                @click.stop="
                  () => {
                    expand[kurumsalBirimTipi.ad] = !expand[kurumsalBirimTipi.ad];
                  }
                "
              >
                <path
                  d="M903.232 256l56.768 50.432L512 768 64 306.432 120.768 256 512 659.072z"
                  class="fill-current"
                />
              </svg>
              <span class="text-base font-medium ml-2">{{ kurumsalBirimTipi.ad }}</span>
            </div>
            <div class="flex items-center">
              <div
                class="bg-blue-600 size-6 flex items-center justify-center text-neutral-200 py-1 px-2 rounded-full text-xs font-semibold mr-2"
              >
                <span class="text-xs">{{ kurumsalBirimTipi.birimCount || 0 }}</span>
              </div>
              <button
                type="button"
                class="cursor-pointer text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-1.5 text-center me-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
                @click.stop="
                  () => {
                    selectedKurumsalBirim = undefined;
                    editMode = false;
                    showModal[kurumsalBirimTipi.id] = !showModal[kurumsalBirimTipi.id];
                  }
                "
              >
                Yeni Ekle
              </button>
            </div>
          </div>
          <div
            v-if="loading[kurumsalBirimTipi.ad]"
            class="w-full flex items-center justify-center py-4"
          >
            <div
              class="size-7 border-4 border-blue-500 border-t-transparent rounded-full animate-spin"
            ></div>
          </div>
          <!-- <SirketCreateModal
          :sirket="selectedSirket"
          :edit-mode="selectedSirket ? true : false"
          @close-modal="
            (p) => {
              showModal.sirketler = p;

              selectedSirket = undefined;
            }
          "
          @refresh="getSirketler"
          v-if="showModal.sirketler"
        /> -->
          <KurumsalBirimCreateModal
            @close-modal="
              (p) => {
                showModal[kurumsalBirimTipi.id] = !showModal[kurumsalBirimTipi.id];
                selectedKurumsalBirimTipi = undefined;
              }
            "
            :birim="selectedKurumsalBirim"
            :edit-mode="editMode"
            :selected-kurumsal-birim-tip="kurumsalBirimTipi.id"
            v-if="showModal[kurumsalBirimTipi.id]"
            @refresh="getKurumsalBirimTipleri()"
          />
          <KurumsalBirimTipiCreateModal
            v-if="showYeniBirimTipiCreateModal"
            @close-modal="
              (p) => {
                showYeniBirimTipiCreateModal = false;
              }
            "
            :hiyerarsi-seviyesi="yeniBirimTipHiyerarsiSeviyesi"
            :edit-mode="false"
            :birim-tipi="undefined"
            @refresh="getKurumsalBirimTipleri()"
          />
          <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
            <TableLayout
              v-if="expand[kurumsalBirimTipi.ad] && !loading[kurumsalBirimTipi.ad]"
              :tableHeaders="[
                { key: 'ad', value: 'Ad' },
                { key: 'personelCount', value: 'Personel' },
                { key: 'createdAt', value: 'Oluşturulma Tarihi' },
                { key: 'isActive', value: 'Durum' },
              ]"
              :tableContent="filteredkurumsalBirimler(kurumsalBirimTipi.kurumsalBirimler)"
              :islemler="['edit', 'detaylar']"
              :page-count="
                Math.ceil(paginationParamsSirket.count / paginationParamsSirket.pageSize) == 0
                  ? 1
                  : Math.ceil(paginationParamsSirket.count / paginationParamsSirket.pageSize)
              "
              :count="paginationParamsSirket.count"
              :page-size="
                paginationParamsSirket.pageSize > paginationParamsSirket.count
                  ? paginationParamsSirket.count
                  : paginationParamsSirket.pageSize
              "
              :current-page="paginationParamsSirket.pageNumber"
              @edit-click="openBirimModal"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
