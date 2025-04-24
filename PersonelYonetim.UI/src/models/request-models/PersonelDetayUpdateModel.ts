export interface PersonelDetayUpdateModel {
  id: string;
  personelId?: string;

  // Kimlik Bilgileri
  tckn?: string;
  nufusIl?: string;
  nufusIlce?: string;
  anaAdi?: string;
  babaAdi?: string;
  dogumYeri?: string;
  dogumTarihi?: Date;
  medeniHali?: string;
  cinsiyet?: string;
  uyruk?: string;

  // İletişim Bilgileri
  cepTelefonu?: string;
  isTelefonu?: string;
  eposta?: string;
  epostaIs?: string;
  adres?: string;
  ikametIl?: string;
  ikametIlce?: string;
  postaKodu?: string;

  // Eğitim Bilgileri
  egitimDurumu?: string;
  mezuniyetOkulu?: string;
  mezuniyetBolumu?: string;
  mezuniyetTarihi?: Date;

  // Askerlik Bilgileri
  askerlikDurumu?: string;
  askerlikTarihi?: Date;

  // Ehliyet Bilgileri
  ehliyetSinifi?: string;
  ehliyetVerilisTarihi?: Date;

  // Sağlık Bilgileri
  engelliMi?: boolean;
  engelOrani?: number;
  saglikDurumu?: string;
  kanGrubu?: string;

  // Acil Durum Bilgileri
  acilDurumKisiAdi?: string;
  acilDurumKisiTelefon?: string;
  acilDurumKisiYakinlik?: string;

  // Aile Bilgileri
  cocukSayisi?: number;
  esCalisiyorMu?: boolean;

  // Banka Bilgileri
  bankaAdi?: string;
  iban?: string;

  // Diğer
  notlar?: string;
}
