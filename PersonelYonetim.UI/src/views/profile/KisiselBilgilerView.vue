<script setup lang="ts">
import { ref } from "vue";

// profilim kısmı
const personel = {
  adSoyad: "Erkan Demir",
  unvan: "Yazılım Personeli",
  departman: "Yazılım Üretim",
  iseBaslamaTarihi: "2 Ağustos 2019",
  sozlesmeTuru: "Süresiz",
  calismaSuresi: "5 yıl 7 ay 21 gün",
  sozlesmeBitisTarihi: null,
  pozisyonBaslamaTarihi: "8 Ağustos 2024",
  calismaSekli: "Tam zamanlı",
  sirket: "ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ",
  yonetici: {
    adSoyad: "Adil Mert Şahin",
    unvan: "Yazılım Müdürü",
    resim:
      "https://www.indir.com/haber/wp-content/uploads/2021/11/anonimsinde-hesaba-profil-fotografi-nasil-eklenir-.jpg",
  },
  iletisim: {
    isEposta: "erkan.demir@elasoft.com.tr",
    isTelefon: null,
    kisiselEposta: "",
    kisiselTelefon: "+90 551 159 19 57",
  },
  vatandaslik: {
    dogumTarihi: "1 Mayıs 1989",
    cinsiyet: "Erkek",
    engelDerecesi: null,
    uyrugu: "Türkiye",
    kimlikNumarasi: "41557015086",
    askerlikDurumu: "Tamamlandı",
  },
  egitim: {
    egitimDurumu: "Mezun",
    enYuksekEgitim: "Yüksek Lisans",
    sonEgitimKurumu: null,
  },
  acilDurum: {
    adSoyad: null,
    telefon: null,
    yakinlikDerece: null,
  },
  aile: {
    medeniHal: "Evli",
    esCalismaDurumu: null,
    cocukSayisi: null,
  },
  adres: {
    adres: "Aydınlıkevler mah. Hasan Paşa cad. kardeşler apt. sitesi no:74 iç kapı no:3 ortahisar",
    adresDevam: null,
    sehir: "Trabzon",
    ulke: "Türkiye",
    telefon: null,
    postaKodu: "61000",
  },
  bankaHesabi: {
    bankaAdi: null,
    hesapTipi: null,
    hesapNumarasi: null,
    iban: "TR780001001225565740685001",
  },
};

const iletisimForm = ref(false);
const personelEmail = ref("");
const personelTelefon = ref(personel.iletisim.kisiselTelefon);

const iletisimFormKaydet = () => {
  personel.iletisim.kisiselTelefon = personelTelefon.value;
  personel.iletisim.kisiselEposta = personelEmail.value;
  iletisimForm.value = false;
};

const acilDurumForm = ref(false);
const acilDurumAdSoyad = ref(personel.acilDurum.adSoyad);
const acilDurumTelefon = ref(personel.acilDurum.telefon);
const acilDurumYakinlikDerecesi = ref(personel.acilDurum.yakinlikDerece);

const acilDurumFormKaydet = () => {
  personel.acilDurum.adSoyad = acilDurumAdSoyad.value;
  personel.acilDurum.telefon = acilDurumTelefon.value;
  personel.acilDurum.yakinlikDerece = acilDurumYakinlikDerecesi.value;
  acilDurumForm.value = false;
};

const aileForm = ref(false);
const aileMedeniHal = ref(personel.aile.medeniHal);
const aileEsCalismaDurumu = ref(personel.aile.esCalismaDurumu);
const aileCocoukSayisi = ref(personel.aile.cocukSayisi);

const aileFormKaydet = () => {
  personel.aile.medeniHal = aileMedeniHal.value;
  personel.aile.esCalismaDurumu = aileEsCalismaDurumu.value;
  personel.aile.cocukSayisi = aileCocoukSayisi.value;
  aileForm.value = false;
};

const bankaForm = ref(false);
const bankaAdi = ref(personel.bankaHesabi.bankaAdi);
const bankaHesapTipi = ref(personel.bankaHesabi.hesapTipi);
const bankaHesapNumarasi = ref(personel.bankaHesabi.hesapNumarasi);
const bankaIBAN = ref(personel.bankaHesabi.iban);

const bankaFormKaydet = () => {
  personel.bankaHesabi.bankaAdi = bankaAdi.value;
  personel.bankaHesabi.hesapTipi = bankaHesapTipi.value;
  personel.bankaHesabi.hesapNumarasi = bankaHesapNumarasi.value;
  personel.bankaHesabi.iban = bankaIBAN.value;
  bankaForm.value = false;
};

const vatandaslikForm = ref(false);
const vatandaslikDogumTarihi = ref(personel.vatandaslik.dogumTarihi);
const vatandaslikCinsiyet = ref(personel.vatandaslik.cinsiyet);
const vatandaslikEngelDerecisi = ref(personel.vatandaslik.engelDerecesi);
const vatandaslikUyruk = ref(personel.vatandaslik.uyrugu);
const vatandaslikKimlikNumarasi = ref(personel.vatandaslik.kimlikNumarasi);
const vatandaslikAskerlikDurumu = ref(personel.vatandaslik.askerlikDurumu);

const vatandaslikFormKaydet = () => {
  personel.vatandaslik.dogumTarihi = vatandaslikDogumTarihi.value;
  personel.vatandaslik.cinsiyet = vatandaslikCinsiyet.value;
  personel.vatandaslik.engelDerecesi = vatandaslikEngelDerecisi.value;
  personel.vatandaslik.uyrugu = vatandaslikUyruk.value;
  personel.vatandaslik.kimlikNumarasi = vatandaslikKimlikNumarasi.value;
  personel.vatandaslik.askerlikDurumu = vatandaslikAskerlikDurumu.value;
  vatandaslikForm.value = false;
};
</script>

<template>
  <div class="space-y-6 flex text-neutral-700 dark:text-neutral-100">
    <div class="flex-3 flex flex-col py-10 pl-10">
      <!-- Vatandaşlık -->
      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md mb-5">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Vatandaşlık</h2>
          <button @click="vatandaslikForm = true">
            <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1"></i>
          </button>
        </div>
        <div class="p-4 rounded-lg space-y-3 dark:bg-neutral-700/20 shadow-md bg-neutral-200/40">
          <!-- Doğum Tarihi - Cinsiyet -->
          <div class="flex justify-between">
            <div>
              <p class="text-sm">Doğum Tarihi</p>
              <p class="text-sm">{{ personel.vatandaslik.dogumTarihi }}</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px">
              <p class="text-sm">Cinsiyet</p>
              <p class="text-sm">{{ personel.vatandaslik.cinsiyet }}</p>
            </div>
          </div>

          <!-- Engel Derecesi -->
          <div>
            <p class="text-sm">Engel Derecesi</p>
            <p class="">{{ personel.vatandaslik.engelDerecesi || "—" }}</p>
          </div>

          <hr class="my-4 border-gray-300 dark:border-gray-600" />
          <br />
          <!-- Uyruğu - Kimlik Numarası -->
          <div class="flex justify-between">
            <div>
              <p class="text-sm">Uyruğu</p>
              <p class="text-sm">{{ personel.vatandaslik.uyrugu }}</p>
            </div>
            <div class="flex-grow" style="margin-left: 339px">
              <div>
                <p class="text-sm">Kimlik Numarası</p>
                <p class="text-sm">{{ personel.vatandaslik.kimlikNumarasi }}</p>
              </div>
              <br />
            </div>
          </div>

          <!-- Askerlik Durumu -->
          <div>
            <p class="text-sm">Askerlik Durumu</p>
            <p class="text-sm">{{ personel.vatandaslik.askerlikDurumu }}</p>
          </div>
        </div>
      </div>

      <!-- Eğitim -->
      <div class="bg-nwutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md mb-5">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Eğitim</h2>
        </div>
        <div class="dark:bg-neutral-700/20 shadow-md bg-neutral-200/40 p-4 rounded-lg space-y-3">
          <div class="flex justify-between">
            <div>
              <p class="text-base">Eğitim Durumu</p>
              <p class="text-sm">{{ personel.egitim.egitimDurumu }}</p>
            </div>
            <div class="">
              <p class="text-base">Tamamlanan En Yüksek Eğitim Seviyesi</p>
              <p class="text-sm">{{ personel.egitim.enYuksekEgitim }}</p>
            </div>
          </div>
          <br />

          <div>
            <p class="text-sm">Son Tamamlanan Eğitim Kurumu</p>
            <p class="">{{ personel.egitim.sonEgitimKurumu || "—" }}</p>
          </div>
        </div>
      </div>

      <!-- Aile -->
      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md mb-5">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Aile</h2>
          <button @click="aileForm = true">
            <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1"></i>
          </button>
        </div>
        <div
          class="dark:bg-neutral-700/20 shadow-md bg-neutral-200/40 p-4 rounded-lg space-y-3"
          style="height: 150px"
        >
          <div class="flex justify-between">
            <div>
              <p class="text-base">Medeni Hal</p>
              <p class="text-sm">{{ personel.aile.medeniHal || "—" }}</p>
            </div>
            <div class="mr-5">
              <p class="text-base">Eş Çalışma Durumu</p>
              <p class="text-sm">{{ personel.aile.esCalismaDurumu || "—" }}</p>
            </div>
          </div>

          <br />
          <div>
            <p class="text-base">Çocuk Sayısı</p>
            <p class="text-sm">{{ personel.aile.cocukSayisi || "—" }}</p>
          </div>
        </div>
      </div>

      <!-- Adres -->
      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md mb-5">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Adres</h2>
        </div>
        <div class="dark:bg-neutral-700/20 shadow-md bg-neutral-200/40 p-4 rounded-lg space-y-3">
          <div class="flex justify-between">
            <div>
              <p class="text-base">Adres</p>
              <p class="text-sm">{{ personel.adres.adres || "—" }}</p>
            </div>
          </div>

          <div>
            <p class="text-base">Adres (devam)</p>
            <p class="text-base">{{ personel.adres.adresDevam || "—" }}</p>
          </div>
          <div class="flex">
            <div class="flex flex-col justify-start">
              <div>
                <p class="text-base">Şehir</p>
                <p class="text-sm">{{ personel.adres.sehir || "—" }}</p>
              </div>
              <div class="mt-3">
                <p class="text-base">Posta Kodu</p>
                <p class="text-sm">{{ personel.adres.postaKodu || "—" }}</p>
              </div>
            </div>
            <div class="flex flex-col justify-start ml-[10rem]">
              <div class="">
                <p class="text-base">Ülke</p>
                <p class="text-sm">{{ personel.adres.ulke || "—" }}</p>
              </div>
              <div class="mt-3">
                <p class="text-base">Telefon</p>
                <p class="text-sm">{{ personel.adres.telefon || "—" }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- Banka hesabı -->

      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Banka Hesabı</h2>
          <button @click="bankaForm = true">
            <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1"></i>
          </button>
        </div>
        <div
          class="dark:bg-neutral-700/20 shadow-md bg-neutral-200/40 p-4 rounded-lg space-y-3 flex justify-between"
        >
          <div class="flex justify-between flex-col">
            <div>
              <p class="text-sm">Banka Adı</p>
              <p class="text-sm">{{ personel.bankaHesabi.bankaAdi || "—" }}</p>
            </div>
            <div class="mt-5">
              <p class="text-sm">Hesap Tipi</p>
              <p class="text-sm">{{ personel.bankaHesabi.hesapTipi || "—" }}</p>
            </div>
          </div>

          <div class="flex justify-between flex-col">
            <div>
              <p class="text-sm">Hesap Numarası</p>
              <p class="text-sm">{{ personel.bankaHesabi.hesapNumarasi || "—" }}</p>
            </div>
            <div class="">
              <p class="text-sm">IBAN</p>
              <p class="text-sm">{{ personel.bankaHesabi.iban || "—" }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="flex-2 flex flex-col px-10 py-5">
      <!-- İletişim -->
      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 my-5 rounded-lg shadow-md">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">İletişim</h2>
          <button @click="iletisimForm = true">
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
              <p class="text-blue-600 font-medium">{{ personel.iletisim.kisiselEposta || "—" }}</p>
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
              <p class="text-blue-600 font-medium">{{ personel.iletisim.kisiselTelefon || "—" }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Acil Durum -->

      <div class="bg-neutral-100 dark:bg-neutral-800 p-4 rounded-lg shadow-md">
        <div class="flex justify-between items-center mb-2">
          <h2 class="text-lg font-semibold">Acil Durum</h2>
          <button @click="acilDurumForm = true">
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
            <p class="">{{ personel.acilDurum.adSoyad || "—" }}</p>
          </div>

          <div>
            <p class="text-sm">Telefon</p>
            <p class="">{{ personel.acilDurum.telefon || "—" }}</p>
          </div>
          <div>
            <p class="text-sm">Yakınlık Derecesi</p>
            <p class="">{{ personel.acilDurum.yakinlikDerece || "—" }}</p>
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
              v-model="vatandaslikDogumTarihi"
              type="text"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            />
          </div>
          <div>
            <label class="block text-sm font-semibold mb-1">Cinsiyet</label>

            <select
              disabled
              v-model="vatandaslikCinsiyet"
              id="vatandaslikCinsiyet"
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
            v-model="vatandaslikEngelDerecisi"
            id="engelDerece"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
          >
            <option value="">—</option>
            <option value="yok">yok</option>
            <option value="1.derece">1.derece</option>
            <option value="2.derece">2.derece</option>
            <option value="3.derece">3.derece</option>
          </select>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <div class="grid grid-cols-2 gap-4 mb-6">
          <div>
            <label class="block text-sm font-semibold mb-1">Uyruğu</label>
            <input
              disabled
              v-model="vatandaslikUyruk"
              type="text"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            />
          </div>
          <div>
            <div>
              <label class="block text-sm font-semibold mb-1">Kimlik Numarası</label>
              <input
                disabled
                v-model="vatandaslikKimlikNumarasi"
                type="text"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
              />
            </div>
          </div>
        </div>

        <div>
          <label class="block text-sm font-semibold mb-1">Askerlik Durumu</label>

          <select
            disabled
            v-model="vatandaslikAskerlikDurumu"
            id="askerlik"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
          >
            <option value="">—</option>
            <option value="Tamamlandı">Tamamlandı</option>
            <option value="Tamamlanmadı">Tamamlanmadı</option>
          </select>
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
            @click="vatandaslikFormKaydet"
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
              v-model="aileMedeniHal"
              id="seçenekler"
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
              v-model="aileEsCalismaDurumu"
              id="seçenekler"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            >
              <option value="">—</option>
              <option value="Çalışıyor">Çalışıyor</option>
              <option value="Çalışmıyor">Çalışmıyor</option>
            </select>
          </div>
        </div>

        <div>
          <label for="aileCocoukSayisi" class="block text-sm font-semibold mb-1"
            >Çocuk Sayısı</label
          >
          <input
            id="aileCocoukSayisi"
            v-model="aileCocoukSayisi"
            type="text"
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
            @click="aileFormKaydet"
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
              v-model="bankaAdi"
              type="text"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            />
          </div>
          <div>
            <label class="block text-sm font-semibold mb-1">Hesap Tipi</label>

            <select
              v-model="bankaHesapTipi"
              id="seçenekler"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
              style="height: 43px"
            >
              <option value="">—</option>
              <option value="Vadesiz">Vadesiz</option>
              <option value="Vadeli">Vadeli</option>
              <option value="Çek">Çek</option>
              <option value="Diğer">Diğer</option>
            </select>
          </div>
        </div>

        <div class="grid grid-cols-2 gap-4 mb-6">
          <div>
            <label class="block text-sm font-semibold mb-1">Hesap Numarası</label>
            <input
              v-model="bankaHesapNumarasi"
              type="text"
              class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
            />
          </div>
          <div>
            <label class="block text-sm font-semibold mb-1">IBAN</label>
            <input
              v-model="bankaIBAN"
              type="text"
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
            @click="bankaFormKaydet"
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
                value="erkan.demir@elasoft.com.tr"
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
                  value=""
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
                v-model="personelEmail"
                type="email"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
                placeholder="E-posta (Kişisel)"
              />
            </div>
            <div>
              <label class="block text-sm font-semibold mb-1">Telefon (Kişisel)</label>

              <input
                v-model="personelTelefon"
                type="text"
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
              @click="iletisimFormKaydet"
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
                v-model="acilDurumAdSoyad"
                type="text"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
              />
            </div>
            <div>
              <label class="block text-sm font-semibold mb-1">Telefon</label>

              <input
                v-model="acilDurumTelefon"
                type="text"
                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 dark:bg-neutral-700 dark:border-neutral-600 focus:shadow-[0px_0px_5px_3px_rgba(_15,_122,_195,_0.3)] outline-none dark:placeholder-gray-400 dark:text-white"
              />
            </div>
          </div>

          <div>
            <label class="block text-sm font-semibold mb-1">Yakınlık Derecesi</label>
            <input
              v-model="acilDurumYakinlikDerecesi"
              type="text"
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
              @click="acilDurumFormKaydet"
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
