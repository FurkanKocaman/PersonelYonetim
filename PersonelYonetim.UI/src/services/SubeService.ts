import type { SubeModel } from "@/models/entity-models/SubeModel";
import api from "./Axios";
import type { SubeCreateRequest } from "@/models/request-models/SubeCreateRequest";

class SubeService {
  async subelerGet(sirketId: string): Promise<{ Subeler: SubeModel[]; count: number } | undefined> {
    try {
      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/subeler?sirketId=${sirketId}`,
        {
          params: {
            $count: true,
          },
        }
      );
      return { Subeler: response.data.value, count: response.data["@odata.count"] };
    } catch (error) {
      console.error(error);
    }
  }
  
  async subelerCreate(request: SubeCreateRequest): Promise<string> {
    const response = await api.post(`${import.meta.env.VITE_API_URL}/subeler/create`, request);
    console.log(response);

    return "a";
  }
  
  // Şube güncelleme metodu
  async subelerUpdate(id: number, request: SubeCreateRequest): Promise<string> {
    const response = await api.put(`${import.meta.env.VITE_API_URL}/subeler/update/${id}`, request);
    console.log(response);

    return "a";
  }
  
  // Şube silme metodu
  async subelerDelete(id: number): Promise<string> {
    const response = await api.delete(`${import.meta.env.VITE_API_URL}/subeler/delete/${id}`);
    console.log(response);

    return "a";
  }
  
  // Şube detaylarını getirme metodu
  async subelerGetById(id: number): Promise<SubeModel | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/subeler/${id}`);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new SubeService();
