<script setup lang="ts">
import { ref, onMounted } from "vue";
import type { LoginRequest } from "@/models/LoginRequest";
import AuthService from "@/services/AuthService";

const email = ref("");
const password = ref("");
const isLoading = ref(false);
const loginResponse = ref("");


const images = ref([
  "https://www.fotovizyon.net/wp-content/uploads/2023/10/MG_50301.jpg",
  "https://lwfiles.mycourse.app/64c2ad6a42f5698b2785c5da-public/6838886abe627023dbd894f478353a0d.jpeg",
  "https://images.pexels.com/photos/290538/pexels-photo-290538.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
  "https://www.fotovizyon.net/wp-content/uploads/2023/10/MG_50301.jpg",
  "https://istanbulpolisekip1.org/upload/galeri/germany_nature_wide_1366x768.jpg"
]);

const currentImageIndex = ref(0); 


const changeImage = () => {
  currentImageIndex.value = (currentImageIndex.value + 1) % images.value.length;
};


const nextImage = () => {
  currentImageIndex.value = (currentImageIndex.value + 1) % images.value.length;
};


const prevImage = () => {
  currentImageIndex.value = (currentImageIndex.value - 1 + images.value.length) % images.value.length;
};


onMounted(() => {
  setInterval(changeImage, 4000); 
});

const handleLogin = async () => {
  try {
    isLoading.value = true;
    const loginData: LoginRequest = { usernameOrEmail: email.value, password: password.value };
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
    <div class="w-full h-full flex items-center justify-center bg-gradient-to-r from-sky-600 to-sky-700">
      
      <div class="relative w-full h-full">
        <img
          :src="images[currentImageIndex]"
          alt="Personel Yönetim"
          class="object-cover w-full h-full"
        />
        <button
          @click="prevImage"
          class="absolute left-0 top-1/2 transform -translate-y-1/2 bg-opacity-50 p-2 text-white"
        >
          ‹
        </button>
        <button
          @click="nextImage"
          class="absolute right-0 top-1/2 transform -translate-y-1/2 bg-opacity-50 p-2 text-white"
        >
          ›
        </button>
      </div>

      
    </div>
    <div class="flex flex-col items-center justify-center md:w-1/2 h-full dark:bg-neutral-700">

    <div class="flex flex-col items-center justify-center md:w-1/2 h-full dark:bg-neutral-700 ">

      

      <form @submit.prevent="handleLogin" class="space-y-4 w-full px-16">
        <div class="formDisi ">
          <h1 class="text-2xl font-semibold text-center mb-6 text-white">Giriş Sayfası</h1>
        <div>
          <label for="email" class="block text-sm/5 font-semibold my-2 text-white">Eposta | Kullanıcı adı</label>
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
              class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 dark:text-neutral-200 rounded text-sm pl-10 text-white"
              placeholder="user@mail.com"
            />
          </div>
        </div>
        <div>
          <label for="password" class="block text-sm/5 font-semibold my-2 text-white">Şifre</label>
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
              class="w-full outline-1 outline-neutral-300 dark:outline-neutral-800/20 dark:bg-neutral-600/20 focus:shadow-[0px_0px_3px_2px_rgba(59,_130,_246,_0.5)] p-3 dark:text-neutral-200 rounded text-sm pl-10 text-white"
              placeholder="******"
            />
          </div>
        </div>
        <div class="flex items-center justify-between my-5">
          <label class="flex items-center text-neutral-400 hover:text-neutral-700">
            <input type="checkbox" class="rounded border-gray-300 size-4 accent-sky-600" />
            <span class="ml-2 text-sm text-white">Remember me</span>
          </label>
          <a
            href="#"
            class="text-sm text-neutral-400 dark:hover:text-neutral-200 hover:text-neutral-600 text-white"
            >Forgot password?</a
          >
        </div>

        <button
          type="submit" style="cursor:pointer"
          class="flex items-center justify-center w-full bg-gradient-to-r from-purple-600 via-purple-700 to-purple-800 text-white p-2 rounded hover:from-purple-700 hover:via-purple-800 hover:to-purple-900"
          :disabled="isLoading"
        >
          <svg
            viewBox="0 0 24 24"
            class="w-4 h-4 fill-none dark:fill-neutral-200 mr-3"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M12 8V16M8 12H16"
              class="stroke-neutral-800 dark:stroke-neutral-200"
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
body{
  background: linear-gradient(135deg, #c7d6df, #2e1588);

}
.formDisi{
  background: rgba(255, 255, 255, 0.1);
            padding-top: 50px;
            padding-left:50px;
            padding-right:50px;
            border-radius: 12px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
            text-align: center;
            backdrop-filter: blur(10px);
            width: 100%;
            height: 450px;
            position: relative;
}



</style>