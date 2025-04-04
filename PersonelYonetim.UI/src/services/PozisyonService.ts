import type { PozisyonModel } from "@/models/entity-models/PozisyonModel";
import api from "./Axios";
import type { PozisyonCreateRequest } from "@/models/request-models/PozisyonlarCreateRequest";
import { useToastStore } from "@/stores/ToastStore";
import type { PaginationParams } from "@/models/request-models/PaginationParams";

class PozisyonService {
  async pozisyonlarGet(
    sirketId: string,
    paginationParams?: PaginationParams
  ): Promise<
    { items: PozisyonModel[]; count: number; pageSize: number; pageNumber: number } | undefined
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
        `${import.meta.env.VITE_API_URL}/odata/pozisyonlar?${queryParams}`,
        {
          params: {
            $count: true,
            sirketId: sirketId,
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

  async pozisyonlarCreate(request: PozisyonCreateRequest): Promise<string> {
    const response = await api.post(`${import.meta.env.VITE_API_URL}/pozisyonlar/create`, request);
    console.log(response);
    if (response.status == 200) {
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data.data;
    }
    return response.data;
  }

  async pozisyonlarUpdate(id: string, request: PozisyonCreateRequest): Promise<string> {
    request.id = id;
    const response = await api.put(`${import.meta.env.VITE_API_URL}/pozisyonlar/update/`, request);
    useToastStore().addToast(response.data.data, "", "success", 5000, true);

    return response.data.data;
  }

  // Pozisyon silme metodu
  async pozisyonlarDelete(id: number): Promise<string> {
    const response = await api.delete(`${import.meta.env.VITE_API_URL}/pozisyonlar/delete/${id}`);
    console.log(response);

    return "a";
  }

  // Pozisyon detaylarını getirme metodu
  async pozisyonlarGetById(id: number): Promise<PozisyonModel | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/pozisyonlar/${id}`);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new PozisyonService();
