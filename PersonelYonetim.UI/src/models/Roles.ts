export interface Role {
  name: string;
  value: number;
}

class Roles {
  public static readonly Calisan: Role = { name: "Çalışan", value: 0 };
  public static readonly DepartmanYardimci: Role = {
    name: "Departman Yönetici Yardımcı",
    value: 1,
  };
  public static readonly DepartmanYonetici: Role = { name: "Departman Yönetici", value: 2 };
  public static readonly SubeYardimci: Role = { name: "Şube Yönetici Yardımcı", value: 3 };
  public static readonly SubeYonetici: Role = { name: "Şube Yönetici", value: 4 };
  public static readonly SirketYardimci: Role = { name: "Şirket Yönetici Yardımcı", value: 5 };
  public static readonly SirketYonetici: Role = { name: "Şirket Yönetici", value: 6 };
  public static readonly Admin: Role = { name: "Admin", value: 7 };

  public static getRoleByValue(value: number): Role {
    return Object.values(Roles).find((role) => role.value === value);
  }
}

export default Roles;
