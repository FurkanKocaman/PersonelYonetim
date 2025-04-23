import type { EntityDto } from "../entity-models/EntityDto";

export interface TenantGetModel extends EntityDto {
  name: string;
  displayName: string;
  logoUrl?: string;

  sgkIsyeri?: string;
  sgkNumarasi?: string;
  vergiDairesiAdi?: string;
  vergiNumarasi?: string;
  tabiOlduguKanun?: string;
  address?: string;
  city?: string;
  postalCode?: string;
  phone?: string;
  email?: string;

  // Bordro için gerekli değişkenler
  asgariUcret: number;
  sgkPrimIsciKesintiOrani: number;
  sgkIssizlikPrimIsciKesintiOrani: number;
  sgkPrimIsverenKesintiOrani: number;
  sgkIssizlikPrimIsverenKesintiOrani: number;
  damgaVergisiOrani: number;

  fazlaMesaiKatsayi: number;
  haftasonuFazlaMesaiKatsayi: number;
  resmiTatilFazlaMesaiKatsayi: number;
}
