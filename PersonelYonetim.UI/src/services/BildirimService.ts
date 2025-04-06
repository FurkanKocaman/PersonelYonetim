import type { BildirimModel } from "@/models/entity-models/BildirimModel";
import api from "./Axios";

class BildirimService {
  async getBildirimler(): Promise<BildirimModel[] | undefined> {
    try {
      const queryParams = new URLSearchParams();
      queryParams.append("$top", "5");
      // queryParams.append("$skip", ((pageNumber - 1) * pageSize).toString());
      queryParams.append("$orderby", "createdAt desc");
      //queryParams.append("$filter", filter);
      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/bildirimler?${queryParams}`,
        {
          params: {
            $count: true,
          },
        }
      );
      return response.data.value;
    } catch (error) {
      console.error("Bildirim listesi alınırken hata oluştu:", error);
      throw error;
    }
  }
  async setBildirimOkundu(bildirimId: string): Promise<void> {
    try {
      const response = await api.put(
        `${import.meta.env.VITE_API_URL}/bildirimler/update`,
        { bildirimId: bildirimId },
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      return response.data;
    } catch (error) {
      console.error("Bildirim okundu olarak işaretlenirken hata oluştu:", error);
      throw error;
    }
  }
}

export default new BildirimService();
