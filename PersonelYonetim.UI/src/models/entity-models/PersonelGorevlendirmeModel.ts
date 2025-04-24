import type { EntityDto } from "./EntityDto";

export interface PersonelGorevlendirmeModel extends EntityDto {
  personelId: string;
  personelFullName: string;

  kurumsalBirimId?: string | null;
  kurumsalBirimAd?: string | null;

  pozisyonId?: string | null;
  pozisyonAd?: string | null;

  iseGirisTarihi: string;
  istenCikisTarihi?: string | null;

  pozisyonBaslangicTarihi: string;
  pozisyonBitisTarihi?: string | null;

  birincilGorevMi: string;
  gorevlendirmeTipiAd?: string | null;
  calismaSekliAd?: string | null;

  gorevlendirmeRolleriAd: (string | null)[];
  izinKuralAd?: string | null;

  raporlananPersonelId?: string | null;
  raporlananPersonelAd?: string | null;

  mesaiOnaySurecAd?: string | null;
  calismaTakvimiAd?: string | null;

  brutUcret: number;

  sgkIsyeri?: string | null;
  sgkNumarasi?: string | null;
  vergiDairesiAdi?: string | null;
  vergiNumarasi?: string | null;
  tabiOlduguKanun?: string | null;
  meslekKodu?: string | null;
}
