import type { EntityDto } from "./EntityDto";

export interface KurumsalBirimModel extends EntityDto {
  ad: string;
  kod: string | undefined;
  logoUrl: string | undefined;
  birimTipiId: string;
  ustBirimId: string | undefined;
}
