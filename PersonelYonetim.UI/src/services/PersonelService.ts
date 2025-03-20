import type {
  PersonelItem,
  // PersonelRequest,
  PersonelListResponse,
  PersonelPaginationParams,
} from "@/models/PersonelModels";
import api from "./Axios";

class PersonelService {
  async getPersonelList(params?: PersonelPaginationParams): Promise<PersonelListResponse> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/odata/personeller`, {
        params: {
          $count: true,
        },
      });

      const personelList: PersonelItem[] = response.data.value;

      console.log(response.data.value);

      // Mock veri
      // const mockPersonelList: PersonelItem[] = [
      //   {
      //     id: 1,
      //     ad: "Ahmet",
      //     soyad: "Yılmaz",
      //     departman: "Bilgi İşlem",
      //     pozisyon: "Yazılım Geliştirici",
      //     iseGirisTarihi: "2020-01-15",
      //     email: "ahmet.yilmaz@sirket.com",
      //     telefon: "555-123-4567",
      //     adres: "Ankara, Çankaya",
      //     durum: "Aktif",
      //   },
      //   {
      //     id: 2,
      //     ad: "Ayşe",
      //     soyad: "Demir",
      //     departman: "İnsan Kaynakları",
      //     pozisyon: "İK Uzmanı",
      //     iseGirisTarihi: "2019-05-20",
      //     email: "ayse.demir@sirket.com",
      //     telefon: "555-234-5678",
      //     adres: "İstanbul, Kadıköy",
      //     durum: "Aktif",
      //   },
      //   {
      //     id: 3,
      //     ad: "Mehmet",
      //     soyad: "Kaya",
      //     departman: "Muhasebe",
      //     pozisyon: "Muhasebe Müdürü",
      //     iseGirisTarihi: "2018-03-10",
      //     email: "mehmet.kaya@sirket.com",
      //     telefon: "555-345-6789",
      //     adres: "İzmir, Konak",
      //     durum: "Aktif",
      //   },
      //   {
      //     id: 4,
      //     ad: "Zeynep",
      //     soyad: "Çelik",
      //     departman: "Pazarlama",
      //     pozisyon: "Pazarlama Uzmanı",
      //     iseGirisTarihi: "2021-02-01",
      //     email: "zeynep.celik@sirket.com",
      //     telefon: "555-456-7890",
      //     adres: "Bursa, Nilüfer",
      //     durum: "Aktif",
      //   },
      //   {
      //     id: 5,
      //     ad: "Ali",
      //     soyad: "Öztürk",
      //     departman: "Satış",
      //     pozisyon: "Satış Temsilcisi",
      //     iseGirisTarihi: "2020-07-15",
      //     email: "ali.ozturk@sirket.com",
      //     telefon: "555-567-8901",
      //     adres: "Antalya, Muratpaşa",
      //     durum: "İzinli",
      //   },
      // ];

      return {
        items: personelList,
        toplamSayfa: 1,
        mevcutSayfa: 1,
        toplamKayit: personelList.length,
      };
    } catch (error) {
      console.error("Personel listesi alınırken hata oluştu:", error);
      throw error;
    }
  }

  /**
   * Belirli bir personelin detaylarını ID'ye göre getirir
   * @param id Personel ID'si
   * @returns Personel detayları
   */
  async getPersonelById(id: number): Promise<PersonelItem> {
    try {
      // Gerçek API çağrısı:
      // const response = await axios.get(`${this.baseUrl}/${id}`);
      // return response.data;

      // Mock veri
      const mockPersonelList: PersonelItem[] = [];

      const personel = mockPersonelList.find((p) => p.id === id);

      if (!personel) {
        throw new Error("Personel bulunamadı");
      }

      return personel;
    } catch (error) {
      console.error(`ID: ${id} olan personel alınırken hata oluştu:`, error);
      throw error;
    }
  }

  /**
   * Yeni bir personel kaydı oluşturur
   * @param personelData Personel verisi
   * @returns Oluşturulan personel kaydı
   */
  // async createPersonel(personelData: PersonelRequest): Promise<PersonelItem> {
  //   try {
  //     // Gerçek API çağrısı:
  //     // const response = await axios.post(this.baseUrl, personelData);
  //     // return response.data;

  //     // Mock yanıt
  //     return {
  //       id: Math.floor(Math.random() * 1000) + 6,
  //       ...personelData,
  //     };
  //   } catch (error) {
  //     console.error("Personel kaydı oluşturulurken hata oluştu:", error);
  //     throw error;
  //   }
  // }

  /**
   * Mevcut bir personel kaydını günceller
   * @param id Personel ID'si
   * @param personelData Güncellenmiş personel verisi
   * @returns Güncellenmiş personel kaydı
   */
  // async updatePersonel(id: number, personelData: PersonelRequest): Promise<PersonelItem> {
  //   try {
  //     // Gerçek API çağrısı:
  //     // const response = await axios.put(`${this.baseUrl}/${id}`, personelData);
  //     // return response.data;

  //     // Mock yanıt
  //     return {
  //       id,
  //       ...personelData,
  //     };
  //   } catch (error) {
  //     console.error(`ID: ${id} olan personel güncellenirken hata oluştu:`, error);
  //     throw error;
  //   }
  // }

  /**
   * Bir personel kaydını siler
   * @param id Silinecek personel kaydının ID'si
   * @returns İşlem başarılı olup olmadığını belirten yanıt
   */
  async deletePersonel(id: number): Promise<{ success: boolean }> {
    try {
      // Gerçek API çağrısı:
      // await axios.delete(`${this.baseUrl}/${id}`);

      // Mock yanıt
      return { success: true };
    } catch (error) {
      console.error(`ID: ${id} olan personel silinirken hata oluştu:`, error);
      throw error;
    }
  }

  /**
   * Departman listesini döndürür
   * @returns Departman listesi
   */
  getDepartmanList(): string[] {
    return [
      "Bilgi İşlem",
      "İnsan Kaynakları",
      "Muhasebe",
      "Pazarlama",
      "Satış",
      "Üretim",
      "Lojistik",
      "Yönetim",
      "Ar-Ge",
    ];
  }

  /**
   * Personel durumu listesini döndürür
   * @returns Durum listesi
   */
  getDurumList(): string[] {
    return ["Aktif", "Pasif", "İzinli", "İşten Ayrılmış"];
  }
}

export default new PersonelService();
