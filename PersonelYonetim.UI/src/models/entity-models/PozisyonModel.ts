export interface PozisyonModel {
  id: string;
  ad: string;
  kod: string | undefined;
  aciklama: string | undefined;
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
