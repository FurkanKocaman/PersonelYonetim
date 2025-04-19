import type { EntityDto } from "../entity-models/EntityDto";

export interface KurumsalBirimGetModel extends EntityDto {
  ad: string;
  kod: string | undefined;
  birimTipiId: string;
  birimTipiAd: string;
  personelCount: number;
}
