import { defineStore } from "pinia";
import { reactive } from "vue";
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
    const response = await AuthService.getCurrentUser();
    Object.assign(user, response);
  };

  const logout = () => {
    Object.assign(user, {});

    localStorage.removeItem("token");
  };

  return { user, getUser, logout };
});
