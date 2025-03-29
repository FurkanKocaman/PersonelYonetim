import type { EntityDto } from "../EntityDto";

export interface IzinTalep extends EntityDto {
  personelId: string;
  baslangicTarihi: Date;
  bitisTarihi: Date;
  mesaiBaslangicTarihi: Date;
  toplamSure: number;
  izinTurId: string;
  aciklama?: string;
  degerlendirmeDurumu: DegerlendirmeDurumEnum;
  degerlendirenId?: string;
  degerlendirilmeTarihi?: Date;
}

export enum DegerlendirmeDurumEnum {
  Onaylandi = 0,
  Reddedildi = 1,
  Beklemede = 2,
}
