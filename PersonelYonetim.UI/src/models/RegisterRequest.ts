export interface RegisterRequest {
  sirketAd: string;
  ad: string;
  soyad: string;
  dogumTarihi: Date;
  cinsiyet: boolean | undefined;
  iletisim: Iletisim;
  adres: Adres;
  sifre: string;
}
export interface Adres {
  ulke: string;
  sehir: string;
  ilce: string;
  tamAdres: string;
}
export interface Iletisim {
  eposta: string;
  telefon: string;
}
