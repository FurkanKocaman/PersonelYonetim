export interface DepartmanModel {
  id: string;
  ad: string;
  aciklama: string | undefined;
  isActive: boolean;
  createdAt: Date;
  createUserId: string;
  createuserName: string;
  updateAt: Date;
  updateUserId: string;
  updateuserName: string;
  isDeleted: boolean;
  deleteAt: Date;
}
