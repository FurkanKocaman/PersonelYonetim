export interface BordroGetByPersonelModel {
  id: string;
  fullName: string;
  durum: string;
  yil: number;
  ay: number;

  // Girdiler
  brutUcret: number;
  sgkGun: number;
  ekOdemelerToplam: number;
  kesintilerToplam: number;

  // Kazançlar
  gunlukUcret: number;
  odemeyeEsasGunSayisi: number;

  // SGK
  fiiliCalisma: number;
  ucretliIzin: number;
  raporlu: number;
  ucretsizIzin: number;
  digerEksikGun: number;

  // Gelir Vergisi
  kumulatifToplam: number;
  ekOdemeIstisnaToplam: number;
  gvAylikMatrah: number;
  gvOdemesi: number;

  // Damga Vergisi
  ekOdemeIstisna: number;
  dvAylikMatrah: number;
  dvOdemesi: number;

  // Kesintiler
  yasalKesintiler: number;
  ozelKesintiler: number;
  tumKesintiler: number;

  // Maliyet
  eleGecenUcret: number;
  tesvikler: number;
  tesvikliMaliyet: number;
}
