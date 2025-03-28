import type { SirketModel } from "@/models/entity-models/SirketModel";
import api from "./Axios";
import type { SirketCreateRequest } from "@/models/request-models/SirketCreateRequest";

class SirketService {
  async sirketlerGet(): Promise<{ Sirketler: SirketModel[]; count: number } | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/sirketler`, {
        params: {
          $count: true,
        },
      });

      return { Sirketler: response.data.value, count: response.data["@odata.count"] };
    } catch (error) {
      console.error(error);
    }
  }

  async sirketlerCreate(request: SirketCreateRequest): Promise<string> {
    const response = await api.post(`${import.meta.env.VITE_API_URL}/sirketler/create`, request);
    console.log(response);

    return "a";
  }
  
  // Şirket güncelleme metodu
  async sirketlerUpdate(id: number, request: SirketCreateRequest): Promise<string> {
    const response = await api.put(`${import.meta.env.VITE_API_URL}/sirketler/update/${id}`, request);
    console.log(response);

    return "a";
  }
  
  // Şirket silme metodu
  async sirketlerDelete(id: number): Promise<string> {
    const response = await api.delete(`${import.meta.env.VITE_API_URL}/sirketler/delete/${id}`);
    console.log(response);

    return "a";
  }
  
  // Şirket detaylarını getirme metodu
  async sirketlerGetById(id: number): Promise<SirketModel | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/sirketler/${id}`);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new SirketService();
