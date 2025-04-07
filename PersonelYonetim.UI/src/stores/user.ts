import { defineStore } from "pinia";
import { reactive } from "vue";
import router from "@/router";
import AuthService from "@/services/AuthService";
import type { PersonelItem } from "@/models/PersonelModels";

export const useUserStore = defineStore("user", () => {
  const user: PersonelItem = reactive({
    id: "",
    ad: "",
    soyad: "",
    fullName: "",
    dogumTarihi: new Date(),
    cinsiyet: undefined,
    profilResimUrl: undefined,
    iletisim: {
      eposta: "",
      telefon: "",
    },
    adres: {
      ulke: "",
      sehir: "",
      ilce: "",
      tamAdres: "",
    },
    yonetici: undefined,
    yoneticiPozisyon: undefined,
    sirketId: "",
    sirketAd: "",
    subeId: undefined,
    subeAd: undefined,
    departmanId: undefined,
    departmanAd: undefined,
    pozisyonId: undefined,
    pozisyonAd: undefined,
    calismaTakvimiId: undefined,
    sozlesmeTuruValue: 0,
    pozisyonBaslangicTarih: new Date(),
    sozlesmeBitisTarihi: undefined,
    izinKuralId: undefined,
    role: 0,
    isActive: true,
    createdAt: new Date(),
    createUserId: "",
    createUserName: undefined,
    updateAt: undefined,
    updateUserId: undefined,
    updateUserName: undefined,
    isDeleted: false,
    deleteAt: undefined,
  });

  const getUser = async () => {
    try {
      const response = await AuthService.getCurrentUser();
      Object.assign(user, response);
    } catch (error) {
      console.error(error);
      router.push({ name: "login" });
    }
  };

  const logout = () => {
    Object.assign(user, {});

    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    router.push({ name: "login" });
  };

  return { user, getUser, logout };
});
