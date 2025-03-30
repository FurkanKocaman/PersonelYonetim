// Maaş veri modelleri

/**
 * Maaş kaydı için veri modeli
 */
export interface MaasItem {
  id: number;
  personelAdi: string;
  departman: string;
  maas: number;
  odenmeTarihi: string;
  durum: string;
}

/**
 * Maaş oluşturma/güncelleme isteği için veri modeli
 */
export interface MaasRequest {
  personelAdi: string;
  departman: string;
  maas: number;
  odenmeTarihi: string;
  durum: string;
}

/**
 * Maaş listesi yanıtı için sayfalama parametreleri
 */
export interface MaasPaginationParams {
  sayfa: number;
  sayfaBoyutu: number;
  siralamaAlani?: string;
  siralama?: 'asc' | 'desc';
}

/**
 * Maaş listesi yanıtı için veri modeli
 */
export interface MaasListResponse {
  items: MaasItem[];
  toplamSayfa: number;
  mevcutSayfa: number;
  toplamKayit: number;
}
