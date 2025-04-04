import { type PersonelItem, type PersonelListResponse } from "@/models/PersonelModels";
import api from "./Axios";
import type { PersonelCreateRequest } from "@/models/request-models/PersonelCreateRequest";
import { useToastStore } from "@/stores/ToastStore";

class PersonelService {
  // async getPersonelList(params?: PersonelPaginationParams): Promise<PersonelListResponse> {
  async getPersonelList(
    sirketId: string,
    subeId: string | undefined,
    departmanId: string | undefined
  ): Promise<PersonelListResponse> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/personeller`, {
        params: {
          $count: true,
          SirketId: sirketId,
          SubeId: subeId,
          DepartmanId: departmanId,
        },
      });

      const personelList: PersonelItem[] = response.data.value;

      return {
        items: personelList,
        toplamSayfa: 1,
        mevcutSayfa: 1,
        toplamKayit: personelList.length,
      };
    } catch (error) {
      console.error("Personel listesi alınırken hata oluştu:", error);
      throw error;
    }
  }

  async createPersonel(request: PersonelCreateRequest): Promise<string> {
    try {
      const response = await api.post(
        `${import.meta.env.VITE_API_URL}/personeller/create`,
        request
      );
      if (response.status == 200) {
        useToastStore().addToast(response.data.data, "", "success", 5000, true);
        return response.data.data;
      }
      return response.data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
  async updatePersonel(request: PersonelCreateRequest): Promise<string> {
    try {
      const response = await api.put(`${import.meta.env.VITE_API_URL}/personeller/update`, request);
      if (response.status == 200) {
        useToastStore().addToast(response.data.data, "", "success", 5000, true);
        return response.data.data;
      }
      return response.data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  /**
   * Belirli bir personelin detaylarını ID'ye göre getirir
   * @param id Personel ID'si
   * @returns Personel detayları
   */
  // async getPersonelById(id: number): Promise<PersonelItem> {
  //   try {
  //     // Gerçek API çağrısı:
  //     // const response = await axios.get(`${this.baseUrl}/${id}`);
  //     // return response.data;

  //     // Mock veri
  //     const mockPersonelList: PersonelItem[] = [];

  //     const personel = mockPersonelList.find((p) => p.id === id);

  //     if (!personel) {
  //       throw new Error("Personel bulunamadı");
  //     }

  //     return personel;
  //   } catch (error) {
  //     console.error(`ID: ${id} olan personel alınırken hata oluştu:`, error);
  //     throw error;
  //   }
  // }

  /**
   * Mevcut bir personel kaydını günceller
   * @param id Personel ID'si
   * @param personelData Güncellenmiş personel verisi
   * @returns Güncellenmiş personel kaydı
   */
  // async updatePersonel(id: number, personelData: PersonelRequest): Promise<PersonelItem> {
  //   try {
  //     // Gerçek API çağrısı:
  //     // const response = await axios.put(`${this.baseUrl}/${id}`, personelData);
  //     // return response.data;

  //     // Mock yanıt
  //     return {
  //       id,
  //       ...personelData,
  //     };
  //   } catch (error) {
  //     console.error(`ID: ${id} olan personel güncellenirken hata oluştu:`, error);
  //     throw error;
  //   }
  // }

  /**
   * Bir personel kaydını siler
   * @param id Silinecek personel kaydının ID'si
   * @returns İşlem başarılı olup olmadığını belirten yanıt
   */
  async deletePersonel(id: number): Promise<{ success: boolean }> {
    try {
      // Gerçek API çağrısı:
      // await axios.delete(`${this.baseUrl}/${id}`);

      // Mock yanıt
      return { success: true };
    } catch (error) {
      console.error(`ID: ${id} olan personel silinirken hata oluştu:`, error);
      throw error;
    }
  }

  /**
   * Departman listesini döndürür
   * @returns Departman listesi
   */
  getDepartmanList(): string[] {
    return [
      "Bilgi İşlem",
      "İnsan Kaynakları",
      "Muhasebe",
      "Pazarlama",
      "Satış",
      "Üretim",
      "Lojistik",
      "Yönetim",
      "Ar-Ge",
    ];
  }

  /**
   * Personel durumu listesini döndürür
   * @returns Durum listesi
   */
  getDurumList(): string[] {
    return ["Aktif", "Pasif", "İzinli", "İşten Ayrılmış"];
  }
}

export default new PersonelService();
