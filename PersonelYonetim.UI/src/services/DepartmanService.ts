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
  
  // Departman güncelleme metodu
  async departmanlarUpdate(id: number, request: DepartmanCreateRequest): Promise<string> {
    const response = await api.put(`${import.meta.env.VITE_API_URL}/departmanlar/update/${id}`, request);
    console.log(response);

    return "a";
  }
  
  // Departman silme metodu
  async departmanlarDelete(id: number): Promise<string> {
    const response = await api.delete(`${import.meta.env.VITE_API_URL}/departmanlar/delete/${id}`);
    console.log(response);

    return "a";
  }
  
  // Departman detaylarını getirme metodu
  async departmanlarGetById(id: number): Promise<DepartmanModel | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/departmanlar/${id}`);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new DepartmanService();
