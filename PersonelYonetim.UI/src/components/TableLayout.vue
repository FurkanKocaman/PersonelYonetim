<script setup lang="ts">
import { computed, defineProps } from "vue";

const props = defineProps<{
  tableHeaders: string[];
  tableContent: Array<Record<string, unknown>>;
  islemler: string[];
}>();

const tableKeys = computed<string[]>(() => {
  return props.tableContent.length > 0 ? Object.keys(props.tableContent[0]) : [];
});

const formatValue = (value: unknown) => {
  if (value instanceof Date) {
    return value.toLocaleDateString();
  }
  if (typeof value === "object" && value !== null) {
    return JSON.stringify(value);
  }
  if (value === null || value === undefined || value === "") {
    return "-";
  }

  return value;
};
</script>

<template>
  <div class="bg-white dark:bg-neutral-100 shadow-sm mt-5">
    <div class="overflow-auto">
      <table class="w-full divide-y divide-gray-200 dark:divide-neutral-700">
        <thead class="bg-gray-50 dark:bg-neutral-800">
          <tr>
            <th
              v-for="header in props.tableHeaders"
              :key="header"
              scope="col"
              class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
            >
              <div class="flex cursor-pointer w-fit select-none">
                {{ header }}
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
              class="px-6 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
            >
              İşlemler
            </th>
          </tr>
        </thead>
        <tbody
          class="bg-white dark:bg-neutral-900 divide-y divide-gray-200 dark:divide-neutral-700"
        >
          <tr
            v-for="(row, rowIndex) in tableContent"
            :key="rowIndex"
            class="hover:bg-gray-50 dark:hover:bg-neutral-800 border-b dark:border-neutral-600 border-neutral-400"
          >
            <td
              v-for="key in tableKeys"
              :key="key"
              class="px-6 py-4 max-w-[170px] overflow-hidden text-ellipsis whitespace-nowrap text-sm text-gray-900 dark:text-gray-200"
            >
              {{ formatValue(row[key as string]) }}
            </td>

            <td class="px-6 py-4 whitespace-nowrap text-right text-base font-medium">
              <button
                v-if="props.islemler.some((item) => item.includes('detaylar'))"
                class="text-sky-600 dark:text-sky-400 hover:text-sky-800 dark:hover:text-sky-300 mr-3 cursor-pointer"
              >
                <i class="fas fa-eye"></i>
              </button>

              <button
                v-if="props.islemler.some((item) => item.includes('edit'))"
                class="text-amber-600 dark:text-amber-400 hover:text-amber-800 dark:hover:text-amber-300 cursor-pointer"
              >
                <i class="fas fa-edit"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<style scoped>
/* Add any component-specific styles here.  Scoped to this component. */
/* Example:
.some-class {
  color: blue;
}
*/
</style>
