<script setup lang="ts">
// Re-export the component from its new location
import QuickAccessButtons from './dashboard/QuickAccessButtons.vue';
</script>

<template>
  <!-- The QuickAccessButtons component doesn't have any required props -->
  <QuickAccessButtons v-bind="$attrs" />
</template>
