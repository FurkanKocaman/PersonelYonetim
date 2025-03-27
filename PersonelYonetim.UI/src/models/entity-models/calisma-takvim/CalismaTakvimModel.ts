import type { EntityDto } from "../EntityDto";

export interface CalismaTakvimModel extends EntityDto {
  ad: string;
  aciklama: string | undefined;
  gunler: CalismaGunModel[];
}

export interface CalismaGunModel {
  id: string;
  gun: string; // DayOfWeek karşılığı için number (0 = Pazar, 6 = Cumartesi)
  calismaBaslangic?: string;
  calismaBitis?: string;
  molaBaslangic?: string;
  molaBitis?: string;
  isCalismaGunu: boolean;
}
