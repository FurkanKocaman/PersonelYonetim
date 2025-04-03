<script setup lang="ts">
import TableLayout from "@/components/TableLayout.vue";
import { computed, ref } from "vue";

const veriler = ref([
  {
    baslangicTarihi: "2024-11-20T21:00",
    sure: "3 saat",
    aciklama: "Bilet: 8105-YKS-Online kayıt başvurusu...",
    durum: "Onaylandı",
    olusturmaTarihi: "2024-11-21T01:16",
  },
  {
    baslangicTarihi: "2024-11-10T18:30",
    sure: "1 saat",
    aciklama: "Tarsus Bilet No: 9241 HAZIRLIK DETAY BİLGİSİ...",
    durum: "Onaylandı",
    olusturmaTarihi: "2024-11-10T23:55",
  },
  {
    baslangicTarihi: "2024-11-09T23:00",
    sure: "1 saat",
    aciklama: "Trakya hazırlık detay gitmeyenler 440 öğrenci...",
    durum: "Onaylandı",
    olusturmaTarihi: "2024-11-10T01:36",
  },
  {
    baslangicTarihi: "2024-11-07T23:00",
    sure: "1 saat",
    aciklama: "Rize hazırlık detay listesinin gönderilmesi tamamlandı...",
    durum: "Onaylandı",
    olusturmaTarihi: "2024-11-08T00:01",
  },
  {
    baslangicTarihi: "2024-10-21T23:00",
    sure: "2 saat",
    aciklama: "YÖS için uyruk program kontenjan görüntüleme...",
    durum: "Onaylandı",
    olusturmaTarihi: "2024-10-22T02:21",
  },
]);

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

const yillar = Array.from({ length: 2025 - 2007 + 1 }, (_, i) => (2025 - i).toString());

const filtrelenmisVeri = computed(() => {
  return veriler.value.filter((kayit) => {
    const yil = kayit.baslangicTarihi.split("-")[0];
    const ay = kayit.baslangicTarihi.split("-")[1];

    return (
      (secilenYil.value === "" || secilenYil.value === yil) &&
      (secilenAy.value === "" || secilenAy.value === ay) &&
      (secilenDurum.value === "" || secilenDurum.value === kayit.durum)
    );
  });
});
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

    <div>
      <TableLayout
        :table-headers="['Başlangıç', 'Süre', 'Açıklama', 'Durum', 'Oluşturulma Tarihi']"
        :table-content="filtrelenmisVeri"
        :islemler="['detaylar']"
      />
    </div>
  </div>
</template>
