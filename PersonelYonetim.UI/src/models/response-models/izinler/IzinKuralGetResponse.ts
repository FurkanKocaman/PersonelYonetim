import type { EntityDto } from "@/models/entity-models/EntityDto";

export interface IzinKuralGetResponse extends EntityDto {
  ad: string;
  aciklama: string | undefined;
  personelCount: number;
  izinTurler: IzinTurDto[];
}

export interface IzinTurDto {
  id: string;
  ad: string;
  aciklama: string | undefined;
  ucretliMi: boolean;
  limitTipiName: string;
  limitGunSayisi: number;
}
