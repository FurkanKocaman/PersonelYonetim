export interface UserModel {
  id: string;
  fullName: string;
  dogumTarihi: Date;
  cinsiyet: boolean | undefined;
  profilResimUrl: string | undefined;
  departmanAd: string;
  pozisyonAd: string;
  eposta: string;
  telefon: string;
  ulke: string;
  sehir: string;
  ilce: string;
  tamAdres: string;
  role: number;
  userId: string;
}

interface NameValue {
  value: number;
  name: string;
}

export class SozlesmeTuru {
  public static readonly Sureli: NameValue = { name: "Süreli Sözleşme", value: 0 };
  public static readonly Suresiz: NameValue = { name: "Süresiz Sözleşme", value: 1 };
  public static readonly Donemsel: NameValue = { name: "Dönemsel Sözleşme", value: 2 };
  public static readonly Esnek: NameValue = { name: "Esnek Sözleşme", value: 3 };

  public static getSozlesmeByValue(value: number): NameValue {
    return Object.values(SozlesmeTuru).find((p) => p.value === value);
  }
  public static getSozlesmeByName(name: string): NameValue {
    return Object.values(SozlesmeTuru).find((p) => p.name === name);
  }
}
export class CalismaSekli {
  public static readonly TamZamanli: NameValue = { name: "Tam Zamanlı", value: 0 };
  public static readonly YariZamanli: NameValue = { name: "Yari Zamanlı", value: 1 };
  public static readonly EsnekZamanli: NameValue = { name: "Esnek Zamanlı", value: 2 };

  public static getCalismaSekliByValue(value: number): NameValue {
    return Object.values(CalismaSekli).find((p) => p.value === value);
  }
}
