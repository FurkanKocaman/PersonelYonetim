// Personel veri modelleri

/**
 * Personel kaydı için veri modeli
 */
export interface PersonelItem {
  id: number;
  ad: string;
  soyad: string;
  departman: string;
  pozisyon: string;
  iseGirisTarihi: string;
  email: string;
  telefon: string;
  adres: string;
  durum: string;
}

/**
 * Personel oluşturma/güncelleme isteği için veri modeli
 */
export interface PersonelRequest {
  ad: string;
  soyad: string;
  departman: string;
  pozisyon: string;
  iseGirisTarihi: string;
  email: string;
  telefon: string;
  adres: string;
  durum: string;
}

/**
 * Personel listesi yanıtı için sayfalama parametreleri
 */
export interface PersonelPaginationParams {
  sayfa: number;
  sayfaBoyutu: number;
  siralamaAlani?: string;
  siralama?: 'asc' | 'desc';
}

/**
 * Personel listesi yanıtı için veri modeli
 */
export interface PersonelListResponse {
  items: PersonelItem[];
  toplamSayfa: number;
  mevcutSayfa: number;
  toplamKayit: number;
}
