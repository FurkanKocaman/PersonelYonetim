import api from "./Axios";
import type { IzinKuralModel } from "@/models/entity-models/izin/IzinKuralModel";
import type { IzinTurModel } from "@/models/entity-models/izin/IzinTurModel";
import type { IzinTalepCreateCommand } from "@/models/request-models/IzinTalepCreateCommand";
import type { IzinTalepGetResponse } from "@/models/response-models/izinler/IzinTalepGetResponse";
import { useToastStore } from "@/stores/ToastStore";

export class IzinService {
  private baseUrl = "/api/izin";

  async getIzinKural(): Promise<{ IzinKurallar: IzinKuralModel[]; count: number } | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/izin-kurallar`, {
        params: {
          $count: true,
          $expand: "IzinTurler",
        },
      });

      return { IzinKurallar: response.data.value, count: response.data["@odata.count"] };
    } catch (error) {
      console.error(error);
    }
  }

  async getIzinTurler(): Promise<{ IzinTurler: IzinTurModel[]; count: number } | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/izin-turler`, {
        params: {
          $count: true,
        },
      });

      return { IzinTurler: response.data.value, count: response.data["@odata.count"] };
    } catch (error) {
      console.error(error);
    }
  }

  async createIzinTalep(request: IzinTalepCreateCommand): Promise<string | undefined> {
    try {
      const response = await api.post(`${import.meta.env.VITE_API_URL}/izin-talep/create`, request);
      if (response.status == 200) {
        useToastStore().addToast(response.data.data, "", "success", 5000, true);
      } else {
        useToastStore().addToast(response.data.data, "", "error", 5000, true);
      }
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }

  async getIzinTalepler(): Promise<IzinTalepGetResponse[] | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/izin-talepler`);
      console.log(response);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
  async izinTalepDegerlendir(id: string, onayDurum: number): Promise<string | undefined> {
    try {
      const response = await api.post(`${import.meta.env.VITE_API_URL}/izin-talep/degerlendir`, {
        id: id,
        onayDurum: onayDurum,
      });
      console.log(response);
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new IzinService();
