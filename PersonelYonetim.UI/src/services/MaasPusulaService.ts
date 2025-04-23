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
  async getMaasPusulaPdf(personelId: string, yil: number, ay: number): Promise<string> {
    try {
      const response: {
        data: { data: string; errorMessages: string[]; isSuccessful: boolean; statusCode: number };
      } = await api.post(`${import.meta.env.VITE_API_URL}/maas-pusula/pdf`, {
        personelId: personelId,
        yil: yil,
        ay: ay,
      });
      console.log("Res", response);
      if (response.data.isSuccessful) {
        window.open(`${import.meta.env.VITE_API_URL}${response.data.data}`, "_blank");
      }

      return response.data.data;
    } catch (error) {
      console.error("PDF açılamadı", error);
      throw error;
    }
  }
}

export default new MaasPusulaService();
