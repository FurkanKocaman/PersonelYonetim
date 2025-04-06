<script setup lang="ts">
import Roles from "@/models/Roles";
import { computed, defineProps, onMounted } from "vue";

const props = defineProps<{
  tableHeaders: { key: string; value: string; backgroundColor?: string; width?: string }[];
  tableContent: Array<Record<string, unknown>>;
  islemler: string[];
  count: number;
  pageSize: number;
  pageCount: number;
  currentPage: number;
}>();
const hiddenColumns = ["id", "yoneticiResim"];

const tableKeys = computed<string[]>(() => {
  return props.tableHeaders.map((h) => h.key).filter((key) => !hiddenColumns.includes(key));
});

const formatValue = (value: unknown) => {
  if (value instanceof Date) {
    return new Date(value).toLocaleString("tr-TR", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    });
  }
  if (typeof value === "object" && value !== null) {
    return JSON.stringify(value);
  }
  if (value === null || value === undefined || value === "") {
    return "-";
  }

  return value;
};

onMounted(() => {});

const emit = defineEmits(["edit-click", "detail-click", "remove-click", "setPage", "order-by"]);

const handleEdit = (item: unknown) => {
  emit("edit-click", item);
};

const handleDetail = (item: unknown) => {
  emit("detail-click", item);
};
const handleRemove = (item: unknown) => {
  emit("remove-click", item);
};
const setPage = (pageNumber: number) => {
  emit("setPage", pageNumber);
};
const orderBy = (order: string) => {
  emit("order-by", order);
};

const getColumnBgColor = (key: string, value: unknown) => {
  if (key === "isActive") {
    if (value === "Aktif") return "#00870e";
    if (value === "Pasif") return "#ff0000";
  }

  if (key === "degerlendirmeDurumu") {
    if (value === "Onaylandı") return "#00870e";
    if (value === "Beklemede") return "#ffb700";
    if (value === "Reddedildi") return "#ff0000";
  }

  const header = props.tableHeaders.find((h) => h.key === key);
  return header?.backgroundColor || "";
};
</script>

<template>
  <table class="divide-y divide-gray-200 dark:divide-neutral-700 w-full">
    <thead class="bg-gray-200/60 dark:bg-neutral-800 w-full">
      <tr>
        <th
          v-for="header in props.tableHeaders"
          :key="header.key"
          scope="col"
          class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider w-full"
          :class="header.width != undefined ? header.width : 'w-fit'"
        >
          <div class="flex cursor-pointer select-none" @click="orderBy(header.key)">
            {{ header.value }}
            <svg
              class="w-3 h-3 ms-1.5"
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                d="M8.574 11.024h6.852a2.075 2.075 0 0 0 1.847-1.086 1.9 1.9 0 0 0-.11-1.986L13.736 2.9a2.122 2.122 0 0 0-3.472 0L6.837 7.952a1.9 1.9 0 0 0-.11 1.986 2.074 2.074 0 0 0 1.847 1.086Zm6.852 1.952H8.574a2.072 2.072 0 0 0-1.847 1.087 1.9 1.9 0 0 0 .11 1.985l3.426 5.05a2.123 2.123 0 0 0 3.472 0l3.427-5.05a1.9 1.9 0 0 0 .11-1.985 2.074 2.074 0 0 0-1.846-1.087Z"
              />
            </svg>
          </div>
        </th>
        <th
          scope="col"
          class="w-1/12 px-6 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
        >
          İşlemler
        </th>
      </tr>
    </thead>
    <tbody
      class="bg-neutral-50 dark:bg-neutral-800 divide-y divide-gray-300 dark:divide-neutral-700"
    >
      <tr
        v-for="(row, rowIndex) in tableContent"
        :key="rowIndex"
        class="hover:bg-gray-200 dark:hover:bg-neutral-900"
      >
        <td
          v-for="key in tableKeys"
          :key="key"
          class="px-2 py-2 overflow-hidden whitespace-nowrap text-ellipsis text-sm text-gray-900 dark:text-gray-200"
        >
          <span
            :style="{ backgroundColor: getColumnBgColor(key, row[key as string]) }"
            class="px-2 py-1 rounded-md"
            >{{
              key == "role"
                ? Roles.getRoleByValue(row[key as string] as number).name
                : formatValue(row[key as string])
            }}</span
          >
        </td>

        <td class="px-6 py-4 whitespace-nowrap text-right text-base font-medium">
          <button
            v-if="props.islemler.includes('detaylar')"
            @click="handleDetail(row)"
            class="text-sky-600 dark:text-sky-400 mx-2 hover:text-sky-800 dark:hover:text-sky-300 cursor-pointer"
          >
            <i class="fas fa-eye"></i>
          </button>

          <button
            v-if="props.islemler.includes('edit')"
            @click="handleEdit(row)"
            class="text-amber-600 dark:text-amber-400 mx-2 hover:text-amber-800 dark:hover:text-amber-300 cursor-pointer"
          >
            <i class="fas fa-edit"></i>
          </button>
          <button
            v-if="props.islemler.includes('remove')"
            @click="handleRemove(row)"
            class="text-red-600 hover:text-red-800 cursor-pointer mx-2"
          >
            <i class="fas fa-trash"></i>
          </button>
        </td>
      </tr>
    </tbody>
  </table>
  <nav
    class="flex flex-col justify-center w-full items-center my-3"
    aria-label="Page navigation example"
  >
    <span class="text-sm text-gray-700 dark:text-gray-400 mb-2">
      <span class="font-semibold text-gray-900 dark:text-white">{{
        (props.currentPage - 1) * props.pageSize == 0 ? 1 : (props.currentPage - 1) * props.pageSize
      }}</span>
      den
      <span class="font-semibold text-gray-900 dark:text-white">{{
        props.pageSize * props.currentPage
      }}</span>
      'a kadar olan kayıtlar gösteriliyor.<span class="font-semibold text-gray-900 dark:text-white"
        >Toplam {{ props.count }}</span
      >
      Kayıt
    </span>
    <ul class="flex items-center -space-x-px h-8 text-sm">
      <li>
        <button
          class="flex items-center justify-center px-3 h-8 ms-0 leading-tight text-gray-500 bg-white border border-e-0 border-gray-300 rounded-s-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-neutral-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
          @click="props.currentPage != 1 ? setPage(props.currentPage - 1) : ''"
        >
          <span class="sr-only">Previous</span>
          <svg
            class="w-2.5 h-2.5 rtl:rotate-180"
            aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 6 10"
          >
            <path
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M5 1 1 5l4 4"
            />
          </svg>
        </button>
      </li>
      <li v-for="x in props.pageCount" :key="x">
        <button
          class="flex items-center justify-center px-3 h-8 leading-tight text-gray-500 border border-gray-300 hover:bg-gray-100 hover:text-gray-700 dark:bg-neutral-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
          @click="setPage(x)"
        >
          {{ x }}
        </button>
      </li>

      <li>
        <button
          class="flex items-center justify-center px-3 h-8 leading-tight text-gray-500 bg-white border border-gray-300 rounded-e-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-neutral-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white"
          @click="
            props.currentPage != Math.ceil(props.count / props.pageSize)
              ? setPage(props.currentPage + 1)
              : ''
          "
        >
          <span class="sr-only">Next</span>
          <svg
            class="w-2.5 h-2.5 rtl:rotate-180"
            aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 6 10"
          >
            <path
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="m1 9 4-4-4-4"
            />
          </svg>
        </button>
      </li>
    </ul>
  </nav>
</template>

<style scoped></style>
