import type { DepartmanCreateRequest } from "@/models/request-models/DepartmanCreateCommand";
import api from "./Axios";
import type { DepartmanModel } from "@/models/entity-models/DepartmanModel";
import { useToastStore } from "@/stores/ToastStore";
import type { PaginationParams } from "@/models/request-models/PaginationParams";

class DepartmanService {
  async departmanlarGet(
    subeId: string,
    paginationParams?: PaginationParams
  ): Promise<
    { items: DepartmanModel[]; count: number; pageSize: number; pageNumber: number } | undefined
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
        `${import.meta.env.VITE_API_URL}/odata/departmanlar?subeId=${subeId}&${queryParams}`,
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

  async departmanlarCreate(request: DepartmanCreateRequest): Promise<string> {
    const response = await api.post(`${import.meta.env.VITE_API_URL}/departmanlar/create`, request);
    if (response.status == 200) {
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data.data;
    }

    return response.data;
  }

  async departmanlarUpdate(id: string, request: DepartmanCreateRequest): Promise<string> {
    request.id = id;
    const response = await api.put(`${import.meta.env.VITE_API_URL}/departmanlar/update`, request);
    useToastStore().addToast(response.data.data, "", "success", 5000, true);
    return response.data.value;
  }

  // Departman silme metodu
  async departmanlarDelete(id: number): Promise<string> {
    const response = await api.delete(`${import.meta.env.VITE_API_URL}/departmanlar/delete/${id}`);

    return response.data.data;
  }

  // Departman detaylarını getirme metodu
  async departmanlarGetById(id: number): Promise<DepartmanModel | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/departmanlar/${id}`);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}

export default new DepartmanService();
