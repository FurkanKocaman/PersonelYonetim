export interface IzinKuralModel {
  id: string;
  ad: string;
  aciklama: string;
  sirketId: string;
  sirketAd: string;
  izinTurler: IzinTurResponse[];
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

export interface IzinTurResponse {
  id: string;
  ad: string;
  aciklama?: string;
  ucretliMi: boolean;
  limitTipiName: string;
  kalanGunSayisi: number;
}
