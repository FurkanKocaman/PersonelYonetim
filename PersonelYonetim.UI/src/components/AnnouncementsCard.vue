<script setup lang="ts">
// Re-export the component from its new location
import AnnouncementsCard from './dashboard/AnnouncementsCard.vue';
import { defineProps } from 'vue';

// Define the same props as the original component
interface Announcement {
  id: number;
  title: string;
  date: string;
  content: string;
}

// Define the same props as the original component
const props = defineProps({
  announcements: {
    type: Array as () => Announcement[],
    required: true
  }
});
</script>

<template>
  <!-- Forward all props to the actual component -->
  <AnnouncementsCard :announcements="announcements" />
</template>
