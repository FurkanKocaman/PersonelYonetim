export interface UserModel {
  id: string;
  fullName: string;
  dogumTarihi: Date;
  cinsiyet: boolean | null;
  profilResimUrl: string | null;
  departmanAd: string;
  pozisyonAd: string;
  eposta: string;
  telefon: string;
  ulke: string;
  sehir: string;
  ilce: string;
  tamAdres: string;
}
