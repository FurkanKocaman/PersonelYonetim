import type { CalismaTakvimModel } from "@/models/entity-models/calisma-takvim/CalismaTakvimModel";
import api from "./Axios";

export default class CalismaTakvimService {
  public static async getCalismaTakvimleri(): Promise<CalismaTakvimModel[]> {
    try {
      const res = await api.get(`${import.meta.env.VITE_API_URL}/odata/calisma-takvim`, {
        params: {
          $expand: "Gunler",
        },
      });

      return res.data.value;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
}
