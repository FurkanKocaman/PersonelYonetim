<script setup lang="ts">
import { ref } from "vue";

const secilenYil = ref("");
const secilenAy = ref("");
const secilenDurum = ref("");

const aylar = [
  { etiket: "Ocak", deger: "01" },
  { etiket: "Şubat", deger: "02" },
  { etiket: "Mart", deger: "03" },
  { etiket: "Nisan", deger: "04" },
  { etiket: "Mayıs", deger: "05" },
  { etiket: "Haziran", deger: "06" },
  { etiket: "Temmuz", deger: "07" },
  { etiket: "Ağustos", deger: "08" },
  { etiket: "Eylül", deger: "09" },
  { etiket: "Ekim", deger: "10" },
  { etiket: "Kasım", deger: "11" },
  { etiket: "Aralık", deger: "12" },
];

const yillar = Array.from({ length: 2025 - 2010 + 1 }, (_, i) => (2025 - i).toString());
</script>

<template>
  <div class="space-y-6 mt-10 mx-5">
    <div class="flex w-1/2">
      <select
        v-model="secilenYil"
        class="mr-5 bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
      >
        <option value="">Yıl</option>
        <option v-for="yil in yillar" :key="yil" :value="yil">{{ yil }}</option>
      </select>

      <select
        v-model="secilenAy"
        class="mr-5 bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
      >
        <option value="">Ay</option>
        <option v-for="ay in aylar" :key="ay.deger" :value="ay.deger">{{ ay.etiket }}</option>
      </select>

      <select
        v-model="secilenDurum"
        class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
      >
        <option value="">Tümü</option>
        <option value="Ekstra İzne Çevrildi">Ekstra İzne Çevrildi</option>
        <option value="Onay Bekliyor">Onay Bekliyor</option>
        <option value="Onaylandı">Onaylandı</option>
        <option value="Ödemeye Çevrildi">Ödemeye Çevrildi</option>
        <option value="Reddedildi">Reddedildi</option>
      </select>
    </div>

    <div></div>
  </div>
</template>
