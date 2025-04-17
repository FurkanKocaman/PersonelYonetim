import api from "./Axios";
import type { IzinKuralModel, IzinTurResponse } from "@/models/entity-models/izin/IzinKuralModel";
import type { IzinTalepCreateCommand } from "@/models/request-models/IzinTalepCreateCommand";
import type { PaginationParams } from "@/models/request-models/PaginationParams";
import type { IzinTalepGetResponse } from "@/models/response-models/izinler/IzinTalepGetResponse";
import { useToastStore } from "@/stores/ToastStore";
import type { IzinlerKalanResponse } from "@/models/response-models/izinler/İzinlerKalanResponse";

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
    { items: IzinTurResponse[]; count: number; pageSize: number; pageNumber: number } | undefined
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
        `${import.meta.env.VITE_API_URL}/odata/izin-talepler-onay-bekleyenler?${queryParams}`,
        {
          params: {
            $count: true,
          },
        }
      );
      console.log(response.data);
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
      useToastStore().addToast(response.data.data, "", "success", 5000, true);
      return response.data;
    } catch (error) {
      useToastStore().addToast(error.response.data.errorMessages[0], "", "error", 5000, true);
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

  async getIzinlerKalan(): Promise<IzinlerKalanResponse | undefined> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/getkalanizinler`);

      return response.data;
    } catch (error) {
      console.error(error);
    }
  }
}
export default new IzinService();
