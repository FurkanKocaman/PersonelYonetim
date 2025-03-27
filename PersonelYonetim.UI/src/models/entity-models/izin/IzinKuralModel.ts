export interface IzinKuralModel {
  id: string;
  ad: string;
  aciklama: string;
  sirketId: string;
  sirketAd: string;
  izinTur: string[];
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
