import type { PozisyonModel } from "@/models/entity-models/PozisyonModel";
import api from "./Axios";
import type { PozisyonCreateRequest } from "@/models/request-models/PozisyonlarCreateRequest";

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

    return "a";
  }
}

export default new PozisyonService();
