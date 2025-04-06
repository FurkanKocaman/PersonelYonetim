import axios, {
  type AxiosInstance,
  type AxiosResponse,
  type InternalAxiosRequestConfig,
} from "axios";

const api: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {},
});

api.interceptors.request.use(
  (config: InternalAxiosRequestConfig): InternalAxiosRequestConfig => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) {
      config.headers.Authorization = `Bearer ${accessToken}`;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response: AxiosResponse): AxiosResponse => {
    return response;
  },
  (error) => {
    if (error.response) {
      if (error.response.status === 401) {
        console.warn("Yetkilendirme hatası giriş yapmanız gerekiyor.");
        //Giriş sayfasına yönlendirilebilir
      } else if (error.response.status === 403) {
        console.warn("Yetkisiz giriş");
      } else if (error.response.status === 500) {
        console.warn("Server hatası");
      }
    }
    return Promise.reject(error);
  }
);

export default api;
