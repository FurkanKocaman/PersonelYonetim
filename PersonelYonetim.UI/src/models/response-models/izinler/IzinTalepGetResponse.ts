import type { EntityDto } from "@/models/entity-models/EntityDto";

export interface IzinTalepGetResponse extends EntityDto {
  personelId: string;
  personelFullName: string;
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
  rol?: string;
  sira: number;
  durum: string;
}
