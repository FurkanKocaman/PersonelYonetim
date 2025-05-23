import type { Adres } from "../entity-models/Adres";
import type { Iletisim } from "../entity-models/Iletisim";

export interface PersonelCreateCommand {
  ad: string;
  soyad: string;
  dogumTarihi: Date | undefined;
  cinsiyet: boolean | undefined;
  avatarUrl: string | undefined;
  iletisim: Iletisim;
  adres: Adres | undefined;

  kurumsalBirimId: string | undefined;
  pozisyonId: string | undefined;
  roleId: string[] | undefined;

  iseGirisTarihi: Date;
  istenCikisTarihi: Date | undefined;
  pozisyonBaslangicTarihi: Date;
  pozisyonBitisTarihi: Date | undefined;

  birincilGorevMi: boolean;
  gorevlendirmeTipiValue: number;
  calismaSekliValue: number;
  raporlananGorevlendirmeId: string | undefined;
  izinKuralId: string | undefined;
  calismaTakvimiId: string | undefined;
  brutUcret: number | undefined;
  tenantId: string | undefined;
}
