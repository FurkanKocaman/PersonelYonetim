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
      console.log(response);
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
}

export default new SubeService();
