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
}

export default new SirketService();
