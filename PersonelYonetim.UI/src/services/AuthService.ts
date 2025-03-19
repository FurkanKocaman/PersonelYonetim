import type { LoginRequest } from "@/models/LoginRequest";
import axios from "axios";
import type { LoginResponse } from "@/models/LoginResponse";

class AuthService {
  async login(data: LoginRequest): Promise<{ success: boolean; message: string }> {
    try {
      console.log(data);
      const response = await axios.post(`${import.meta.env.VITE_API_URL}/auth/login`, data);
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
      console.error(error);
      return { success: false, message: "Beklenmeyen bir hata oluştu" };
    }
  }

  isAuthenticated = (): boolean => {
    //Token doğruluğu api isteğiyle kontrol edilecek
    return localStorage.getItem("accessToken") !== null;
  };
}

export default new AuthService();
