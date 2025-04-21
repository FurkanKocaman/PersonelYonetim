<script setup lang="ts">
import type { PersonelDetayUpdateModel } from "@/models/request-models/PersonelDetayUpdateModel";
import type { PersonelDetaylarGetModel } from "@/models/response-models/PersonelDetaylarGetModel";
import PersonelService from "@/services/PersonelService";
import { onMounted, reactive, ref } from "vue";

const vatandaslikForm = ref(false);
const aileForm = ref(false);
const bankaForm = ref(false);
const iletisimForm = ref(false);
const acilDurumForm = ref(false);

const personel: PersonelDetaylarGetModel = reactive({
  id: "",
  personelId: "",
  fullName: "",
  avatarUrl: undefined,
  iletisim: {
    eposta: "",
    telefon: "",
  },
  adres: {
    ulke: "",
    sehir: "",
    ilce: "",
    tamAdres: "",
  },
  kurumsalBirimAd: "",
  pozisyonAd: "",
  gorevlendirmeTipi: "",
  calismaSekli: "",
  yoneticiAd: undefined,
  yoneticiPozisyon: undefined,
  baslangicTarih: undefined,
  bitisTarih: undefined,

  // Kimlik Bilgileri
  tckn: undefined,
  nufusIl: undefined,
  nufusIlce: undefined,
  anaAdi: undefined,
  babaAdi: undefined,
  dogumYeri: undefined,
  dogumTarihi: new Date().toISOString(),
  medeniHali: undefined,
  cinsiyet: undefined,
  uyruk: undefined,

  // İletişim Bilgileri
  isTelefonu: undefined,
  epostaIs: undefined,
  postaKodu: undefined,

  // Eğitim Bilgileri
  egitimDurumu: undefined,
  mezuniyetOkulu: undefined,
  mezuniyetBolumu: undefined,
  mezuniyetTarihi: undefined,

  // Askerlik Bilgileri
  askerlikDurumu: undefined,
  askerlikTarihi: undefined,

  // Ehliyet Bilgileri
  ehliyetSinifi: undefined,
  ehliyetVerilisTarihi: undefined,

  // Sağlık Bilgileri
  engelliMi: false,
  engelOrani: undefined,
  saglikDurumu: undefined,
  kanGrubu: undefined,

  // Acil Durum Bilgileri
  acilDurumKisiAdi: undefined,
  acilDurumKisiTelefon: undefined,
  acilDurumKisiYakinlik: undefined,

  // Aile Bilgileri
  cocukSayisi: undefined,
  esCalisiyorMu: undefined,

  // Banka Bilgileri
  bankaAdi: undefined,
  iban: undefined,

  // Diğer
  notlar: undefined,
  tenantId: undefined,

  isActive: true,
  createdAt: new Date(),
  createUserId: undefined,
  createUserName: undefined,
  updateAt: undefined,
  updateUserId: undefined,
  isDeleted: false,
  deleteAt: undefined,
});
const personelUpdateRequest: PersonelDetayUpdateModel = reactive({
  id: "",
  personelId: "",

  // Kimlik Bilgileri
  tckn: "",
  nufusIl: "",
  nufusIlce: "",
  anaAdi: "",
  babaAdi: "",
  dogumYeri: "",
  dogumTarihi: undefined,
  medeniHali: "",
  cinsiyet: "",
  uyruk: "",

  // İletişim Bilgileri
  cepTelefonu: "",
  isTelefonu: "",
  eposta: "",
  epostaIs: "",
  adres: "",
  ikametIl: "",
  ikametIlce: "",
  postaKodu: "",

  // Eğitim Bilgileri
  egitimDurumu: "",
  mezuniyetOkulu: "",
  mezuniyetBolumu: "",
  mezuniyetTarihi: undefined,

  // Askerlik Bilgileri
  askerlikDurumu: "",
  askerlikTarihi: undefined,

  // Ehliyet Bilgileri
  ehliyetSinifi: "",
  ehliyetVerilisTarihi: undefined,

  // Sağlık Bilgileri
  engelliMi: false,
  engelOrani: 0,
  saglikDurumu: "",
  kanGrubu: "",

  // Acil Durum Bilgileri
  acilDurumKisiAdi: "",
  acilDurumKisiTelefon: "",
  acilDurumKisiYakinlik: "",

  // Aile Bilgileri
  cocukSayisi: 0,
  esCalisiyorMu: false,

  // Banka Bilgileri
  bankaAdi: "",
  iban: "",

  // Diğer
  notlar: "",
});

// const apiUrl = ref(import.meta.env.VITE_API_URL);

onMounted(async () => {
  const res = await PersonelService.getPersonelDetaylar();
  Object.assign(personel, res);
  Object.assign(personelUpdateRequest, res);
  console.log(personelUpdateRequest);
});

const personelDetayUpdate = async () => {
  Object.assign(personelUpdateRequest, personel);
  await PersonelService.updatePersonelDetaylar(personelUpdateRequest);
};
</script>

<template>
  <div class="space-y-6 flex flex-col md:flex-row text-neutral-700 dark:text-neutral-100">
    <div class="md:flex-3 flex flex-col md:py-10 md:pl-10">
      <!-- Vatandaşlık -->
      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md mb-5 mx-2">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Vatandaşlık</h2>
          <button @click="vatandaslikForm = !vatandaslikForm">
            <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1"></i>
          </button>
        </div>
        <div class="p-4 rounded-lg space-y-3 dark:bg-neutral-700/20 shadow-md bg-neutral-200/40">
          <!-- Doğum Tarihi - Cinsiyet -->
          <div class="flex justify-between">
            <div class="flex-1">
              <p class="text-sm">Doğum Tarihi</p>
              <p class="text-sm">01.01.2000</p>
            </div>
            <div class="flex-1">
              <p class="text-sm">Cinsiyet</p>
              <p class="text-sm">{{ personel.cinsiyet }}</p>
            </div>
          </div>

          <!-- Engel Derecesi -->
          <div>
            <p class="text-sm">Engel Derecesi</p>
            <p class="">{{ personel.engelOrani || "—" }}</p>
          </div>

          <hr class="my-4 border-gray-300 dark:border-gray-600" />
          <br />
          <!-- Uyruğu - Kimlik Numarası -->
          <div class="flex justify-between w-full">
            <div class="flex-1">
              <p class="text-sm">Uyruğu</p>
              <p class="text-sm">{{ personel.uyruk ?? "-" }}</p>
            </div>
            <div class="flex-1">
              <div>
                <p class="text-sm">Kimlik Numarası</p>
                <p class="text-sm">{{ personel.tckn ?? "-" }}</p>
              </div>
              <br />
            </div>
          </div>

          <!-- Askerlik Durumu -->
          <div>
            <p class="text-sm">Askerlik Durumu</p>
            <p class="text-sm">{{ personel.askerlikDurumu ?? "-" }}</p>
          </div>
        </div>
      </div>

      <!-- Eğitim -->
      <div class="bg-nwutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md mb-5 mx-2">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Eğitim</h2>
        </div>
        <div class="dark:bg-neutral-700/20 shadow-md bg-neutral-200/40 p-4 rounded-lg space-y-3">
          <div class="flex justify-between">
            <div class="flex-1">
              <p class="text-base">Eğitim Durumu</p>
              <p class="text-sm">{{ personel.egitimDurumu ?? "-" }}</p>
            </div>
            <div class="flex-1">
              <p class="text-base">Mezun Olunan Bölüm</p>
              <p class="text-sm">{{ personel.mezuniyetBolumu ?? "-" }}</p>
            </div>
          </div>
          <br />

          <div>
            <p class="text-sm">Son Tamamlanan Eğitim Kurumu</p>
            <p class="">{{ personel.mezuniyetOkulu || "—" }}</p>
          </div>
        </div>
      </div>

      <!-- Aile -->
      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md mb-5 mx-2">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Aile</h2>
          <button @click="aileForm = !aileForm">
            <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1"></i>
          </button>
        </div>
        <div
          class="dark:bg-neutral-700/20 shadow-md bg-neutral-200/40 p-4 rounded-lg space-y-3"
          style="height: 150px"
        >
          <div class="flex justify-between">
            <div class="flex-1">
              <p class="text-base">Medeni Hal</p>
              <p class="text-sm">{{ personel.medeniHali ?? "—" }}</p>
            </div>
            <div class="flex-1">
              <p class="text-base">Eş Çalışma Durumu</p>
              <p class="text-sm">{{ personel.esCalisiyorMu || "—" }}</p>
            </div>
          </div>

          <br />
          <div>
            <p class="text-base">Çocuk Sayısı</p>
            <p class="text-sm">{{ personel.cocukSayisi || "—" }}</p>
          </div>
        </div>
      </div>

      <!-- Adres -->
      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md mb-5 mx-2">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Adres</h2>
        </div>
        <div class="dark:bg-neutral-700/20 shadow-md bg-neutral-200/40 p-4 rounded-lg space-y-3">
          <div class="flex justify-between">
            <div>
              <p class="text-base">Adres</p>
              <p class="text-sm">{{ "—" }}</p>
            </div>
          </div>

          <div>
            <p class="text-base">Adres (devam)</p>
            <p class="text-base">{{ personel.adres.tamAdres || "—" }}</p>
          </div>
          <div class="flex">
            <div class="flex flex-col justify-start">
              <div>
                <p class="text-base">Şehir</p>
                <p class="text-sm">{{ personel.adres.sehir || "—" }}</p>
              </div>
              <div class="mt-3">
                <p class="text-base">Posta Kodu</p>
                <p class="text-sm">{{ personel.postaKodu || "—" }}</p>
              </div>
            </div>
            <div class="flex flex-col justify-start ml-[10rem]">
              <div class="">
                <p class="text-base">Ülke</p>
                <p class="text-sm">{{ personel.adres.ulke || "—" }}</p>
              </div>
              <div class="mt-3">
                <p class="text-base">Telefon</p>
                <p class="text-sm">{{ personel.iletisim.telefon || "—" }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- Banka hesabı -->

      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md mx-2">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Banka Hesabı</h2>
          <button @click="bankaForm = !bankaForm">
            <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1"></i>
          </button>
        </div>
        <div
          class="dark:bg-neutral-700/20 shadow-md bg-neutral-200/40 p-4 rounded-lg space-y-3 flex justify-between"
        >
          <div class="flex justify-between flex-col">
            <div>
              <p class="text-sm">Banka Adı</p>
              <p class="text-sm">{{ personel.bankaAdi || "—" }}</p>
            </div>
            <!-- <div class="mt-5">
              <p class="text-sm">Hesap Tipi</p>
              <p class="text-sm">{{ personel.bankaHesabi.hesapTipi || "—" }}</p>
            </div> -->
          </div>

          <div class="flex justify-between flex-col">
            <!-- <div>
              <p class="text-sm">Hesap Numarası</p>
              <p class="text-sm">{{ personel.iban || "—" }}</p>
            </div> -->
            <div class="">
              <p class="text-sm">IBAN</p>
              <p class="text-sm">{{ personel.iban ?? "—" }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="md:flex-2 flex flex-col md:px-10 md:py-5">
      <!-- İletişim -->
      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 md:my-5 rounded-lg shadow-md mx-2">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">İletişim</h2>
          <button @click="iletisimForm = !iletisimForm">
            <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1"></i>
          </button>
        </div>
        <div class="dark:bg-neutral-700/20 shadow-md bg-neutral-200/40 p-4 rounded-lg space-y-3">
          <div class="flex items-center space-x-3">
            <i
              class="fa-solid fa-eye-slash"
              style="color: #3562b1"
              title="iş arkadaşlarına gösterilmez"
            ></i>
            <div class="flex-1">
              <p class="text-sm">E-Posta (Kişisel)</p>
              <p class="text-blue-600 font-medium">{{ personel.iletisim.eposta || "—" }}</p>
            </div>
          </div>
          <hr class="my-4 border-gray-300 dark:border-gray-600" />

          <div class="flex items-center space-x-3">
            <i
              class="fa-solid fa-eye-slash"
              style="color: #3562b1"
              title="iş arkadaşlarına gösterilmez"
            ></i>
            <div class="flex-1">
              <p class="text-sm">Telefon (Kişisel)</p>
              <p class="text-blue-600 font-medium">{{ personel.iletisim.telefon || "—" }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Acil Durum -->

      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 mt-4 rounded-lg shadow-md mx-2">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Acil Durum</h2>
          <button @click="acilDurumForm = !acilDurumForm">
            <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1"></i>
          </button>
        </div>
        <div class="dark:bg-neutral-700/20 shadow-md bg-neutral-200/40 p-4 rounded-lg space-y-3">
          <div class="flex justify-between">
            <div>
              <p class="text-sm">Acil durumda erişilebilecek kişinin bilgileri</p>
            </div>
          </div>

          <div>
            <p class="text-sm">Adı Soyadı</p>
            <p class="">{{ personel.acilDurumKisiAdi || "—" }}</p>
          </div>

          <div>
            <p class="text-sm">Telefon</p>
            <p class="">{{ personel.acilDurumKisiTelefon || "—" }}</p>
          </div>
          <div>
            <p class="text-sm">Yakınlık Derecesi</p>
            <p class="">{{ personel.acilDurumKisiYakinlik || "—" }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- vatandaşlık form -->
    <div
      v-if="vatandaslikForm"
      class="fixed inset-0 flex items-center justify-center bg-black/30 backdrop-blur-xs z-50"
    >
      <div class="bg-neutral-100 dark:bg-neutral-800 p-6 rounded-lg shadow-xl w-[700px]">
        <h2 class="text-xl mb-6">Vatandaşlık</h2>
        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <!-- İş E-Posta ve Telefon -->
        <div class="grid grid-cols-2 gap-4 mb-6">
          <div>
            <label class="block text-sm font-semibold mb-1">Doğum Tarihi</label>
            <input
              disabled
              type="text"
              v-model="personel.dogumTarihi"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            />
          </div>
          <div>
            <label class="block text-sm font-semibold mb-1">Cinsiyet</label>

            <select
              disabled
              id="vatandaslikCinsiyet"
              v-model="personel.cinsiyet"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            >
              <option value="">—</option>
              <option value="Erkek">Erkek</option>
              <option value="Kadın">Kadın</option>
            </select>
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold mb-1">Engel Derecesi</label>

          <select
            id="engelDerece"
            v-model="personel.engelOrani"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
          >
            <option :value="null">yok</option>
            <option value="%20">%20</option>
            <option value="%40">%40</option>
            <option value="%60">%60</option>
            <option value="%80">%80</option>
          </select>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <div class="grid grid-cols-2 gap-4 mb-6">
          <div>
            <label class="block text-sm font-semibold mb-1">Uyruğu</label>
            <input
              disabled
              type="text"
              v-model="personel.uyruk"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            />
          </div>
          <div>
            <div>
              <label class="block text-sm font-semibold mb-1">Kimlik Numarası</label>
              <input
                disabled
                type="text"
                v-model="personel.tckn"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
              />
            </div>
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold mb-1">Askerlik Durumu</label>

          <input
            type="text"
            v-model="personel.askerlikDurumu"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
          />
        </div>

        <br />

        <div class="flex justify-end gap-2">
          <button
            @click="vatandaslikForm = false"
            class="text-gray-900 hover:text-white border border-gray-800 hover:bg-gray-900 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-gray-600 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-800"
          >
            İptal
          </button>
          <button
            @click="
              () => {
                personelDetayUpdate();
                vatandaslikForm = false;
              }
            "
            class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
          >
            Kaydet
          </button>
        </div>
      </div>
    </div>

    <!-- aile form -->
    <div
      v-if="aileForm"
      class="fixed inset-0 flex items-center justify-center bg-black/30 backdrop-blur-xs z-50"
    >
      <div class="bg-neutral-100 dark:bg-neutral-800 p-6 rounded-lg shadow-xl w-1/2">
        <h2 class="text-xl mb-6">Aile</h2>
        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <div class="grid grid-cols-2 gap-4 mb-6">
          <div>
            <label class="block text-sm font-semibold mb-1">Medeni Hal</label>
            <!-- <input
            v-model="acilDurumAdSoyad"
              type="text"
              value=""
              class="w-full border border-gray-300 rounded px-3 py-2 bg-gray-100" -->

            <select
              id="seçenekler"
              v-model="personel.medeniHali"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            >
              <option value="Evli">Evli</option>
              <option value="Bekar">Bekar</option>
              <option value="Boşanmış">Boşanmış</option>
              <option value="Belirtilmemiş">Belirtilmemiş</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-semibold mb-1">Eş Çalışma Durumu</label>

            <select
              id="seçenekler"
              v-model="personel.esCalisiyorMu"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            >
              tion>
              <option value="true">Çalışıyor</option>
              <option value="false">Çalışmıyor</option>
            </select>
          </div>
        </div>

        <div>
          <label for="aileCocoukSayisi" class="block text-sm font-semibold mb-1"
            >Çocuk Sayısı</label
          >
          <input
            id="aileCocoukSayisi"
            type="text"
            v-model="personel.cocukSayisi"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
          />
        </div>
        <br />

        <div class="flex justify-end gap-2">
          <button
            @click="aileForm = false"
            class="text-gray-900 hover:text-white border border-gray-800 hover:bg-gray-900 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-gray-600 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-800"
          >
            İptal
          </button>
          <button
            @click="
              () => {
                personelDetayUpdate();
                aileForm = false;
              }
            "
            class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
          >
            Kaydet
          </button>
        </div>
      </div>
    </div>

    <!-- banka form -->
    <div
      v-if="bankaForm"
      class="fixed inset-0 flex items-center justify-center bg-black/30 backdrop-blur-xs z-50"
    >
      <div class="bg-neutral-100 dark:bg-neutral-800 p-6 rounded-lg shadow-xl w-1/2">
        <h2 class="text-xl mb-6">Banka Hesabı</h2>
        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <div class="grid grid-cols-2 gap-4 mb-6">
          <div>
            <label class="block text-sm font-semibold mb-1">Banka Adı</label>
            <input
              type="text"
              v-model="personel.bankaAdi"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            />
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4 mb-6">
          <div>
            <label class="block text-sm font-semibold mb-1">IBAN</label>
            <input
              type="text"
              v-model="personel.iban"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            />
          </div>
        </div>

        <br />

        <div class="flex justify-end gap-2">
          <button
            @click="bankaForm = false"
            class="text-gray-900 hover:text-white border border-gray-800 hover:bg-gray-900 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-gray-600 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-800"
          >
            İptal
          </button>
          <button
            @click="
              () => {
                personelDetayUpdate();
                bankaForm = false;
              }
            "
            class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
          >
            Kaydet
          </button>
        </div>
      </div>
    </div>

    <div>
      <!--iletişim form -->
      <div
        v-if="iletisimForm"
        class="fixed inset-0 flex items-center justify-center bg-black/30 backdrop-blur-xs z-50"
      >
        <div class="bg-neutral-100 dark:bg-neutral-800 p-6 rounded-lg shadow-xl w-1/2">
          <h2 class="text-xl mb-6">İletişim</h2>
          <hr class="my-4 border-gray-300 dark:border-gray-600" />

          <!-- İş E-Posta ve Telefon -->
          <div class="grid grid-cols-2 gap-4 mb-6">
            <div>
              <label class="block text-sm font-semibold mb-1">E-Posta (İş)</label>
              <input
                type="text"
                v-model="personel.epostaIs"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                disabled
              />
            </div>
            <div>
              <label class="block text-sm font-semibold mb-1">Telefon (İş)</label>
              <div class="flex items-center rounded bg-neutral-700">
                <span class="mx-2">🇹🇷</span>
                <input
                  type="text"
                  v-model="personel.isTelefonu"
                  class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                  disabled
                />
              </div>
            </div>
          </div>

          <!-- Kişisel E-Posta ve Telefon -->
          <h3 class="text-xl font-semibold mb-4 text-neutral-400">Kişisel</h3>
          <div class="grid grid-cols-2 gap-4 mb-6">
            <div>
              <label class="block text-sm font-semibold mb-1">E-Posta (Kişisel)</label>
              <input
                type="email"
                v-model="personel.iletisim.eposta"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                placeholder="E-posta (Kişisel)"
              />
            </div>
            <div>
              <label class="block text-sm font-semibold mb-1">Telefon (Kişisel)</label>

              <input
                type="text"
                v-model="personel.iletisim.telefon"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                placeholder="Telefon (Kişisel)"
              />
            </div>
          </div>

          <div class="flex justify-end gap-2">
            <button
              @click="iletisimForm = false"
              class="text-gray-900 hover:text-white border border-gray-800 hover:bg-gray-900 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-gray-600 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-800"
            >
              İptal
            </button>
            <button
              @click="
                () => {
                  personelDetayUpdate();
                  iletisimForm = false;
                }
              "
              class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
            >
              Kaydet
            </button>
          </div>
        </div>
      </div>
    </div>

    <div>
      <!-- acil durum form -->
      <div
        v-if="acilDurumForm"
        class="fixed inset-0 flex items-center justify-center bg-black/30 backdrop-blur-xs z-50"
      >
        <div class="bg-neutral-100 dark:bg-neutral-800 p-6 rounded-lg shadow-xl w-1/2">
          <h2 class="text-xl mb-6">Acil Durum</h2>
          <hr class="my-4 border-gray-300 dark:border-gray-600" />

          <div class="grid grid-cols-2 gap-4 mb-6">
            <div>
              <label class="block text-sm font-semibold mb-1">Adı Soyadı</label>
              <input
                type="text"
                v-model="personel.acilDurumKisiAdi"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
              />
            </div>
            <div>
              <label class="block text-sm font-semibold mb-1">Telefon</label>

              <input
                type="text"
                v-model="personel.acilDurumKisiTelefon"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
              />
            </div>
          </div>

          <div>
            <label class="block text-sm font-semibold mb-1">Yakınlık Derecesi</label>
            <input
              type="text"
              v-model="personel.acilDurumKisiYakinlik"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            />
          </div>
          <br />

          <div class="flex justify-end gap-2">
            <button
              @click="acilDurumForm = false"
              class="text-gray-900 hover:text-white border border-gray-800 hover:bg-gray-900 focus:ring-4 focus:outline-none focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-gray-600 dark:text-gray-400 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-800"
            >
              İptal
            </button>
            <button
              @click="
                () => {
                  personelDetayUpdate();
                  acilDurumForm = false;
                }
              "
              class="text-blue-700 hover:text-white border border-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center me-2 mb-2 dark:border-blue-500 dark:text-blue-500 dark:hover:text-white dark:hover:bg-blue-500 dark:focus:ring-blue-800"
            >
              Kaydet
            </button>
          </div>
        </div>
      </div>
    </div>

    <br />
  </div>
</template>
