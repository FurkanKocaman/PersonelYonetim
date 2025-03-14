import type { MaasItem, MaasListResponse, MaasRequest, MaasPaginationParams } from '@/models/MaasModels';
import axios from 'axios';

export class MaasService {
  private baseUrl = '/api/payroll';
  
  /**
   * Maaş listesini getirir
   * @param params Sayfalama parametreleri
   * @returns Maaş listesi yanıtı
   */
  async getMaasList(params?: MaasPaginationParams): Promise<MaasListResponse> {
    try {
      // Gerçek API çağrısı:
      // const response = await axios.get(this.baseUrl, { params });
      // return response.data;
      
      // Mock veri
      const mockMaasList: MaasItem[] = [
        {
          id: 1,
          personelAdi: 'Ahmet Yılmaz',
          departman: 'Bilgi İşlem',
          maas: 15000,
          odenmeTarihi: '2023-05-15',
          durum: 'Ödendi'
        },
        {
          id: 2,
          personelAdi: 'Ayşe Demir',
          departman: 'İnsan Kaynakları',
          maas: 12000,
          odenmeTarihi: '2023-05-15',
          durum: 'Ödendi'
        },
        {
          id: 3,
          personelAdi: 'Mehmet Kaya',
          departman: 'Muhasebe',
          maas: 13500,
          odenmeTarihi: '2023-05-15',
          durum: 'Ödendi'
        },
        {
          id: 4,
          personelAdi: 'Zeynep Çelik',
          departman: 'Pazarlama',
          maas: 14000,
          odenmeTarihi: '2023-06-15',
          durum: 'Beklemede'
        },
        {
          id: 5,
          personelAdi: 'Ali Öztürk',
          departman: 'Satış',
          maas: 16000,
          odenmeTarihi: '2023-06-15',
          durum: 'Beklemede'
        }
      ];
      
      return {
        items: mockMaasList,
        toplamSayfa: 1,
        mevcutSayfa: 1,
        toplamKayit: mockMaasList.length
      };
    } catch (error) {
      console.error('Maaş listesi alınırken hata oluştu:', error);
      throw error;
    }
  }
  
  /**
   * Belirli bir maaş kaydını ID'ye göre getirir
   * @param id Maaş kaydı ID'si
   * @returns Maaş kaydı
   */
  async getMaasById(id: number): Promise<MaasItem> {
    try {
      // Gerçek API çağrısı:
      // const response = await axios.get(`${this.baseUrl}/${id}`);
      // return response.data;
      
      // Mock veri
      const mockMaasList: MaasItem[] = [
        {
          id: 1,
          personelAdi: 'Ahmet Yılmaz',
          departman: 'Bilgi İşlem',
          maas: 15000,
          odenmeTarihi: '2023-05-15',
          durum: 'Ödendi'
        },
        {
          id: 2,
          personelAdi: 'Ayşe Demir',
          departman: 'İnsan Kaynakları',
          maas: 12000,
          odenmeTarihi: '2023-05-15',
          durum: 'Ödendi'
        }
      ];
      
      const maas = mockMaasList.find(m => m.id === id);
      
      if (!maas) {
        throw new Error('Maaş kaydı bulunamadı');
      }
      
      return maas;
    } catch (error) {
      console.error(`ID: ${id} olan maaş kaydı alınırken hata oluştu:`, error);
      throw error;
    }
  }
  
  /**
   * Yeni bir maaş kaydı oluşturur
   * @param maasData Maaş verisi
   * @returns Oluşturulan maaş kaydı
   */
  async createMaas(maasData: MaasRequest): Promise<MaasItem> {
    try {
      // Gerçek API çağrısı:
      // const response = await axios.post(this.baseUrl, maasData);
      // return response.data;
      
      // Mock yanıt
      return {
        id: Math.floor(Math.random() * 1000) + 6,
        ...maasData
      };
    } catch (error) {
      console.error('Maaş kaydı oluşturulurken hata oluştu:', error);
      throw error;
    }
  }
  
  /**
   * Mevcut bir maaş kaydını günceller
   * @param id Maaş kaydı ID'si
   * @param maasData Güncellenmiş maaş verisi
   * @returns Güncellenmiş maaş kaydı
   */
  async updateMaas(id: number, maasData: MaasRequest): Promise<MaasItem> {
    try {
      // Gerçek API çağrısı:
      // const response = await axios.put(`${this.baseUrl}/${id}`, maasData);
      // return response.data;
      
      // Mock yanıt
      return {
        id,
        ...maasData
      };
    } catch (error) {
      console.error(`ID: ${id} olan maaş kaydı güncellenirken hata oluştu:`, error);
      throw error;
    }
  }
  
  /**
   * Bir maaş kaydını siler
   * @param id Silinecek maaş kaydının ID'si
   * @returns İşlem başarılı olup olmadığını belirten yanıt
   */
  async deleteMaas(id: number): Promise<{ success: boolean }> {
    try {
      // Gerçek API çağrısı:
      // await axios.delete(`${this.baseUrl}/${id}`);
      
      // Mock yanıt
      return { success: true };
    } catch (error) {
      console.error(`ID: ${id} olan maaş kaydı silinirken hata oluştu:`, error);
      throw error;
    }
  }
  
  /**
   * Departman listesini döndürür
   * @returns Departman listesi
   */
  getDepartmanList(): string[] {
    return [
      'Bilgi İşlem',
      'İnsan Kaynakları',
      'Muhasebe',
      'Pazarlama',
      'Satış',
      'Üretim',
      'Lojistik',
      'Yönetim',
      'Ar-Ge'
    ];
  }
  
  /**
   * Maaş durumu listesini döndürür
   * @returns Durum listesi
   */
  getDurumList(): string[] {
    return [
      'Ödendi',
      'Beklemede',
      'İptal Edildi'
    ];
  }
}
