import { useToastStore } from "@/stores/ToastStore";
import api from "./Axios";

class MaasPusulaService {
  async maasPusulaDegerlendir(id: string, degerlendirmeDurumValue: number): Promise<string> {
    try {
      const res = await api.post(`${import.meta.env.VITE_API_URL}/maas-pusula/degerlendir`, {
        id: id,
        degerlendirmeDurumValue: degerlendirmeDurumValue,
      });
      useToastStore().addToast(res.data.data, "", "success", 5000, true);
      return res.data.data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
}

export default new MaasPusulaService();
