import type { Adres } from "./Adres";
import type { Iletisim } from "./Iletisim";

export interface SubeModel {
  id: string;
  ad: string;
  aciklama: string | undefined;
  adres: Adres;
  iletisim: Iletisim;
  sirketId: string;
  sirketAd: string;
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
