<script setup lang="ts">
import { CalismaSekli, SozlesmeTuru } from "@/models/entity-models/UserModel";
import type { PersonelItem } from "@/models/PersonelModels";
import Roles from "@/models/Roles";
import PersonelService from "@/services/PersonelService";
import { onMounted, reactive, ref } from "vue";

const personel: PersonelItem = reactive({
  id: "",
  ad: "",
  soyad: "",
  fullName: "",
  dogumTarihi: new Date(),
  cinsiyet: undefined,
  profilResimUrl: undefined,
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
  yonetici: undefined,
  yoneticiPozisyon: undefined,
  sirketId: "",
  sirketAd: "",
  subeId: undefined,
  subeAd: undefined,
  departmanId: undefined,
  departmanAd: undefined,
  pozisyonId: undefined,
  pozisyonAd: undefined,
  calismaTakvimiId: undefined,
  sozlesmeTuruValue: 0,
  pozisyonBaslangicTarih: new Date(),
  sozlesmeBitisTarihi: undefined,
  izinKuralId: undefined,
  role: 0,
  isActive: true,
  createdAt: new Date(),
  createUserId: "",
  createUserName: undefined,
  updateAt: undefined,
  updateUserId: undefined,
  updateUserName: undefined,
  isDeleted: false,
  deleteAt: undefined,
});

const apiUrl = ref(import.meta.env.VITE_API_URL);

onMounted(async () => {
  const res = await PersonelService.getCurrentPersonel();
  Object.assign(personel, res);
  console.log("Personel", personel);
  console.log(res!.fullName);
});

const iletisimForm = ref(false);
const personelEmail = ref("");

const getCalismaSuresi = (startDateStr: string): string => {
  const startDate = new Date(startDateStr);
  const now = new Date();

  const diffMs = now.getTime() - startDate.getTime();
  const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));

  const years = Math.floor(diffDays / 365);
  const months = Math.floor((diffDays % 365) / 30);
  const days = diffDays - (years * 365 + months * 30);

  return `${years} yıl ${months} ay ${days} gün`;
};
</script>

<template>
  <div
    v-if="personel != undefined"
    class="flex flex-col w-full h-full text-neutral-700 dark:text-neutral-200"
  >
    <div class="flex md:flex-row flex-col justify-start">
      <!-- Kişisel bilgiler -->
      <div class="bg-neutral-100 dark:bg-neutral-800 rounded-lg shadow-sm p-6 flex-1 m-5">
        <div class="flex justify-between items-start mb-4">
          <div>
            <h2 class="text-xl font-semibold">{{ personel.fullName }}</h2>
            <br />
            <p class="text-gray-600 dark:text-gray-300">
              {{ personel.pozisyonAd || Roles.getRoleByValue(personel.role).name }}
            </p>
            <p class="text-sm text-gray-500 dark:text-gray-400">{{ personel.departmanAd }}</p>
          </div>
          <img
            v-if="personel.profilResimUrl"
            class="object-cover mx-2 size-20 rounded-full border-1 border-sky-500"
            :src="apiUrl + personel.profilResimUrl"
            alt="Avatar"
            width="100"
            height="100"
          />
          <div
            v-else
            class="text-4xl font-semibold text-sky-600 transition-all duration-300 ease-in-out mx-2 rounded-full border-1 border-sky-500 w-16 h-16 flex items-center justify-center"
          >
            {{ personel.fullName[0] }}
          </div>
        </div>

        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <div class="grid grid-cols-2 gap-4 text-sm">
          <div>
            <p class="text-gray-500 dark:text-gray-400">İşe Başlama Tarihi</p>
            <p class="font-medium">
              {{
                new Date(personel.pozisyonBaslangicTarih).toLocaleString("tr-TR", {
                  day: "2-digit",
                  month: "2-digit",
                  year: "numeric",
                  hour: "2-digit",
                  minute: "2-digit",
                  hour12: false,
                })
              }}
            </p>
          </div>
          <div>
            <p class="text-gray-500 dark:text-gray-400">Sözleşme Türü</p>
            <p class="font-medium">
              {{ SozlesmeTuru.getSozlesmeByValue(personel.sozlesmeTuruValue).name }}
            </p>
          </div>
          <div>
            <p class="text-gray-500 dark:text-gray-400">Çalışma Süresi</p>
            <p class="font-medium">
              {{ getCalismaSuresi(personel.pozisyonBaslangicTarih.toString()) }}
            </p>
          </div>
          <div>
            <p class="text-gray-500 dark:text-gray-400">Sözleşme Bitiş Tarihi</p>
            <p class="font-medium">{{ personel.sozlesmeBitisTarihi || "—" }}</p>
          </div>
        </div>

        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <div class="grid grid-cols-2 gap-4 text-sm">
          <div>
            <p class="text-gray-500 dark:text-gray-400">Pozisyon Başlama Tarihi</p>
            <p class="font-medium">
              {{
                new Date(personel.createdAt).toLocaleString("tr-TR", {
                  day: "2-digit",
                  month: "2-digit",
                  year: "numeric",
                  hour: "2-digit",
                  minute: "2-digit",
                  hour12: false,
                })
              }}
            </p>
          </div>
          <div>
            <p class="text-gray-500 dark:text-gray-400">Çalışma Şekli</p>
            <p class="font-medium">{{ CalismaSekli.TamZamanli.name }}</p>
          </div>
          <div class="col-span-2">
            <p class="text-gray-500 dark:text-gray-400">Şirket</p>
            <p class="font-medium">{{ personel.sirketAd }}</p>
          </div>
          <div>
            <p class="text-gray-500 dark:text-gray-400">Departman</p>
            <p class="font-medium">{{ personel.departmanAd || "—" }}</p>
          </div>
          <div>
            <p class="text-gray-500 dark:text-gray-400">Unvan</p>
            <p class="font-medium">
              {{ personel.pozisyonAd || Roles.getRoleByValue(personel.role).name }}
            </p>
          </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <div class="mt-4 text-right">
          <RouterLink
            to="/dashboard/profile/kariyerim"
            class="text-blue-600 dark:text-blue-400 hover:underline"
            >Kariyer >
          </RouterLink>
        </div>
      </div>
      <div class="flex flex-col flex-1 my-5 mr-5 md:ml:0 ml-5">
        <!-- Yönetici bilgileri -->
        <div class="flex flex-col p-4 rounded-lg shadow-md mb-4 bg-neutral-100 dark:bg-neutral-800">
          <h2 class="text-lg font-semibold mb-2">Yöneticim</h2>
          <div
            class="p-3 rounded-md flex items-center space-x-3 shadow-md dark:bg-neutral-700/20 bg-neutral-200/50"
          >
            <img
              src="https://www.indir.com/haber/wp-content/uploads/2021/11/anonimsinde-hesaba-profil-fotografi-nasil-eklenir-.jpg"
              alt="Yönetici Resmi"
              class="w-10 h-10 rounded-full"
            />
            <div>
              <p class="text-base font-medium">{{ personel.yonetici }}</p>
              <p class="text-sm text-neutral-400 dark:text-neutral-400">Unvan</p>
            </div>
          </div>
        </div>

        <!-- iletişim -->
        <div class="p-4 rounded-lg shadow-md bg-neutral-100 dark:bg-neutral-800">
          <div class="flex justify-between items-center mb-2">
            <h2 class="text-lg font-semibold">İletişim</h2>
            <button @click="iletisimForm = true">
              <i class="fa-solid fa-pen cursor-pointer text-blue-500"></i>
            </button>
          </div>
          <div class="dark:bg-neutral-700/20 bg-neutral-200/50 shadow-md p-4 rounded-lg space-y-3">
            <!-- E-Posta (İş) -->
            <div class="flex items-center space-x-3">
              <i class="fa-solid fa-envelope" style="color: #3562b1"></i>
              <div class="flex-1">
                <p class="text-sm">E-Posta (İş)</p>
                <p class="text-blue-600 font-medium">{{ personel.iletisim.eposta }}</p>
              </div>
            </div>
            <hr class="my-4 border-gray-300 dark:border-gray-600" />
            <!-- Telefon (İş) -->
            <div class="flex items-center space-x-3">
              <i class="fa-solid fa-phone" style="color: #3562b1"></i>
              <div class="flex-1">
                <p class="text-sm">Telefon (İş)</p>
                <p class="text-blue-600 font-medium">{{ "—" }}</p>
              </div>
            </div>
            <hr class="my-4 border-gray-300 dark:border-gray-600" />
            <!-- E-Posta (Kişisel) -->
            <div class="flex items-center space-x-3">
              <i
                class="fa-solid fa-eye-slash"
                style="color: #3562b1"
                title="iş arkadaşlarına gösterilmez"
              ></i>
              <div class="flex-1">
                <p class="text-sm">E-Posta (Kişisel)</p>
                <p class="text-blue-600 font-medium">
                  {{ "—" }}
                </p>
              </div>
            </div>
            <hr class="my-4 border-gray-300 dark:border-gray-600" />
            <!-- Telefon (Kişisel) -->
            <div class="flex items-center space-x-3">
              <i
                class="fa-solid fa-eye-slash"
                style="color: #3562b1"
                title="iş arkadaşlarına gösterilmez"
              ></i>
              <div class="flex-1">
                <p class="text-sm">Telefon (Kişisel)</p>
                <p class="text-blue-600 font-medium">
                  {{ personel.iletisim.telefon || "—" }}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- iletişim form -->

      <div
        v-if="iletisimForm"
        class="fixed inset-0 flex z-50 items-center justify-center bg-black/60 backdrop-blur-xs"
      >
        <div class="bg-neutral-50 dark:bg-neutral-800 p-6 rounded-lg shadow-xl w-[700px]">
          <h2 class="text-xl mb-6 font-medium">İletişim</h2>
          <hr class="my-4 border-gray-300 dark:border-gray-600" />

          <!-- İş E-Posta ve Telefon -->
          <div class="grid grid-cols-2 gap-4 mb-6">
            <div>
              <label class="block text-gray-600 text-sm font-semibold mb-1">E-Posta (İş)</label>
              <input
                type="text"
                value="erkan.demir@elasoft.com.tr"
                class="w-full border border-gray-300 rounded px-3 py-2 bg-gray-100 cursor-not-allowed"
                disabled
              />
            </div>
            <div>
              <label class="block text-gray-600 text-sm font-semibold mb-1">Telefon (İş)</label>
              <div class="flex items-center border border-gray-300 rounded px-3 py-2 bg-gray-100">
                <span class="mr-2">🇹🇷</span>
                <input
                  type="text"
                  value=""
                  class="w-full bg-gray-100 outline-none cursor-not-allowed"
                  disabled
                />
              </div>
            </div>
          </div>

          <!-- Kişisel E-Posta ve Telefon -->
          <h3 class="text-md font-semibold mb-4 text-sky-600">Kişisel</h3>
          <div class="grid grid-cols-2 gap-4 mb-6">
            <div>
              <label class="block text-gray-600 text-sm font-semibold mb-1"
                >E-Posta (Kişisel)</label
              >
              <input
                v-model="personelEmail"
                type="email"
                class="w-full border border-gray-300 rounded px-3 py-2 focus:ring-2 focus:ring-blue-300"
                placeholder="E-posta (Kişisel)"
              />
            </div>
            <div>
              <label class="block text-gray-600 text-sm font-semibold mb-1"
                >Telefon (Kişisel)</label
              >
              <div
                class="flex items-center border border-gray-300 rounded px-3 py-2 focus-within:ring-2 focus-within:ring-blue-300"
              >
                <span class="mr-2">🇹🇷</span>
                <input type="text" class="w-full outline-none" placeholder="Telefon (Kişisel)" />
              </div>
            </div>
          </div>

          <div class="flex justify-end gap-2">
            <button
              @click="iletisimForm = false"
              class="bg-gray-300 px-4 py-2 rounded text-gray-700 hover:bg-gray-400"
            >
              İptal
            </button>
            <button class="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">
              Kaydet
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Destek  -->
    <!-- <div class="bg-gray-100 p-4 rounded-lg shadow-md w-full">
      <div class="flex justify-between items-center mb-2">
        <h2 class="text-lg font-semibold text-gray-700">Destek</h2>
      </div>
      <div class="bg-white p-4 rounded-lg space-y-3">
        <div class="flex items-center space-x-3">
          <div class="flex-1">
            <p class="text-blue-600 font-small">İzin nasıl talep edilir?</p>
          </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <div class="flex items-center space-x-3">
          <div class="flex-1">
            <p class="text-blue-600 font-small">Harcama nasıl talep edilir?</p>
          </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <div class="flex items-center space-x-3">
          <div class="flex-1">
            <p class="text-blue-600 font-small">İzin talebimi nasıl iptal ederim?</p>
          </div>
        </div>
        <hr class="my-4 border-gray-300 dark:border-gray-600" />

        <div class="flex items-center space-x-3">
          <div class="flex-1">
            <p class="text-blue-600 font-small">Parolamı unuttum nasıl sıfırlarım?</p>
          </div>
        </div>
      </div>
    </div> -->
  </div>
</template>
