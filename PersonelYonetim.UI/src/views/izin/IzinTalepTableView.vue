<script setup lang="ts">
import { ref, onMounted, onActivated, computed, type Ref } from "vue";
import IzinService from "@/services/IzinService";
import type {
  CakisanIzinTalep,
  IzinTalepGetResponse,
  OnaySureci,
} from "@/models/response-models/izinler/IzinTalepGetResponse";
import TableLayout from "@/components/TableLayout.vue";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { CSSProperties } from "vue";
import dayjs from "dayjs";
import "dayjs/locale/tr";

const izinList = ref<IzinTalepGetResponse[]>([]);
const cakisanIzinler = ref<CakisanIzinTalep[] | undefined>([]);
const onayAdimlari = ref<OnaySureci[] | undefined>([]);

const selectedIzin = ref<IzinTalepGetResponse | undefined>(undefined);
const selectedTab = ref<"cakisanIzinler" | "onaySureci">("cakisanIzinler");

dayjs.locale("tr");
const loading = ref(true);
const error = ref(false);
const apiUrl = ref("");

const showDetailModal = ref(false);

const paginationParams: Ref<PaginationParams> = ref({
  count: 0,
  pageNumber: 1,
  pageSize: 10,
  orderBy: "createdAt desc",
  filter: "",
});

const statusColors: Record<string, string> = {
  Onaylandı: "text-green-600 bg-green-100 dark:bg-green-900 dark:text-green-300",
  Beklemede: "text-amber-600 bg-amber-100 dark:bg-amber-900 dark:text-amber-300",
  Reddedildi: "text-red-600 bg-red-100 dark:bg-red-900 dark:text-red-300",
};

const filterOptions = ref({
  durum: "",
  izinTipi: "",
});

const filteredIzinList = computed(() => {
  return izinList.value.filter((izin) => {
    const durumMatch =
      !filterOptions.value.durum || izin.degerlendirmeDurumu === filterOptions.value.durum;
    const izinTipiMatch =
      !filterOptions.value.izinTipi || izin.izinTuru === filterOptions.value.izinTipi;
    return durumMatch && izinTipiMatch;
  });
});

const filteredIzinTalepler = computed<Record<string, unknown>[]>(() => {
  return (filteredIzinList.value || []).map(
    ({
      id,
      personelFullName,
      baslangicTarihi,
      bitisTarihi,
      mesaiBaslangicTarihi,
      toplamSure,
      izinTuru,
      degerlendirmeDurumu,
    }) => ({
      id,
      personelFullName,
      baslangicTarihi: new Date(baslangicTarihi),
      bitisTarihi: new Date(bitisTarihi),
      mesaiBaslangicTarihi: new Date(mesaiBaslangicTarihi),
      toplamSure,
      izinTuru,
      degerlendirmeDurumu,
    })
  );
});

onMounted(() => {
  getIzinTalepler();
  apiUrl.value = import.meta.env.VITE_API_URL;
});

onActivated(() => {
  getIzinTalepler();
});

const setPageNumber = (pageNumber: number) => {
  paginationParams.value.pageNumber = pageNumber;
  getIzinTalepler();
};

const getIzinTalepler = async () => {
  const response = await IzinService.getIzinTalepler(paginationParams.value);
  izinList.value = response!.items;
  loading.value = false;
  paginationParams.value.count = response!.count;
  console.log(response);
};

const izinDegerlendir = async (id: string, degerlendirme: number, yorum: string | undefined) => {
  const response = await IzinService.izinTalepDegerlendir(id, degerlendirme, yorum);
  showDetailModal.value = false;
  console.log(response);
  getIzinTalepler();
};

const openIzinEdit = (item: IzinTalepGetResponse) => {
  selectedIzin.value = izinList.value.find((p) => p.id == item.id);
  cakisanIzinler.value = selectedIzin.value?.cakisanIzinTalepler;
  onayAdimlari.value = selectedIzin.value?.onayAdimlari;
  showDetailModal.value = true;
};

const orderBy = (order: string) => {
  paginationParams.value.orderBy = order;
  getIzinTalepler();
};

const baslangicTarih = dayjs("2025-04-11");
const bitisTarih = dayjs("2025-04-13");

const gunler = computed(() => {
  const gunListesi: string[] = [];

  let g = baslangicTarih.subtract(1, "day");
  const sonGun = bitisTarih.add(1, "day");

  while (g.isBefore(sonGun) || g.isSame(sonGun, "day")) {
    gunListesi.push(g.format("YYYY-MM-DD"));
    g = g.add(1, "day");
  }

  return gunListesi;
});

const uniquePersoneller = computed(() => {
  if (cakisanIzinler.value != undefined)
    return [...new Set(cakisanIzinler.value.map((i) => i.personelFullName))];

  return undefined;
});

function formatDate(date: string): string {
  return dayjs(date).format("DD MMM dddd");
}

function getLeaveSegmentStyle(personelAd: string | undefined, gun: string): CSSProperties | null {
  if (cakisanIzinler.value == undefined) return null;

  const izin = cakisanIzinler.value.find((i) => i.personelFullName === personelAd);
  if (!izin) return null;

  const izinBaslangic = dayjs(izin.baslangicTarihi);
  const izinBitis = dayjs(izin.bitisTarihi);
  const gunStart = dayjs(gun).startOf("day");
  const gunEnd = dayjs(gun).endOf("day");

  if (izinBaslangic.isAfter(gunEnd) || izinBitis.isBefore(gunStart)) {
    return null;
  }

  const effectiveStart = izinBaslangic.isAfter(gunStart) ? izinBaslangic : gunStart;
  const effectiveEnd = izinBitis.isBefore(gunEnd) ? izinBitis : gunEnd;

  const totalDayMinutes = 24 * 60;

  const offsetMinutes = effectiveStart.diff(gunStart, "minute");

  let durationMinutes = effectiveEnd.diff(effectiveStart, "minute");

  if (effectiveStart.isSame(gunStart) && effectiveEnd.isSame(gunEnd)) {
    durationMinutes = totalDayMinutes;
  } else if (effectiveEnd.isSame(gunEnd)) {
    durationMinutes = gunEnd.add(1, "millisecond").diff(effectiveStart, "minute");
  } else if (
    izinBitis.hour() === 0 &&
    izinBitis.minute() === 0 &&
    izinBitis.second() === 0 &&
    izinBitis.millisecond() === 0 &&
    effectiveEnd.isSame(izinBitis.subtract(1, "millisecond").endOf("day"))
  ) {
    durationMinutes = gunEnd.add(1, "millisecond").diff(effectiveStart, "minute");
  }

  durationMinutes = Math.max(0, durationMinutes);

  if (durationMinutes === 0) {
    return null;
  }

  const leftPercentage = (offsetMinutes / totalDayMinutes) * 100;
  const widthPercentage = (durationMinutes / totalDayMinutes) * 100;

  const finalWidthPercentage = Math.min(widthPercentage, 100 - leftPercentage);

  return {
    position: "absolute",
    left: `${leftPercentage}%`,
    width: `${finalWidthPercentage}%`,
    top: "2px",
    bottom: "2px",
    backgroundColor: "#3b82f6",
    borderRadius: "4px",
    // minWidth: '5px',
  };
}
</script>
<template>
  <!-- İçerik Alanı -->
  <main class="p-2 flex flex-col max-w[80dvw]">
    <div v-if="loading" class="flex justify-center items-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
    </div>

    <div
      v-else-if="error"
      class="bg-red-100 border border-red-200 text-red-700 dark:bg-red-900 dark:border-red-800 dark:text-red-300 rounded-lg p-4 mb-6"
    >
      <div class="flex items-center">
        <i class="fas fa-exclamation-circle mr-2"></i>
        <span>İzin listesi yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.</span>
      </div>
    </div>

    <div v-else-if="izinList.length > 0" class="overflow-auto">
      <TableLayout
        :table-headers="[
          { key: 'personelFullName', value: 'Personel' },
          { key: 'baslangicTarihi', value: 'Başlangıç' },
          { key: 'bitisTarihi', value: 'Bitiş' },
          { key: 'mesaiBaslangicTarihi', value: 'Mesai Baslangic' },
          { key: 'toplamSure', value: 'Süre' },
          { key: 'izinTuru', value: 'İzin Tipi' },
          { key: 'degerlendirmeDurumu', value: 'Durum' },
        ]"
        :table-content="filteredIzinTalepler"
        :islemler="['edit']"
        @edit-click="openIzinEdit"
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
        @order-by="orderBy"
      />
    </div>

    <!-- Veri Yoksa -->
    <div v-else class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 text-center">
      <div class="flex flex-col items-center justify-center py-12">
        <i class="fas fa-calendar-times text-4xl text-gray-400 dark:text-gray-600 mb-4"></i>
        <h3 class="text-lg font-medium text-gray-900 dark:text-gray-200 mb-2">
          İzin Kaydı Bulunamadı
        </h3>
        <p class="text-gray-500 dark:text-gray-400 mb-6">Henüz hiç izin talebi oluşturulmamış.</p>
      </div>
    </div>
  </main>

  <div v-if="showDetailModal" class="fixed inset-0 transition-opacity z-10">
    <div class="absolute inset-0 bg-gray-500 dark:bg-neutral-900 opacity-75"></div>
  </div>

  <div
    v-if="showDetailModal"
    class="fixed inset-0 z-30 overflow-hidden flex items-center justify-center"
  >
    <!-- Modal İçeriği -->
    <div
      class="bg-neutral-100 dark:bg-neutral-800 rounded-lg text-left shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-2xl sm:w-full"
    >
      <button
        type="button"
        class="absolute right-0"
        @click="
          () => {
            showDetailModal = false;
          }
        "
      >
        <svg
          class="size-10 group cursor-pointer"
          viewBox="0 0 24 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M10.0303 8.96965C9.73741 8.67676 9.26253 8.67676 8.96964 8.96965C8.67675 9.26255 8.67675 9.73742 8.96964 10.0303L10.9393 12L8.96966 13.9697C8.67677 14.2625 8.67677 14.7374 8.96966 15.0303C9.26255 15.3232 9.73743 15.3232 10.0303 15.0303L12 13.0607L13.9696 15.0303C14.2625 15.3232 14.7374 15.3232 15.0303 15.0303C15.3232 14.7374 15.3232 14.2625 15.0303 13.9696L13.0606 12L15.0303 10.0303C15.3232 9.73744 15.3232 9.26257 15.0303 8.96968C14.7374 8.67678 14.2625 8.67678 13.9696 8.96968L12 10.9393L10.0303 8.96965Z"
            class="fill-neutral-800 group-hover:fill-neutral-500 dark:fill-neutral-200 dark:group-hover:fill-neutral-400"
          />
        </svg>
      </button>
      <div
        class="flex justify-start border-b dark:border-neutral-500 border-neutral-300 mx-5 py-2 mt-3"
      >
        <svg
          class="size-6 fill-neutral-700 dark:fill-neutral-200 mr-2"
          viewBox="0 0 16 16"
          xmlns="http://www.w3.org/2000/svg"
          fill="currentColor"
        >
          <path
            d="M4 .5a.5.5 0 0 0-1 0V1H2a2 2 0 0 0-2 2v1h16V3a2 2 0 0 0-2-2h-1V.5a.5.5 0 0 0-1 0V1H4V.5ZM16 14V5H0v9a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2ZM9.5 7h1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-1a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5Zm3 0h1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-1a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5Zm-9 2h1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-1a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5Zm3 0h1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-1a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5Zm3 0h1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-1a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5Zm3 0h1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-1a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5Zm-9 2h1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-1a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5Zm3 0h1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-1a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5Zm3 0h1a.5.5 0 0 1 .5.5v1a.5.5 0 0 1-.5.5h-1a.5.5 0 0 1-.5-.5v-1a.5.5 0 0 1 .5-.5Z"
          />
        </svg>
        <h3 class="text-lg leading-6 text-neutral-700 dark:text-neutral-200 font-medium mb-2">
          İzin Talebi
        </h3>
      </div>

      <div class="overflow-y-auto max-h-[60dvh] px-4 pt-5 pb-4 sm:p-6 sm:pb-4 max-w-full">
        <!-- Main start -->
        <div class="flex flex-col justify-center items-center gap-4 mx-2">
          <div class="w-fit flex items-center justify-center mb-4">
            <img
              v-if="selectedIzin?.avatarUrl"
              class="size-12 rounded-md object-cover mx-2 mt-2"
              width="100"
              height="100"
              :src="apiUrl + selectedIzin.avatarUrl"
              alt=""
            />
            <div
              v-else
              class="text-xl font-semibold text-sky-600 transition-all duration-300 ease-in-out mr-2 rounded-md border-1 border-sky-500 mx-2 mt-2 size-12 flex items-center justify-center"
            >
              {{ selectedIzin?.personelFullName![0] }}
            </div>
            <div class="bg-neutral-300 dark:bg-neutral-700 rounded-md mx-5 p-2 text-sm">
              <p>
                <span class="text-neutral-900 dark:text-neutral-50 font-semibold">{{
                  selectedIzin?.personelFullName
                }}</span
                >, {{ selectedIzin?.izinTuru }} için {{ selectedIzin?.toplamSure }} günlük izin
                talep ediyor.
              </p>
            </div>
          </div>

          <div class="px-10 w-full divide-y divide-neutral-300 dark:divide-neutral-600">
            <div class="w-full flex justify-between items-center mb-2 pb-2">
              <span class="text-sm text-neutral-600 dark:text-neutral-300">Durum</span>
              <span
                class="text-xs px-2 py-1 rounded-md"
                :class="statusColors[selectedIzin!.degerlendirmeDurumu]"
                >{{ selectedIzin?.degerlendirmeDurumu }}</span
              >
            </div>
            <div class="w-full flex justify-between items-center mb-2 pb-2">
              <span class="text-sm text-neutral-600 dark:text-neutral-300">İzin Türü</span>
              <span class="text-sm px-2 py-1 rounded-md">{{ selectedIzin?.izinTuru }}</span>
            </div>
            <div class="w-full flex justify-between items-center mb-2 pb-2">
              <span class="text-sm text-neutral-600 dark:text-neutral-300">Toplam Sure</span>
              <span class="text-sm px-2 py-1 rounded-md">{{ selectedIzin?.toplamSure }} gün</span>
            </div>
            <div class="w-full flex justify-between items-center mb-2 pb-2">
              <span class="text-sm text-neutral-600 dark:text-neutral-300">İzin Başlangıç</span>
              <span class="text-sm px-2 py-1 rounded-md"
                >{{
                  new Date(selectedIzin!.baslangicTarihi).toLocaleString("tr-TR", {
                    day: "2-digit",
                    month: "2-digit",
                    year: "numeric",
                    hour: "2-digit",
                    minute: "2-digit",
                    hour12: false,
                  })
                }}
              </span>
            </div>
            <div class="w-full flex justify-between items-center mb-2 pb-2">
              <span class="text-sm text-neutral-600 dark:text-neutral-300">İzin Bitiş</span>
              <span class="text-sm px-2 py-1 rounded-md">{{
                new Date(selectedIzin!.bitisTarihi).toLocaleString("tr-TR", {
                  day: "2-digit",
                  month: "2-digit",
                  year: "numeric",
                  hour: "2-digit",
                  minute: "2-digit",
                  hour12: false,
                })
              }}</span>
            </div>
            <div class="w-full flex justify-between items-center mb-2 pb-2">
              <span class="text-sm text-neutral-600 dark:text-neutral-300">Mesai Başlangıç</span>
              <span class="text-sm px-2 py-1 rounded-md">{{
                new Date(selectedIzin!.mesaiBaslangicTarihi).toLocaleString("tr-TR", {
                  day: "2-digit",
                  month: "2-digit",
                  year: "numeric",
                  hour: "2-digit",
                  minute: "2-digit",
                  hour12: false,
                })
              }}</span>
            </div>
            <div class="w-full flex justify-between items-center mb-2 pb-2">
              <span class="text-sm text-neutral-600 dark:text-neutral-300">Açıklama</span>
              <span class="text-sm px-2 py-1 rounded-md">{{ selectedIzin?.aciklama ?? "-" }}</span>
            </div>
          </div>
        </div>
        <!-- Main end -->
        <!-- Nav Buttons -->
        <div class="mx-10">
          <div class="border-b border-gray-200 dark:border-gray-700">
            <ul
              class="flex flex-wrap -mb-px text-sm font-medium text-center text-gray-500 dark:text-gray-400"
            >
              <li class="me-2">
                <button
                  class="inline-flex items-center justify-center p-4 rounded-t-lg group cursor-pointer"
                  :class="
                    selectedTab == 'cakisanIzinler'
                      ? 'text-blue-600 border-blue-600 dark:text-blue-500 dark:border-blue-500 border-b-2'
                      : 'hover:text-gray-600 hover:border-gray-300 dark:hover:text-gray-300'
                  "
                  @click="selectedTab = 'cakisanIzinler'"
                >
                  Çakışan İzinler
                </button>
              </li>
              <li class="me-2">
                <button
                  class="inline-flex items-center justify-center p-4 rounded-t-lg active group cursor-pointer"
                  :class="
                    selectedTab == 'onaySureci'
                      ? 'text-blue-600 border-blue-600 dark:text-blue-500 dark:border-blue-500 border-b-2'
                      : 'hover:text-gray-600 hover:border-gray-300 dark:hover:text-gray-300'
                  "
                  @click="selectedTab = 'onaySureci'"
                >
                  Onay Süreci
                </button>
              </li>
            </ul>
          </div>
          <!-- Çakışan İzinler -->
          <div v-if="selectedTab === 'cakisanIzinler'" class="py-5">
            <p
              v-if="cakisanIzinler!.length === 0 && uniquePersoneller!.length === 0"
              class="text-center text-gray-500 dark:text-gray-400"
            >
              Bu tarih aralığında çakışan başka izin bulunamadı.
            </p>
            <div
              v-else
              class="overflow-x-auto border border-neutral-300 dark:border-neutral-600 rounded-md"
            >
              <table class="min-w-full divide-y divide-neutral-300 dark:divide-neutral-600">
                <thead class="bg-neutral-200 dark:bg-neutral-700">
                  <tr>
                    <th
                      class="sticky left-0 z-10 bg-neutral-200 dark:bg-neutral-700 px-3 py-2 text-left text-xs font-medium text-neutral-600 dark:text-neutral-300 uppercase tracking-wider"
                    >
                      Personel
                    </th>
                    <th
                      v-for="day in gunler"
                      :key="day"
                      class="px-2 py-2 text-center text-xs font-medium text-neutral-600 dark:text-neutral-300 uppercase tracking-wider whitespace-nowrap"
                    >
                      {{ formatDate(day) }}
                    </th>
                  </tr>
                </thead>
                <tbody
                  class="bg-white dark:bg-neutral-800 divide-y divide-neutral-200 dark:divide-neutral-700"
                >
                  <tr v-for="personel in uniquePersoneller" :key="personel">
                    <td
                      class="sticky left-0 z-10 bg-white dark:bg-neutral-800 px-3 py-2 whitespace-nowrap text-sm font-medium text-neutral-900 dark:text-neutral-100"
                    >
                      {{ personel }}
                    </td>
                    <td
                      v-for="day in gunler"
                      :key="day"
                      class="border-l border-neutral-200 dark:border-neutral-700 h-10 relative min-w-[100px] px-0 py-0"
                      :title="`${personel} - ${formatDate(day)}`"
                    >
                      <div
                        v-if="getLeaveSegmentStyle(personel, day)"
                        :style="getLeaveSegmentStyle(personel, day)"
                        :title="`İzin: ${dayjs(
                          cakisanIzinler!.find((i) => i.personelFullName === personel)?.baslangicTarihi
                        ).format('HH:mm')} - ${dayjs(
                          cakisanIzinler!.find((i) => i.personelFullName === personel)?.bitisTarihi
                        ).format('HH:mm')}`"
                      ></div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
          <!-- Onay Süreci -->
          <div v-if="selectedTab === 'onaySureci'" class="py-5">
            <div class="flex items-center justify-between">
              <div class="flex text-sm">
                <img
                  v-if="selectedIzin?.avatarUrl"
                  class="size-10 rounded-md object-cover mx-2 mt-2"
                  width="100"
                  height="100"
                  :src="apiUrl + selectedIzin.avatarUrl"
                  alt=""
                />
                <div
                  v-else
                  class="text-xl font-semibold text-sky-600 transition-all duration-300 ease-in-out mr-2 rounded-md border-1 border-sky-500 mx-2 mt-2 size-10 flex items-center justify-center"
                >
                  {{ selectedIzin?.personelFullName![0] }}
                </div>
                <div>
                  <h3>{{ selectedIzin?.personelFullName }}</h3>
                  <p class="dark:text-neutral-400 text-neutral-500">
                    {{
                      selectedIzin?.createdAt
                        ? dayjs(selectedIzin.createdAt).format("D MMMM YYYY, dddd") +
                          " tarihinde oluşturuldu."
                        : "-"
                    }}
                  </p>
                </div>
              </div>
              <div class="text-xs p-1 bg-blue-500 rounded-md h-fit">Oluşturuldu</div>
            </div>

            <div
              v-for="adim in onayAdimlari"
              :key="adim.sira"
              class="flex items-center justify-between my-4"
            >
              <div class="flex text-sm relative">
                <span
                  class="absolute bg-neutral-200 dark:bg-neutral-700 rounded-full text-neutral-800 dark:text-neutral-200 size-5 flex justify-center items-center"
                  >{{ adim.sira }}</span
                >
                <img
                  v-if="adim.avatarUrl"
                  class="size-10 rounded-md object-cover mx-2 mt-2"
                  width="100"
                  height="100"
                  :src="apiUrl + adim.avatarUrl"
                  alt=""
                />
                <div
                  v-else
                  class="text-xl font-semibold text-sky-600 transition-all duration-300 ease-in-out mr-2 rounded-md border-1 border-sky-500 mx-2 mt-2 size-10 flex items-center justify-center"
                >
                  {{ adim.personelAd![0] }}
                </div>

                <div class="flex flex-col">
                  <div class="flex">
                    <h3>{{ adim.personelAd }}</h3>
                    <p class="dark:text-neutral-400 text-neutral-500 ml-2">
                      ({{ adim.pozisyonAd }})
                    </p>
                  </div>
                  <p class="dark:text-neutral-400 text-neutral-500">
                    {{
                      adim.degerlendirilmeTarihi
                        ? dayjs(adim.degerlendirilmeTarihi).format("D MMMM YYYY HH:mm, dddd") +
                          " tarihinde " +
                          adim.onayDurum.toLocaleLowerCase() +
                          "."
                        : "-"
                    }}
                  </p>
                </div>
              </div>
              <div
                class="text-xs p-1 rounded-md h-fit"
                :class="
                  adim.onayDurum == 'Beklemede'
                    ? 'bg-yellow-500'
                    : adim.onayDurum == 'Onaylandı'
                    ? 'bg-green-500'
                    : adim.onayDurum == 'Reddedildi'
                    ? 'bg-red-500'
                    : 'bg-blue-500'
                "
              >
                {{ adim.onayDurum }}
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Buttons start -->
      <div class="px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
        <button
          type="button"
          class="text-gray-900 hover:text-white border border-gray-800 hover:bg-gray-900 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-gray-600 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-800"
          @click="
            () => {
              showDetailModal = false;
            }
          "
        >
          İptal
        </button>
        <button
          type="button"
          class="text-red-700 hover:text-white border border-red-700 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-red-500 dark:text-red-500 dark:hover:text-white dark:hover:bg-red-600 dark:focus:ring-red-900"
          @click="izinDegerlendir(selectedIzin?.id!, 2, undefined)"
        >
          Reddet
        </button>
        <button
          type="button"
          class="text-green-700 hover:text-white border border-green-700 hover:bg-green-800 focus:ring-4 focus:outline-none focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-green-500 dark:text-green-500 dark:hover:text-white dark:hover:bg-green-600 dark:focus:ring-green-800"
          @click="izinDegerlendir(selectedIzin?.id!, 1, undefined)"
        >
          Onayla
        </button>
      </div>
      <!-- Buttons end -->
    </div>
  </div>
</template>
