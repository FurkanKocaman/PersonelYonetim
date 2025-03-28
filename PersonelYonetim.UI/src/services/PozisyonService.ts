import type { PozisyonModel } from "@/models/entity-models/PozisyonModel";
import api from "./Axios";
import type { PozisyonCreateRequest } from "@/models/request-models/PozisyonlarCreateRequest";
import { useToastStore } from "@/stores/ToastStore";

class PozisyonService {
  async pozisyonlarGet(
    sirketId: string
  ): Promise<{ Pozisyonlar: PozisyonModel[]; count: number } | undefined> {
    try {
      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/pozisyonlar?sirketId=${sirketId}`,
        {
          params: {
            $count: true,
          },
        }
      );
      return { Pozisyonlar: response.data.value, count: response.data["@odata.count"] };
    } catch (error) {
      console.error(error);
    }
  }

  async pozisyonlarCreate(request: PozisyonCreateRequest): Promise<string> {
    const response = await api.post(`${import.meta.env.VITE_API_URL}/pozisyonlar/create`, request);
    console.log(response);
    if (response.status == 200) {
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data.data;
    }
    return response.data;
  }
  
  // Pozisyon güncelleme metodu
  async pozisyonlarUpdate(id: number, request: PozisyonCreateRequest): Promise<string> {
    const response = await api.put(`${import.meta.env.VITE_API_URL}/pozisyonlar/update/${id}`, request);
    console.log(response);

    return "a";
  }
  
  // Pozisyon silme metodu
  async pozisyonlarDelete(id: number): Promise<string> {
    const response = await api.delete(`${import.meta.env.VITE_API_URL}/pozisyonlar/delete/${id}`);
    console.log(response);

    return "a";
  }
  
  // Pozisyon detaylarını getirme metodu
  async pozisyonlarGetById(id: number): Promise<PozisyonModel | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/pozisyonlar/${id}`);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new PozisyonService();
