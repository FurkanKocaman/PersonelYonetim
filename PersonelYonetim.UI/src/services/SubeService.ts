import type { SubeModel } from "@/models/entity-models/SubeModel";
import api from "./Axios";
import type { SubeCreateRequest } from "@/models/request-models/SubeCreateRequest";
import { useToastStore } from "@/stores/ToastStore";
import type { PaginationParams } from "@/models/request-models/PaginationParams";

class SubeService {
  async subelerGet(
    sirketId: string,
    paginationParams?: PaginationParams
  ): Promise<
    { items: SubeModel[]; count: number; pageSize: number; pageNumber: number } | undefined
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
        `${import.meta.env.VITE_API_URL}/odata/subeler?sirketId=${sirketId}&${queryParams}`,
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

  async subelerCreate(request: SubeCreateRequest): Promise<string> {
    const response = await api.post(`${import.meta.env.VITE_API_URL}/subeler/create`, request);
    console.log(response);

    if (response.status == 200) {
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data.data;
    }

    return response.data;
  }

  async subelerUpdate(id: string, request: SubeCreateRequest): Promise<string> {
    request.id = id;
    const response = await api.put(`${import.meta.env.VITE_API_URL}/subeler/update`, request);
    useToastStore().addToast(response.data.data, "", "success", 5000, true);

    return response.data.data;
  }

  async subelerDelete(id: string): Promise<string> {
    const response = await api.delete(`${import.meta.env.VITE_API_URL}/subeler/delete/${id}`);
    useToastStore().addToast(response.data.data, "", "success", 5000, true);

    return response.data.data;
  }

  // Şube detaylarını getirme metodu
  async subelerGetById(id: number): Promise<SubeModel | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/subeler/${id}`);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new SubeService();
