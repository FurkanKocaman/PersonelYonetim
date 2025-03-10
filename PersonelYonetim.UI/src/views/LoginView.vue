<script setup lang="ts">
import { ref } from "vue";
import type { LoginRequest } from "@/models/LoginRequest";
import AuthService from "@/services/AuthService";

const email = ref("");
const password = ref("");
const isLoading = ref(false);

const handleLogin = async () => {
  try {
    isLoading.value = true;
    const loginData: LoginRequest = { email: email.value, password: password.value };
    const response = await AuthService.login(loginData);
    console.log(response);
    isLoading.value = false;
  } catch (error) {
    console.error(error);
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="flex justify-center items-center min-h-screen bg-gray-100 dark:bg-neutral-800">
    <div class="bg-white p-8 rounded-lg shadow-md w-96">
      <h1 class="text-2xl font-bold text-center mb-6">Login</h1>
      <form @submit.prevent="handleLogin" class="space-y-4">
        <div>
          <label for="email" class="block text-sm font-medium">Email:</label>
          <input
            id="email"
            v-model="email"
            type="email"
            required
            class="w-full p-2 border border-gray-300 rounded"
          />
        </div>
        <div>
          <label for="password" class="block text-sm font-medium">Password:</label>
          <input
            id="password"
            v-model="password"
            type="password"
            required
            class="w-full p-2 border border-gray-300 rounded"
          />
        </div>
        <button type="submit" class="w-full bg-sky-600 text-white p-2 rounded hover:bg-sky-700">
          {{ isLoading ? "Loading" : "Login" }}
        </button>
      </form>
    </div>
  </div>
</template>
