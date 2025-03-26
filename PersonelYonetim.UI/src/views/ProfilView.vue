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

      


</template>