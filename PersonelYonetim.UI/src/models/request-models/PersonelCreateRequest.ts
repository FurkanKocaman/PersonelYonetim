// Personel oluşturma isteği modeli

export interface PersonelCreateRequest {
  ad: string;
  soyad: string;
  dogumTarihi: string;
  cinsiyet: boolean;
  departmanId: string;
  pozisyonId: string;
  iseGirisTarihi: string;
  eposta: string;
  telefon: string;
  ulke: string;
  sehir: string;
  ilce: string;
  tamAdres: string;
  isActive: boolean;
}
