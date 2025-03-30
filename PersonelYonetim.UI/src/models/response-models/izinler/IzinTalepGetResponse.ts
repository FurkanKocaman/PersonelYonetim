export interface IzinTalepGetResponse {
  id: string;
  personelId: string;
  personelFullName: string;
  baslangicTarihi: Date;
  bitisTarihi: Date;
  mesaiBaslangicTarihi: Date;
  toplamSure: number;
  izinTuru: string;
  aciklama?: string;
  degerlendirmeDurumu: string;
  degerlendirenId?: string;
  degerlendirenAd?: string;
}
