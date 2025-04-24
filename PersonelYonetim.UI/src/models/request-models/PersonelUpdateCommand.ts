import type { Adres } from "../entity-models/Adres";
import type { Iletisim } from "../entity-models/Iletisim";

export interface PersonelUpdateCommand {
  id: string;
  ad: string;
  soyad: string;
  dogumTarihi: Date;
  cinsiyet?: boolean;
  profilResimUrl?: string;
  iletisim: Iletisim;
  adres: Adres;

  kurumsalBirimId?: string;
  pozisyonId?: string;
  roleIdler?: string[];
  baslangicTarihi: string;
  bitisTarihi?: string;
  birincilGorevMi: boolean;
  gorevlendirmeTipiValue: number;
  calismaSekliValue: number;
  raporlananPersonelId?: string;
  izinKuralId?: string;
  calismaTakvimId?: string;
  brutUcret: number;
  tabiOlduguKanun?: string;
  sgkIsYeri?: string;
  vergiDairesiAdi?: string;
  meslekKodu?: string;
}
