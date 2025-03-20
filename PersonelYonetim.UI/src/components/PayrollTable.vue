<script setup lang="ts">
// Re-export the component from its new location
import PayrollTable from './dashboard/PayrollTable.vue';
import { defineProps } from 'vue';

// Define the same props as the original component
interface PayrollItem {
  id: number;
  name: string;
  department: string;
  status: string;   
  date: string;
}

// Define the same props as the original component
const props = defineProps({
  payrollItems: {
    type: Array as () => PayrollItem[],
    required: true
  }
});
</script>

<template>
  <!-- Forward all props to the actual component -->
  <PayrollTable :payrollItems="payrollItems" />
</template>
