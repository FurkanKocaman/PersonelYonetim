import type { LoginRequest } from "@/models/request-models/LoginRequest";
import axios from "axios";
import type { LoginResponse } from "@/models/response-models/LoginResponse";
import type { RegisterRequest } from "@/models/request-models/RegisterRequest";
import api from "./Axios";
import type { PersonelItem } from "@/models/PersonelModels";
import type { PasswordResetModel } from "@/models/request-models/PasswordResetModel";

class AuthService {
  async login(data: LoginRequest): Promise<{ success: boolean; message: string }> {
    try {
      console.log(data);
      const response = await axios.post(`${import.meta.env.VITE_API_URL}/auth/login`, data);
      console.log(response);
      const loginResponse: LoginResponse = response.data.data;

      localStorage.setItem("accessToken", loginResponse.accessToken);
      localStorage.setItem("refreshToken", loginResponse.refreshToken);

      return { success: true, message: "Giriş başarılı" };
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.error("Error while logging in", error);
        if (error.response) {
          if (error.response.status === 401) {
            return { success: false, message: "UnAuthorized" };
          }
          return { success: false, message: "Kullanıcı adı veya şifre hatalı" };
        }
        return { success: false, message: "Sunucuya bağlanılırken hata oluştu" };
      }
      return { success: false, message: "Beklenmeyen bir hata oluştu" };
    }
  }
  async register(data: RegisterRequest): Promise<{ success: boolean; message: string }> {
    try {
      console.log(data);
      const response = await axios.post(`${import.meta.env.VITE_API_URL}/auth/register`, data);
      const loginResponse: LoginResponse = response.data.data;

      localStorage.setItem("accessToken", loginResponse.accessToken);
      localStorage.setItem("refreshToken", loginResponse.refreshToken);

      return { success: true, message: "Giriş başarılı" };
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.error("Error while logging in", error);
        if (error.response) {
          if (error.response.status === 401) {
            return { success: false, message: "UnAuthorized" };
          }
          return { success: false, message: "Kullanıcı adı veya şifre hatalı" };
        }
        return { success: false, message: "Sunucuya bağlanılırken hata oluştu" };
      }
      return { success: false, message: "Beklenmeyen bir hata oluştu" };
    }
  }

  async getCurrentUser() {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/personel-current`);

      const User: PersonelItem = response.data[0];
      User.role = Number(User.role);
      return User;
    } catch (error) {
      return error;
    }
  }

  async SendPasswordResetMail(
    email: string
  ): Promise<
    { data: string; errorMessages: string[]; isSuccessful: boolean; statusCode: number } | undefined
  > {
    try {
      const res = await axios.post(
        `${import.meta.env.VITE_API_URL}/auth/send-reset-password-token`,
        { email: email }
      );

      return res.data;
    } catch (error) {
      return error.response.data;
    }
  }

  async ResetPassword(
    request: PasswordResetModel
  ): Promise<
    { data: string; errorMessages: string[]; isSuccessful: boolean; statusCode: number } | undefined
  > {
    try {
      console.log("REQ", request);
      const response = await axios.post(
        `${import.meta.env.VITE_API_URL}/auth/reset-password`,
        request
      );

      return response.data;
    } catch (error) {
      return error.response.data;
    }
  }

  isAuthenticated = (): boolean => {
    //Token doğruluğu api isteğiyle kontrol edilecek
    return localStorage.getItem("accessToken") !== null;
  };
}

export default new AuthService();
