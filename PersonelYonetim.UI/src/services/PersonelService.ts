import { type PersonelItem } from "@/models/PersonelModels";
import api from "./Axios";
import { useToastStore } from "@/stores/ToastStore";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { PersonelAtamaModel } from "@/models/entity-models/PersonelAtamaModel";
import type { PersonelCreateCommand } from "@/models/request-models/PersonelCreateCommand";

class PersonelService {
  // async getPersonelList(params?: PersonelPaginationParams): Promise<PersonelListResponse> {
  async getPersonelList(
    kurumsalBirimId: string | undefined,
    paginationParams?: PaginationParams
  ): Promise<
    { items: PersonelItem[]; count: number; pageSize: number; pageNumber: number } | undefined
  > {
    try {
      const queryParams = new URLSearchParams();
      if (paginationParams) {
        const { pageNumber, pageSize, orderBy, filter } = paginationParams;

        queryParams.append("$top", pageSize.toString());
        queryParams.append("$skip", ((pageNumber - 1) * pageSize).toString());

        if (orderBy) queryParams.append("$orderby", orderBy);
        if (filter) queryParams.append("$filter", filter);
      }
      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/personeller?${queryParams}`,
        {
          params: {
            $count: true,
            kurumsalBirimId: kurumsalBirimId,
          },
        }
      );
      return {
        items: response.data.value,
        count: response.data["@odata.count"],
        pageSize: paginationParams?.pageSize ?? 0,
        pageNumber: paginationParams?.pageNumber ?? 0,
      };
    } catch (error) {
      console.error("Personel listesi alınırken hata oluştu:", error);
      throw error;
    }
  }

  async createPersonel(request: PersonelCreateCommand): Promise<string> {
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
  async updatePersonel(request: PersonelCreateCommand): Promise<string> {
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
  getCurrentPersonel = async (): Promise<PersonelItem | undefined> => {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/personel-current`);
      return response.data[0];
    } catch (error) {
      console.error(error);
    }
  };

  getPersonelAtamalar = async (
    paginationParams?: PaginationParams
  ): Promise<
    { items: PersonelAtamaModel[]; count: number; pageSize: number; pageNumber: number } | undefined
  > => {
    try {
      const queryParams = new URLSearchParams();
      if (paginationParams) {
        const { pageNumber, pageSize, orderBy, filter } = paginationParams;

        queryParams.append("$top", pageSize.toString());
        queryParams.append("$skip", ((pageNumber - 1) * pageSize).toString());

        if (orderBy) queryParams.append("$orderby", orderBy);
        if (filter) queryParams.append("$filter", filter);
      }

      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/personel-atamalar?${queryParams}`,
        {
          params: {
            $count: true,
          },
        }
      );

      return {
        items: response.data.value,
        count: response.data["@odata.count"],
        pageSize: paginationParams?.pageSize ?? 0,
        pageNumber: paginationParams?.pageNumber ?? 0,
      };
    } catch (error) {
      console.error(error);
    }
  };

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
