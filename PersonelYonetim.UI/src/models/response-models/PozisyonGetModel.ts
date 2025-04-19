import type { EntityDto } from "../entity-models/EntityDto";

export interface PozisyonGetModel extends EntityDto {
  ad: string;
  kod: string | undefined;
  aciklama: string | undefined;
}
