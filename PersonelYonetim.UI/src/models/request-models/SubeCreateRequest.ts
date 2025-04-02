import type { Adres } from "../entity-models/Adres";
import type { Iletisim } from "../entity-models/Iletisim";

export interface SubeCreateRequest {
  id?: string;
  ad: string;
  aciklama: string | null;
  sirketId: string;
  adres: Adres;
  iletisim: Iletisim;
}
