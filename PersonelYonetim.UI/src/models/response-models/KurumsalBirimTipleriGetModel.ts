import type { EntityDto } from "../entity-models/EntityDto";
import type { KurumsalBirimGetModel } from "./KurumsalBirimGetModel";

export interface KurumsalBirimTipiGetModel extends EntityDto {
  ad: string;
  aciklama: string | undefined;
  hiyerarsiSeviyesi: number;
  yoneticisiOlabilirMi: boolean;
  kurumsalBirimler: KurumsalBirimGetModel[];
  birimCount: number;
}
