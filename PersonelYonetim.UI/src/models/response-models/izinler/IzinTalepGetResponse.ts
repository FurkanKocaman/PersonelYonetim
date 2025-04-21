import type { EntityDto } from "@/models/entity-models/EntityDto";

export interface IzinTalepGetResponse extends EntityDto {
  personelId: string;
  personelFullName: string;
  avatarUrl: string | undefined;
  baslangicTarihi: Date;
  bitisTarihi: Date;
  mesaiBaslangicTarihi: Date;
  toplamSure: number;
  izinTuru: string;
  aciklama?: string;
  degerlendirmeDurumu: string;
  degerlendirenId?: string;
  degerlendirenAd?: string;
  cakisanIzinTalepler: CakisanIzinTalep[];
  onayAdimlari?: OnaySureci[];
}

export interface CakisanIzinTalep {
  personelId?: string;
  personelFullName?: string;
  baslangicTarihi: Date;
  bitisTarihi: Date;
}

export interface OnaySureci {
  personelId?: string;
  personelAd?: string;
  avatarUrl?: string;
  kurumsalBirimAd: string;
  pozisyonAd: string;
  sira: number;
  onayDurum: string;
  degerlendirilmeTarihi: Date | undefined;
}
