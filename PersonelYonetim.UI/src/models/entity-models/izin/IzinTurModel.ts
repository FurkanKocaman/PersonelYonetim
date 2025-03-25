export interface IzinTurModel {
  ad: string;
  aciklama?: string | null;
  ucretliMi: boolean;
  limitTipi?: LimitTipiEnum | null;
  limitGunSayisi: number;
  eksiBakiyeHakki?: EksiBakiyeHakkıEnum | null;
  eksiBakiyeLimit: number;
  hakEdis: HakEdisEnum;
  hakEdisDonem: boolean;
  hakEdisBaslangic: boolean;
  devretmeTipi?: DevretmeTipiEnum | null;
  devretmeGunLimit: number;
  devretmelimitTarih?: string | null;
  kidemYil: number;
  kidemArtiGun: number;
  enAzTalep: number;
  enCokTalep: number;
  hesapSekli: boolean;
  aciklamaZorunlu: boolean;
  yerineBakacakZorunlu: boolean;
  sirketId: string;
  sirketAd: string;
  isActive: boolean;
  isDeleted: boolean;
  createUserId: string;
  createUserName: string;
  createdAt: Date;
  deleteAt: string | null;
  updateAt: string | null;
  updateUserId: string | null;
  updateUserName: string | null;
}

export type LimitTipiEnum = "Limitsiz" | "Her talep için limitli" | "Yıl içinde limitli";
export type EksiBakiyeHakkıEnum =
  | "Eksi bakiye hakkı yok"
  | "Limitsiz eksi bakiye hakkı"
  | "Limitli eksi bakiye hakkı";
export type HakEdisEnum = "Yıllık" | "Aylık" | "Günlük";
export type DevretmeTipiEnum =
  | "İzin hakkı sıfırlama"
  | "Limitsiz devretme"
  | "Limitli toplam izin hakkı"
  | "Limitli devretme"
  | "Geçerlilik tarihli limitli devretme";
