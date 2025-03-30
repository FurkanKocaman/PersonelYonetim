<script setup lang="ts">
import { ref, onMounted } from "vue";
import type { MaasItem, MaasRequest } from "@/models/entity-models/MaasModels";
import { MaasService } from "@/services/MaasService";

// Maaş servisi örneği oluştur
const maasService = new MaasService();

// Bileşen durumu
const isLoading = ref(true);
const error = ref(false);
const maasList = ref<MaasItem[]>([]);
const showModal = ref(false);
const modalType = ref<"new" | "edit">("new");
const currentItem = ref<MaasItem | null>(null);
const showConfirmModal = ref(false);
const itemToDelete = ref<number | null>(null);

// Form durumu
const formData = ref<Partial<MaasRequest>>({
  personelAdi: "",
  departman: "",
  maas: 0,
  odenmeTarihi: "",
  durum: "Beklemede",
});

// Departman seçenekleri
const departmentOptions = maasService.getDepartmanList();

// Durum seçenekleri
const statusOptions = maasService.getDurumList();

// Maaş listesini yükle
const loadMaasList = async () => {
  try {
    // API çağrısı
    isLoading.value = true;

    const response = await maasService.getMaasList();
    maasList.value = response.items;

    isLoading.value = false;
  } catch (err) {
    console.error("Maaş listesi yüklenirken hata oluştu:", err);
    error.value = true;
    isLoading.value = false;
  }
};

// Para birimini formatla
const formatCurrency = (value: number) => {
  return new Intl.NumberFormat("tr-TR", { style: "currency", currency: "TRY" }).format(value);
};

// Yeni maaş kaydı için modal aç
const openNewModal = () => {
  modalType.value = "new";
  formData.value = {
    personelAdi: "",
    departman: "",
    maas: 0,
    odenmeTarihi: new Date().toISOString().split("T")[0],
    durum: "Beklemede",
  };
  showModal.value = true;
};

// Maaş kaydı düzenlemek için modal aç
const openEditModal = (item: MaasItem) => {
  modalType.value = "edit";
  currentItem.value = item;
  formData.value = { ...item };
  showModal.value = true;
};

// Modal kapat
const closeModal = () => {
  showModal.value = false;
  formData.value = {
    personelAdi: "",
    departman: "",
    maas: 0,
    odenmeTarihi: "",
    durum: "Beklemede",
  };
  currentItem.value = null;
};

// Maaş kaydını kaydet
const saveSalaryRecord = async () => {
  if (!formData.value.personelAdi || !formData.value.departman || !formData.value.odenmeTarihi) {
    alert("Lütfen tüm zorunlu alanları doldurun.");
    return;
  }

  try {
    if (modalType.value === "new") {
      // Yeni kayıt oluştur
      await maasService.createMaas(formData.value as MaasRequest);
    } else if (modalType.value === "edit" && currentItem.value) {
      // Mevcut kaydı güncelle
      await maasService.updateMaas(currentItem.value.id, formData.value as MaasRequest);
    }

    // Listeyi yenile
    await loadMaasList();

    // Modal kapat
    closeModal();
  } catch (error) {
    console.error("Kayıt işlemi sırasında hata oluştu:", error);
    alert("Kayıt işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.");
  }
};

// Silme onay modalını aç
const openDeleteConfirmation = (id: number) => {
  itemToDelete.value = id;
  showConfirmModal.value = true;
};

// Maaş kaydını sil
const deleteSalaryRecord = async () => {
  if (itemToDelete.value) {
    try {
      // Kaydı sil
      await maasService.deleteMaas(itemToDelete.value);

      // Listeyi yenile
      await loadMaasList();

      // Onay modalını kapat
      showConfirmModal.value = false;
      itemToDelete.value = null;
    } catch (error) {
      console.error("Silme işlemi sırasında hata oluştu:", error);
      alert("Silme işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.");
    }
  }
};

// Maaş verilerini dışa aktar
const exportData = () => {
  try {
    // CSV içeriği oluştur
    const headers = ["ID", "Personel Adı", "Departman", "Maaş", "Ödenme Tarihi", "Durum"];

    // Verileri CSV formatına dönüştür
    let csvContent = headers.join(",") + "\n";

    maasList.value.forEach((item) => {
      const row = [
        item.id,
        item.personelAdi,
        item.departman,
        item.maas,
        item.odenmeTarihi,
        item.durum,
      ];
      csvContent += row.join(",") + "\n";
    });

    // CSV içeriği ile bir Blob oluştur
    const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });

    // İndirme bağlantısı oluştur
    const link = document.createElement("a");
    const url = URL.createObjectURL(blob);

    // Bağlantıyı belgeye ekle, tıkla ve kaldır
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    // URL nesnesini temizle
    setTimeout(() => {
      URL.revokeObjectURL(url);
    }, 100);
  } catch (error) {
    console.error("Dışa aktarma hatası:", error);
    alert("Dışa aktarma sırasında bir hata oluştu.");
  }
};

// Maaş raporu oluştur
const generateReport = () => {
  try {
    // Özet istatistikleri hesapla
    const totalSalary = maasList.value.reduce((sum, item) => sum + item.maas, 0);
    const averageSalary = totalSalary / maasList.value.length;
    const maxSalary = Math.max(...maasList.value.map((item) => item.maas));
    const minSalary = Math.min(...maasList.value.map((item) => item.maas));

    // Departmana göre sayım
    const departmentCounts = maasList.value.reduce((acc, item) => {
      acc[item.departman] = (acc[item.departman] || 0) + 1;
      return acc;
    }, {} as Record<string, number>);

    // Duruma göre sayım
    const statusCounts = maasList.value.reduce((acc, item) => {
      acc[item.durum] = (acc[item.durum] || 0) + 1;
      return acc;
    }, {} as Record<string, number>);

    // Rapor için yeni bir pencere aç
    const reportWindow = window.open("", "_blank");
    if (!reportWindow) {
      alert(
        "Lütfen tarayıcınızın açılır pencere engelleyicisini devre dışı bırakın ve tekrar deneyin."
      );
      return;
    }

    // Raporun HTML içeriğini ayarla
    reportWindow.document.write(`
      <!DOCTYPE html>
      <html>
      <head>
        <title>Maaş Raporu - ${new Date().toLocaleDateString("tr-TR")}</title>
        <meta charset="utf-8">
        <style>
          body {
            font-family: Arial, sans-serif;
            margin: 20px;
            color: #333;
          }
          h1, h2 {
            color: #2563eb;
          }
          .report-header {
            text-align: center;
            margin-bottom: 30px;
            padding-bottom: 10px;
            border-bottom: 1px solid #ddd;
          }
          .report-date {
            color: #666;
            font-size: 14px;
            margin-top: 5px;
          }
          .summary-section {
            margin-bottom: 30px;
            padding: 15px;
            background-color: #f8fafc;
            border-radius: 5px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
          }
          .summary-grid {
            display: grid;
            grid-template-columns: repeat(2, 1fr);
            gap: 15px;
            margin-top: 15px;
          }
          .summary-item {
            background-color: white;
            padding: 10px 15px;
            border-radius: 4px;
            box-shadow: 0 1px 2px rgba(0,0,0,0.05);
          }
          .summary-label {
            font-weight: bold;
            margin-bottom: 5px;
            color: #4b5563;
          }
          .summary-value {
            font-size: 18px;
            color: #1e40af;
          }
          table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
          }
          th, td {
            border: 1px solid #ddd;
            padding: 8px 12px;
            text-align: left;
          }
          th {
            background-color: #e5edff;
            color: #1e40af;
          }
          tr:nth-child(even) {
            background-color: #f9fafb;
          }
          .status-badge {
            display: inline-block;
            padding: 3px 8px;
            border-radius: 12px;
            font-size: 12px;
            font-weight: bold;
          }
          .status-approved {
            background-color: #d1fae5;
            color: #065f46;
          }
          .status-pending {
            background-color: #fef3c7;
            color: #92400e;
          }
          .status-rejected {
            background-color: #fee2e2;
            color: #b91c1c;
          }
          .department-section, .status-section {
            margin-top: 30px;
          }
          .print-button {
            position: fixed;
            top: 20px;
            right: 20px;
            padding: 8px 16px;
            background-color: #2563eb;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-weight: bold;
          }
          .print-button:hover {
            background-color: #1d4ed8;
          }
          @media print {
            .print-button {
              display: none;
            }
            body {
              margin: 0;
              padding: 15px;
            }
          }
        </style>
      </head>
      <body>
        <button class="print-button" onclick="window.print()">Yazdır / PDF Olarak Kaydet</button>

        <div class="report-header">
          <h1>Maaş Raporu</h1>
          <div class="report-date">Oluşturulma Tarihi: ${new Date().toLocaleDateString(
            "tr-TR"
          )} ${new Date().toLocaleTimeString("tr-TR")}</div>
        </div>

        <div class="summary-section">
          <h2>Özet Bilgiler</h2>
          <div class="summary-grid">
            <div class="summary-item">
              <div class="summary-label">Toplam Personel Sayısı</div>
              <div class="summary-value">${maasList.value.length}</div>
            </div>
            <div class="summary-item">
              <div class="summary-label">Toplam Maaş Ödemesi</div>
              <div class="summary-value">${new Intl.NumberFormat("tr-TR", {
                style: "currency",
                currency: "TRY",
              }).format(totalSalary)}</div>
            </div>
            <div class="summary-item">
              <div class="summary-label">Ortalama Maaş</div>
              <div class="summary-value">${new Intl.NumberFormat("tr-TR", {
                style: "currency",
                currency: "TRY",
              }).format(averageSalary)}</div>
            </div>
            <div class="summary-item">
              <div class="summary-label">En Yüksek Maaş</div>
              <div class="summary-value">${new Intl.NumberFormat("tr-TR", {
                style: "currency",
                currency: "TRY",
              }).format(maxSalary)}</div>
            </div>
            <div class="summary-item">
              <div class="summary-label">En Düşük Maaş</div>
              <div class="summary-value">${new Intl.NumberFormat("tr-TR", {
                style: "currency",
                currency: "TRY",
              }).format(minSalary)}</div>
            </div>
          </div>
        </div>

        <div class="department-section">
          <h2>Departmanlara Göre Dağılım</h2>
          <table>
            <thead>
              <tr>
                <th>Departman</th>
                <th>Personel Sayısı</th>
                <th>Oran</th>
              </tr>
            </thead>
            <tbody>
              ${Object.entries(departmentCounts)
                .map(
                  ([dept, count]) => `
                <tr>
                  <td>${dept}</td>
                  <td>${count}</td>
                  <td>${((count / maasList.value.length) * 100).toFixed(2)}%</td>
                </tr>
              `
                )
                .join("")}
            </tbody>
          </table>
        </div>

        <div class="status-section">
          <h2>Durumlara Göre Dağılım</h2>
          <table>
            <thead>
              <tr>
                <th>Durum</th>
                <th>Personel Sayısı</th>
                <th>Oran</th>
              </tr>
            </thead>
            <tbody>
              ${Object.entries(statusCounts)
                .map(
                  ([status, count]) => `
                <tr>
                  <td>${status}</td>
                  <td>${count}</td>
                  <td>${((count / maasList.value.length) * 100).toFixed(2)}%</td>
                </tr>
              `
                )
                .join("")}
            </tbody>
          </table>
        </div>

        <div class="detail-section">
          <h2>Detaylı Maaş Listesi</h2>
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Personel Adı</th>
                <th>Departman</th>
                <th>Maaş</th>
                <th>Ödenme Tarihi</th>
                <th>Durum</th>
              </tr>
            </thead>
            <tbody>
              ${maasList.value
                .map(
                  (item) => `
                <tr>
                  <td>${item.id}</td>
                  <td>${item.personelAdi}</td>
                  <td>${item.departman}</td>
                  <td>${new Intl.NumberFormat("tr-TR", {
                    style: "currency",
                    currency: "TRY",
                  }).format(item.maas)}</td>
                  <td>${item.odenmeTarihi}</td>
                  <td>
                    <span class="status-badge ${
                      item.durum === "Onaylandı"
                        ? "status-approved"
                        : item.durum === "Beklemede"
                        ? "status-pending"
                        : "status-rejected"
                    }">
                      ${item.durum}
                    </span>
                  </td>
                </tr>
              `
                )
                .join("")}
            </tbody>
          </table>
        </div>
      </body>
      </html>
    `);

    reportWindow.document.close();
  } catch (error) {
    console.error("Rapor oluşturma hatası:", error);
    alert("Rapor oluşturulurken bir hata oluştu.");
  }
};

// Bileşeni başlat
onMounted(() => {
  loadMaasList();
});
</script>

<template>
  <div class="container mx-auto px-4 py-8">
    <div class="mb-6">
      <h1 class="text-2xl font-bold text-gray-800 dark:text-white">Maaş Yönetimi</h1>
      <p class="text-gray-600 dark:text-gray-400">
        Personel maaş bilgilerini görüntüleyin ve yönetin
      </p>
    </div>

    <!-- Loading State -->
    <div v-if="isLoading" class="flex justify-center items-center h-64">
      <div
        class="w-16 h-16 border-4 border-sky-600 border-t-transparent rounded-full animate-spin"
      ></div>
    </div>

    <!-- Error State -->
    <div
      v-else-if="error"
      class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-6"
    >
      <strong class="font-bold">Hata!</strong>
      <span class="block sm:inline">
        Maaş listesi yüklenirken bir sorun oluştu. Lütfen daha sonra tekrar deneyin.</span
      >
      <button
        @click="loadMaasList"
        class="mt-2 bg-red-600 hover:bg-red-700 text-white font-bold py-2 px-4 rounded"
      >
        Yeniden Dene
      </button>
    </div>

    <!-- Content -->
    <div v-else>
      <!-- Action Buttons -->
      <div class="flex flex-wrap gap-4 mb-6">
        <button
          @click="openNewModal"
          class="bg-sky-600 hover:bg-sky-700 text-white font-bold py-2 px-4 rounded flex items-center"
        >
          <i class="fas fa-plus mr-2"></i> Yeni Maaş Kaydı
        </button>
        <button
          @click="exportData"
          class="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded flex items-center"
        >
          <i class="fas fa-file-export mr-2"></i> Dışa Aktar
        </button>
        <button
          @click="generateReport"
          class="bg-purple-600 hover:bg-purple-700 text-white font-bold py-2 px-4 rounded flex items-center"
        >
          <i class="fas fa-chart-bar mr-2"></i> Maaş Raporu
        </button>
      </div>

      <!-- Salary Table -->
      <div class="bg-white dark:bg-neutral-800 rounded-lg shadow overflow-hidden">
        <table class="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
          <thead class="bg-gray-50 dark:bg-neutral-700">
            <tr>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
              >
                ID
              </th>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
              >
                Personel
              </th>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
              >
                Departman
              </th>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
              >
                Maaş
              </th>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
              >
                Ödenme Tarihi
              </th>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
              >
                Durum
              </th>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider"
              >
                İşlemler
              </th>
            </tr>
          </thead>
          <tbody class="bg-white dark:bg-neutral-800 divide-y divide-gray-200 dark:divide-gray-700">
            <tr
              v-for="maas in maasList"
              :key="maas.id"
              class="hover:bg-gray-50 dark:hover:bg-neutral-700"
            >
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400">
                {{ maas.id }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="text-sm font-medium text-gray-900 dark:text-white">
                  {{ maas.personelAdi }}
                </div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <div class="text-sm text-gray-500 dark:text-gray-400">{{ maas.departman }}</div>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400">
                {{ formatCurrency(maas.maas) }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500 dark:text-gray-400">
                {{ maas.odenmeTarihi }}
              </td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span
                  class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full"
                  :class="{
                    'bg-green-100 text-green-800': maas.durum === 'Onaylandı',
                    'bg-yellow-100 text-yellow-800': maas.durum === 'Beklemede',
                    'bg-red-100 text-red-800': maas.durum === 'Reddedildi',
                  }"
                >
                  {{ maas.durum }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                <button
                  @click="openEditModal(maas)"
                  class="text-sky-600 hover:text-sky-900 dark:hover:text-sky-400 mr-3"
                >
                  <i class="fas fa-edit"></i>
                </button>
                <button
                  @click="openDeleteConfirmation(maas.id)"
                  class="text-red-600 hover:text-red-900 dark:hover:text-red-400"
                >
                  <i class="fas fa-trash"></i>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="flex justify-between items-center mt-6">
        <div class="text-sm text-gray-700 dark:text-gray-300">
          Toplam <span class="font-medium">{{ maasList.length }}</span> kayıt
        </div>
        <div class="flex space-x-2">
          <button
            class="bg-gray-200 dark:bg-neutral-700 text-gray-700 dark:text-gray-300 px-3 py-1 rounded-md"
          >
            <i class="fas fa-chevron-left"></i>
          </button>
          <button class="bg-sky-600 text-white px-3 py-1 rounded-md">1</button>
          <button
            class="bg-gray-200 dark:bg-neutral-700 text-gray-700 dark:text-gray-300 px-3 py-1 rounded-md"
          >
            <i class="fas fa-chevron-right"></i>
          </button>
        </div>
      </div>
    </div>
  </div>

  <!-- Add/Edit Modal -->
  <div
    v-if="showModal"
    class="fixed inset-0 backdrop-blur-sm bg-black/30 flex items-center justify-center z-50 transition-all duration-300"
  >
    <div
      class="bg-white dark:bg-neutral-800 rounded-lg shadow-xl max-w-md w-full mx-4 transform transition-all duration-300 scale-100"
    >
      <div class="border-b border-gray-200 dark:border-gray-700 px-6 py-4">
        <h3 class="text-lg font-medium text-gray-900 dark:text-white">
          {{ modalType === "new" ? "Yeni Maaş Kaydı" : "Maaş Kaydını Düzenle" }}
        </h3>
      </div>
      <div class="px-6 py-4">
        <div class="mb-4">
          <label
            class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2"
            for="personelAdi"
          >
            Personel Adı *
          </label>
          <input
            id="personelAdi"
            v-model="formData.personelAdi"
            type="text"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="Personel adını girin"
          />
        </div>
        <div class="mb-4">
          <label
            class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2"
            for="departman"
          >
            Departman *
          </label>
          <select
            id="departman"
            v-model="formData.departman"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
          >
            <option value="" disabled>Departman seçin</option>
            <option v-for="dept in departmentOptions" :key="dept" :value="dept">{{ dept }}</option>
          </select>
        </div>
        <div class="mb-4">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="maas">
            Maaş (TL) *
          </label>
          <input
            id="maas"
            v-model.number="formData.maas"
            type="number"
            min="0"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
            placeholder="Maaş miktarını girin"
          />
        </div>
        <div class="mb-4">
          <label
            class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2"
            for="odenmeTarihi"
          >
            Ödenme Tarihi *
          </label>
          <input
            id="odenmeTarihi"
            v-model="formData.odenmeTarihi"
            type="date"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
          />
        </div>
        <div class="mb-4">
          <label class="block text-gray-700 dark:text-gray-300 text-sm font-bold mb-2" for="durum">
            Durum *
          </label>
          <select
            id="durum"
            v-model="formData.durum"
            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 dark:text-white dark:bg-neutral-700 dark:border-neutral-600 leading-tight focus:outline-none focus:shadow-outline"
          >
            <option v-for="status in statusOptions" :key="status" :value="status">
              {{ status }}
            </option>
          </select>
        </div>
      </div>
      <div
        class="border-t border-gray-200 dark:border-gray-700 px-6 py-4 flex justify-end space-x-2"
      >
        <button
          @click="closeModal"
          class="bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
        >
          İptal
        </button>
        <button
          @click="saveSalaryRecord"
          class="bg-sky-600 hover:bg-sky-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
        >
          Kaydet
        </button>
      </div>
    </div>
  </div>

  <!-- Delete Confirmation Modal -->
  <div
    v-if="showConfirmModal"
    class="fixed inset-0 backdrop-blur-sm bg-black/30 flex items-center justify-center z-50 transition-all duration-300"
  >
    <div
      class="bg-white dark:bg-neutral-800 rounded-lg shadow-xl max-w-md w-full mx-4 transform transition-all duration-300 scale-100"
    >
      <div class="border-b border-gray-200 dark:border-gray-700 px-6 py-4">
        <h3 class="text-lg font-medium text-gray-900 dark:text-white">Kaydı Sil</h3>
      </div>
      <div class="px-6 py-4">
        <p class="text-gray-700 dark:text-gray-300">
          Bu maaş kaydını silmek istediğinizden emin misiniz? Bu işlem geri alınamaz.
        </p>
      </div>
      <div
        class="border-t border-gray-200 dark:border-gray-700 px-6 py-4 flex justify-end space-x-2"
      >
        <button
          @click="showConfirmModal = false"
          class="bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
        >
          İptal
        </button>
        <button
          @click="deleteSalaryRecord"
          class="bg-red-600 hover:bg-red-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
        >
          Sil
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.animate-spin {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
