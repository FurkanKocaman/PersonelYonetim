export interface DuyuruCreateRequest {
  baslik: string;
  aciklama?: string;
  sirketId: string;
  aliciTipiValue: number;
  aliciId?: string;
  aliciIdler?: string[];
}
