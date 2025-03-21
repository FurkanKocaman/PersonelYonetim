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
    role: "",
  });

  const getUser = async () => {
    const response = await AuthService.getCurrentUser();
    Object.assign(user, response);
  };

  return { user, getUser };
});
