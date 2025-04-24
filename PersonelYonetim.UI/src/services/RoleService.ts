import api from "./Axios";
import type { PozisyonCreateRequest } from "@/models/request-models/PozisyonlarCreateRequest";
import { useToastStore } from "@/stores/ToastStore";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { RoleModel } from "@/models/response-models/RoleModel";
import type { RoleCreateCommand } from "@/models/request-models/RoleCreateCommand";

class RoleService {
  async rollerGet(
    tenantId: string | undefined,
    paginationParams?: PaginationParams
  ): Promise<
    { items: RoleModel[]; count: number; pageSize: number; pageNumber: number } | undefined
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
        `${import.meta.env.VITE_API_URL}/odata/roller?${queryParams}`,
        {
          params: {
            $count: true,
            tenantId: tenantId,
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
  async claimsGet(
    tenantId: string | undefined,
    paginationParams?: PaginationParams
  ): Promise<
    | {
        items: { label: string; value: string }[];
        count: number;
        pageSize: number;
        pageNumber: number;
      }
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
        `${import.meta.env.VITE_API_URL}/odata/claims?${queryParams}`,
        {
          params: {
            $count: true,
            tenantId: tenantId,
          },
        }
      );
      console.log(response);
      return {
        items: response.data,
        count: response.data["@odata.count"],
        pageSize: paginationParams?.pageSize ?? 0,
        pageNumber: paginationParams?.pageNumber ?? 0,
      };
    } catch (error) {
      console.error(error);
    }
  }

  async rolCreate(request: RoleCreateCommand): Promise<string> {
    const response = await api.post(`${import.meta.env.VITE_API_URL}/role/create`, request);
    console.log(response);
    if (response.status == 200) {
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data.data;
    }
    return response.data;
  }

  async rolUpdate(id: string, request: PozisyonCreateRequest): Promise<string> {
    request.id = id;
    const response = await api.put(`${import.meta.env.VITE_API_URL}/pozisyonlar/update/`, request);
    useToastStore().addToast(response.data.data, "", "success", 5000, true);

    return response.data.data;
  }

  async rolDelete(id: string): Promise<string> {
    const response = await api.delete(`${import.meta.env.VITE_API_URL}/role/delete/${id}`);
    useToastStore().addToast(response.data.data, "", "success", 5000, true);

    return response.data.data;
  }
}

export default new RoleService();
