import type { Adres } from "./Adres";
import type { Iletisim } from "./Iletisim";

export interface SirketModel {
  id: string;
  ad: string;
  aciklama: string | undefined;
  logoUrl: string | undefined;
  adres: Adres;
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
