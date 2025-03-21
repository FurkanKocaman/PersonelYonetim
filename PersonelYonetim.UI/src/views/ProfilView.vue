<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
const activeTab = ref('genel');
const setActiveTab = (tab: string) => {
  activeTab.value = tab;
};
</script>

<template>
<div class="mb-6 border-b border-gray-200 dark:border-gray-700">
        <ul class="flex flex-wrap -mb-px">
          <li class="mr-2">
            <button 
              @click="setActiveTab('profilim')" 
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="activeTab === 'profilim' ? 'text-sky-600 border-sky-600' : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'"
            >
            <i class="fa-solid fa-user"></i> Profilim
            </button>
          </li>
          <li class="mr-2">
            <button 
              @click="setActiveTab('kisiselBilgilerim')" 
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="activeTab === 'kisiselBilgilerim' ? 'text-sky-600 border-sky-600' : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'"
            >
            <i class="fa-solid fa-person"></i> Kişisel Bilgilerim
            </button>
          </li>
          <li class="mr-2">
            <button 
              @click="setActiveTab('kariyerim')" 
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="activeTab === 'kariyerim' ? 'text-sky-600 border-sky-600' : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'"
            >
            <i class="fa-solid fa-user-tie"></i> Kariyerim
            </button>
          </li>
          <li class="mr-2">
            <button 
              @click="setActiveTab('izinlerim')" 
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="activeTab === 'izinlerim' ? 'text-sky-600 border-sky-600' : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'"
            >
            <i class="fa-solid fa-mug-hot"></i> İzinlerim
            </button>
          </li>

          <li class="mr-2">
            <button 
              @click="setActiveTab('odemelerim')" 
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="activeTab === 'odemelerim' ? 'text-sky-600 border-sky-600' : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'"
            >
            <i class="fa-solid fa-money-bill"></i> Ödemelerim
            </button>
          </li>

          <li class="mr-2">
            <button 
              @click="setActiveTab('mesailerim')" 
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="activeTab === 'mesailerim' ? 'text-sky-600 border-sky-600' : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'"
            >
            <i class="fa-solid fa-briefcase"></i> Mesailerim
            </button>
          </li>

          <li class="mr-2">
            <button 
              @click="setActiveTab('diger')" 
              class="inline-block py-4 px-4 text-sm font-medium text-center border-b-2 rounded-t-lg"
              :class="activeTab === 'diger' ? 'text-sky-600 border-sky-600' : 'text-gray-500 border-transparent hover:text-gray-600 hover:border-gray-300 dark:text-gray-400 dark:hover:text-gray-300'"
            >
            <i class="fa-solid fa-square-poll-horizontal"></i> Diğer
            </button>
          </li>
        </ul>
      </div>



      <!-- <div v-if="activeTab === 'profilim'" class="space-y-6">
       

     
            
      </div> -->



</template>