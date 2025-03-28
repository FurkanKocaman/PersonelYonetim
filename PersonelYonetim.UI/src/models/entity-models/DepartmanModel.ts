import type { Iletisim } from "./Iletisim";

export interface DepartmanModel {
  id: string;
  ad: string;
  subeId: string;
  subeAd: string;
  sirketId: string;
  sirketAd: string;
  aciklama: string | undefined;
  iletisim: Iletisim;
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
