import api from "./Axios";
import type { DepartmanModel } from "@/models/DepartmanModel";

class DepartmanService {
  async getDepartmanlar(subeId: string): Promise<DepartmanModel[] | undefined> {
    try {
      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/departmanlar/${subeId}`
      );
      return response.data.value;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new DepartmanService();
