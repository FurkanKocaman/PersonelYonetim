import type { Adres } from "../entity-models/Adres";
import type { Iletisim } from "../entity-models/Iletisim";

export interface PersonelCreateRequest {
  ad: string;
  soyad: string;
  dogumTarihi: Date;
  cinsiyet: boolean | undefined;
  profilResimUrl: string | undefined;
  iletisim: Iletisim;
  adres: Adres;
  yoneticiId: string | undefined;
  sirketId: string;
  subeId: string | undefined;
  departmanId: string | undefined;
  pozisyonId: string | undefined;
  calismaTakvimiId: string | undefined;
  sozlesmeTuruValue: number;
  pozisyonBaslangicTarih: Date;
  sozlesmeBitisTarihi: Date | undefined;
  izinKuralId: string | undefined;
  rolValue: number;
}
