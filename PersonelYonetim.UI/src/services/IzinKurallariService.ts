import api from "@/utils/api";

// İzin Kuralları için veri modelleri
export interface IzinKurali {
  id: number;
  name: string;
  description: string;
  code?: string;
  active: boolean;
  paid: boolean;
  entitlement: string;
  frequency: string;
  minDays: number;
  maxDays: number;
  days: number;
}

export interface IzinKurallariResponse {
  items: IzinKurali[];
  totalCount: number;
}

// Örnek izin kuralları verileri
export const mockIzinKurallari: IzinKurali[] = [
  {
    id: 1,
    name: 'Askerlik İzni',
    description: 'Askerlik nedeniyle çalışanların kullandığı izin türüdür.',
    code: 'ASK001',
    active: true,
    paid: true,
    entitlement: 'Her talep için 21 gün',
    frequency: '—',
    minDays: 0,
    maxDays: 0,
    days: 21
  },
  {
    id: 2,
    name: 'Babalık İzni',
    description: 'Yeni baba olan çalışanların kullandığı izin türüdür.',
    active: true,
    paid: true,
    entitlement: 'Her talep için 5 gün',
    frequency: '—',
    minDays: 0,
    maxDays: 0,
    days: 5
  },
  {
    id: 3,
    name: 'Doğum İzni',
    description: 'Doğum yapan çalışanların kullandığı izin türüdür.',
    active: true,
    paid: true,
    entitlement: 'Her talep için 112 gün',
    frequency: '—',
    minDays: 0,
    maxDays: 0,
    days: 112
  },
  {
    id: 4,
    name: 'Doğum Sonrası İzni',
    description: 'Doğum sonrası çalışanların kullandığı izin türüdür.',
    active: true,
    paid: false,
    entitlement: 'Her talep için 180 gün',
    frequency: '—',
    minDays: 0,
    maxDays: 0,
    days: 180
  },
  {
    id: 5,
    name: 'Evlilik İzni',
    description: 'Evlenen çalışanların kullandığı izin türüdür.',
    active: true,
    paid: true,
    entitlement: 'Her talep için 3 gün',
    frequency: '—',
    minDays: 0,
    maxDays: 0,
    days: 3
  }
];

// İzin kuralları için servis
class IzinKurallariService {
  /**
   * İzin kuralları listesini getirir
   */
  async getIzinKurallari(): Promise<IzinKurallariResponse> {
    try {
      // API'den verileri al
      const response = await api.get(`${import.meta.env.VITE_API_URL}/izin/kurallar`);
      return response.data;
    } catch (error) {
      console.error("İzin kuralları yüklenirken hata oluştu:", error);
      // Hata durumunda örnek verileri döndür
      return {
        items: mockIzinKurallari,
        totalCount: mockIzinKurallari.length
      };
    }
  }

  /**
   * Belirli bir izin kuralını ID'ye göre getirir
   */
  async getIzinKuraliById(id: number): Promise<IzinKurali> {
    try {
      // API'den veriyi al
      const response = await api.get(`${import.meta.env.VITE_API_URL}/izin/kurallar/${id}`);
      return response.data;
    } catch (error) {
      console.error(`ID: ${id} olan izin kuralı yüklenirken hata oluştu:`, error);
      // Hata durumunda örnek verilerden ilgili kuralı bul
      const mockKural = mockIzinKurallari.find(kural => kural.id === id);
      if (mockKural) {
        return mockKural;
      }
      // Bulunamazsa hata fırlat
      throw new Error(`ID: ${id} olan izin kuralı bulunamadı`);
    }
  }

  /**
   * Yeni bir izin kuralı oluşturur
   */
  async createIzinKurali(izinKurali: Omit<IzinKurali, 'id'>): Promise<IzinKurali> {
    try {
      // API'ye veriyi gönder
      const response = await api.post(`${import.meta.env.VITE_API_URL}/izin/kurallar`, izinKurali);
      return response.data;
    } catch (error) {
      console.error("İzin kuralı oluşturulurken hata oluştu:", error);
      // Hata durumunda örnek bir yanıt oluştur
      return {
        id: Math.max(...mockIzinKurallari.map(k => k.id)) + 1,
        ...izinKurali
      };
    }
  }

  /**
   * Var olan bir izin kuralını günceller
   */
  async updateIzinKurali(id: number, izinKurali: Partial<IzinKurali>): Promise<IzinKurali> {
    try {
      // API'ye veriyi gönder
      const response = await api.put(`${import.meta.env.VITE_API_URL}/izin/kurallar/${id}`, izinKurali);
      return response.data;
    } catch (error) {
      console.error(`ID: ${id} olan izin kuralı güncellenirken hata oluştu:`, error);
      // Hata durumunda örnek bir yanıt oluştur
      const mockKural = mockIzinKurallari.find(kural => kural.id === id);
      if (mockKural) {
        return { ...mockKural, ...izinKurali };
      }
      // Bulunamazsa hata fırlat
      throw new Error(`ID: ${id} olan izin kuralı bulunamadı`);
    }
  }

  /**
   * Bir izin kuralını siler
   */
  async deleteIzinKurali(id: number): Promise<void> {
    try {
      // API'ye silme isteği gönder
      await api.delete(`${import.meta.env.VITE_API_URL}/izin/kurallar/${id}`);
    } catch (error) {
      console.error(`ID: ${id} olan izin kuralı silinirken hata oluştu:`, error);
      // Hata durumunda sadece loglama yap
    }
  }

  /**
   * İzin kuralı şablonlarını getirir
   */
  async getIzinSablonlari(): Promise<IzinKurallariResponse> {
    try {
      const response = await api.get(`${import.meta.env.VITE_API_URL}/izin/sablonlar`);
      return response.data;
    } catch (error) {
      console.error("İzin şablonları yüklenirken hata oluştu:", error);
      // Hata durumunda örnek verileri döndür
      return {
        items: mockIzinKurallari,
        totalCount: mockIzinKurallari.length
      };
    }
  }
}

export default new IzinKurallariService();
