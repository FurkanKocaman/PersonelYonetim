import type { TakvimEtkinlikModel } from "@/models/entity-models/TakvimEtkinlikModel";
import api from "./Axios";
import type { TakvimEtkinlikCreateCommand } from "@/models/request-models/TakvimEtkinlikCreateCommand";
import { useToastStore } from "@/stores/ToastStore";

export interface Etkinlik {
  id: number;
  baslik: string;
  tarih: string;
}

export const TakvimService = {
  etkinlikler: <Etkinlik[]>[
    { id: 1, baslik: "Toplantı", tarih: "2025-03-05" },
    { id: 2, baslik: "Doğum Günü", tarih: "2025-04-23" },
  ],

  tatilGunleri: {
    "1-1": "Yılbaşı",
    "4-23": "23 Nisan",
    "5-19": "19 Mayıs",
    "8-30": "30 Ağustos",
    "10-29": "Cumhuriyet Bayramı",
  },

  async getEtkinlikler(): Promise<TakvimEtkinlikModel[] | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/takvim-etkinlikler`, {
        params: {
          $count: true,
        },
      });
      console.log(response);
      return response.data.value;
    } catch (error) {
      console.error(error);
    }
    return undefined;
  },

  async createEtkinlik(request: TakvimEtkinlikCreateCommand): Promise<string | undefined> {
    try {
      const response = await api.post(
        `${import.meta.env.VITE_API_URL}/takvim-etkinlik/create`,
        request
      );

      if (response.status == 200) {
        useToastStore().addToast("Etkinlik başarıyla oluşturuldu.", "", "success", 5000, true);
      }
      return response.data.data;
    } catch (error) {
      console.error(error);
    }
  },
  async updateEtkinlik(request: TakvimEtkinlikCreateCommand): Promise<string | undefined> {
    try {
      const response = await api.put(
        `${import.meta.env.VITE_API_URL}/takvim-etkinlik/update`,
        request
      );

      if (response.status == 200) {
        useToastStore().addToast("Etkinlik başarıyla güncellendi.", "", "success", 5000, true);
      }
      return response.data;
    } catch (error) {
      console.error(error);
    }
  },
  async deleteEtkinlik(request: string): Promise<string | undefined> {
    console.log("delete");
    try {
      const response = await api.delete(
        `${import.meta.env.VITE_API_URL}/takvim-etkinlik/delete/${request}`
      );

      if (response.status == 200) {
        useToastStore().addToast(response.data.data, "", "success", 5000, true);
      }
      return response.data;
    } catch (error) {
      console.error(error);
    }
  },

  getTatilGunleri() {
    return Promise.resolve(this.tatilGunleri);
  },

  ekleEtkinlik(etkinlik: Etkinlik) {
    this.etkinlikler.push(etkinlik);
  },

  silEtkinlik(id: number) {
    this.etkinlikler = this.etkinlikler.filter((e) => e.id !== id);
  },
};
