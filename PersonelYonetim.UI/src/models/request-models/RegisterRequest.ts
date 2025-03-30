import type { Adres } from "./entity-models/Adres";
import type { Iletisim } from "./entity-models/Iletisim";

export interface RegisterRequest {
  ad: string;
  soyad: string;
  dogumTarihi: Date;
  cinsiyet: boolean | undefined;
  personelIletisim: Iletisim;
  personelAdres: Adres;
  sirketAd: string;
  sirketKurulusTarihi: Date;
  sirketIletisim: Iletisim;
  sirketAdres: Adres;
}
