export interface BordroGetCalisanlarModel {
  id: string;
  fullName: string;
  avatarUrl: string | undefined;
  TCKN: string | undefined;
  iseBaslangicTarihi: Date | undefined;
  istenCikisTarihi: Date | undefined;
  engelDerecesi: number;
  tabiOlduguKanun: string;
  SGKIsyeri: string | undefined;
  vergiDairesiAdi: string | undefined;
  kumulatifVergiMatrahi: string | undefined;
  birimAdi: string | undefined;
  pozisyonAd: string | undefined;
  meslekKodu: string | undefined;
}
