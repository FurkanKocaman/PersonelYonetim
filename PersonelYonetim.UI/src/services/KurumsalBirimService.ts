import type { SirketModel } from "@/models/entity-models/SirketModel";
import api from "./Axios";
import type { SirketCreateRequest } from "@/models/request-models/SirketCreateRequest";
import { useToastStore } from "@/stores/ToastStore";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { KurumsalBirimGetModel } from "@/models/response-models/KurumsalBirimGetModel";
import type { KurumsalBirimTipiGetModel } from "@/models/response-models/KurumsalBirimTipleriGetModel";
import type { KurumsalBirimCreateCommand } from "@/models/request-models/KurumsalBirimCreateCommand";

class KurumsalBirimService {
  async kurumsalBirimlerGet(
    paginationParams?: PaginationParams
  ): Promise<
    | { items: KurumsalBirimGetModel[]; count: number; pageSize: number; pageNumber: number }
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
        `${import.meta.env.VITE_API_URL}/odata/kurumsal-birimler?${queryParams}`,
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

  async birimCreate(request: KurumsalBirimCreateCommand): Promise<string> {
    const response = await api.post(
      `${import.meta.env.VITE_API_URL}/kurumsal-birim/create`,
      request
    );
    if (response.status == 200) {
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data.data;
    }
    return response.data;
  }

  async birimUpdate(id: string, request: SirketCreateRequest): Promise<string> {
    request.id = id;
    const response = await api.put(`${import.meta.env.VITE_API_URL}/sirketler/update`, request);
    useToastStore().addToast(response.data.data, "", "success", 5000, true);

    return response.data.data;
  }

  async birimDelete(id: string): Promise<string> {
    const response = await api.delete(`${import.meta.env.VITE_API_URL}/sirketler/delete/${id}`);
    useToastStore().addToast(response.data.data, "", "success", 5000, true);

    return response.data.data;
  }
  async sirketlerGetById(id: number): Promise<SirketModel | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/sirketler/${id}`);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new KurumsalBirimService();
