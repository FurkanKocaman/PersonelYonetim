export interface EntityDto {
  id: string;
  isActive: boolean;
  createdAt: Date;
  createUserId?: string;
  createUserName?: string;
  updateAt?: Date;
  updateUserId?: string;
  updateUserName?: string;
  isDeleted: boolean;
  deleteAt?: Date;
}
