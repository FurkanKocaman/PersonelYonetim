// Personel veri modelleri

import type { Adres } from "./entity-models/Adres";
import type { EntityDto } from "./entity-models/EntityDto";
import type { Iletisim } from "./entity-models/Iletisim";

/**
 * Personel kaydı için veri modeli
 */
export interface PersonelItem extends EntityDto {
  ad: string;
  soyad: string;
  fullName: string;
  dogumTarihi: Date;
  cinsiyet: boolean | undefined;
  profilResimUrl: string | undefined;
  iletisim: Iletisim;
  adres: Adres;
  yoneticiId: string | undefined;
  sirketId: string;
  sirketAd: string;
  subeId: string | undefined;
  subeAd: string | undefined;
  departmanId: string | undefined;
  departmanAd: string | undefined;
  pozisyonId: string | undefined;
  pozisyonAd: string | undefined;
  calismaTakvimiId: string | undefined;
  sozlesmeTuruValue: number;
  pozisyonBaslangicTarih: Date;
  sozlesmeBitisTarihi: Date | undefined;
  izinKuralId: string | undefined;
  rolValue: number;
  rolAd: string;
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
  siralama?: "asc" | "desc";
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
