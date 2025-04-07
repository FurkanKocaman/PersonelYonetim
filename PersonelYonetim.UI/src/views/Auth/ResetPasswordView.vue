<script setup lang="ts">
import { ref, onMounted, type Ref } from "vue";
import AuthService from "@/services/AuthService";
import { useRoute } from "vue-router";
import type { PasswordResetModel } from "@/models/request-models/PasswordResetModel";
import router from "@/router";

const isLoading = ref(false);

const request: Ref<PasswordResetModel> = ref({
  userId: "",
  token: "",
  newPassword: "",
});

const response: Ref<
  | {
      data: string;
      errorMessages: string[];
      isSuccessful: boolean;
      statusCode: number;
    }
  | undefined
> = ref(undefined);

const route = useRoute();

const userId = route.query.userId as string;
const token = route.query.token as string;

onMounted(() => {});

const handlePasswordReset = async () => {
  request.value!.userId = userId;
  request.value!.token = token;
  const res = await AuthService.ResetPassword(request.value);
  response.value = res;
  if (response.value?.isSuccessful) {
    router.push({ name: "login" });
  }
};
</script>

<template>
  <div class="flex justify-center items-center h-dvh w-full">
    <div
      class="flex flex-col items-center justify-center w-full xl:w-[50dvw] xl:h-[50dvh] bg-neutral-200 dark:bg-neutral-700 rounded-lg py-10 mx-5"
    >
      <form @submit.prevent="handlePasswordReset" class="w-full px-5 md:px-10">
        <div>
          <h1 class="text-2xl font-semibold text-center mb-6">Yeni Şifre</h1>
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
                v-model="request.newPassword"
                type="password"
                required
                class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 rounded text-sm pl-10"
                placeholder="******"
              />
            </div>
          </div>

          <div class="w-full flex items-center justify-center">
            <button
              type="submit"
              class="flex mt-10 group text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
              :disabled="isLoading"
            >
              <span>Kaydet</span>
            </button>
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
