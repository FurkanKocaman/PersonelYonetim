import type { DepartmanCreateRequest } from "@/models/request-models/DepartmanCreateCommand";
import api from "./Axios";
import type { DepartmanModel } from "@/models/entity-models/DepartmanModel";

class DepartmanService {
  async departmanlarGet(
    subeId: string
  ): Promise<{ Departmanlar: DepartmanModel[]; count: number } | undefined> {
    try {
      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/departmanlar?subeId=${subeId}`,
        {
          params: {
            $count: true,
          },
        }
      );
      return { Departmanlar: response.data.value, count: response.data["@odata.count"] };
    } catch (error) {
      console.error(error);
    }
  }

  async departmanlarCreate(request: DepartmanCreateRequest): Promise<string> {
    const response = await api.post(`${import.meta.env.VITE_API_URL}/departmanlar/create`, request);
    console.log(response);

    return "a";
  }
}

export default new DepartmanService();
