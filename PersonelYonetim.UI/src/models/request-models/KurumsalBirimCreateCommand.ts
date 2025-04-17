export interface KurumsalBirimCreateCommand {
  id: string | undefined;
  ad: string;
  kod: string | undefined;
  logoUrl: string | undefined;
  birimTipiId: string;
  ustBirimId: string | undefined;
  tenantId: string | undefined;
}
