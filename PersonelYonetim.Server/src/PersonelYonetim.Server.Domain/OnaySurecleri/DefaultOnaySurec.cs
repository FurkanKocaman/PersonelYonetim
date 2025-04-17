using PersonelYonetim.Server.Domain.Rols;

namespace PersonelYonetim.Server.Domain.OnaySurecleri;
public static class DefaultOnaySurec
{
    public static List<OnaySurec> GetDefaultOnaySurecleri(Guid tenantId)
    {
        var defaultOnaySurecleri = new List<OnaySurec>
        {
            new OnaySurec
            {
                Ad = "Default izin talep onay süreci",
                Aciklama = "Izin onayi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.Izin,
                TenantId = tenantId
            },
            new OnaySurec
            {
                Ad = "Default mesai talep onay süreci",
                Aciklama = "Mesai onayi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.Mesai,
                TenantId = tenantId
            },
            new OnaySurec
            {
                Ad = "Default eşya talep onay süreci",
                Aciklama = "Eşya talebi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.EsyaTalep,
                TenantId = tenantId
            },
            new OnaySurec
            {
                Ad = "Default avans talep onay süreci",
                Aciklama = "Avans talebi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.Avans,
                TenantId = tenantId
            }
        };
        return defaultOnaySurecleri;
    }
    public static OnaySureciAdimi GetDefaultOnayAdim(Guid tenantId, Guid onaySurecId,Guid roleId )
    {
        OnaySureciAdimi onaySureciAdimi = new OnaySureciAdimi
        {
            Sira = 1,
            PersonelId = null,
            RolId = roleId,
            OnaySurecId = onaySurecId,
            TenantId = tenantId,
            OnaylayiciTanimTipi = OnaylayiciTanimTipiEnum.YapisalRol_TalepEdenBirimi
        };
        return onaySureciAdimi;
    }
}
