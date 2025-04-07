import type { EntityDto } from "./EntityDto";

export interface DuyuruModel extends EntityDto {
  baslik: string;
  aciklama?: string;
  sirketId?: string;
  sirketAd?: string;
  aliciTipi: string;
}
