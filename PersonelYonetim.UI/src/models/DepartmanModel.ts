export interface DepartmanModel {
  id: string;
  ad: string;
  subeAd: string;
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
