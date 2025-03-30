export interface TakvimEtkinlikModel {
  id: string;
  baslik: string;
  aciklama: string;
  baslangicTarihi: Date;
  bitisTarihi: Date;
  sirketId: string;
  sirketAd: string;
  isPublic: boolean;
  Katilimcilar: KatilimciDto[];
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

export interface KatilimciDto {
  id: string;
  fullName: string;
}
