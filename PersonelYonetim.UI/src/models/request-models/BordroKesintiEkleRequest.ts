export interface BordroKesintiEkleRequest {
  maasPusulaId: string;
  kesintiTuru: string;
  aciklama: string | undefined;
  tutar: number;
  sgkMatrahinaDahil: boolean;
  gelirVergisiMatrahinaDahil: boolean;
  damgaVergisiMatrahinaDahil: boolean;
}
