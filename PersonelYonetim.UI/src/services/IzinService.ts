import type { IzinTurModel } from "@/models/entity-models/izin/IzinTurModel";
import api from "./Axios";
import type { IzinKuralModel } from "@/models/entity-models/izin/IzinKuralModel";
import type { IzinTalepCreateCommand } from "@/models/request-models/IzinTalepCreateCommand";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { IzinTalepGetResponse } from "@/models/response-models/izinler/IzinTalepGetResponse";
import { useToastStore } from "@/stores/ToastStore";

export class IzinService {
  async getIzinKural(
    paginationParams: PaginationParams
  ): Promise<
    { items: IzinKuralModel[]; count: number; pageSize: number; pageNumber: number } | undefined
  > {
    try {
      const { pageNumber, pageSize, orderBy, filter } = paginationParams;

      const queryParams = new URLSearchParams();
      queryParams.append("$top", pageSize.toString());
      queryParams.append("$skip", ((pageNumber - 1) * pageSize).toString());

      if (orderBy) queryParams.append("$orderby", orderBy);
      if (filter) queryParams.append("$filter", filter);

      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/izin-kurallar?${queryParams}`,
        {
          params: {
            $count: true,
            $expand: "IzinTurler",
          },
        }
      );

      return {
        items: response.data.value,
        count: response.data["@odata.count"],
        pageSize: pageSize,
        pageNumber: pageNumber,
      };
    } catch (error) {
      console.error(error);
    }
  }

  async getIzinTurler(
    paginationParams: PaginationParams
  ): Promise<
    { items: IzinTurModel[]; count: number; pageSize: number; pageNumber: number } | undefined
  > {
    try {
      const { pageNumber, pageSize, orderBy, filter } = paginationParams;

      const queryParams = new URLSearchParams();
      queryParams.append("$top", pageSize.toString());
      queryParams.append("$skip", ((pageNumber - 1) * pageSize).toString());

      if (orderBy) queryParams.append("$orderby", orderBy);
      if (filter) queryParams.append("$filter", filter);

      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/izin-turler?${queryParams}`,
        {
          params: {
            $count: true,
          },
        }
      );

      return {
        items: response.data.value,
        count: response.data["@odata.count"],
        pageSize: pageSize,
        pageNumber: pageNumber,
      };
    } catch (error) {
      console.error(error);
    }
  }

  async createIzinTalep(request: IzinTalepCreateCommand): Promise<string | undefined> {
    console.log("REUQETS", request);
    try {
      const response = await api.post(`${import.meta.env.VITE_API_URL}/izin-talep/create`, request);
      if (response.status == 200) {
        useToastStore().addToast(response.data.data, "", "success", 5000, true);
      } else {
        useToastStore().addToast(response.data.data, "", "error", 5000, true);
      }
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }

  async getIzinTalepler(
    paginationParams: PaginationParams
  ): Promise<
    | { items: IzinTalepGetResponse[]; count: number; pageSize: number; pageNumber: number }
    | undefined
  > {
    const queryParams = new URLSearchParams();
    if (paginationParams) {
      const { pageNumber, pageSize, orderBy, filter } = paginationParams;

      queryParams.append("$top", pageSize.toString());
      queryParams.append("$skip", ((pageNumber - 1) * pageSize).toString());

      if (orderBy) queryParams.append("$orderby", orderBy);
      if (filter) queryParams.append("$filter", filter);
    }

    try {
      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/izin-talepler?${queryParams}`,
        {
          params: {
            $count: true,
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
      console.error("Error", error);
    }
  }
  async izinTalepDegerlendir(id: string, onayDurum: number): Promise<string | undefined> {
    try {
      const response = await api.post(`${import.meta.env.VITE_API_URL}/izin-talep/degerlendir`, {
        id: id,
        onayDurum: onayDurum,
      });
      console.log(response);
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data;
    } catch (error) {
      console.error(error);
    }
  }

  async getPersonelIzinTalepler(
    paginationParams: PaginationParams
  ): Promise<
    | { items: IzinTalepGetResponse[]; count: number; pageSize: number; pageNumber: number }
    | undefined
  > {
    const { pageNumber, pageSize, orderBy, filter } = paginationParams;

    const queryParams = new URLSearchParams();
    queryParams.append("$top", pageSize.toString());
    queryParams.append("$skip", ((pageNumber - 1) * pageSize).toString());

    if (orderBy) queryParams.append("$orderby", orderBy);
    if (filter) queryParams.append("$filter", filter);

    try {
      const response = await api.get(
        `${import.meta.env.VITE_API_URL}/odata/personel-izin-talepler?${queryParams}`,
        {
          params: {
            $count: true,
          },
        }
      );
      console.log(response);
      return {
        items: response.data.value,
        count: response.data["@odata.count"],
        pageSize: pageSize,
        pageNumber: pageNumber,
      };
    } catch (error) {
      console.error(error);
    }
  }
}
export default new IzinService();
