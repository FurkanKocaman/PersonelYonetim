import type { Adres } from "./entity-models/Adres";
import type { EntityDto } from "./entity-models/EntityDto";
import type { Iletisim } from "./entity-models/Iletisim";

export interface PersonelItem extends EntityDto {
  personelGorevlendirmeId: string | undefined;
  ad: string;
  soyad: string;
  dogumTarihi: Date;
  cinsiyet: string | undefined;
  avatarUrl: string | undefined;
  iletisim: Iletisim;
  adres: Adres;

  iseGirisTarihi: Date;
  istenCikisTarihi: Date | undefined;
  pozisyonBaslangicTarihi: Date;
  pozisyonBitisTarihi: Date | undefined;

  raporlananGorevlendirmeId: string | undefined;
  yoneticiAd: string | undefined;
  yoneticiPozisyon: string | undefined;

  kurumsalBirimId: string | undefined;
  kurumsalBirimAd: string | undefined;

  pozisyonId: string | undefined;
  pozisyonAd: string | undefined;

  brutUcret: number;

  calismaTakvimiId: string;

  gorevlendirmeTipiValue: number | undefined;
  calismaSekliValue: number | undefined;

  roller: string[] | undefined;

  roleClaims: string[];
}
