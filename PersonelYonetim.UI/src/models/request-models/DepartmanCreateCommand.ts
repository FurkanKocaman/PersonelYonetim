import type { Iletisim } from "../entity-models/Iletisim";

export interface DepartmanCreateRequest {
  id?: string;
  ad: string;
  aciklama: string | null;
  subeId: string;
  iletisim?: Iletisim;
}
