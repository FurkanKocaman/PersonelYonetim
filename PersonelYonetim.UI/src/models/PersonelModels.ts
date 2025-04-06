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
  yonetici: string | undefined;
  yoneticiPozisyon: string | undefined;
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
  role: number;
}

export interface PersonelPaginationParams {
  sayfa: number;
  sayfaBoyutu: number;
  siralamaAlani?: string;
  siralama?: "asc" | "desc";
}

export interface PersonelListResponse {
  items: PersonelItem[];
  toplamSayfa: number;
  mevcutSayfa: number;
  toplamKayit: number;
}
