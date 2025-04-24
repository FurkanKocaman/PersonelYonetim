export interface BordroKazancEkleRequest {
  maasPusulaId: string;
  kazancTuru: string;
  aciklama: string | undefined;
  tutar: number;
  sgkMatrahinaDahil: boolean;
  gelirVergisiMatrahinaDahil: boolean;
  damgaVergisiMatrahinaDahil: boolean;
}
