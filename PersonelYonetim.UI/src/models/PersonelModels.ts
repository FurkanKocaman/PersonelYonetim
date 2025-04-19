import type { Adres } from "./entity-models/Adres";
import type { EntityDto } from "./entity-models/EntityDto";
import type { Iletisim } from "./entity-models/Iletisim";

export interface PersonelItem extends EntityDto {
  ad: string;
  soyad: string;
  dogumTarihi: Date;
  cinsiyet: string | undefined;
  avatarUrl: string | undefined;
  iletisim: Iletisim;
  adres: Adres;
  yoneticiAd: string | undefined;
  yoneticiPozisyon: string | undefined;
  kurumsalBirimAd: string;
  pozisyonAd: string;
  sozlesmeTuruValue: number; //Backendde yok
  baslangicTarih: Date;
  sozlesmeBitisTarihi: Date | undefined; //Backendde yok
  izinKuralId: string | undefined; //Backendde yok
  roleClaims: string[];
}
