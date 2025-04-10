using PersonelYonetim.Server.Domain.Rols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelYonetim.Server.Domain.OnaySurecleri;
public static class DefaultOnaySurec
{
    public static List<OnaySurec> GetDefaultOnaySurecleri(Guid sirketId)
    {
        var defaultOnaySurecleri = new List<OnaySurec>
        {
            new OnaySurec
            {
                Ad = "Izin Onayi",
                Aciklama = "Izin onayi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.Izin,
                SirketId = sirketId
            },
            new OnaySurec
            {
                Ad = "Mesai Onayi",
                Aciklama = "Mesai onayi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.Mesai,
                SirketId = sirketId
            },
            new OnaySurec
            {
                Ad = "Eşya Talebi",
                Aciklama = "Eşya talebi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.EsyaTalep,
                SirketId = sirketId
            },
            new OnaySurec
            {
                Ad = "Avans",
                Aciklama = "Avans talebi icin kullanilan onay sureci",
                OnaySurecTuruEnum = OnaySurecTuruEnum.EsyaTalep,
                SirketId = sirketId
            }
        };
        return defaultOnaySurecleri;
    }
    public static List<OnaySureciAdimi> GetDefaultOnayItem(Guid onaySurecId, RolTipiEnum rolTipi)
    {
        var defaultOnayItems = new List<OnaySureciAdimi>
        {
            new OnaySureciAdimi
            {
                Sira = 1,
                PersonelId = null,
                Rol = rolTipi,
                OnaySurecId = onaySurecId,
            },
        };
        return defaultOnayItems;
    }
}
