import type { SubeModel } from "@/models/SubeModel";
import api from "./Axios";

class SubeService {
  async getSubeler(sirketId: string): Promise<SubeModel[] | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/subeler/${sirketId}`);
      return response.data.value;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new SubeService();
