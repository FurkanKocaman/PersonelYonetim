<script setup lang="ts">
import { ref, onMounted, type Ref } from "vue";
import AuthService from "@/services/AuthService";
import router from "@/router";

const email = ref("");
const isLoading = ref(false);
const response: Ref<
  | {
      data: string;
      errorMessages: string[];
      isSuccessful: boolean;
      statusCode: number;
    }
  | undefined
> = ref(undefined);

onMounted(() => {});

const SendPasswordResetMail = async () => {
  console.log("HERE");
  isLoading.value = true;
  try {
    const res = await AuthService.SendPasswordResetMail(email.value);
    console.log(res);
    response.value = res;
  } catch (error) {
    console.error(error);
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="flex justify-center items-center h-dvh w-full">
    <div
      class="bg-neutral-200 dark:bg-neutral-700 p-2 rounded-lg w-full xl:w-[50dvw] xl:h-[50dvh] mx-5"
    >
      <span
        class="flex items-center group hover:text-blue-600 cursor-pointer"
        @click="
          () => {
            router.back();
          }
        "
      >
        <svg class="size-5 fill-none mr-1" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path
            fill-rule="evenodd"
            clip-rule="evenodd"
            d="M11.7071 4.29289C12.0976 4.68342 12.0976 5.31658 11.7071 5.70711L6.41421 11H20C20.5523 11 21 11.4477 21 12C21 12.5523 20.5523 13 20 13H6.41421L11.7071 18.2929C12.0976 18.6834 12.0976 19.3166 11.7071 19.7071C11.3166 20.0976 10.6834 20.0976 10.2929 19.7071L3.29289 12.7071C3.10536 12.5196 3 12.2652 3 12C3 11.7348 3.10536 11.4804 3.29289 11.2929L10.2929 4.29289C10.6834 3.90237 11.3166 3.90237 11.7071 4.29289Z"
            class="dark:fill-neutral-400 fill-neutral-800 group-hover:fill-blue-600"
          />
        </svg>
        Back to Login</span
      >
      <div class="flex flex-col items-center justify-center w-full rounded-lg py-10">
        <form
          v-if="!response?.isSuccessful"
          @submit.prevent="SendPasswordResetMail"
          class="w-full px-5 md:px-10"
        >
          <div>
            <h1 class="text-2xl font-semibold text-center mb-6">Şifremi unuttum</h1>

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
              <div v-if="response && !response?.isSuccessful">
                <h3 v-for="error in response.errorMessages" :key="error" class="text-red-600 mt-2">
                  {{ "*" + error }}
                </h3>
              </div>
            </div>

            <div class="w-full flex items-center justify-center">
              <button
                type="submit"
                class="flex mt-10 group text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
                :disabled="isLoading"
              >
                <span>Kod Gönder</span>
              </button>
            </div>
          </div>
        </form>
        <div v-else>
          <h3 v-if="response.isSuccessful">{{ response.data }}</h3>
        </div>
      </div>
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
