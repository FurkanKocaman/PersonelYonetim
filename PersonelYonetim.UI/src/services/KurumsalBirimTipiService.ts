import api from "./Axios";
import { useToastStore } from "@/stores/ToastStore";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { KurumsalBirimTipiGetModel } from "@/models/response-models/KurumsalBirimTipleriGetModel";
import type { KurumsalBirimTipiCreateModel } from "@/models/request-models/KurumsalBirimTipiCreateModel";

class KurumsalBirimTipiService {
  async kurumsalBirimlerCreate(request: KurumsalBirimTipiCreateModel): Promise<string> {
    try {
      const response = await api.post(
        `${import.meta.env.VITE_API_URL}/kurumsal-birim-tipi/create`,
        request
      );

      if (response.status == 200) {
        useToastStore().addToast(response.data.data, "", "success", 5000, true);
        return response.data.data;
      }
      return response.data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
  async kurumsalBirimlerDelete(id: string): Promise<string> {
    try {
      const response = await api.delete(
        `${import.meta.env.VITE_API_URL}/kurumsal-birim-tipi/delete/${id}`
      );

      if (response.status == 200) {
        useToastStore().addToast(response.data.data, "", "success", 5000, true);
        return response.data.data;
      }
      return response.data.data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  async kurumsalBirimTipleriGet(
    paginationParams?: PaginationParams
  ): Promise<
    | { items: KurumsalBirimTipiGetModel[]; count: number; pageSize: number; pageNumber: number }
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
        `${import.meta.env.VITE_API_URL}/odata/kurumsal-birim-tipleri?${queryParams}`,
        {
          params: {
            $count: true,
            $expand: "KurumsalBirimler",
          },
        }
      );
      console.log(response);
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

export default new KurumsalBirimTipiService();
