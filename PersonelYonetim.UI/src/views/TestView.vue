<template>
  <div class="flex flex-col mx-10 my-3">
    <div class="flex justify-between">
      <h3 class="text-neutral-900 font-semibold">Ücret Pusulası</h3>
      <h3 class="text-base text-neutral-500">Ocak 2025</h3>
      <img class="size-10 rounded-md" src="https://picsum.photos/200/300" alt="Şirket Logo" />
    </div>
    <div class="flex mt-10 divide-x divide-neutral-400">
      <div class="flex flex-col flex-1 px-3 divide-y divide-neutral-400">
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">Adı Soyadı</h4>
          <p class="text-base font-semibold">Furkan Kocaman</p>
        </div>
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">TC Kimlik No</h4>
          <p class="text-base font-semibold">12345678900</p>
        </div>
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">İşe Giriş Tarihi</h4>
          <p class="text-base font-semibold">10.02.2025</p>
        </div>
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">İşten Çıkış Tarihi</h4>
          <p class="text-base font-semibold">-</p>
        </div>
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">Ücret Tipi</h4>
          <p class="text-base font-semibold">Aylık</p>
        </div>
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">Brut Ücret</h4>
          <p class="text-base font-semibold">
            <span class="text-base text-neutral-500">₺</span> 50.000<span
              class="text-neutral-600 text-sm"
              >,00 (Net)</span
            >
          </p>
        </div>
      </div>
      <div class="flex flex-col flex-1 px-3 divide-y divide-neutral-400">
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">İşyeri Unvanı</h4>
          <p class="text-base font-semibold">Five Software Ltd. Şti.</p>
        </div>
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">SGK İşyeri Adı</h4>
          <p class="text-base font-semibold">SGK Trabzon</p>
        </div>
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">SGK Numarası</h4>
          <p class="text-base font-semibold">457896547845215</p>
        </div>
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">Vergi Dairesi</h4>
          <p class="text-base font-semibold">Kadıköy VD</p>
        </div>
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">Vergi Numarası</h4>
          <p class="text-base font-semibold">35618812003</p>
        </div>
        <div class="flex justify-between py-2">
          <h4 class="text-neutral-700">Tabi Olduğu Kanun</h4>
          <p class="text-base font-semibold">05510</p>
        </div>
      </div>
    </div>

    <!-- Kazançlar, Kesintiler -->
    <div class="flex mt-10 rounded-md border border-neutral-300">
      <div class="flex flex-col flex-1 rounded-md">
        <h1 class="py-4 px-5 bg-neutral-200 rounded-tl-md font-bold text-neutral-700">KAZANÇLAR</h1>
        <div class="border-l-4 border-green-600/80">
          <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
            <h4 class="text-neutral-700">Normal Kazanç</h4>
            <p class="text-base font-semibold">
              <span class="text-base text-neutral-500">₺</span> 42.910<span
                class="text-neutral-600 text-sm"
                >,00</span
              >
            </p>
          </div>
          <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
            <h4 class="text-neutral-700">Fazla Çalışma</h4>
            <p class="text-base font-semibold">
              <span class="text-base text-neutral-500">₺</span> 42.910<span
                class="text-neutral-600 text-sm"
                >,00</span
              >
            </p>
          </div>
          <div class="flex justify-between p-2">
            <h4 class="text-neutral-700">Ek Ödemeler</h4>
            <p class="text-base font-semibold">
              <span class="text-base text-neutral-500">₺</span> 42.910<span
                class="text-neutral-600 text-sm"
                >,00</span
              >
            </p>
          </div>
          <div class="flex justify-between py-4 px-2 bg-green-100/80">
            <h4 class="text-neutral-700 font-semibold">Tüm Gelirler Toplamı</h4>
            <p class="text-base font-semibold">
              <span class="text-base text-neutral-500">₺</span> 42.910<span
                class="text-neutral-600 text-sm"
                >,00</span
              >
            </p>
          </div>
        </div>
      </div>
      <div class="flex flex-col flex-1 rounded-md">
        <h1
          class="py-4 px-5 bg-neutral-200 rounded-tr-md border-l-4 border-neutral-400 font-bold text-neutral-700"
        >
          KESİNTİLER
        </h1>

        <div class="border-l-4 border-red-600/80">
          <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
            <h4 class="text-neutral-700">Yasal Kesintiler</h4>
            <p class="text-base font-semibold">
              <span class="text-base text-neutral-500">₺</span> 42.910<span
                class="text-neutral-600 text-sm"
                >,00</span
              >
            </p>
          </div>
          <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
            <h4 class="text-neutral-700">Ayni Yardım Kesintileri</h4>
            <p class="text-base font-semibold">
              <span class="text-base text-neutral-500">₺</span> 42.910<span
                class="text-neutral-600 text-sm"
                >,00</span
              >
            </p>
          </div>
          <div class="flex justify-between mx-2 py-2">
            <h4 class="text-neutral-700">Özel Kesintiler</h4>
            <p class="text-base font-semibold">
              <span class="text-base text-neutral-500">₺</span> 42.910<span
                class="text-neutral-600 text-sm"
                >,00</span
              >
            </p>
          </div>
          <div class="flex justify-between py-4 px-2 bg-red-100/80">
            <h4 class="text-neutral-700 font-semibold">Kesintiler Toplamı</h4>
            <p class="text-base font-semibold">
              <span class="text-base text-neutral-500">₺</span> 42.910<span
                class="text-neutral-600 text-sm"
                >,00</span
              >
            </p>
          </div>
        </div>
      </div>
    </div>

    <!-- SGK -->
    <div class="flex flex-col mt-10 rounded-md border border-neutral-300">
      <div>
        <h1 class="py-4 px-5 bg-neutral-200 rounded-tr-md font-bold text-neutral-700">SGK</h1>
      </div>
      <div class="flex flex-1">
        <div class="flex flex-col flex-1 rounded-md">
          <div class="border-l-4 border-slate-600/80">
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">Fiili Çalışma Günü</h4>
              <p class="text-base font-semibold">22</p>
            </div>
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">Ücretli İzin</h4>
              <p class="text-base font-semibold">0</p>
            </div>
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">Raporlu</h4>
              <p class="text-base font-semibold">0</p>
            </div>
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">Hafta Tatili</h4>
              <p class="text-base font-semibold">8</p>
            </div>
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">Resmi Tatil</h4>
              <p class="text-base font-semibold">0</p>
            </div>

            <div class="flex justify-between py-4 px-2 bg-slate-300/80">
              <h4 class="text-neutral-700 font-semibold">SGK Günü</h4>
              <p class="text-base font-semibold">30</p>
            </div>
          </div>
        </div>
        <div class="flex flex-col flex-1 rounded-md">
          <div class="border-l-4 border-yellow-600/80">
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">Ücretsiz İzin</h4>
              <p class="text-base font-semibold">1</p>
            </div>
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">Diğer Eksik Gün</h4>
              <p class="text-base font-semibold">0</p>
            </div>
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">SGK Aylık Matrah</h4>
              <p class="text-base font-semibold">
                <span class="text-base text-neutral-500">₺</span> 42.910<span
                  class="text-neutral-600 text-sm"
                  >,00</span
                >
              </p>
            </div>
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">SGK Primi</h4>
              <p class="text-base font-semibold">
                <span class="text-base text-neutral-500">₺</span> 42.910<span
                  class="text-neutral-600 text-sm"
                  >,00</span
                >
              </p>
            </div>
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">SGK İşsizlik Sigortası Primi</h4>
              <p class="text-base font-semibold">
                <span class="text-base text-neutral-500">₺</span> 42.910<span
                  class="text-neutral-600 text-sm"
                  >,00</span
                >
              </p>
            </div>
            <div class="flex justify-between py-4 px-2 bg-yellow-100/80">
              <h4 class="text-neutral-700 font-semibold">Çalışan SGK Toplamı</h4>
              <p class="text-base font-semibold">
                <span class="text-base text-neutral-500">₺</span> 42.910<span
                  class="text-neutral-600 text-sm"
                  >,00</span
                >
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- Gelir Vergisi, Damga Vergisi -->
    <div class="flex flex-col mt-20 rounded-md border border-neutral-300">
      <div class="flex flex-1">
        <div class="flex flex-col flex-1 rounded-md">
          <h1 class="py-4 px-5 bg-neutral-200 rounded-tl-md font-bold text-neutral-700">
            GELİR VERGİSİ
          </h1>
          <div class="">
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">Kümülatif Gelir Vergisi Matrahı</h4>
              <p class="text-base font-semibold">
                <span class="text-base text-neutral-500">₺</span> 42.910<span
                  class="text-neutral-600 text-sm"
                  >,00</span
                >
              </p>
            </div>
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">GV Aylık Matrah</h4>
              <p class="text-base font-semibold">
                <span class="text-base text-neutral-500">₺</span> 42.910<span
                  class="text-neutral-600 text-sm"
                  >,00</span
                >
              </p>
            </div>
          </div>
        </div>

        <div class="flex flex-col flex-1 rounded-md h-full">
          <h1
            class="py-4 px-5 bg-neutral-200 rounded-tr-md border-l-4 border-neutral-400 font-bold text-neutral-700"
          >
            DAMGA VERGİSİ
          </h1>

          <div class="">
            <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
              <h4 class="text-neutral-700">DV Aylık Matrah</h4>
              <p class="text-base font-semibold">
                <span class="text-base text-neutral-500">₺</span> 42.910<span
                  class="text-neutral-600 text-sm"
                  >,00</span
                >
              </p>
            </div>
          </div>
        </div>
      </div>
      <div class="flex-1 flex">
        <div
          class="flex-1 flex justify-between py-4 px-2 bg-yellow-700/30 border-l-4 border-yellow-700/80"
        >
          <h4 class="text-neutral-700 font-semibold">Gelir Vergisi</h4>
          <p class="text-base font-semibold">
            <span class="text-base text-neutral-500">₺</span> 42.910<span
              class="text-neutral-600 text-sm"
              >,00</span
            >
          </p>
        </div>
        <div
          class="flex-1 flex justify-between py-4 px-2 bg-pink-200/50 border-l-4 border-pink-800/50"
        >
          <h4 class="text-neutral-700 font-semibold">Damga Vergisi</h4>
          <p class="text-base font-semibold">
            <span class="text-base text-neutral-500">₺</span> 42.910<span
              class="text-neutral-600 text-sm"
              >,00</span
            >
          </p>
        </div>
      </div>
    </div>
    <!-- Ele Geçen Ücret -->
    <div class="flex flex-col mt-10 rounded-md border border-neutral-300">
      <div class="flex flex-1">
        <div class="flex flex-col flex-1 rounded-md">
          <div class="flex items-center bg-green-200/60 border-b-4 border-green-600/80">
            <h1 class="flex-1 py-4 px-5 rounded-t-md font-bold text-neutral-700">
              Ele Geçen Ücret
            </h1>
            <div class="mx-3 text-base font-semibold">
              <span class="text-base text-neutral-500">₺</span> 42.910<span
                class="text-neutral-600 text-sm"
                >,00</span
              >
            </div>
          </div>

          <div class="flex justify-between mx-2 py-2 border-b border-neutral-300">
            <p class="flex-1 text-sm mx-5 my-5 text-neutral-700">
              İşe başladığım günden itibaren Ocak 2025 ayının sonuna kadar tüm maaş, fazla çalışma,
              izin ve sosyal haklarımı aldım.
            </p>
            <div class="flex-1 border rounded-md border-neutral-400 text-sm">
              <span class="ml-2 pt-2 text-neutral-500">İmza</span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- Genel Tablo -->
    <div class="flex flex-col mt-10 rounded-md border border-neutral-300"></div>
  </div>
</template>
