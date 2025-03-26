import type { IzinItem, IzinRequest, IzinListResponse } from "@/models/IzinModels";

import api from "./Axios";
import type { IzinKuralModel } from "@/models/entity-models/izin/IzinKuralModel";
import type { IzinTurModel } from "@/models/entity-models/izin/IzinTurModel";

// Mock veri listesi - IzinView.vue tarafından kullanılıyor
export const mockIzinList: IzinItem[] = [
  {
    id: 1,
    personelAdi: "Ahmet Yılmaz",
    izinTipi: "Yıllık İzin",
    baslangicTarihi: "2023-06-15",
    bitisTarihi: "2023-06-25",
    gun: 10,
    aciklama: "Yaz tatili",
    durum: "Onaylandı",
  },
  {
    id: 2,
    personelAdi: "Ayşe Demir",
    izinTipi: "Hastalık İzni",
    baslangicTarihi: "2023-05-10",
    bitisTarihi: "2023-05-12",
    gun: 3,
    aciklama: "Grip",
    durum: "Onaylandı",
  },
  {
    id: 3,
    personelAdi: "Mehmet Kaya",
    izinTipi: "Mazeret İzni",
    baslangicTarihi: "2023-07-05",
    bitisTarihi: "2023-07-05",
    gun: 1,
    aciklama: "Aile ziyareti",
    durum: "Onaylandı",
  },
  {
    id: 4,
    personelAdi: "Zeynep Çelik",
    izinTipi: "Yıllık İzin",
    baslangicTarihi: "2023-08-01",
    bitisTarihi: "2023-08-15",
    gun: 15,
    aciklama: "Yaz tatili",
    durum: "Beklemede",
  },
  {
    id: 5,
    personelAdi: "Ali Öztürk",
    izinTipi: "Ücretsiz İzin",
    baslangicTarihi: "2023-09-01",
    bitisTarihi: "2023-09-30",
    gun: 30,
    aciklama: "Kişisel projeler",
    durum: "Beklemede",
  },
];

export class IzinService {
  private baseUrl = "/api/izin";

  async getIzinKurallar(): Promise<{ IzinKurallar: IzinKuralModel[]; count: number } | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/izin-kurallar`, {
        params: {
          $count: true,
        },
      });

      return { IzinKurallar: response.data.value, count: response.data["@odata.count"] };
    } catch (error) {
      console.error(error);
    }
  }

  async getIzinTurler(): Promise<{ IzinTurler: IzinTurModel[]; count: number } | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/izin-kurallar`, {
        params: {
          $count: true,
        },
      });

      return { IzinTurler: response.data.value, count: response.data["@odata.count"] };
    } catch (error) {
      console.error(error);
    }
  }

  /**
   * İzin listesini getirir
   * @param params Sayfalama parametreleri
   * @returns İzin listesi yanıtı
   */
  async getIzinList(): Promise<IzinListResponse> {
    try {
      // Gerçek API çağrısı:
      // const response = await axios.get(this.baseUrl, { params });
      // return response.data;

      // Mock veri
      const mockIzinList: IzinItem[] = [
        {
          id: 1,
          personelAdi: "Ahmet Yılmaz",
          izinTipi: "Yıllık İzin",
          baslangicTarihi: "2023-06-15",
          bitisTarihi: "2023-06-25",
          gun: 10,
          aciklama: "Yaz tatili",
          durum: "Onaylandı",
        },
        {
          id: 2,
          personelAdi: "Ayşe Demir",
          izinTipi: "Hastalık İzni",
          baslangicTarihi: "2023-05-10",
          bitisTarihi: "2023-05-12",
          gun: 3,
          aciklama: "Grip",
          durum: "Onaylandı",
        },
        {
          id: 3,
          personelAdi: "Mehmet Kaya",
          izinTipi: "Mazeret İzni",
          baslangicTarihi: "2023-07-05",
          bitisTarihi: "2023-07-05",
          gun: 1,
          aciklama: "Aile ziyareti",
          durum: "Onaylandı",
        },
        {
          id: 4,
          personelAdi: "Zeynep Çelik",
          izinTipi: "Yıllık İzin",
          baslangicTarihi: "2023-08-01",
          bitisTarihi: "2023-08-15",
          gun: 15,
          aciklama: "Yaz tatili",
          durum: "Beklemede",
        },
        {
          id: 5,
          personelAdi: "Ali Öztürk",
          izinTipi: "Ücretsiz İzin",
          baslangicTarihi: "2023-09-01",
          bitisTarihi: "2023-09-30",
          gun: 30,
          aciklama: "Kişisel projeler",
          durum: "Beklemede",
        },
      ];

      return {
        items: mockIzinList,
        toplamSayfa: 1,
        mevcutSayfa: 1,
        toplamKayit: mockIzinList.length,
      };
    } catch (error) {
      console.error("İzin listesi alınırken hata oluştu:", error);
      throw error;
    }
  }

  /**
   * Belirli bir izin kaydını ID'ye göre getirir
   * @param id İzin kaydı ID'si
   * @returns İzin kaydı
   */
  async getIzinById(id: number): Promise<IzinItem> {
    try {
      // Gerçek API çağrısı:
      // const response = await axios.get(`${this.baseUrl}/${id}`);
      // return response.data;

      // Mock veri
      const mockIzinList: IzinItem[] = [
        {
          id: 1,
          personelAdi: "Ahmet Yılmaz",
          izinTipi: "Yıllık İzin",
          baslangicTarihi: "2023-06-15",
          bitisTarihi: "2023-06-25",
          gun: 10,
          aciklama: "Yaz tatili",
          durum: "Onaylandı",
        },
        {
          id: 2,
          personelAdi: "Ayşe Demir",
          izinTipi: "Hastalık İzni",
          baslangicTarihi: "2023-05-10",
          bitisTarihi: "2023-05-12",
          gun: 3,
          aciklama: "Grip",
          durum: "Onaylandı",
        },
      ];

      const izin = mockIzinList.find((i) => i.id === id);

      if (!izin) {
        throw new Error("İzin kaydı bulunamadı");
      }

      return izin;
    } catch (error) {
      console.error(`ID: ${id} olan izin kaydı alınırken hata oluştu:`, error);
      throw error;
    }
  }

  /**
   * Yeni bir izin kaydı oluşturur
   * @param izinData İzin verisi
   * @returns Oluşturulan izin kaydı
   */
  async createIzin(izinData: IzinRequest): Promise<IzinItem> {
    try {
      // Gerçek API çağrısı:
      // const response = await axios.post(this.baseUrl, izinData);
      // return response.data;

      // Mock yanıt
      return {
        id: Math.floor(Math.random() * 1000) + 6,
        ...izinData,
      };
    } catch (error) {
      console.error("İzin kaydı oluşturulurken hata oluştu:", error);
      throw error;
    }
  }

  /**
   * Mevcut bir izin kaydını günceller
   * @param id İzin kaydı ID'si
   * @param izinData Güncellenmiş izin verisi
   * @returns Güncellenmiş izin kaydı
   */
  async updateIzin(id: number, izinData: IzinRequest): Promise<IzinItem> {
    try {
      // Gerçek API çağrısı:
      // const response = await axios.put(`${this.baseUrl}/${id}`, izinData);
      // return response.data;

      // Mock yanıt
      return {
        id,
        ...izinData,
      };
    } catch (error) {
      console.error(`ID: ${id} olan izin kaydı güncellenirken hata oluştu:`, error);
      throw error;
    }
  }

  /**
   * Bir izin kaydını siler
   * @param id Silinecek izin kaydının ID'si
   * @returns İşlem başarılı olup olmadığını belirten yanıt
   */
  async deleteIzin(id: number): Promise<{ success: boolean }> {
    try {
      // Gerçek API çağrısı:
      // await axios.delete(`${this.baseUrl}/${id}`);

      // Mock yanıt
      return { success: true };
    } catch (error) {
      console.error(`ID: ${id} olan izin kaydı silinirken hata oluştu:`, error);
      throw error;
    }
  }

  /**
   * İzin tipleri listesini döndürür
   * @returns İzin tipleri listesi
   */
  getIzinTipleri(): string[] {
    return [
      "Yıllık İzin",
      "Hastalık İzni",
      "Mazeret İzni",
      "Doğum İzni",
      "Babalık İzni",
      "Evlilik İzni",
      "Ölüm İzni",
      "Ücretsiz İzin",
    ];
  }

  /**
   * İzin durumu listesini döndürür
   * @returns Durum listesi
   */
  getDurumList(): string[] {
    return ["Onaylandı", "Beklemede", "Reddedildi"];
  }
}

export default new IzinService();
