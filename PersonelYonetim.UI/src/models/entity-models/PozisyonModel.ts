export interface PozisyonModel {
  id: string;
  ad: string;
  aciklama: string | undefined;
  sirketId: string;
  sirketAd: string;
  departmanId?: string; 
  departmanAd?: string; 
  isActive: boolean;
  createdAt: Date;
  createUserId: string;
  createUserName: string;
  updateAt: Date;
  updateUserId: string;
  updateuserName: string;
  isDeleted: boolean;
  deleteAt: Date;
}
