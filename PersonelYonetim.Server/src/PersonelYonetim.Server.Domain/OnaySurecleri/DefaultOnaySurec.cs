using PersonelYonetim.Server.Domain.Rols;

namespace PersonelYonetim.Server.Domain.OnaySurecleri;
public static class DefaultOnaySurec
{
    public static List<OnaySurec> GetDefaultOnaySurecleri(Guid sirketId)
    {
        var defaultOnaySurecleri = new List<OnaySurec>
        {
            new OnaySurec
            {
                Ad = "Default izin talep onay süreci",
                Aciklama = "Izin onayi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.Izin,
                SirketId = sirketId
            },
            new OnaySurec
            {
                Ad = "Default mesai talep onay süreci",
                Aciklama = "Mesai onayi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.Mesai,
                SirketId = sirketId
            },
            new OnaySurec
            {
                Ad = "Default eşya talep onay süreci",
                Aciklama = "Eşya talebi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.EsyaTalep,
                SirketId = sirketId
            },
            new OnaySurec
            {
                Ad = "Default avans talep onay süreci",
                Aciklama = "Avans talebi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.Avans,
                SirketId = sirketId
            }
        };
        return defaultOnaySurecleri;
    }
    public static OnaySureciAdimi GetDefaultOnayAdim(Guid onaySurecId, RolTipiEnum? rolTipi)
    {
        OnaySureciAdimi onaySureciAdimi = new OnaySureciAdimi
        {
            Sira = 1,
            PersonelId = null,
            Rol = rolTipi ?? RolTipiEnum.DepartmanYonetici,
            OnaySurecId = onaySurecId,
        };
        return onaySureciAdimi;
    }
}
