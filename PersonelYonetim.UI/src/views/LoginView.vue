<script setup lang="ts">
import { ref } from "vue";
import type { LoginRequest } from "@/models/LoginRequest";
import AuthService from "@/services/AuthService";

const email = ref("");
const password = ref("");
const isLoading = ref(false);
const loginResponse = ref("");

const handleLogin = async () => {
  try {
    isLoading.value = true;
    const loginData: LoginRequest = { emailOrPassword: email.value, password: password.value };
    const response = await AuthService.login(loginData);
    console.log("Response", response);
    loginResponse.value = response;
    isLoading.value = false;
  } catch (error) {
    console.error("Error", error);
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="flex justify-end h-dvh">
    <div
      class="w-full h-full flex items-center justify-center bg-gradient-to-r from-sky-600 to-sky-700"
    >
      <h1 class="text-5xl text-neutral-200">Personel Yönetim</h1>
    </div>
    <div class="flex flex-col items-center justify-center md:w-1/2 h-full dark:bg-neutral-700">
      <h1 class="text-2xl font-semibold text-center mb-6">Giriş Sayfası</h1>

      <form @submit.prevent="handleLogin" class="space-y-4 w-full px-16">
        <div>
          <label for="email" class="block text-sm/5 font-semibold my-2"
            >Eposta | Kullanıcı adı</label
          >

          <div class="relative">
            <svg
              viewBox="0 0 24 24"
              class="size-7 fill-none absolute mt-2 left-2"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M5 21C5 17.134 8.13401 14 12 14C15.866 14 19 17.134 19 21M16 7C16 9.20914 14.2091 11 12 11C9.79086 11 8 9.20914 8 7C8 4.79086 9.79086 3 12 3C14.2091 3 16 4.79086 16 7Z"
                class="dark:stroke-neutral-200 stroke-neutral-800 stroke-1"
              />
            </svg>

            <input
              id="email"
              v-model="email"
              type="text"
              required
              class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 dark:text-neutral-200 rounded text-sm pl-10"
              placeholder="user@mail.com"
            />
          </div>
        </div>
        <div>
          <label for="password" class="block text-sm/5 font-semibold my-2">Şifre</label>

          <div class="relative">
            <svg
              viewBox="0 0 24 24"
              class="size-7 fill-none absolute mt-2 left-2"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M12 14.5V16.5M7 10.0288C7.47142 10 8.05259 10 8.8 10H15.2C15.9474 10 16.5286 10 17 10.0288M7 10.0288C6.41168 10.0647 5.99429 10.1455 5.63803 10.327C5.07354 10.6146 4.6146 11.0735 4.32698 11.638C4 12.2798 4 13.1198 4 14.8V16.2C4 17.8802 4 18.7202 4.32698 19.362C4.6146 19.9265 5.07354 20.3854 5.63803 20.673C6.27976 21 7.11984 21 8.8 21H15.2C16.8802 21 17.7202 21 18.362 20.673C18.9265 20.3854 19.3854 19.9265 19.673 19.362C20 18.7202 20 17.8802 20 16.2V14.8C20 13.1198 20 12.2798 19.673 11.638C19.3854 11.0735 18.9265 10.6146 18.362 10.327C18.0057 10.1455 17.5883 10.0647 17 10.0288M7 10.0288V8C7 5.23858 9.23858 3 12 3C14.7614 3 17 5.23858 17 8V10.0288"
                class="dark:stroke-neutral-200 stroke-neutral-800 stroke-1"
              />
            </svg>

            <input
              id="password"
              v-model="password"
              type="password"
              required
              class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 dark:text-neutral-200 rounded text-sm pl-10"
              placeholder="******"
            />
          </div>
        </div>
        <div class="flex items-center justify-between my-5">
          <label class="flex items-center text-neutral-400 hover:text-neutral-700">
            <input type="checkbox" class="rounded border-gray-300 size-4 accent-sky-600" />
            <span class="ml-2 text-sm">Remember me</span>
          </label>
          <a
            href="#"
            class="text-sm text-neutral-400 dark:hover:text-neutral-200 hover:text-neutral-600"
            >Forgot password?</a
          >
        </div>

        <button
          type="submit"
          class="flex items-center justify-center w-full bg-sky-600 text-white p-2 rounded hover:bg-sky-700"
          :disabled="isLoading"
        >
          <svg
            viewBox="0 0 24 24"
            class="size-6 fill-none mr-1"
            xmlns="http://www.w3.org/2000/svg"
            :class="isLoading ? 'hidden' : ''"
          >
            <path
              d="M14 4L17.5 4C20.5577 4 20.5 8 20.5 12C20.5 16 20.5577 20 17.5 20H14M15 12L3 12M15 12L11 16M15 12L11 8"
              class="stroke-neutral-200 stroke-1"
            />
          </svg>
          {{ isLoading ? "" : "Login" }}
          <svg
            v-if="isLoading"
            class="animate-spin size-6 text-white"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            :class="isLoading ? '' : 'hidden'"
          >
            <path
              d="M12 21C10.5316 20.9987 9.08574 20.6382 7.78865 19.9498C6.49156 19.2614 5.38261 18.2661 4.55853 17.0507C3.73446 15.8353 3.22029 14.4368 3.06088 12.977C2.90147 11.5172 3.10167 10.0407 3.644 8.67604C4.18634 7.31142 5.05434 6.10024 6.17229 5.14813C7.29024 4.19603 8.62417 3.53194 10.0577 3.21378C11.4913 2.89563 12.9809 2.93307 14.3967 3.32286C15.8124 3.71264 17.1113 4.44292 18.18 5.45C18.3205 5.59062 18.3993 5.78125 18.3993 5.98C18.3993 6.17875 18.3205 6.36937 18.18 6.51C18.1111 6.58075 18.0286 6.63699 17.9376 6.67539C17.8466 6.71378 17.7488 6.73357 17.65 6.73357C17.5512 6.73357 17.4534 6.71378 17.3624 6.67539C17.2714 6.63699 17.189 6.58075 17.12 6.51C15.8591 5.33065 14.2303 4.62177 12.508 4.5027C10.7856 4.38362 9.07478 4.86163 7.66357 5.85624C6.25237 6.85085 5.22695 8.30132 4.75995 9.96345C4.29296 11.6256 4.41292 13.3979 5.09962 14.9819C5.78633 16.5659 6.99785 17.865 8.53021 18.6604C10.0626 19.4558 11.8222 19.6989 13.5128 19.3488C15.2034 18.9987 16.7218 18.0768 17.8123 16.7383C18.9028 15.3998 19.4988 13.7265 19.5 12C19.5 11.8011 19.579 11.6103 19.7197 11.4697C19.8603 11.329 20.0511 11.25 20.25 11.25C20.4489 11.25 20.6397 11.329 20.7803 11.4697C20.921 11.6103 21 11.8011 21 12C21 14.3869 20.0518 16.6761 18.364 18.364C16.6761 20.0518 14.387 21 12 21Z"
              class="fill-neutral-200"
            />
          </svg>
        </button>
        <span class="w-full text-start text-sm text-red-500">{{ loginResponse }}</span>
      </form>
    </div>
  </div>
</template>
