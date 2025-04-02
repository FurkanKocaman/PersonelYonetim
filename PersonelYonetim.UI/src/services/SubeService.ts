import type { SubeModel } from "@/models/entity-models/SubeModel";
import api from "./Axios";
import type { SubeCreateRequest } from "@/models/request-models/SubeCreateRequest";
import { useToastStore } from "@/stores/ToastStore";

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

    if (response.status == 200) {
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data.data;
    }

    return response.data;
  }

  async subelerUpdate(id: string, request: SubeCreateRequest): Promise<string> {
    request.id = id;
    const response = await api.put(`${import.meta.env.VITE_API_URL}/subeler/update`, request);
    useToastStore().addToast(response.data.data, "", "success", 5000, true);

    return response.data.data;
  }

  async subelerDelete(id: string): Promise<string> {
    const response = await api.delete(`${import.meta.env.VITE_API_URL}/subeler/delete/${id}`);
    useToastStore().addToast(response.data.data, "", "success", 5000, true);

    return response.data.data;
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
