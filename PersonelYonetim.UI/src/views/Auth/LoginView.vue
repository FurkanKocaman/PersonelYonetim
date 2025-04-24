<script setup lang="ts">
import { ref, onMounted } from "vue";
import type { LoginRequest } from "@/models/request-models/LoginRequest";
import AuthService from "@/services/AuthService";
import { useRouter } from "vue-router";

const router = useRouter();

const email = ref("");
const password = ref("");
const isLoading = ref(false);
const loginResponse = ref<{ success: boolean; message: string }>({ success: true, message: "" });

onMounted(() => {});

const handleLogin = async () => {
  try {
    isLoading.value = true;
    const loginData: LoginRequest = { usernameOrEmail: email.value, password: password.value };
    const response = await AuthService.login(loginData);
    if (response.success) {
      loginResponse.value = response;
      router.push("/");
    } else {
      loginResponse.value = response;
    }
    isLoading.value = false;
  } catch (error) {
    console.error(error);
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="flex xl:justify-end h-dvh w-full">
    <div
      class="hidden xl:flex w-full h-full items-center justify-center bg-gradient-to-r from-sky-600 to-sky-700"
    ></div>

    <div
      class="flex flex-col items-center justify-center w-full xl:w-[50dvw] h-full bg-neutral-200 dark:bg-neutral-700"
    >
      <form @submit.prevent="handleLogin" class="w-full px-5 md:px-10">
        <div>
          <h1 class="text-2xl font-semibold text-center mb-6">Giriş Sayfası</h1>
          <div
            v-if="!loginResponse.success"
            class="border rounded-md border-red-600 flex items-center"
          >
            <svg
              class="size-8 fill-red-600 ml-2"
              viewBox="0 0 1024 1024"
              version="1.1"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M332 663.2c-9.6 9.6-9.6 25.6 0 35.2s25.6 9.6 35.2 0l349.6-356c9.6-9.6 9.6-25.6 0-35.2s-25.6-9.6-35.2 0L332 663.2z"
                fill=""
              />
              <path
                d="M681.6 698.4c9.6 9.6 25.6 9.6 35.2 0s9.6-25.6 0-35.2L367.2 307.2c-9.6-9.6-25.6-9.6-35.2 0s-9.6 25.6 0 35.2l349.6 356z"
                fill=""
              />
              <path
                d="M516.8 1014.4c-277.6 0-503.2-225.6-503.2-503.2S239.2 7.2 516.8 7.2s503.2 225.6 503.2 503.2-225.6 504-503.2 504z m0-959.2c-251.2 0-455.2 204.8-455.2 456s204 455.2 455.2 455.2 455.2-204 455.2-455.2-204-456-455.2-456z"
                fill=""
              />
            </svg>
            <span class="text-sm text-red-500 py-3 px-6">{{ loginResponse.message }}</span>
          </div>
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
                class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm pl-10"
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
                class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm pl-10"
                placeholder="******"
              />
            </div>
          </div>
          <div class="flex items-center justify-between my-5">
            <label class="flex items-center hover:text-neutral-800 dark:hover:text-neutral-100">
              <input type="checkbox" class="rounded border-gray-300 size-4 accent-sky-600" />
              <span class="ml-2 text-sm">Remember me</span>
            </label>
            <RouterLink
              to="/forgot-password"
              class="text-sm dark:hover:text-neutral-100 hover:text-neutral-900"
              >Şifremi unuttum?</RouterLink
            >
          </div>

          <button
            type="submit"
            class="flex items-center justify-center w-full bg-sky-600 text-neutral-200 p-2 rounded hover:bg-sky-500"
            :disabled="isLoading"
            v-on:click="handleLogin()"
          >
            <svg
              class="w-5 h-5 fill-none mr-1"
              viewBox="0 0 24 24"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M14 4L17.5 4C20.5577 4 20.5 8 20.5 12C20.5 16 20.5577 20 17.5 20H14M15 12L3 12M15 12L11 16M15 12L11 8"
                class="stroke-neutral-200"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
            </svg>
            <span>Giriş Yap</span>
          </button>
          <hr class="my-5" />
          <div class="w-full flex justify-center">
            <RouterLink
              to="/register"
              class="text-gray-900 hover:text-white border border-gray-800 hover:bg-gray-900 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-gray-600 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-800"
              >Şirket Oluştur</RouterLink
            >
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<style>
input:-webkit-autofill,
input:-webkit-autofill:hover,
input:-webkit-autofill:focus,
input:-webkit-autofill:active {
  -webkit-box-shadow: 0 0 0px 1000px white inset !important;
  box-shadow: 0 0 0px 1000px white inset !important;
  background-color: red !important;
  color: black !important;
}
</style>
