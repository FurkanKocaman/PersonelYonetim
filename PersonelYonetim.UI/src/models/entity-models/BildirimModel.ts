import type { EntityDto } from "./EntityDto";

export interface BildirimModel extends EntityDto {
  baslik: string;
  aciklama: string;
  bildirimTipi: string;
  aliciTipi: string;
  url?: string | null;
  okunduMu: boolean;
  okunduTarihi?: Date | null;
}
