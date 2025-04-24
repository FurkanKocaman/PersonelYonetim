export interface DuyuruCreateRequest {
  baslik: string;
  aciklama?: string;
  tenantId: string | undefined;
  aliciTipiValue: number;
  aliciId?: string;
  aliciIdler?: string[];
}
