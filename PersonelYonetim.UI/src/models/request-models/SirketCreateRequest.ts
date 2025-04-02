import type { Adres } from "../entity-models/Adres";
import type { Iletisim } from "../entity-models/Iletisim";

export interface SirketCreateRequest {
  id?: string;
  ad: string;
  aciklama: string | null;
  logoUrl: string | null;
  adres: Adres;
  iletisim: Iletisim;
}
