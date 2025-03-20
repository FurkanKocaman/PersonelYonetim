import type { PozisyonModel } from "@/models/PozisyonModel";
import api from "./Axios";

class PozisyonService {
  async getPozisyonlar(sirketId: string): Promise<PozisyonModel[] | undefined> {
    try {
      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/pozisyonlar?sirketId=${sirketId}`
      );
      return response.data.value;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new PozisyonService();
