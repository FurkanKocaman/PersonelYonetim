import type { LoginRequest } from "@/models/LoginRequest";
import axios from "axios";
import type { LoginResponse } from "@/models/LoginResponse";

class AuthService {
  async login(data: LoginRequest): Promise<string> {
    try {
      console.log(data);
      const response = await axios.post("https://localhost:7063/auth/login", data);
      const loginResponse: LoginResponse = response.data.data;
      localStorage.setItem("accessToken", loginResponse.accessToken);
      localStorage.setItem("refreshToken", loginResponse.refreshToken);
      return "Giriş başarılı";
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.error("Error while logging in", error);
        if (error.response) {
          if (error.status === 401) {
            return "UnAuthorized";
          }
          return "Kullanıcı adı veya şifre hatalı";
        }
        return "Sunucuya bağlanılırken hata oluştu";
      }
      return "Beklenmeyen bir hata oluştu";
    }
  }
  isAuthenticated = (): boolean => {
    return localStorage.getItem("accessToken") !== null;
  };
}

export default new AuthService();
