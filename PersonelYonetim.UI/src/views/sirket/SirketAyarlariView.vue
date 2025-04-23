<script setup lang="ts">
import type { TenantGetModel } from "@/models/response-models/TenantGetModel";
import TenantService from "@/services/TenantService";
import { onMounted, ref, type Ref } from "vue";

const tenant: Ref<TenantGetModel> = ref({
  id: "",
  name: "",
  displayName: "",
  logoUrl: "",

  sgkIsyeri: "",
  sgkNumarasi: "",
  vergiDairesiAdi: "",
  vergiNumarasi: "",
  tabiOlduguKanun: "",
  address: "",
  city: "",
  postalCode: "",
  phone: "",
  email: "",

  asgariUcret: 0,
  sgkPrimIsciKesintiOrani: 0,
  sgkIssizlikPrimIsciKesintiOrani: 0,
  sgkPrimIsverenKesintiOrani: 0,
  sgkIssizlikPrimIsverenKesintiOrani: 0,
  damgaVergisiOrani: 0,

  fazlaMesaiKatsayi: 0,
  haftasonuFazlaMesaiKatsayi: 0,
  resmiTatilFazlaMesaiKatsayi: 0,

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

onMounted(() => {
  getTenant();
});

const getTenant = async () => {
  const res = await TenantService.tenantGet();
  tenant.value = res.item;
  console.log(tenant.value);
};

const updateTenant = async () => {
  await TenantService.tenantUpdate(tenant.value);
};
</script>

<template>
  <form class="mx-5" @submit.prevent="updateTenant()">
    <div class="space-y-12">
      <div class="border-b border-gray-900/10 pb-5">
        <p class="mt-1 text-sm/6 text-gray-600">
          Şirketinizin ayarlarını ve varsayılan değerleri buradan düzenleyebilirsiniz.
        </p>

        <div class="mt-5 flex flex-col">
          <div class="flex w-full">
            <div class="flex-1 mr-2">
              <label for="username" class="block text-sm/6 font-medium">Şirket Adı</label>
              <div class="mt-2">
                <div
                  class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
                >
                  <input
                    type="text"
                    name="username"
                    id="username"
                    v-model="tenant.name"
                    class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                    placeholder="FiveSoft Ltd. Şti."
                  />
                </div>
              </div>
            </div>
            <div class="flex-1 ml-2">
              <label for="username" class="block text-sm/6 font-medium"
                >Görüntülenecek Şirket Adı</label
              >
              <div class="mt-2">
                <div
                  class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
                >
                  <input
                    type="text"
                    name="username"
                    id="username"
                    v-model="tenant.displayName"
                    class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                    placeholder="Five"
                  />
                </div>
              </div>
            </div>
          </div>
          <div class="flex flex-col mt-5">
            <label for="photo" class="block text-sm/6 font-medium">Şirket Logo</label>
            <div class="flex mt-3">
              <img src="https://picsum.photos/200/300" alt="" class="size-20 rounded-md mr-5" />
              <div class="mt-2 flex items-center gap-x-3">
                <button
                  type="button"
                  class="rounded-md bg-neutral-100 dark:bg-neutral-700 px-2.5 py-1.5 text-sm font-semibold shadow-xs border border-indigo-600 hover:bg-indigo-600 hover:text-neutral-50 cursor-pointer"
                >
                  Değiştir
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- SGK, Vergi start -->

      <div class="flex flex-col">
        <h1 class="text-xl mb-5 font-semibold">SGK Vergi Bilgileri</h1>
        <div class="flex w-full mb-5">
          <div class="flex-1 mr-2">
            <label for="username" class="block text-sm/6 font-medium">SGK İşyeri Adı</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="username"
                  id="username"
                  v-model="tenant.sgkIsyeri"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder=""
                />
              </div>
            </div>
          </div>
          <div class="flex-1 ml-2">
            <label for="username" class="block text-sm/6 font-medium">SGK Numarası</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="username"
                  id="username"
                  v-model="tenant.sgkNumarasi"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="123456789"
                />
              </div>
            </div>
          </div>
        </div>
        <div class="flex w-full mb-5">
          <div class="flex-1 mr-2">
            <label for="username" class="block text-sm/6 font-medium">Vergi Dairesi Adı</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="username"
                  id="username"
                  v-model="tenant.vergiDairesiAdi"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="FiveSoft Ltd. Şti."
                />
              </div>
            </div>
          </div>
          <div class="flex-1 ml-2">
            <label for="username" class="block text-sm/6 font-medium">Vergi Numarası</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="username"
                  id="username"
                  v-model="tenant.vergiNumarasi"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="Five"
                />
              </div>
            </div>
          </div>
        </div>
        <div class="flex w-full">
          <div class="w-1/2 mr-2">
            <label for="username" class="block text-sm/6 font-medium">Tabii Olduğu Kanun</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="username"
                  id="username"
                  v-model="tenant.tabiOlduguKanun"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="FiveSoft Ltd. Şti."
                />
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- SGK, Vergi end -->

      <!-- Adres, Iletisi start -->
      <div class="flex flex-col">
        <h1 class="text-xl mb-5 font-semibold">Adres İletişim Bilgileri</h1>
        <div class="flex w-full mb-5">
          <div class="flex-1 mr-2">
            <label for="username" class="block text-sm/6 font-medium">E-Posta</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="username"
                  id="username"
                  v-model="tenant.email"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="info@five.com"
                />
              </div>
            </div>
          </div>
          <div class="flex-1 ml-2">
            <label for="username" class="block text-sm/6 font-medium">Telefon</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="username"
                  id="username"
                  v-model="tenant.phone"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="0850 356 61 61"
                />
              </div>
            </div>
          </div>
        </div>
        <div class="flex w-full mb-5">
          <div class="flex-1 mr-2">
            <label for="username" class="block text-sm/6 font-medium">Şehir</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="username"
                  id="username"
                  v-model="tenant.city"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="Trabzon"
                />
              </div>
            </div>
          </div>
          <div class="flex-1 ml-2">
            <label for="username" class="block text-sm/6 font-medium">Posta Kodu</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="username"
                  id="username"
                  v-model="tenant.postalCode"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="61000"
                />
              </div>
            </div>
          </div>
        </div>
        <div class="flex w-full">
          <div class="w-1/2 mr-2">
            <label for="username" class="block text-sm/6 font-medium">Adres</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="username"
                  id="username"
                  v-model="tenant.address"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="Çarşıbaşı/Trabzon Merkez mah. "
                />
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- Adres, Iletisi end -->

      <!-- Bordro start -->
      <div class="flex flex-col">
        <h1 class="text-xl mb-5 font-semibold">Bordro Ayarları</h1>
        <div class="flex w-full mb-5">
          <div class="flex-1 mr-2">
            <label for="asgariUcret" class="block text-sm/6 font-medium">Asgari Ücret</label>
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="number"
                  name="asgariUcret"
                  id="asgariUcret"
                  step="any"
                  v-model="tenant.asgariUcret"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="20104"
                />
              </div>
            </div>
          </div>
          <div class="flex-1 ml-2">
            <label for="SGKPrimIsciKesintiOrani" class="block text-sm/6 font-medium"
              >SGK Primi Isci Kesinti Orani</label
            >
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="number"
                  name="SGKPrimIsciKesintiOrani"
                  id="SGKPrimIsciKesintiOrani"
                  step="any"
                  v-model="tenant.sgkPrimIsciKesintiOrani"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="0.14"
                />
              </div>
            </div>
          </div>
        </div>
        <div class="flex w-full mb-5">
          <div class="flex-1 mr-2">
            <label for="SGKIssizlikPrimIsciKesintiOrani" class="block text-sm/6 font-medium"
              >SGK Issizlik Primi Isci Kesinti Orani</label
            >
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="number"
                  name="SGKIssizlikPrimIsciKesintiOrani"
                  id="SGKIssizlikPrimIsciKesintiOrani"
                  step="any"
                  v-model="tenant.sgkIssizlikPrimIsciKesintiOrani"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="0.01"
                />
              </div>
            </div>
          </div>
          <div class="flex-1 ml-2">
            <label for="SGKPrimIsverenKesintiOrani" class="block text-sm/6 font-medium"
              >SGK Primi Isveren Kesinti Orani</label
            >
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="text"
                  name="SGKPrimIsverenKesintiOrani"
                  id="SGKPrimIsverenKesintiOrani"
                  step="any"
                  v-model="tenant.sgkPrimIsverenKesintiOrani"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="0.20"
                />
              </div>
            </div>
          </div>
        </div>
        <div class="flex w-full">
          <div class="flex-1 mr-2">
            <label for="SGKIssizlikPrimIsverenKesintiOrani" class="block text-sm/6 font-medium"
              >SGK Issizlik Primi Isveren Kesinti Orani</label
            >
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="number"
                  name="SGKIssizlikPrimIsverenKesintiOrani"
                  id="SGKIssizlikPrimIsverenKesintiOrani"
                  step="any"
                  v-model="tenant.sgkIssizlikPrimIsverenKesintiOrani"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="0.02 "
                />
              </div>
            </div>
          </div>
          <div class="flex-1 mr-2">
            <label for="DamgaVergisiOrani" class="block text-sm/6 font-medium"
              >Damga Vergisi Orani</label
            >
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="number"
                  name="DamgaVergisiOrani"
                  id="DamgaVergisiOrani"
                  step="any"
                  v-model="tenant.damgaVergisiOrani"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="0.00759"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- Bordro end -->
      <!-- Mesai ayarları start -->
      <div class="flex flex-col">
        <h1 class="text-xl mb-5 font-semibold">Mesai Ayarları</h1>
        <div class="flex w-full mb-5">
          <div class="flex-1 mr-2">
            <label for="FazlaMesaiKatsayi" class="block text-sm/6 font-medium"
              >Fazla Mesai Katsayi</label
            >
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="number"
                  name="FazlaMesaiKatsayi"
                  id="FazlaMesaiKatsayi"
                  step="any"
                  v-model="tenant.fazlaMesaiKatsayi"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="1.5"
                />
              </div>
            </div>
          </div>
          <div class="flex-1 ml-2">
            <label for="HaftasonuFazlaMesaiKatsayi" class="block text-sm/6 font-medium"
              >Haftasonu FazlaMesai Katsayi</label
            >
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="number"
                  name="HaftasonuFazlaMesaiKatsayi"
                  id="HaftasonuFazlaMesaiKatsayi"
                  step="any"
                  v-model="tenant.haftasonuFazlaMesaiKatsayi"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="1.5"
                />
              </div>
            </div>
          </div>
        </div>
        <div class="flex w-full mb-5">
          <div class="w-1/2 mr-2">
            <label for="ResmiTatilFazlaMesaiKatsayi" class="block text-sm/6 font-medium"
              >ResmiTatil FazlaMesai Katsayi</label
            >
            <div class="mt-2">
              <div
                class="flex items-center bg-neutral-100 dark:bg-neutral-700 px-2 outline-none border-b-2 border-indigo-600 rounded-sm"
              >
                <input
                  type="number"
                  name="ResmiTatilFazlaMesaiKatsayi"
                  id="ResmiTatilFazlaMesaiKatsayi"
                  step="any"
                  v-model="tenant.resmiTatilFazlaMesaiKatsayi"
                  class="block min-w-0 grow py-1.5 pr-3 pl-1 text-base placeholder:text-gray-400 focus:outline-none sm:text-sm/6"
                  placeholder="2"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- Mesai ayarları end -->

      <div class="border-b border-gray-900/10 pb-12">
        <h2 class="text-base/7 font-semibold">Bildirimler</h2>
        <p class="mt-1 text-sm/6 text-gray-600">
          Personellere bildirim gönderilecek durumaları buradan düzenleyebilirsiniz.
        </p>

        <div class="mt-10 space-y-10">
          <fieldset>
            <legend class="text-sm/6 font-semibold">E-posta</legend>
            <div class="mt-6 space-y-6">
              <div class="flex gap-3">
                <div class="flex h-6 shrink-0 items-center">
                  <div class="group grid size-4 grid-cols-1">
                    <input
                      id="comments"
                      aria-describedby="comments-description"
                      name="comments"
                      type="checkbox"
                      checked
                      class="col-start-1 row-start-1 appearance-none rounded-sm border border-gray-300 bg-white checked:border-indigo-600 checked:bg-indigo-600 indeterminate:border-indigo-600 indeterminate:bg-indigo-600 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 disabled:border-gray-300 disabled:bg-gray-100 disabled:checked:bg-gray-100 forced-colors:appearance-auto"
                    />
                    <svg
                      class="pointer-events-none col-start-1 row-start-1 size-3.5 self-center justify-self-center stroke-white group-has-disabled:stroke-gray-950/25"
                      viewBox="0 0 14 14"
                      fill="none"
                    >
                      <path
                        class="opacity-0 group-has-checked:opacity-100"
                        d="M3 8L6 11L11 3.5"
                        stroke-width="2"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                      <path
                        class="opacity-0 group-has-indeterminate:opacity-100"
                        d="M3 7H11"
                        stroke-width="2"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                    </svg>
                  </div>
                </div>
                <div class="text-sm/6">
                  <label for="comments" class="font-medium">İzin Talepleri</label>
                  <p id="comments-description" class="text-gray-500">
                    Personel izin talebi oluşturunca veya izin talebi değerledirilince e-posta
                    gönder.
                  </p>
                </div>
              </div>
              <div class="flex gap-3">
                <div class="flex h-6 shrink-0 items-center">
                  <div class="group grid size-4 grid-cols-1">
                    <input
                      id="candidates"
                      aria-describedby="candidates-description"
                      name="candidates"
                      type="checkbox"
                      class="col-start-1 row-start-1 appearance-none rounded-sm border border-gray-300 bg-white checked:border-indigo-600 checked:bg-indigo-600 indeterminate:border-indigo-600 indeterminate:bg-indigo-600 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 disabled:border-gray-300 disabled:bg-gray-100 disabled:checked:bg-gray-100 forced-colors:appearance-auto"
                    />
                    <svg
                      class="pointer-events-none col-start-1 row-start-1 size-3.5 self-center justify-self-center stroke-white group-has-disabled:stroke-gray-950/25"
                      viewBox="0 0 14 14"
                      fill="none"
                    >
                      <path
                        class="opacity-0 group-has-checked:opacity-100"
                        d="M3 8L6 11L11 3.5"
                        stroke-width="2"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                      <path
                        class="opacity-0 group-has-indeterminate:opacity-100"
                        d="M3 7H11"
                        stroke-width="2"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                    </svg>
                  </div>
                </div>
                <div class="text-sm/6">
                  <label for="candidates" class="font-medium">Mesai Talepleri</label>
                  <p id="candidates-description" class="text-gray-500">
                    Personel mesai talebi oluşturunca veya mesai talebi değerledirilince e-posta
                    gönder.
                  </p>
                </div>
              </div>
              <div class="flex gap-3">
                <div class="flex h-6 shrink-0 items-center">
                  <div class="group grid size-4 grid-cols-1">
                    <input
                      id="offers"
                      aria-describedby="offers-description"
                      name="offers"
                      type="checkbox"
                      class="col-start-1 row-start-1 appearance-none rounded-sm border border-gray-300 bg-white checked:border-indigo-600 checked:bg-indigo-600 indeterminate:border-indigo-600 indeterminate:bg-indigo-600 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 disabled:border-gray-300 disabled:bg-gray-100 disabled:checked:bg-gray-100 forced-colors:appearance-auto"
                    />
                    <svg
                      class="pointer-events-none col-start-1 row-start-1 size-3.5 self-center justify-self-center stroke-white group-has-disabled:stroke-gray-950/25"
                      viewBox="0 0 14 14"
                      fill="none"
                    >
                      <path
                        class="opacity-0 group-has-checked:opacity-100"
                        d="M3 8L6 11L11 3.5"
                        stroke-width="2"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                      <path
                        class="opacity-0 group-has-indeterminate:opacity-100"
                        d="M3 7H11"
                        stroke-width="2"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                    </svg>
                  </div>
                </div>
                <div class="text-sm/6">
                  <label for="offers" class="font-medium">Bordro</label>
                  <p id="offers-description" class="text-gray-500">
                    Aylık bordro oluşturlduğunda personele email ile bildir.
                  </p>
                </div>
              </div>
            </div>
          </fieldset>

          <fieldset>
            <legend class="text-sm/6 font-semibold">Bildirimler</legend>
            <p class="mt-1 text-sm/6 text-gray-600">
              Uygulama içi gönderilecek bildirimleri ayarlayabilirsiniz.
            </p>
            <div class="mt-6 space-y-6">
              <div class="flex items-center gap-x-3">
                <input
                  id="push-everything"
                  name="push-notifications"
                  type="radio"
                  checked
                  class="relative size-4 appearance-none rounded-full border border-gray-300 bg-white before:absolute before:inset-1 before:rounded-full before:bg-white not-checked:before:hidden checked:border-indigo-600 checked:bg-indigo-600 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 disabled:border-gray-300 disabled:bg-gray-100 disabled:before:bg-gray-400 forced-colors:appearance-auto forced-colors:before:hidden"
                />
                <label for="push-everything" class="block text-sm/6 font-medium">Herşey</label>
              </div>
              <div class="flex items-center gap-x-3">
                <input
                  id="push-email"
                  name="push-notifications"
                  type="radio"
                  class="relative size-4 appearance-none rounded-full border border-gray-300 bg-white before:absolute before:inset-1 before:rounded-full before:bg-white not-checked:before:hidden checked:border-indigo-600 checked:bg-indigo-600 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 disabled:border-gray-300 disabled:bg-gray-100 disabled:before:bg-gray-400 forced-colors:appearance-auto forced-colors:before:hidden"
                />
                <label for="push-email" class="block text-sm/6 font-medium">E-posta ile aynı</label>
              </div>
              <div class="flex items-center gap-x-3">
                <input
                  id="push-nothing"
                  name="push-notifications"
                  type="radio"
                  class="relative size-4 appearance-none rounded-full border border-gray-300 bg-white before:absolute before:inset-1 before:rounded-full before:bg-white not-checked:before:hidden checked:border-indigo-600 checked:bg-indigo-600 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 disabled:border-gray-300 disabled:bg-gray-100 disabled:before:bg-gray-400 forced-colors:appearance-auto forced-colors:before:hidden"
                />
                <label for="push-nothing" class="block text-sm/6 font-medium">Hİçbir şey</label>
              </div>
            </div>
          </fieldset>
        </div>
      </div>
    </div>

    <div class="mt-6 flex items-center justify-end gap-x-6 mb-10">
      <button type="button" class="text-sm/6 font-semibold cursor-pointer">İptal</button>
      <button
        type="submit"
        class="rounded-md bg-indigo-600 px-3 py-2 text-sm font-semibold text-white shadow-xs hover:bg-indigo-500 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 cursor-pointer"
      >
        Kaydet
      </button>
    </div>
  </form>
</template>
