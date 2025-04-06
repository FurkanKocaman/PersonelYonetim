// İzin veri modelleri

/**
 * İzin kaydı için veri modeli
 */
export interface IzinItem {
  id: number;
  personelAdi: string;
  izinTipi: string;
  baslangicTarihi: string;
  bitisTarihi: string;
  gun: number;
  aciklama: string;
  durum: string;
}

/**
 * İzin oluşturma/güncelleme isteği için veri modeli
 */
export interface IzinRequest {
  personelAdi: string;
  izinTipi: string;
  baslangicTarihi: string;
  bitisTarihi: string;
  gun: number;
  aciklama: string;
  durum: string;
}

/**
 * İzin listesi yanıtı için sayfalama parametreleri
 */
export interface IzinPaginationParams {
  sayfa: number;
  sayfaBoyutu: number;
  siralamaAlani?: string;
  siralama?: "asc" | "desc";
}

/**
 * İzin listesi yanıtı için veri modeli
 */
export interface IzinListResponse {
  items: IzinItem[];
  toplamSayfa: number;
  mevcutSayfa: number;
  toplamKayit: number;
}

/**
 * İzin tipi için veri modeli
 */
export interface IzinTipi {
  id: number;
  ad: string;
  maksimumGun: number;
}
