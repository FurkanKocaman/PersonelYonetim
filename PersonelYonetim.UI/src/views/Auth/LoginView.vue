<script setup lang="ts">
import { ref, onMounted } from "vue";
import type { LoginRequest } from "@/models/request-models/LoginRequest";
import AuthService from "@/services/AuthService";
import { useRouter } from "vue-router";

const router = useRouter();

const email = ref("");
const password = ref("");
const isLoading = ref(false);
const loginResponse = ref("");

onMounted(() => {});

const handleLogin = async () => {
  try {
    isLoading.value = true;
    const loginData: LoginRequest = { usernameOrEmail: email.value, password: password.value };
    const response = await AuthService.login(loginData);
    if (response.success) {
      loginResponse.value = response.message;
      router.push("/");
    } else {
      loginResponse.value = response.message;
      console.error(response);
    }
    isLoading.value = false;
  } catch (error) {
    console.error("Error", error);
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="flex xl:justify-end h-dvh w-full bg-red-500">
    <div
      class="hidden xl:flex w-full h-full items-center justify-center bg-gradient-to-r from-sky-600 to-sky-700"
    ></div>

    <div
      class="flex flex-col items-center justify-center w-full xl:w-[50dvw] h-full bg-neutral-200 dark:bg-neutral-700"
    >
      <form @submit.prevent="handleLogin" class="w-full px-5 md:px-10">
        <div>
          <h1 class="text-2xl font-semibold text-center mb-6">Giriş Sayfası</h1>
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
            <a href="#" class="text-sm dark:hover:text-neutral-100 hover:text-neutral-900"
              >Forgot password?</a
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
