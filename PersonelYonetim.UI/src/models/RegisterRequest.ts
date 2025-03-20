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
