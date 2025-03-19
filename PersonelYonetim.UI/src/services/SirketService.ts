import type { SirketModel } from "@/models/SirketModel";
import api from "./Axios";

class SirketService {
  async getSirketler(): Promise<SirketModel[] | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/sirketler`);
      return response.data.value;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new SirketService();
