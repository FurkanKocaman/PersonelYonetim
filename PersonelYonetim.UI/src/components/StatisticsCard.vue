<script setup lang="ts">
// Re-export the component from its new location
import StatisticsCard from './dashboard/StatisticsCard.vue';
import { defineProps } from 'vue';

// Define the same props as the original component
interface StatItem {
  title: string;
  value: number;
  icon: string;
  color: string;
}

// Define the same props as the original component
const props = defineProps({
  stats: {
    type: Array as () => StatItem[],
    required: true
  }
});
</script>

<template>
  <!-- Forward all props to the actual component -->
  <StatisticsCard :stats="stats" />
</template>
