import type { Adres, Iletisim } from "./RegisterRequest";

export interface SubeModel {
  id: string;
  ad: string;
  aciklama: string | undefined;
  adres: Adres;
  iletisim: Iletisim;
  sirketId: string;
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
