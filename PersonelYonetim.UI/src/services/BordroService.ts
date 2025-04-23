import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { BordroGetAllModel } from "@/models/response-models/BordroGetAllModel";
import api from "./Axios";
import type { BordroGetCalisanlarModel } from "@/models/response-models/BordroCalisanlarGetModel";
import type { BordroGetByPersonelModel } from "@/models/response-models/BordroGetByPersonelModel";
import { useToastStore } from "@/stores/ToastStore";

class BordroService {
  async bordroCreate(
    yil: number,
    ay: number,
    personelId: string[] | undefined,
    tekrarHesapla: boolean
  ): Promise<string> {
    try {
      const res = await api.post(`${import.meta.env.VITE_API_URL}/bordro/create`, {
        yil: yil,
        ay: ay,
        personelId: personelId,
        tekrarHesapla: tekrarHesapla,
      });
      useToastStore().addToast(res.data.data, "", "success", 5000, true);
      return res.data.data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  async bordroGetAll(
    yil: number,
    ay: number,
    paginationParams?: PaginationParams
  ): Promise<
    { items: BordroGetAllModel[]; count: number; pageSize: number; pageNumber: number } | undefined
  > {
    try {
      const queryParams = new URLSearchParams();
      if (paginationParams) {
        const { pageNumber, pageSize, orderBy, filter } = paginationParams;
        queryParams.append("$top", pageSize.toString());
        queryParams.append("$skip", ((pageNumber - 1) * pageSize).toString());

        if (orderBy) queryParams.append("$orderby", orderBy);
        if (filter) queryParams.append("$filter", filter);
      }

      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/bordro?${queryParams}`,
        {
          params: {
            $count: true,
            yil: yil,
            ay: ay,
          },
        }
      );

      return {
        items: response.data.value,
        count: response.data["@odata.count"],
        pageSize: paginationParams?.pageSize ?? 0,
        pageNumber: paginationParams?.pageNumber ?? 0,
      };
    } catch (error) {
      console.error(error);
    }
  }
  async bordroGetCalisanlarAll(
    paginationParams?: PaginationParams
  ): Promise<
    | { items: BordroGetCalisanlarModel[]; count: number; pageSize: number; pageNumber: number }
    | undefined
  > {
    try {
      const queryParams = new URLSearchParams();
      if (paginationParams) {
        const { pageNumber, pageSize, orderBy, filter } = paginationParams;
        queryParams.append("$top", pageSize.toString());
        queryParams.append("$skip", ((pageNumber - 1) * pageSize).toString());

        if (orderBy) queryParams.append("$orderby", orderBy);
        if (filter) queryParams.append("$filter", filter);
      }

      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/bordro-calisanlar?${queryParams}`,
        {
          params: {
            $count: true,
          },
        }
      );

      return {
        items: response.data.value,
        count: response.data["@odata.count"],
        pageSize: paginationParams?.pageSize ?? 0,
        pageNumber: paginationParams?.pageNumber ?? 0,
      };
    } catch (error) {
      console.error(error);
    }
  }

  async bordroGetByPersonel(
    yil: number | undefined,
    paginationParams?: PaginationParams
  ): Promise<
    | { items: BordroGetByPersonelModel[]; count: number; pageSize: number; pageNumber: number }
    | undefined
  > {
    try {
      const queryParams = new URLSearchParams();
      if (paginationParams) {
        const { pageNumber, pageSize, orderBy, filter } = paginationParams;
        queryParams.append("$top", pageSize.toString());
        queryParams.append("$skip", ((pageNumber - 1) * pageSize).toString());

        if (orderBy) queryParams.append("$orderby", orderBy);
        if (filter) queryParams.append("$filter", filter);
      }

      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/personel-bordrolar?${queryParams}`,
        {
          params: {
            $count: true,
            yil: yil,
          },
        }
      );

      return {
        items: response.data.value,
        count: response.data["@odata.count"],
        pageSize: paginationParams?.pageSize ?? 0,
        pageNumber: paginationParams?.pageNumber ?? 0,
      };
    } catch (error) {
      console.error(error);
    }
  }
}

export default new BordroService();
