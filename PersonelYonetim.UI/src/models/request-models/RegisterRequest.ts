import type { KurumsalBirimCreateCommand } from "./KurumsalBirimCreateCommand";
import type { PersonelCreateCommand } from "./PersonelCreateCommand";

export interface RegisterRequest {
  kurumsalBirimCreateCommand: KurumsalBirimCreateCommand;
  personelCreateCommand: PersonelCreateCommand;
}
