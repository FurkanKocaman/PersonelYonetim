import { defineStore } from "pinia";
import { type UserModel } from "@/models/UserModel";
import { reactive } from "vue";
import AuthService from "@/services/AuthService";

export const useUserStore = defineStore("user", () => {
  const user: UserModel = reactive({
    id: "",
    fullName: "",
    dogumTarihi: new Date(),
    cinsiyet: undefined,
    profilResimUrl: undefined,
    departmanAd: "",
    pozisyonAd: "",
    eposta: "",
    telefon: "",
    ulke: "",
    sehir: "",
    ilce: "",
    tamAdres: "",
    role: -1,
  });

  const getUser = async () => {
    const response = await AuthService.getCurrentUser();
    Object.assign(user, response);
  };

  const logout = () => {
    // Kullanıcı bilgilerini sıfırla
    Object.assign(user, {
      id: "",
      fullName: "",
      dogumTarihi: new Date(),
      cinsiyet: undefined,
      profilResimUrl: undefined,
      departmanAd: "",
      pozisyonAd: "",
      eposta: "",
      telefon: "",
      ulke: "",
      sehir: "",
      ilce: "",
      tamAdres: "",
      role: "",
    });
    
    // Token'ı localStorage'dan kaldır
    localStorage.removeItem("token");
  };

  return { user, getUser, logout };
});
