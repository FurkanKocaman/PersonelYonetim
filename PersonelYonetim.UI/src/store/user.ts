import { defineStore } from "pinia";
import { type UserModel } from "@/models/UserModel";
// import api from "@/services/Axios";

export const useUserStore = defineStore("user", {
  state: () => ({
    user: null as UserModel | null,
  }),
  actions: {
    async getUser() {
      this.user = {
        id: "123456",
        fullName: "Furkan Kocaman",
        dogumTarihi: new Date(),
        cinsiyet: true,
        profilResimUrl: null,
        departmanAd: "Yazılım",
        pozisyonAd: "Yönetici",
        eposta: "furkan61kcmn@gmail.com",
        telefon: "0123",
        ulke: "Türkiye",
        sehir: "Trabzon",
        ilce: "Çarşıbaşı",
        tamAdres: "Trabzon/Çarşıbaşı",
      };
      // const accessToken = localStorage.getItem("accessToken");
      // if (accessToken) {
      //   try {
      //     const response = await api.get(`${import.meta.env.VITE_API_URL}/getUser`);
      //     this.user = response.data;
      //   } catch (error) {
      //     console.error(error);
      //   }
      // }
    },

    logOut() {
      localStorage.removeItem("accessToken");
      this.user = null;
    },
  },
  getters: {
    isAuthenticated: (state) => !!state.user,

    getUser: (state) => state.user,
  },
});
