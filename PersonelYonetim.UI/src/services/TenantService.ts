import api from "./Axios";
import { useToastStore } from "@/stores/ToastStore";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { TenantGetModel } from "@/models/response-models/TenantGetModel";

class TenantService {
  async tenantGet(
    paginationParams?: PaginationParams
  ): Promise<{ item: TenantGetModel; count: number; pageSize: number; pageNumber: number }> {
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
        `${import.meta.env.VITE_API_URL}/odata/tenant?${queryParams}`,
        {
          params: {
            $count: true,
          },
        }
      );
      return {
        item: response.data.value[0],
        count: response.data["@odata.count"],
        pageSize: paginationParams?.pageSize ?? 0,
        pageNumber: paginationParams?.pageNumber ?? 0,
      };
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  async tenantUpdate(request: TenantGetModel): Promise<string> {
    const response = await api.put(`${import.meta.env.VITE_API_URL}/tenants/update`, request);
    console.log(response);
    if (response.status == 200) {
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data.data;
    }
    return response.data;
  }
}

export default new TenantService();
