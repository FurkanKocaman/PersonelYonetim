export interface TakvimEtkinlikCreateCommand {
  etkinlikId: string;
  baslik: string;
  aciklama?: string;
  baslangicTarihi: Date;
  bitisTarihi?: Date;
  isPublic: boolean;
  personelIdler?: string[];
}
