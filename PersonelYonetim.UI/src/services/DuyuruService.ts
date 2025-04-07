import type { DuyuruModel } from "@/models/entity-models/DuyuruModel";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import api from "./Axios";
import type { DuyuruCreateRequest } from "@/models/request-models/DuyuruCreateRequest";
import { useToastStore } from "@/stores/ToastStore";

class DuyuruService {
  async getDuyurular(
    paginationParams?: PaginationParams
  ): Promise<
    { items: DuyuruModel[]; count: number; pageSize: number; pageNumber: number } | undefined
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
        `${import.meta.env.VITE_API_URL}/odata/duyurular?${queryParams}`,
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

  async createDuyuru(request: DuyuruCreateRequest): Promise<string | undefined> {
    try {
      const response = await api.post(`${import.meta.env.VITE_API_URL}/duyurular/create`, request);
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new DuyuruService();
