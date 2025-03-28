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

// kariyerim-calısma takvimi kısmı
const calismaTakvimiVeriler = ref([
  { baslangic: "2020-10-13", bitis: "", calismaTakvimi: "Genel çalışma tablosu", atamaTarihi: "2023-01-16" },
  { baslangic: "2019-08-02", bitis: "2020-10-12", calismaTakvimi: "Genel çalışma tablosu", atamaTarihi: "2023-02-07" }
]);

// Sıralama değişkenleri
const calismaTakvimisiralamaAnahtari = ref("");
const calismaTakvimisiralamaYon = ref(1);


const calismaTakvimiSirala = (anahtar) => {
  if (calismaTakvimisiralamaAnahtari.value === anahtar) {
    calismaTakvimisiralamaYon.value *= -1;
  } else {
    calismaTakvimisiralamaAnahtari.value = anahtar;
    calismaTakvimisiralamaYon.value = 1;
  }
  veriler.value.sort((a, b) => {
    let degerA = new Date(a[anahtar]).getTime();
    let degerB = new Date(b[anahtar]).getTime();
    return degerA > degerB ? calismaTakvimisiralamaYon.value : -calismaTakvimisiralamaYon.value;
  });
};


const calismaTakvimiTarihFormatla = (tarih) => {
  if (!tarih) return "—";
  return new Date(tarih).toLocaleDateString("tr-TR", {
    day: "2-digit",
    month: "short",
    year: "numeric"
  });
};


const hesaplaSure = (baslangic, bitis) => {
  const basTarih = new Date(baslangic);
  const bitTarih = bitis ? new Date(bitis) : new Date();
  
  const farkMs = bitTarih - basTarih;
  const gun = Math.floor(farkMs / (1000 * 60 * 60 * 24));
  const ay = Math.floor(gun / 30);
  const yil = Math.floor(ay / 12);

  return `${yil} yıl ${ay % 12} ay ${gun % 30} gün`;
};


const calismaTakvimiFiltrelenmisVeri = computed(() => veriler.value);


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
            <h2 class="text-xl font-semibold">Erkan Demir</h2>
            <br>
            <p class="text-gray-600 dark:text-gray-300">Yazılım Personeli</p>
            <p class="text-sm text-gray-500 dark:text-gray-400">Yazılım Üretim</p>
        </div>
        <img src="https://www.indir.com/haber/wp-content/uploads/2021/11/anonimsinde-hesaba-profil-fotografi-nasil-eklenir-.jpg" alt="Erkan Demir" class="w-16 h-16 rounded-full object-cover ">
    </div>
    
    <hr class="my-4 border-gray-300 dark:border-gray-600">
    
    
    <div class="grid grid-cols-2 gap-4 text-sm">
        <div>
            <p class="text-gray-500 dark:text-gray-400">İşe Başlama Tarihi</p>
            <p class="font-medium">2 Ağustos 2019</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Sözleşme Türü</p>
            <p class="font-medium">Süresiz</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Çalışma Süresi</p>
            <p class="font-medium">5 yıl 7 ay 21 gün</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Sözleşme Bitiş Tarihi</p>
            <p class="font-medium">—</p>
        </div>
    </div>

    <hr class="my-4 border-gray-300 dark:border-gray-600">

    <div class="grid grid-cols-2 gap-4 text-sm">
        <div>
            <p class="text-gray-500 dark:text-gray-400">Pozisyon Başlama Tarihi</p>
            <p class="font-medium">8 Ağustos 2024</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Çalışma Şekli</p>
            <p class="font-medium">Tam zamanlı</p>
        </div>
        <div class="col-span-2">
            <p class="text-gray-500 dark:text-gray-400">Şirket</p>
            <p class="font-medium">ELASOFT YAZILIM VE BİLİŞİM TEKNOLOJİLERİ SAN.TİC.LTD.ŞTİ</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Departman</p>
            <p class="font-medium">Yazılım Üretim</p>
        </div>
        <div>
            <p class="text-gray-500 dark:text-gray-400">Unvan</p>
            <p class="font-medium">Yazılım Personeli</p>
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
            <p class="text-gray-900 ">Adil Mert Şahin</p>
            <p class="text-gray-500 text-sm">Yazılım Müdürü</p>
        </div>
    </div>
</div>


<div class="bg-gray-100 p-4 rounded-lg shadow-md w-96 " style="margin-left: 780px; width:560px;" >
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">İletişim</h2>
        <span class="text-gray-400 cursor-pointer">✏️</span>
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3">
        <!-- E-Posta (İş) -->
        <div class="flex items-center space-x-3">
            <span class="text-blue-500">📧</span>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">E-Posta (İş)</p>
                <p class="text-blue-600 font-medium">erkan.demir@elasoft.com.tr</p>
            </div>
            <span class="text-gray-400 cursor-pointer">📋</span>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
        <!-- Telefon (İş) -->
        <div class="flex items-center space-x-3">
            <span class="text-blue-500">📞</span>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">Telefon (İş)</p>
                <p class="text-gray-400">—</p>
            </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
        <!-- E-Posta (Kişisel) -->
        <div class="flex items-center space-x-3">
            <span class="text-gray-400">👀</span>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">E-Posta (Kişisel)</p>
                <p class="text-gray-400">—</p>
            </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
        <!-- Telefon (Kişisel) -->
        <div class="flex items-center space-x-3">
            <span class="text-gray-400">👀</span>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">Telefon (Kişisel)</p>
                <p class="text-blue-600 font-medium">+90 551 159 1957</p>
            </div>
            <span class="text-gray-400 cursor-pointer">📋</span>
        </div>
    </div>
</div>


<div class="bg-gray-100 p-4 rounded-lg shadow-md w-96 " style="margin-left: 780px; width:560px;" >
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Destek</h2>
        <span class="text-gray-400 cursor-pointer">✏️</span>
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
        <span class="text-blue-400 cursor-pointer">✏️</span>
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3" style="height: 310px;">
        <!-- Doğum Tarihi - Cinsiyet -->
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Doğum Tarihi</p>
                <p class="text-gray-900 text-sm">1 May 1989</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px;" >
                <p class="text-gray-600 text-sm">Cinsiyet</p>
                <p class="text-gray-900 text-sm">Erkek</p>
            </div>
        </div>

        <!-- Engel Derecesi -->
        <div>
            <p class="text-gray-600 text-sm">Engel Derecesi</p>
            <p class="text-gray-900">—</p>
        </div>

        <hr class="my-4 border-gray-300 dark:border-gray-600">
<br>
        <!-- Uyruğu - Kimlik Numarası -->
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Uyruğu</p>
                <p class="text-gray-900 text-sm">Türkiye</p>
            </div>
            <div class="flex-grow" style="margin-left: 339px;" >
                <div >
                    <p class="text-gray-600 text-sm" >Kimlik Numarası</p>
                    <p class="text-gray-900 text-sm">41557015086</p>
                </div>
                <br>           
            </div>
        </div>

        <!-- Askerlik Durumu -->
        <div>
            <p class="text-gray-600 text-sm">Askerlik Durumu</p>
            <p class="text-gray-900 text-sm">Tamamlandı</p>
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
                <p class="text-gray-900 text-sm">Mezun</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px;" >
                <p class="text-gray-600 text-sm">Tamamlanan En Yüksek Eğitim Seviyesi</p>
                <p class="text-gray-900 text-sm">Yüksek Lisans</p>
            </div>
        </div>
<br>
        
        <div>
            <p class="text-gray-600 text-sm">Son Tamamlanan Eğitim Kurumu</p>
            <p class="text-gray-900">—</p>
        </div>

    </div>
</div>

<br>
    <!-- Aile -->
<div class="bg-gray-100 p-4 rounded-lg shadow-md w-[750px]" style="margin-left: 60px;">
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Aile</h2>
        <span class="text-blue-400 cursor-pointer">✏️</span>
     
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3" style="height: 150px;">
       
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Medeni Hal</p>
                <p class="text-gray-900 text-sm">Evli</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px;" >
                <p class="text-gray-600 text-sm">Eş Çalışma Durumu</p>
                <p class="text-gray-900 text-sm">—</p>
            </div>
        </div>

        <br>
        <div>
            <p class="text-gray-600 text-sm">Çocuk Sayısı</p>
            <p class="text-gray-900">—</p>
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
                <p class="text-gray-900 text-sm">Aydınlıkevler mah. Hasan Paşa cad. kardeşler apt. sitesi no:74 iç kapı no:3 ortahisar</p>
            </div>
        </div>

        
        <div>
            <p class="text-gray-600 text-sm">Adres (devam)</p>
            <p class="text-gray-900">—</p>
        </div>
        <br>
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Şehir</p>
                <p class="text-gray-900 text-sm">Trabzon</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px;" >
                <p class="text-gray-600 text-sm">Ülke</p>
                <p class="text-gray-900 text-sm">Türkiye</p>
                
            </div>
            
        </div>
<br>
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Posta Kodu</p>
                <p class="text-gray-900 text-sm">61000</p>
            </div>
            <div class="flex-grow" style="margin-left: 279px;" >
                <p class="text-gray-600 text-sm">Telefon</p>
                <p class="text-gray-900 text-sm">—</p>
                
            </div>
            
        </div>
       

    </div>
   
</div>
<br>
<!-- Banka hesabı -->

<div class="bg-gray-100 p-4 rounded-lg shadow-md w-[750px]" style="margin-left: 60px;">
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Banka Hesabı</h2>
        <span class="text-blue-400 cursor-pointer">✏️</span>
     
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3" style="height: 150px;">
       
        

      <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Banka Adı</p>
                <p class="text-gray-900 text-sm">—</p>
            </div>
            <div class="flex-grow" style="margin-left: 300px;" >
                <p class="text-gray-600 text-sm">Hesap Tipi</p>
                <p class="text-gray-900 text-sm">—</p>
                
            </div>
            
        </div>
<br>
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Hesap Numarası</p>
                <p class="text-gray-900 text-sm">—</p>
            </div>
            <div class="flex-grow" style="margin-left: 257px;" >
                <p class="text-gray-600 text-sm">IBAN</p>
                <p class="text-gray-900 text-sm">TR780001001225565740685001</p>
                
            </div>
            
        </div>
       

        

    </div>
   
</div>
<br>
    <!-- İletişim -->
<div class="bg-gray-100 p-4 rounded-lg shadow-md w-96 " style="margin-left: 850px; width:560px; margin-top:-1622px; " >
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">İletişim</h2>
        <span class="text-gray-400 cursor-pointer">✏️</span>
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3">

        
        <div class="flex items-center space-x-3">
            <span class="text-gray-400">👀</span>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">E-Posta (Kişisel)</p>
                <p class="text-gray-400">—</p>
            </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600">
       
        <div class="flex items-center space-x-3">
            <span class="text-gray-400">👀</span>
            <div class="flex-1">
                <p class="text-gray-600 text-sm">Telefon (Kişisel)</p>
                <p class="text-blue-600 font-medium">+90 551 159 1957</p>
            </div>
            <span class="text-gray-400 cursor-pointer">📋</span>
        </div>
    </div>
</div>

<!-- Acil Durum -->

<div class="bg-gray-100 p-4 rounded-lg shadow-md w-[560px]" style="margin-left: 850px;">
    <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Acil Durum</h2>
        <span class="text-blue-400 cursor-pointer">✏️</span>
     
    </div>
    <div class="bg-white p-4 rounded-lg space-y-3" style="height: 220px;">
       
        <div class="flex justify-between">
            <div>
                <p class="text-gray-600 text-sm">Acil durumda erişilebilecek kişinin bilgileri</p>
               
            </div>
            
        </div>
        
        <div>
            <p class="text-gray-600 text-sm">Adı Soyadı</p>
            <p class="text-gray-900">—</p>
        </div>

        <div>
          <p class="text-gray-600 text-sm">Telefon</p>
          <p class="text-gray-900">—</p>
        </div>
        <div>
          <p class="text-gray-600 text-sm">Yakınlık Derecesi</p>
          <p class="text-gray-900">—</p>
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

            <div class="kapsayici">
    <!-- Tablo -->
    <table>
      <thead>
        <tr>
          <th @click="calismaTakvimiSirala('baslangic')">Başlangıç ⬇</th>
          <th>Bitiş</th>
          <th>Süre</th>
          <th>Çalışma Takvimi</th>
          <th>Atama Tarihi</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(kayit, index) in filtrelenmisVeri" :key="index">
          <td>
            {{ calismaTakvimiTarihFormatla(kayit.baslangic) }}
            <span v-if="!kayit.bitis" class="etiket-guncel">Güncel</span>
          </td>
          <td>{{ kayit.bitis ? calismaTakvimiTarihFormatla(kayit.bitis) : "—" }}</td>
          <td>{{ hesaplaSure(kayit.baslangic, kayit.bitis) }}</td>
          <td>{{ kayit.calismaTakvimi }}</td>
          <td>{{ calismaTakvimiTarihFormatla(kayit.atamaTarihi) }}</td>
          <td>
            <button class="menu-btn">⋮</button>
          </td>
        </tr>
      </tbody>
    </table>
    <div class="toplam-kayit">Toplam {{ calismaTakvimiFiltrelenmisVeri.length }}</div>
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
            
        </div>

        <div v-if="activeTab3 === 'ekstraIzinler'" class="space-y-6">
            
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

