import type { EntityDto } from "../entity-models/EntityDto";

export interface KurumsalBirimGetModel extends EntityDto {
  ad: string;
  logoUrl: string | undefined;
  kod: string | undefined;
  birimTipiId: string;
  ustBirimId: string | undefined;
  birimTipiAd: string;
  personelCount: number;
}
