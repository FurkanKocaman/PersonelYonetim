import type { Adres } from "../entity-models/Adres";
import type { EntityDto } from "../entity-models/EntityDto";
import type { Iletisim } from "../entity-models/Iletisim";

export interface PersonelDetaylarGetModel extends EntityDto {
  personelId: string;
  fullName?: string;
  avatarUrl?: string;
  iletisim: Iletisim;
  adres: Adres;
  kurumsalBirimAd: string;
  pozisyonAd: string;
  gorevlendirmeTipi?: string;
  calismaSekli?: string;
  yoneticiAd?: string;
  yoneticiPozisyon?: string;
  iseGirisTarihi?: string; // DateTimeOffset string olarak gelir
  istenCikisTarihi?: string;
  pozisyonBaslangicTarih?: string;
  pozisyonBitisTarih?: string;

  // Kimlik Bilgileri
  tckn?: string;
  nufusIl?: string;
  nufusIlce?: string;
  anaAdi?: string;
  babaAdi?: string;
  dogumYeri?: string;
  dogumTarihi?: string;
  medeniHali?: string;
  cinsiyet?: string;
  uyruk?: string;

  // İletişim Bilgileri
  isTelefonu?: string;
  epostaIs?: string;
  postaKodu?: string;

  // Eğitim Bilgileri
  egitimDurumu?: string;
  mezuniyetOkulu?: string;
  mezuniyetBolumu?: string;
  mezuniyetTarihi?: string;

  // Askerlik Bilgileri
  askerlikDurumu?: string;
  askerlikTarihi?: string;

  // Ehliyet Bilgileri
  ehliyetSinifi?: string;
  ehliyetVerilisTarihi?: string;

  // Sağlık Bilgileri
  engelliMi: boolean;
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
  tenantId?: string;
}
