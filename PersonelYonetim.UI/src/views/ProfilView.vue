<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
const activeTab = ref('profilim');
const activeTab2 = ref('pozisyon');
const activeTab3 = ref('izinler');
const activeTab4 = ref('harcama');
const activeTab5 = ref('egitimlerim');
const setActiveTab = (tab: string) => {
  activeTab.value = tab;
};
const setActiveTab2 = (tab: string) => {
  activeTab2.value = tab;
};
const setActiveTab3 = (tab: string) => {
  activeTab3.value = tab;
};

const setActiveTab4 = (tab: string) => {
  activeTab4.value = tab;
};

const setActiveTab5 = (tab: string) => {
  activeTab5.value = tab;
};
// profilim kısmı
const personel = {
  adSoyad: 'Erkan Demir',
  unvan: 'Yazılım Personeli',
  departman: 'Yazılım Üretim',
  iseBaslamaTarihi: '2 Ağustos 2019',
  sozlesmeTuru: 'Süresiz',
  calismaSuresi: '5 yıl 7 ay 21 gün',
  sozlesmeBitisTarihi: null,
  pozisyonBaslamaTarihi: '8 Ağustos 2024',
  calismaSekli: 'Tam zamanlı',
  sirket: 'ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ',
  yonetici: {
    adSoyad: 'Adil Mert Şahin',
    unvan: 'Yazılım Müdürü',
    resim: 'https://www.indir.com/haber/wp-content/uploads/2021/11/anonimsinde-hesaba-profil-fotografi-nasil-eklenir-.jpg'
  },
  iletisim: {
    isEposta: 'erkan.demir@elasoft.com.tr',
    isTelefon: null,
    kisiselEposta: null,
    kisiselTelefon: '+90 551 159 1957'
  },
  vatandaslik: {
    dogumTarihi: '1 Mayıs 1989',
    cinsiyet: 'Erkek',
    engelDerecesi: null,
    uyrugu: 'Türkiye',
    kimlikNumarasi: '41557015086',
    askerlikDurumu: 'Tamamlandı'
  },
  egitim: {
    egitimDurumu: 'Mezun',
    enYuksekEgitim: 'Yüksek Lisans',
    sonEgitimKurumu: null
  },
  acilDurum:{
    adSoyad:null,
    telefon:null,
    yakinlikDerece:null
  },
  aile: {
    medeniHal: 'Evli',
    esCalismaDurumu: null,
    cocukSayisi: null
  },
  adres: {
    adres: 'Aydınlıkevler mah. Hasan Paşa cad. kardeşler apt. sitesi no:74 iç kapı no:3 ortahisar',
    adresDevam:null,
    sehir: 'Trabzon',
    ulke: 'Türkiye',
    telefon:null,
    postaKodu: '61000'
  },
  bankaHesabi:{
    bankaAdi:null,
    hesapTipi:null,
    hesapNumarasi:null,
    iban:'TR780001001225565740685001'
  }
};

const iletisimModal = ref(false);
const iletisimForm = ref({
  isEposta: '',
  isTelefon: '',
  kisiselEposta: '',
  kisiselTelefon: ''
});
const iletisimDuzenle = () => {
  // iletisimForm.value = { ...personel.value.iletisim };
  iletisimModal.value = true;
};

const kaydetIletisim = () => {
  personel.value.iletisim = { ...iletisimForm.value };
  iletisimModal.value = false;
};

// izinlerim kısmı

// const izinler = [
//   { id: 1, baslangic: '5 Mar 2025 09:00', bitis: '5 Mar 2025 12:30', sure: 0.39, tur: 'Yıllık İzin', aciklama: 'Hastane işlemleri', durum: 'Onaylandı' },
//   { id: 2, baslangic: '14 Oca 2025 09:00', bitis: '14 Oca 2025 18:00', sure: 1, tur: 'Yıllık İzin', aciklama: 'Gribal rahatsızlık', durum: 'Onaylandı' },
//   { id: 3, baslangic: '27 Kas 2024 14:00', bitis: '27 Kas 2024 16:30', sure: 0.28, tur: 'Yıllık İzin', aciklama: 'Diş randevusu', durum: 'Onaylandı' }
// ];

// function getStatusClass(durum) {
//   return durum === 'Onaylandı' ? 'bg-green-200 text-green-800 px-2 py-1 rounded' : 'bg-red-200 text-red-800 px-2 py-1 rounded';
// }

// izinlerin üst kısmı
// **Dinamik İzin Bilgileri**
const izinBakiyesi = ref(42.51);
const hakEdisBaslangic = ref("2 Ağu 2024");
const hakEdisBitis = ref("1 Ağu 2025");
const kullanilanIzin = ref(2.61);
const ileriTarihli = ref(0);

// **Yüzdelik Oranlar**
const toplamIzin = computed(() => izinBakiyesi.value + kullanilanIzin.value);
const kullanilanOran = computed(() => (kullanilanIzin.value / toplamIzin.value) * 100);
const kalanOran = computed(() => (izinBakiyesi.value / toplamIzin.value) * 100);
// izinlerin alt kısmı
// **Başlıklar (Tıklanabilir)**

const basliklar = ref([
  { kolon: "baslangic", ad: "Başlangıç" },
  { kolon: "bitis", ad: "Bitiş" },
  { kolon: "mesaiBaslangic", ad: "Mesai Başlangıç" },
  { kolon: "sure", ad: "Süre" },
  { kolon: "izinTuru", ad: "İzin Türü" },
  { kolon: "aciklama", ad: "Açıklama" },
  { kolon: "olusturmaTarihi", ad: "Oluşturulma Tarihi" },
  { kolon: "durum", ad: "Durum" }
]);


const izinler = ref([
  {
    baslangic: "5 Mar 2025 09:00",
    bitis: "5 Mar 2025 12:30",
    mesaiBaslangic: "5 Mar 2025 12:30",
    sure: "0.39 gün",
    izinTuru: "Yıllık İzin",
    aciklama: "5 mart öğleden önce hastane işlerim vardı",
    olusturmaTarihi: "20 Mar 2025 10:51",
    durum: "Onaylandı"
  },
  {
    baslangic: "14 Oca 2025 09:00",
    bitis: "14 Oca 2025 18:00",
    mesaiBaslangic: "15 Oca 2025 09:00",
    sure: "1 gün",
    izinTuru: "Yıllık İzin",
    aciklama: "Gribal rahatsızlığımdan dolayı izin talebimdir",
    olusturmaTarihi: "14 Oca 2025 12:51",
    durum: "Onaylandı"
  },
  {
    baslangic: "27 Kas 2024 14:00",
    bitis: "27 Kas 2024 16:30",
    mesaiBaslangic: "27 Kas 2024 16:30",
    sure: "0.28 gün",
    izinTuru: "Yıllık İzin",
    aciklama: "14.30 Diş randevumdan dolayı izin talebimdir",
    olusturmaTarihi: "26 Kas 2024 18:12",
    durum: "Onaylandı"
  },
  {
    baslangic: "27 Kas 2024 14:00",
    bitis: "27 Kas 2024 16:30",
    mesaiBaslangic: "27 Kas 2024 16:30",
    sure: "0.28 gün",
    izinTuru: "Yıllık İzin",
    aciklama: "14.30 Diş randevumdan dolayı izin talebimdir",
    olusturmaTarihi: "26 Kas 2024 18:12",
    durum: "Onaylandı"
  },
  {
    baslangic: "27 Kas 2024 14:00",
    bitis: "27 Kas 2024 16:30",
    mesaiBaslangic: "27 Kas 2024 16:30",
    sure: "0.28 gün",
    izinTuru: "Yıllık İzin",
    aciklama: "14.30 Diş randevumdan dolayı izin talebimdir",
    olusturmaTarihi: "26 Kas 2024 18:12",
    durum: "Onaylandı"
  }
]);

// **Sıralama Durumları**
const siralananKolon = ref("baslangic");
const izinSiralamaYon = ref("asc");

// **Sıralama Fonksiyonu**
const izinSirala = (kolon) => {
  if (siralananKolon.value === kolon) {
    izinSiralamaYon.value = izinSiralamaYon.value === "asc" ? "desc" : "asc";
  } else {
    siralananKolon.value = kolon;
    izinSiralamaYon.value = "asc";
  }
};

// **Sıralı Veriyi Döndüren Computed Property**
const siraliVeriler = computed(() => {
  return [...izinler.value].sort((a, b) => {
    const degerA = a[siralananKolon.value];
    const degerB = b[siralananKolon.value];

    if (!degerA || !degerB) return 0;

    if (!isNaN(Date.parse(degerA)) && !isNaN(Date.parse(degerB))) {
      return izinSiralamaYon.value === "asc"
        ? new Date(degerA) - new Date(degerB)
        : new Date(degerB) - new Date(degerA);
    }

    return izinSiralamaYon.value === "asc"
      ? degerA.localeCompare(degerB, "tr")
      : degerB.localeCompare(degerA, "tr");
  });
});

// pozisyon

let pozisyonData = [
  {
    baslangic: '8 Ağustos 2024',
    bitis: '8 Ağustos 2024',
    calismaSekli: 'Tam zamanlı',
    yonetici: '',
    yoneticiResim: '',
    sirket: 'ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ',
    sube: '',
    departman: 'Yazılım Üretim',
    unvan: 'Yazılım Personeli',
    varsayilan: false
  },
  {
    baslangic: '8 Ağustos 2024',
    bitis: null,
    calismaSekli: 'Tam zamanlı',
    yonetici: 'Adil Mert Şahin',
    yoneticiResim: 'https://sosyalmedya.co/wp-content/uploads/2011/06/Facebook-Profil-Resimleri-940x470.jpg',
    sirket: 'ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ',
    sube: '',
    departman: 'Yazılım Üretim',
    unvan: 'Yazılım Personeli',
    varsayilan: true
  },
  {
    baslangic: '7 Ağustos 2024',
    bitis: '8 Ağustos 2024',
    calismaSekli: 'Tam zamanlı',
    yonetici: 'BAHAR SERDAR',
    yoneticiResim: 'https://sosyalmedya.co/wp-content/uploads/2011/06/Facebook-Profil-Resimleri-940x470.jpg',
    sirket: 'ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ',
    sube: '',
    departman: 'Yazılım Üretim',
    unvan: 'Yazılım Personeli',
    varsayilan: false
  },
  {
    baslangic: '5 Nisan 2024',
    bitis: '7 Ağustos 2024',
    calismaSekli: 'Tam zamanlı',
    yonetici: 'Adil Mert Şahin',
    yoneticiResim: 'https://sosyalmedya.co/wp-content/uploads/2011/06/Facebook-Profil-Resimleri-940x470.jpg',
    sirket: 'ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ',
    sube: '',
    departman: 'Yazılım Üretim',
    unvan: 'Yazılım Personeli',
    varsayilan: false
  }
];

// ÇALIŞMA TAKVİMİ
let calismaTakvimiData = [
  {
    baslangic: '13 Eki 2020',
    bitis: null,
    sure: '4 yıl 5 ay 16 gün',
    calismaTakvimi: 'Genel çalışma tablosu',
    atamaTarihi: '16 Oca 2023',
    guncel: true
  },
  {
    baslangic: '2 Ağu 2019',
    bitis: '12 Eki 2020',
    sure: '1 yıl 2 ay 13 gün',
    calismaTakvimi: 'Genel çalışma tablosu',
    atamaTarihi: '7 Şub 2023',
    guncel: false
  }
];

// fazla mesai kısmı
const data = ref([
  {
    date: "2024-01-31",
    description: "OCAK AYI MESAİ ÜCRETİ",
    status: "Onaylandı",
    amount: 738.89,
    created_at: "2024-02-26T17:14",
    payroll: "Dahil Değil",
    paid: false,
  },
  {
    date: "2023-12-31",
    description: "ARALIK AYI MESAİ ÜCRETİ",
    status: "Onaylandı",
    amount: 4375.0,
    created_at: "2024-02-26T17:13",
    payroll: "Dahil Değil",
    paid: true,
  },
]);
const sortKey = ref("");
const sortDirection = ref(1);

const sortedData = computed(() => {
  if (!sortKey.value) return data.value;
  return [...data.value].sort((a, b) => {
    let valueA = a[sortKey.value];
    let valueB = b[sortKey.value];

    if (typeof valueA === "string") valueA = valueA.toLowerCase();
    if (typeof valueB === "string") valueB = valueB.toLowerCase();

    return valueA > valueB ? sortDirection.value : -sortDirection.value;
  });
});
const sortTable = (key) => {
  if (sortKey.value === key) {
    sortDirection.value *= -1;
  } else {
    sortKey.value = key;
    sortDirection.value = 1;
  }
};
const formatDate = (date) => {
  return new Date(date).toLocaleDateString("tr-TR", {
    day: "2-digit",
    month: "short",
    year: "numeric",
  });
};

const formatDateTime = (date) => {
  return new Date(date).toLocaleDateString("tr-TR", {
    day: "2-digit",
    month: "long",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
};
const formatCurrency = (amount) => {
  return new Intl.NumberFormat("tr-TR", {
    style: "currency",
    currency: "TRY",
  }).format(amount);
};


// mesailerim kısmı

const veriler = ref([
  { baslangicTarihi: "2024-11-20T21:00", sure: "3 saat", aciklama: "Bilet: 8105-YKS-Online kayıt başvurusu...", durum: "Onaylandı", olusturmaTarihi: "2024-11-21T01:16" },
  { baslangicTarihi: "2024-11-10T18:30", sure: "1 saat", aciklama: "Tarsus Bilet No: 9241 HAZIRLIK DETAY BİLGİSİ...", durum: "Onaylandı", olusturmaTarihi: "2024-11-10T23:55" },
  { baslangicTarihi: "2024-11-09T23:00", sure: "1 saat", aciklama: "Trakya hazırlık detay gitmeyenler 440 öğrenci...", durum: "Onaylandı", olusturmaTarihi: "2024-11-10T01:36" },
  { baslangicTarihi: "2024-11-07T23:00", sure: "1 saat", aciklama: "Rize hazırlık detay listesinin gönderilmesi tamamlandı...", durum: "Onaylandı", olusturmaTarihi: "2024-11-08T00:01" },
  { baslangicTarihi: "2024-10-21T23:00", sure: "2 saat", aciklama: "YÖS için uyruk program kontenjan görüntüleme...", durum: "Onaylandı", olusturmaTarihi: "2024-10-22T02:21" }
]);
// Filtreleme değişkenleri
const secilenYil = ref("");
const secilenAy = ref("");
const secilenDurum = ref("");


const aylar = [
  { etiket: "Ocak", deger: "01" }, { etiket: "Şubat", deger: "02" }, { etiket: "Mart", deger: "03" },
  { etiket: "Nisan", deger: "04" }, { etiket: "Mayıs", deger: "05" }, { etiket: "Haziran", deger: "06" },
  { etiket: "Temmuz", deger: "07" }, { etiket: "Ağustos", deger: "08" }, { etiket: "Eylül", deger: "09" },
  { etiket: "Ekim", deger: "10" }, { etiket: "Kasım", deger: "11" }, { etiket: "Aralık", deger: "12" }
];

const yillar = Array.from({ length: 2025 - 2007 + 1 }, (_, i) => (2025 - i).toString());

// Filtrelenmiş veriyi hesaplama
const filtrelenmisVeri = computed(() => {
  return veriler.value.filter(kayit => {
    const yil = kayit.baslangicTarihi.split("-")[0];
    const ay = kayit.baslangicTarihi.split("-")[1];

    return (
      (secilenYil.value === "" || secilenYil.value === yil) &&
      (secilenAy.value === "" || secilenAy.value === ay) &&
      (secilenDurum.value === "" || secilenDurum.value === kayit.durum)
    );
  });
});
// Sıralama için değişkenler
const siralamaAnahtari = ref("");
const siralamaYon = ref(1);

// Tablo sıralama fonksiyonu
const sirala = (anahtar) => {
  if (siralamaAnahtari.value === anahtar) {
    siralamaYon.value *= -1;
  } else {
    siralamaAnahtari.value = anahtar;
    siralamaYon.value = 1;
  }
  veriler.value.sort((a, b) => {
    let degerA = new Date(a[anahtar]).getTime();
    let degerB = new Date(b[anahtar]).getTime();
    return degerA > degerB ? siralamaYon.value : -siralamaYon.value;
  });
};

const tarihFormatla = (tarih) => {
  return new Date(tarih).toLocaleDateString("tr-TR", {
    day: "2-digit",
    month: "long",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit"
  });
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


        <!-- profil kısmı -->
       <div v-if="activeTab === 'profilim'" class="space-y-6">
       
        <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 w-[600px] h-[500px]" style="margin-left: 100px;">
    <div class="flex justify-between items-start mb-4">
        <div>
            <h2 class="text-xl font-semibold">{{ personel.adSoyad }}</h2>
            <br>
            <p class="text-gray-600 dark:text-gray-300">{{ personel.unvan }}</p>
            <p class="text-sm text-gray-500 dark:text-gray-400">{{ personel.departman }}</p>
        </div>
        <img src="https://www.indir.com/haber/wp-content/uploads/2021/11/anonimsinde-hesaba-profil-fotografi-nasil-eklenir-.jpg" alt="Erkan Demir" class="w-16 h-16 rounded-full object-cover ">
    </div>
    
    <hr class="my-4 border-gray-300 dark:border-gray-600">
    
    
    <div class="grid grid-cols-2 gap-4 text-sm">
        <div>
            <p class="text-gray-500 dark:text-gray-400">İşe Başlama Tarihi</p>
            <p class="font-medium">{{ personel.iseBaslamaTarihi }}</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Sözleşme Türü</p>
            <p class="font-medium">{{ personel.sozlesmeTuru}}</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Çalışma Süresi</p>
            <p class="font-medium">{{ personel.calismaSuresi}}</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Sözleşme Bitiş Tarihi</p>
            <p class="font-medium">{{ personel.sozlesmeBitisTarihi || '—'}}</p>
        </div>
    </div>

    <hr class="my-4 border-gray-300 dark:border-gray-600">

    <div class="grid grid-cols-2 gap-4 text-sm">
        <div>
            <p class="text-gray-500 dark:text-gray-400">Pozisyon Başlama Tarihi</p>
            <p class="font-medium">{{ personel.pozisyonBaslamaTarihi}}</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Çalışma Şekli</p>
            <p class="font-medium">{{ personel.calismaSekli}}</p>
        </div>
        <div class="col-span-2">
            <p class="text-gray-500 dark:text-gray-400">Şirket</p>
            <p class="font-medium">{{ personel.sirket}}</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Departman</p>
            <p class="font-medium">{{ personel.departman}}</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Unvan</p>
            <p class="font-medium">{{ personel.unvan}}</p>
        </div>
    </div>
    <hr class="my-4 border-gray-300 dark:border-gray-600">

    <div class="mt-4 text-right">
        <a href="#" class="text-blue-600 dark:text-blue-400 hover:underline" @click="setActiveTab('kariyerim')">Kariyer &gt;</a>
        
    </div>
    
</div>

<!-- <div class="bg-gray-200 dark:bg-neutral-800 rounded-lg shadow-sm p-6 w-[600px] h-[150px]" style="margin-left: 740px; margin-top: -523px;">
    <div class="flex justify-between items-start mb-4">
        <div>
            <h5 class="text-l font-semibold" style="margin-top:-10px;">Yöneticim</h5>   
        </div>
      

       
      
    </div>
    <div class="bg-white dark:bg-neutral-800 rounded-lg shadow-sm p-6 w-[600px] h-[110px]" style="margin-left: -23px; margin-top: -10px;" >
      <img src="#" alt="Erkan Demir" class="w-10 h-10 rounded-full object-cover border">
      <h1>Erkan Demir</h1>

    </div>
   

      
</div> -->

<div class="bg-gray-100 p-4 rounded-md shadow-md w-140" style="margin-left: 780px; margin-top: -523px;">
    <h2 class="text-lg font-semibold text-gray-700 mb-2">Yöneticim</h2>
    <div class="bg-white p-3 rounded-md flex items-center space-x-3" style="width: 556px; margin-left:-13px;">
        <img src="https://www.indir.com/haber/wp-content/uploads/2021/11/anonimsinde-hesaba-profil-fotografi-nasil-eklenir-.jpg" alt="Yönetici Resmi" class="w-10 h-10 rounded-full ">
        <div>
            <p class="text-gray-900 ">{{ personel.yonetici.adSoyad}}</p>
            <p class="text-gray-500 text-sm">{{ personel.yonetici.unvan}}</p>
        </div>
    </div>
</div>


<div class="bg-gray-100 p-4 rounded-lg shadow-md w-96 " style="margin-left: 780px; width:560px;" >
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">İletişim</h2>
        <button  @click="iletisimDuzenle">
           <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1;"></i> 
           
        </button>
        
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3">
        <!-- E-Posta (İş) -->
        <div class="flex items-center space-x-3">
          <i class="fa-solid fa-envelope" style="color: #3562b1;"></i>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">E-Posta (İş)</p>
                <p class="text-blue-600 font-medium">{{ personel.iletisim.isEposta}}</p>
            </div>
           
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
        <!-- Telefon (İş) -->
        <div class="flex items-center space-x-3">
          <i class="fa-solid fa-phone" style="color: #3562b1;"></i>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">Telefon (İş)</p>
                <p class="text-gray-400">{{ personel.iletisim.isTelefon || '—' }}</p>    
            </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
        <!-- E-Posta (Kişisel) -->
        <div class="flex items-center space-x-3">
          <i class="fa-solid fa-eye-slash" style="color: #3562b1;"title="iş arkadaşlarına gösterilmez"></i>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">E-Posta (Kişisel)</p>
                <p class="text-gray-400">{{ personel.iletisim.kisiselEposta || '—' }}</p>
            </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
        <!-- Telefon (Kişisel) -->
        <div class="flex items-center space-x-3">
          <i class="fa-solid fa-eye-slash" style="color: #3562b1;"title="iş arkadaşlarına gösterilmez"></i>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">Telefon (Kişisel)</p>
                <p class="text-blue-600 font-medium">{{ personel.iletisim.kisiselTelefon || '—' }}</p>
            </div>
           
        </div>
    </div>
    
</div>
<div v-if="iletisimModal" class="modal">
      <div class="modal-content">
        <h3>İletişim Bilgileri</h3>
        <label>E-Posta (İş)</label>
        <input v-model="iletisimForm.isEposta" type="text" />
        <label>Telefon (İş)</label>
        <input v-model="iletisimForm.isTelefon" type="text" />
        <label>E-Posta (Kişisel)</label>
        <input v-model="iletisimForm.kisiselEposta" type="text" />
        <label>Telefon (Kişisel)</label>
        <input v-model="iletisimForm.kisiselTelefon" type="text" />
        <button @click="kaydetIletisim">Kaydet</button>
        <button @click="iletisimModal = false">İptal</button>
      </div>
    </div>



<div class="bg-gray-100 p-4 rounded-lg shadow-md w-96 " style="margin-left: 780px; width:560px;" >
    <div class="flex justify-between items-center mb-2">
      
        <h2 class="text-lg font-semibold text-gray-700">Destek</h2>
       
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3">
        <!-- E-Posta (İş) -->
        <div class="flex items-center space-x-3">
            
            <div class="flex-1">
                
                <p class="text-blue-600 font-small">İzin nasıl talep edilir?</p>
            </div>
            
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
        <!-- Telefon (İş) -->
        <div class="flex items-center space-x-3">
           
            <div class="flex-1">
                
              <p class="text-blue-600 font-small">Harcama nasıl talep edilir?</p>
            </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
        <!-- E-Posta (Kişisel) -->
        <div class="flex items-center space-x-3">
            
            <div class="flex-1">
                
              <p class="text-blue-600 font-small">İzin talebimi nasıl iptal ederim?</p>
            </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
        <!-- Telefon (Kişisel) -->
        <div class="flex items-center space-x-3">
            
            <div class="flex-1">
                
              <p class="text-blue-600 font-small">Parolamı unuttum nasıl sıfırlarım?</p>
            </div>
            
        </div>
    </div>
</div>

      </div> 

      <!-- Kişisel Bilgilerim sekmesi -->

      <div v-if="activeTab === 'kisiselBilgilerim'" class="space-y-6">

          <!-- Vatandaşlık -->
      <div class="bg-gray-100 p-4 rounded-lg shadow-md w-[750px]" style="margin-left: 60px;">
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Vatandaşlık</h2>
        <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1;"></i>
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3" style="height: 310px;">
        <!-- Doğum Tarihi - Cinsiyet -->
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Doğum Tarihi</p>
                <p class="text-gray-900 text-sm">{{ personel.vatandaslik.dogumTarihi }}</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px;" >
                <p class="text-gray-600 text-sm">Cinsiyet</p>
                <p class="text-gray-900 text-sm">{{ personel.vatandaslik.cinsiyet}}</p>
            </div>
        </div>

        <!-- Engel Derecesi -->
        <div>
            <p class="text-gray-600 text-sm">Engel Derecesi</p>
            <p class="text-gray-900">{{ personel.vatandaslik.engelDerecesi || '—'}}</p>
        </div>

        <hr class="my-4 border-gray-300 dark:border-gray-600">
<br>
        <!-- Uyruğu - Kimlik Numarası -->
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Uyruğu</p>
                <p class="text-gray-900 text-sm">{{ personel.vatandaslik.uyrugu}}</p>
            </div>
            <div class="flex-grow" style="margin-left: 339px;" >
                <div >
                    <p class="text-gray-600 text-sm" >Kimlik Numarası</p>
                    <p class="text-gray-900 text-sm">{{ personel.vatandaslik.kimlikNumarasi}}</p>
                </div>
                <br>           
            </div>
        </div>

        <!-- Askerlik Durumu -->
        <div>
            <p class="text-gray-600 text-sm">Askerlik Durumu</p>
            <p class="text-gray-900 text-sm">{{ personel.vatandaslik.askerlikDurumu}}</p>
        </div>
    </div>
</div>


<br>
    <!-- Eğitim -->
<div class="bg-gray-100 p-4 rounded-lg shadow-md w-[750px]" style="margin-left: 60px;">
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Eğitim</h2>
     
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3" style="height: 150px;">
       
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Eğitim Durumu</p>
                <p class="text-gray-900 text-sm">{{ personel.egitim.egitimDurumu}}</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px;" >
                <p class="text-gray-600 text-sm">Tamamlanan En Yüksek Eğitim Seviyesi</p>
                <p class="text-gray-900 text-sm">{{ personel.egitim.enYuksekEgitim}}</p>
            </div>
        </div>
<br>
        
        <div>
            <p class="text-gray-600 text-sm">Son Tamamlanan Eğitim Kurumu</p>
            <p class="text-gray-900">{{ personel.egitim.sonEgitimKurumu || '—'}}</p>
        </div>

    </div>
</div>

<br>
    <!-- Aile -->
<div class="bg-gray-100 p-4 rounded-lg shadow-md w-[750px]" style="margin-left: 60px;">
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Aile</h2>
        <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1;"></i>
     
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3" style="height: 150px;">
       
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Medeni Hal</p>
                <p class="text-gray-900 text-sm">{{ personel.aile.medeniHal || '—'}}</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px;" >
                <p class="text-gray-600 text-sm">Eş Çalışma Durumu</p>
                <p class="text-gray-900 text-sm">{{ personel.aile.esCalismaDurumu || '—'}}</p>
            </div>
        </div>

        <br>
        <div>
            <p class="text-gray-600 text-sm">Çocuk Sayısı</p>
            <p class="text-gray-900">{{ personel.aile.cocukSayisi || '—'}}</p>
        </div>

    </div>
   
</div>
<br>
   <!-- Adres -->
<div class="bg-gray-100 p-4 rounded-lg shadow-md w-[750px]" style="margin-left: 60px;">
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Adres</h2>
     
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3" style="height: 280px;">
       
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Adres</p>
                <p class="text-gray-900 text-sm">{{ personel.adres.adres || '—'}}</p>
            </div>
        </div>

        
        <div>
            <p class="text-gray-600 text-sm">Adres (devam)</p>
            <p class="text-gray-900">{{ personel.adres.adresDevam || '—'}}</p>
        </div>
        <br>
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Şehir</p>
                <p class="text-gray-900 text-sm">{{ personel.adres.sehir || '—'}}</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px;" >
                <p class="text-gray-600 text-sm">Ülke</p>
                <p class="text-gray-900 text-sm">{{ personel.adres.ulke || '—'}}</p>
                
            </div>
            
        </div>
<br>
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Posta Kodu</p>
                <p class="text-gray-900 text-sm">{{ personel.adres.postaKodu || '—'}}</p>
            </div>
            <div class="flex-grow" style="margin-left: 279px;" >
                <p class="text-gray-600 text-sm">Telefon</p>
                <p class="text-gray-900 text-sm">{{ personel.adres.telefon || '—'}}</p>
                
            </div>
            
        </div>
       

    </div>
   
</div>
<br>
<!-- Banka hesabı -->

<div class="bg-gray-100 p-4 rounded-lg shadow-md w-[750px]" style="margin-left: 60px;">
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Banka Hesabı</h2>
        <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1;"></i>
     
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3" style="height: 150px;">
       
        

      <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Banka Adı</p>
                <p class="text-gray-900 text-sm">{{ personel.bankaHesabi.bankaAdi || '—'}}</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px;" >
                <p class="text-gray-600 text-sm">Hesap Tipi</p>
                <p class="text-gray-900 text-sm">{{ personel.bankaHesabi.hesapTipi || '—'}}</p>
                
            </div>
            
        </div>
<br>
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Hesap Numarası</p>
                <p class="text-gray-900 text-sm">{{ personel.bankaHesabi.hesapNumarasi || '—'}}</p>
            </div>
            <div class="flex-grow" style="margin-left: 257px;" >
                <p class="text-gray-600 text-sm">IBAN</p>
                <p class="text-gray-900 text-sm">{{ personel.bankaHesabi.iban || '—'}}</p>
                
            </div>
            
        </div>
       

        

    </div>
   
</div>
<br>
    <!-- İletişim -->
<div class="bg-gray-100 p-4 rounded-lg shadow-md w-96 " style="margin-left: 850px; width:560px; margin-top:-1622px; " >
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">İletişim</h2>
        <i class="fa-solid fa-pen cursor-pointer" style="color: #3562b1;"></i>
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3">

        
        <div class="flex items-center space-x-3">
          <i class="fa-solid fa-eye-slash" style="color: #3562b1;"title="iş arkadaşlarına gösterilmez"></i>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">E-Posta (Kişisel)</p>
                <p class="text-gray-400">{{ personel.iletisim.kisiselEposta || '—'}}</p>
            </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
       
        <div class="flex items-center space-x-3">
          <i class="fa-solid fa-eye-slash" style="color: #3562b1;"title="iş arkadaşlarına gösterilmez"></i>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">Telefon (Kişisel)</p>
                <p class="text-blue-600 font-medium">{{ personel.iletisim.kisiselTelefon || '—'}}</p>
            </div>
            
        </div>
    </div>
</div>

<!-- Acil Durum -->

<div class="bg-gray-100 p-4 rounded-lg shadow-md w-[560px]" style="margin-left: 850px;">
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Acil Durum</h2>
        <i class="fa-solid fa-pen cursor-pointer" @click="acilDurumDuzenle" style="color: #3562b1;"></i>
     
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3" style="height: 220px;">
       
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Acil durumda erişilebilecek kişinin bilgileri</p>
               
            </div>
            
        </div>
        
        <div>
            <p class="text-gray-600 text-sm">Adı Soyadı</p>
            <p class="text-gray-900">{{ personel.acilDurum.adSoyad || '—'}}</p>
        </div>

        <div>
          <p class="text-gray-600 text-sm">Telefon</p>
          <p class="text-gray-900">{{ personel.acilDurum.telefon || '—'}}</p>
        </div>
        <div>
          <p class="text-gray-600 text-sm">Yakınlık Derecesi</p>
          <p class="text-gray-900">{{ personel.acilDurum.yakinlikDerece || '—'}}</p>
        </div>

    </div>
</div>


<br>

</div>

<!-- kariyer kısmı -->

<div v-if="activeTab === 'kariyerim'" class="space-y-6">
    <ul class="flex flex-wrap -mb-px" style="margin-left: 20px;">
          <li class="mr-1">      
            <button class="bg-sky-700 text-white py-2 px-4 rounded flex items-center"
            @click="setActiveTab2('pozisyon')"
            :class="activeTab2 === 'pozisyon' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Pozisyon
        </button>
          </li>
          <li class="mr-1">      
            <button class="bg-sky-700 text-white py-2 px-4 rounded flex items-center"
            @click="setActiveTab2('maas')"
            :class="activeTab2 === 'maas' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Maaş
        </button>
          </li>
          <li class="mr-1">      
            <button class="bg-sky-700 text-white py-2 px-4 rounded flex items-center"
            @click="setActiveTab2('calismaTakvimi')"
            :class="activeTab2 === 'calismaTakvimi' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Çalışma Takvimi
        </button>
          </li>
          <li class="mr-1">      
            <button class="bg-sky-700 text-white py-2 px-4 rounded flex items-center"
            @click="setActiveTab2('performans')"
            :class="activeTab2 === 'performans' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Performans
        </button>
          </li>
        </ul>
        <div v-if="activeTab2 === 'pozisyon'" class="space-y-6">
          <div class="overflow-x-auto" style="margin-top:35px; width:1300px; margin-left:74px;">
    <table class="min-w-full bg-gray ">
      <thead>
        <tr class="bg-gray-100 border-b">
          <th class="px-4 py-2 text-left">Başlangıç</th>
          <th class="px-4 py-2 text-left">Bitiş</th>
          <th class="px-4 py-2 text-left">Çalışma Şekli</th>
          <th class="px-4 py-2 text-left">Yönetici</th>
          <th class="px-4 py-2 text-left">Şirket</th>
          <th class="px-4 py-2 text-left">Şube</th>
          <th class="px-4 py-2 text-left">Departman</th>
          <th class="px-4 py-2 text-left">Unvan</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, index) in pozisyonData" :key="index" class="border-b" style="height:90px;">
          <td class="px-4 py-2">{{ item.baslangic }} <span v-if="item.varsayilan" class="ml-2 px-2 py-1 bg-blue-500 text-white text-xs rounded">Varsayılan</span></td>
          <td class="px-4 py-2">{{ item.bitis || 'Devam Ediyor' }}</td>
          <td class="px-4 py-2">{{ item.calismaSekli }}</td>
          <td class="px-4 py-2 items-center">
            <img v-if="item.yoneticiResim" :src="item.yoneticiResim" alt="Yönetici Resmi" class="w-12 h-12 rounded-full mr-2">
            {{ item.yonetici || '—' }}
          </td>
          <td class="px-4 py-2">{{ item.sirket }}</td>
          <td class="px-4 py-2">{{ item.sube || '—' }}</td>
          <td class="px-4 py-2">{{ item.departman }}</td>
          <td class="px-4 py-2">{{ item.unvan }}</td>
        </tr>
      </tbody>
    </table>
  </div>
        </div>

        <div v-if="activeTab2 === 'maas'" class="space-y-6">

            <div class="flex justify-center items-center h-screen">
  <div class="border-2 border-gray-200 p-6 bg-transparent rounded-lg w-96" style="width:1360px;height:200px; margin-top:-500px;">
    <div class="text-center" style="margin-top: 20px;">
    
      
      <i class="fa-solid fa-wallet fa-2xl" style="color: #3562b1;"></i>
    
      
      
       <br> <br>
      <p class="text-gray-800 text-l mb-4" >Maaş bilgisi bulunamadı</p>
      
     
      <p class="text-gray-700 text-sm">Bordro işlemleri için bir maaş bilgisi ekleyin</p>
    </div>
  </div>
</div>

        </div>

        <div v-if="activeTab2 === 'calismaTakvimi'" class="space-y-6">
          <div class="overflow-x-auto" style="margin-top:35px; width:1300px; margin-left:74px;">
    <table class="min-w-full bg-gray-100  ">
      <thead>
        <tr class="bg-gray-100 border-b">
          <th class="px-4 py-2 text-left">Başlangıç</th>
          <th class="px-4 py-2 text-left">Bitiş</th>
          <th class="px-4 py-2 text-left">Süre</th>
          <th class="px-4 py-2 text-left">Çalışma Takvimi</th>
          <th class="px-4 py-2 text-left">Atama Tarihi</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, index) in calismaTakvimiData" :key="index" class="border-b">
          <td class="px-4 py-2 " >{{ item.baslangic }} <span v-if="item.guncel" class="ml-2 px-2 py-1 bg-blue-500 text-white text-xs rounded">Güncel</span></td>
          <td class="px-4 py-2">{{ item.bitis || '—' }}</td>
          <td class="px-4 py-2">{{ item.sure }}</td>
          <td class="px-4 py-2">{{ item.calismaTakvimi }}</td>
          <td class="px-4 py-2">{{ item.atamaTarihi }}</td>
        </tr>
      </tbody>
    </table>
  </div>

        </div>

        <div v-if="activeTab2 === 'performans'" class="space-y-6">

<div class="flex justify-center items-center h-screen">
<div class="border-2 border-gray-200 p-6 bg-transparent rounded-lg w-96" style="width:1360px;height:200px; margin-top:-500px;">
<div class="text-center" style="margin-top: 20px;">

<i class="fa-solid fa-circle-exclamation fa-2xl" style="color: #3562b1;"></i>

<br> <br>
<p class="text-gray-800 text-l mb-4" >Girilen kriterlere uygun sonuç bulunamadı</p>
</div>
</div>
</div>

</div>

</div>

<!-- izinlerim kısmı -->
<div v-if="activeTab === 'izinlerim'" class="space-y-6">
    <ul class="flex flex-wrap -mb-px" style="margin-left: 20px;">
          <li class="mr-1">      
            <button class="bg-sky-700 text-white  py-2 px-4 rounded flex items-center"
            @click="setActiveTab3('izinler')"
            :class="activeTab3 === 'izinler' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          İzinler
        </button>
          </li>
          <li class="mr-1">      
            <button class="bg-sky-700 text-white py-2 px-4 rounded flex items-center"
            @click="setActiveTab3('ekstraIzinler')"
            :class="activeTab3 === 'ekstraIzinler' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Ekstra İzinler
        </button>
          </li>
        </ul>
        <div v-if="activeTab3 === 'izinler'" class="space-y-6">
          <!-- <div class="p-6">
    <h1 class="text-2xl font-bold mb-4">Yıllık İzin Takibi</h1>
    <div class="bg-white p-4 shadow rounded-lg">
      <table class="w-full border-collapse border border-gray-200">
        <thead>
          <tr class="bg-gray-100">
            <th class="p-2 border">Başlangıç</th>
            <th class="p-2 border">Bitiş</th>
            <th class="p-2 border">Süre (gün)</th>
            <th class="p-2 border">İzin Türü</th>
            <th class="p-2 border">Açıklama</th>
            <th class="p-2 border">Durum</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="izin in izinler" :key="izin.id" class="border">
            <td class="p-2 border">{{ izin.baslangic }}</td>
            <td class="p-2 border">{{ izin.bitis }}</td>
            <td class="p-2 border">{{ izin.sure }}</td>
            <td class="p-2 border">{{ izin.tur }}</td>
            <td class="p-2 border">{{ izin.aciklama }}</td>
            <td class="p-2 border">
              <span :class="getStatusClass(izin.durum)">{{ izin.durum }}</span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div> -->

  <div class="p-4 rounded-lg shadow-md bg-gray" style="width:1400px;margin-left:26px;margin-top:40px;">
    <!-- İzin Bakiyesi Bilgisi -->
    <div class="mb-4">
      <h2 class="text-md font-semibold" style="font-size:15px;">
        Kullanılabilir İzin Bakiyesi / Yıllık İzin
      </h2>
      <p class="text-sm text-gray-600" style="margin-left:1030px;" >
        Güncel Hak Ediş Dönemi <strong>{{ hakEdisBaslangic }} – {{ hakEdisBitis }}</strong>
      </p>
      <p class="text-xl  text-green-600" style="font-size:30px;">{{ izinBakiyesi }} gün</p>
      
    </div>

    <!-- İzin Progress Bar -->
    <div class="relative w-full h-2 bg-gray-200 rounded-full overflow-hidden">
      <!-- Yeşil: Kalan izin -->
      <div
        class="absolute h-full bg-green-600"
        :style="{ width: kalanOran + '%' }"
      ></div>
      <!-- Kırmızı: Kullanılan izin -->
      <div
        class="absolute h-full bg-red-500"
        :style="{ width: kullanilanOran + '%', left: kalanOran + '%' }"
      ></div>
    </div>

    <!-- Açıklamalar -->
    <div class="flex justify-between text-sm mt-2">
      <div class="flex items-center">
        <span class="w-3 h-3 bg-yellow-400 inline-block rounded-full mr-1"></span>
        Dönemde İleri Tarihli {{ ileriTarihli }} 
      </div>
      <div class="flex items-center">
        <span class="w-3 h-3 bg-red-500 inline-block rounded-full mr-1"></span>
        Dönemde Kullanılan {{ kullanilanIzin }}
      </div>
    </div>
  </div>

  <div class="overflow-x-auto" style="margin-top:50px; width:1400px;margin-left:26px;">
    <table class="">
      <thead>
        <tr class="bg-gray-100">
          <th
            v-for="baslik in basliklar"
            :key="baslik.kolon"
            class=" px-4 py-2 cursor-pointer"
            @click="izinSirala(baslik.kolon)"
          >
            {{ baslik.ad }}
            <span v-if="siralananKolon === baslik.kolon">
              {{ izinSiralamaYon === 'asc' ? '⬆️' : '⬇️' }}
            </span>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(izin, index) in siraliVeriler"
          :key="index"
          class="hover:bg-gray-50" style="height:50px;"
        >
          <td class=" px-4 py-2">{{ izin.baslangic }}</td>
          <td class=" px-4 py-2">{{ izin.bitis }}</td>
          <td class=" px-4 py-2">{{ izin.mesaiBaslangic }}</td>
          <td class=" px-4 py-2">{{ izin.sure }}</td>
          <td class=" px-4 py-2">{{ izin.izinTuru }}</td>
          <td class=" px-4 py-2">{{ izin.aciklama }}</td>
          <td class=" px-4 py-2">{{ izin.olusturmaTarihi }}</td>
          <td class=" px-4 py-2">
            <span
              class="px-2 py-1 rounded text-white"
              :class="izin.durum === 'Onaylandı' ? 'bg-green-700' : 'bg-red-500'" style="border-radius: 10px;"
            >
              {{ izin.durum }}
            </span>
          </td>
        </tr>
      </tbody>
    </table>
    <br><br>
  </div>
        </div>

        <div v-if="activeTab3 === 'ekstraIzinler'" class="space-y-6">
            <div class="flex justify-center items-center h-screen">
  <div class="border-2 border-gray-200 p-6 bg-transparent rounded-lg w-96" style="width:1360px;height:200px; margin-top:-500px;">
    <div class="text-center" style="margin-top: 20px;">
    
     
      <i class="fa-solid fa-plane fa-rotate-by fa-2xl" style="--fa-rotate-angle: -45deg;color: #3562b1;"></i>
    
      
      
       <br> <br>
      <p class="text-gray-800 text-l mb-4" >Kayıtlı izin bulunamadı</p>
    </div>
  </div>
</div>
        </div>
</div>

<!-- ödemelerim kısmı -->
<div v-if="activeTab === 'odemelerim'" class="space-y-6">
    <ul class="flex flex-wrap -mb-px" style="margin-left: 20px;">
          <li class="mr-1">      
            <button class="bg-sky-700 text-white  py-2 px-4 rounded flex items-center"
            @click="setActiveTab4('harcama')"
            :class="activeTab4 === 'harcama' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Harcama
        </button>
          </li>
          <li class="mr-1">      
            <button class="bg-sky-700 text-white py-2 px-4 rounded flex items-center"
            @click="setActiveTab4('fazlaMesai')"
            :class="activeTab4 === 'fazlaMesai' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Fazla Mesai
        </button>
          </li>
          <li class="mr-1">      
            <button class="bg-sky-700 text-white py-2 px-4 rounded flex items-center"
            @click="setActiveTab4('ekOdemeler')"
            :class="activeTab4 === 'ekOdemeler' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Ek ödemeler
        </button>
          </li>
          <li class="mr-1">      
            <button class="bg-sky-700 text-white py-2 px-4 rounded flex items-center"
            @click="setActiveTab4('ozelKesintiler')"
            :class="activeTab4 === 'ozelKesintiler' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Özel Kesintiler
        </button>
          </li>
        </ul>

        <div v-if="activeTab4 === 'ekOdemeler'" class="space-y-6">

            <div class="flex justify-center items-center h-screen">
  <div class="border-2 border-gray-200 p-6 bg-transparent rounded-lg w-96" style="width:1360px;height:200px; margin-top:-500px;">
    <div class="text-center" style="margin-top: 20px;">
    
      
      <i class="fa-solid fa-wallet fa-2xl" style="color: #3562b1;"></i>
    
      
      
       <br> <br>
      <p class="text-gray-800 text-l mb-4" >Kayıtlı ödeme bulunamadı</p>
    </div>
  </div>
</div>

        </div>

        <div v-if="activeTab4 === 'ozelKesintiler'" class="space-y-6">

<div class="flex justify-center items-center h-screen">
<div class="border-2 border-gray-200 p-6 bg-transparent rounded-lg w-96" style="width:1360px;height:200px; margin-top:-500px;">
<div class="text-center" style="margin-top: 20px;">

    <i class="fa-solid fa-wallet fa-2xl" style="color: #3562b1;"></i>


<br> <br>
<p class="text-gray-800 text-l mb-4" >Kayıtlı ödeme bulunamadı</p>
</div>
</div>
</div>

       </div>


       <div v-if="activeTab4 === 'harcama'" class="space-y-6">

<div class="flex justify-center items-center h-screen">
<div class="border-2 border-gray-200 p-6 bg-transparent rounded-lg w-96" style="width:1360px;height:200px; margin-top:-500px;">
<div class="text-center" style="margin-top: 20px;">

    <i class="fa-solid fa-wallet fa-2xl" style="color: #3562b1;"></i>


<br> <br>
<p class="text-gray-800 text-l mb-4" >Kayıtlı ödeme bulunamadı</p>
</div>
</div>
</div>

       </div>


        <!-- fazla mesai -->
       <div v-if="activeTab4 === 'fazlaMesai'" class="space-y-6">

        <div class="container">
    <table>
      <thead>
        <tr>
          <th @click="sortTable('date')">Tarih</th>
          <th>Açıklama</th>
          <th>Durum</th>
          <th @click="sortTable('amount')">Miktar</th>
          <th @click="sortTable('created_at')">Oluşturulma Tarihi</th>
          <th>Bordro</th>
          <th>Ödendi</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, index) in sortedData" :key="index">
          <td>{{ formatDate(item.date) }}</td>
          <td>{{ item.description }}</td>
          <td>
            <span class="status">{{ item.status }}</span>
          </td>
          <td>{{ formatCurrency(item.amount) }}</td>
          <td>{{ formatDateTime(item.created_at) }}</td>
          <td>{{ item.payroll }}</td>
          <td>
            <span v-if="item.paid" class="paid">✔</span>
            <span v-else class="not-paid">✖</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>

       </div>

</div>
        <!-- mesailerim -->
<div v-if="activeTab === 'mesailerim'" class="space-y-6">

    <div class="kapsayici">
    <!-- Filtreleme Alanı -->
    <div class="filtreler">
      <select v-model="secilenYil">
        <option value="">Yıl</option>
        <option v-for="yil in yillar" :key="yil" :value="yil">{{ yil }}</option>
      </select>

      <select v-model="secilenAy">
        <option value="">Ay</option>
        <option v-for="ay in aylar" :key="ay.deger" :value="ay.deger">{{ ay.etiket }}</option>
      </select>

      <select v-model="secilenDurum" style="margin-left:1000px">
        <option value="">Tümü</option>
        <option value="Ekstra İzne Çevrildi">Ekstra İzne Çevrildi</option>
        <option value="Onay Bekliyor">Onay Bekliyor</option>
        <option value="Onaylandı">Onaylandı</option>
        <option value="Ödemeye Çevrildi">Ödemeye Çevrildi</option>
        <option value="Reddedildi">Reddedildi</option>
      </select>
    </div>

    <!-- Tablo -->
    <table>
      <thead >
        <tr>
          <th @click="sirala('baslangicTarihi')">Başlangıç</th>
          <th>Süre</th>
          <th>Açıklama</th>
          <th>Durum</th>
          <th @click="sirala('olusturmaTarihi')">Oluşturulma Tarihi</th>
        </tr>
      </thead>
      <tbody class="mesaiTablo">
        <tr v-for="(kayit, index) in filtrelenmisVeri" :key="index">
          <td>{{ tarihFormatla(kayit.baslangicTarihi) }}</td>
          <td>{{ kayit.sure }}</td>
          <td>{{ kayit.aciklama }}</td>
          <td>
            <span class="durum">{{ kayit.durum }}</span>
          </td>
          <td>{{ tarihFormatla(kayit.olusturmaTarihi) }}</td>
        </tr>
      </tbody>
    </table>
  </div>

    
</div>
<!-- diğer -->
<div v-if="activeTab === 'diger'" class="space-y-6">

    <ul class="flex flex-wrap -mb-px" style="margin-left: 20px;">
          <li class="mr-1">      
            <button class="bg-sky-700 text-white  py-2 px-4 rounded flex items-center"
            @click="setActiveTab4('egitimlerim')"
            :class="activeTab4 === 'egitimlerim' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Eğitimlerim
        </button>
          </li>
          <li class="mr-1">      
            <button class="bg-sky-700 text-white py-2 px-4 rounded flex items-center"
            @click="setActiveTab4('vizeBelgesiTaleplerim')"
            :class="activeTab4 === 'vizeBelgesiTaleplerim' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Vize Belgesi Taleplerim
        </button>
          </li>
          <li class="mr-1">      
            <button class="bg-sky-700 text-white py-2 px-4 rounded flex items-center"
            @click="setActiveTab4('zimmetlerim')"
            :class="activeTab4 === 'zimmetlerim' ? 'bg-sky-900' : 'bg-sky-700 '"
          >
          Zimmetlerim
        </button>
          </li>
        </ul>


        <div v-if="activeTab4 === 'egitimlerim'" class="space-y-6">

<div class="flex justify-center items-center h-screen">
<div class="border-2 border-gray-200 p-6 bg-transparent rounded-lg w-96" style="width:1360px;height:200px; margin-top:-500px;">
<div class="text-center" style="margin-top: 20px;">


    <i class="fa-regular fa-chart-bar fa-rotate-270 fa-2xl"style="color: #3562b1;" ></i>



<br> <br>
<p class="text-gray-800 text-l mb-4" >Kayıtlı eğitim bilgisi bulunamadı</p>
</div>
</div>
</div>

</div>

<div v-if="activeTab4 === 'vizeBelgesiTaleplerim'" class="space-y-6">

<div class="flex justify-center items-center h-screen">
<div class="border-2 border-gray-200 p-6 bg-transparent rounded-lg w-96" style="width:1360px;height:200px; margin-top:-500px;">
<div class="text-center" style="margin-top: 20px;">


    <!-- <i class="fa-solid fa-file-invoice fa-2xl" style="color: #3562b1;"></i> -->
    <i class="fa-solid fa-file fa-2xl" style="color: #3562b1;"></i>


<br> <br>
<p class="text-gray-800 text-l mb-4" >Kayıtlı vize belgesi süreci bulunamadı</p>
 </div>
    </div>
    </div>

    </div>

    <div v-if="activeTab4 === 'zimmetlerim'" class="space-y-6">

<div class="flex justify-center items-center h-screen">
<div class="border-2 border-gray-200 p-6 bg-transparent rounded-lg w-96" style="width:1360px;height:200px; margin-top:-500px;">
<div class="text-center" style="margin-top: 20px;">


<i class="fa-solid fa-wallet fa-2xl" style="color: #3562b1;"></i>



<br> <br>
<p class="text-gray-800 text-l mb-4" >Kayıtlı zimmet bulunamadı</p>
</div>
</div>
</div>

</div>
</div>

      


</template>

<style scoped>
.container {
  max-width: 1300px;
  margin: 20px auto;
  font-family: Arial, sans-serif;
}
table {
  width: 100%;
  border-collapse: collapse;
}
th, td {
  padding: 10px;
  border-bottom: 1px solid #ddd;
  text-align: left;
  
}
th {
  cursor: pointer;
  font-size:13px;
}
td{
    font-size:13px;
}
.status {
  background: green;
  color: white;
  padding: 5px 10px;
  border-radius: 10px;
  font-size: 12px;
}
.paid {
  color: green;
  font-size: 18px;
}
.not-paid {
  color: red;
  font-size: 18px;
}


.kapsayici {
  max-width: 1400px;
  margin: 20px auto;
  margin-left:60px;
  font-family: Arial, sans-serif;
}

.filtreler {
  display: flex;
  gap: 10px;
  margin-bottom: 10px;
}

select {
  padding: 5px;
}

.durum {
  background: green;
  color: white;
  padding: 5px 10px;
  border-radius: 10px;
  font-size: 12px;
}

.mesaiTablo tr td {
   padding-bottom:60px;
}




.etiket-guncel {
  background-color: blue;
  color: white;
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  margin-left: 5px;
}

.menu-btn {
  background: none;
  border: none;
  font-size: 20px;
  cursor: pointer;
}

.toplam-kayit {
  margin-top: 10px;
  font-weight: bold;
}

</style>

