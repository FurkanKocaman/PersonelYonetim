import type { EntityDto } from "../entity-models/EntityDto";

export interface RoleModel extends EntityDto {
  ad: string;
  yapisalRolMu: boolean;
  aciklama: string | undefined;
}
