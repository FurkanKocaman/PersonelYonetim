import type { SirketModel } from "@/models/entity-models/SirketModel";
import api from "./Axios";
import type { SirketCreateRequest } from "@/models/request-models/SirketCreateRequest";
import { useToastStore } from "@/stores/ToastStore";

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
    if (response.status == 200) {
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data.data;
    }
    return response.data;
  }
}

export default new SirketService();
