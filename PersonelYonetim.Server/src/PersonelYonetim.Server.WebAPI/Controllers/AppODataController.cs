using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PersonelYonetim.Server.Application.Bildirimler;
using PersonelYonetim.Server.Application.Bordro;
using PersonelYonetim.Server.Application.CalismaCizelgeleri;
using PersonelYonetim.Server.Application.CalismaTakvimleri;
using PersonelYonetim.Server.Application.Duyurular;
using PersonelYonetim.Server.Application.IzinTalepler;
using PersonelYonetim.Server.Application.IzinTurler;
using PersonelYonetim.Server.Application.KurumsalBirimler;
using PersonelYonetim.Server.Application.KurumsalBirimTipleri;
using PersonelYonetim.Server.Application.Personeller;
using PersonelYonetim.Server.Application.Pozisyonlar;
using PersonelYonetim.Server.Application.Roller;
using PersonelYonetim.Server.Application.TakvimEtkinlikler;
using PersonelYonetim.Server.Domain.RoleClaim;

namespace PersonelYonetim.Server.WebAPI.Controllers;

[Route("odata")]
[ApiController]
[EnableQuery]
public class AppODataController(
    ISender sender) : ODataController
{

    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        builder.EntitySet<PersonelGetAllQueryResponse>("personeller");
        builder.EntitySet<PozisyonGetAllQueryResponse>("pozisyonlar");
        builder.EntitySet<RoleGetAllQueryResponse>("roller");
        //builder.EntitySet<IzinKuralGetAllResponse>("izin-kurallar");
        builder.EntitySet<IzinTurGetAllQueryResponse>("izin-turler");
        builder.EntitySet<IzinTalepGetAllQueryResponse>("izin-talepler");
        builder.EntitySet<IzinTalepGetOnayBekleyenQueryResponse>("izin-talepler-onay-bekleyenler");
        builder.EntitySet<IzinTalepGetQueryResponse>("personel-izin-talepler");
        //builder.EntitySet<IzinlerGetKalanQueryResponse>("getkalanizinler");
        builder.EntitySet<CalismaTakvimiGetQueryResponse>("calisma-takvim");
        builder.EntitySet<TakvimEtkinlikGetAllQueryResponse>("takvim-etkinlikler");
        builder.EntitySet<BildirimlerGetQueryResponse>("bildirimler");
        builder.EntitySet<DuyuruGetAllQueryResponse>("duyurular");
        builder.EntitySet<CalismaCizelgeleriGetAllQueryResponse>("calisma-cizelgeler");
        builder.EntitySet<KurumsalBirimGetAllQueryResponse>("kurumsal-birimler");
        builder.EntitySet< KurumsalBirimTipiGetAllQueryResponse>("kurumsal-birim-tipleri");
        builder.EntitySet<CalismaCizelgeGetAllQueryResponse>("calisma-cizelgeler-new");
        builder.EntitySet<BordroGetAllQueryResponse>("bordro");
        builder.EntitySet<BordroGetCalisanlarQueryResponse>("bordro-calisanlar");
        return builder.GetEdmModel();
    }

    [HttpGet("personeller")]
    [Authorize()]
    public async Task<IQueryable<PersonelGetAllQueryResponse>> GetAllPersoneller(
    Guid? KurumsalBirimId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PersonelGetAllQuery(KurumsalBirimId), cancellationToken);

        return response;
    }

    [HttpGet("personeller/{Id}")]
    [Authorize(Permissions.ViewPersonel)]
    public async Task<IQueryable<PersonelGetQueryResponse>> GetPersonel(Guid Id, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PersonelGetQuery(Id), cancellationToken);
        return response;
    }

    [HttpGet("personel-current")]
    [Authorize]
    public async Task<IQueryable<PersonelGetCurrentQueryResponse>> GetCurrentPersonel(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PersonelGetCurrentQuery(), cancellationToken);
        return response;


    }

    [HttpGet("pozisyonlar")]
    [Authorize()]
    public async Task<IQueryable<PozisyonGetAllQueryResponse>> GetAllPozisyonlar(Guid? SirketId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PozisyonGetAllQuery(SirketId), cancellationToken);
        return response;
    }

    [HttpGet("roller")]
    [Authorize()]
    public async Task<IQueryable<RoleGetAllQueryResponse>> GetAllRoller(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new RoleGetAllQuery(null), cancellationToken);
        return response;
    }
    //[HttpGet("izin-kurallar")]
    //[Authorize]
    //public async Task<IQueryable<IzinKuralGetAllResponse>> GetAllIzinKurallar(CancellationToken cancellationToken)
    //{
    //    var response = await sender.Send(new IzinKuralGetAllQuery(), cancellationToken);
    //    return response;
    //}
    [HttpGet("izin-turler")]
    [Authorize]
    public async Task<IQueryable<IzinTurGetAllQueryResponse>> GetAllIzinTurler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new IzinTurGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("izin-talepler")]
    [Authorize(Permissions.ViewIzinler)]
    public async Task<IQueryable<IzinTalepGetAllQueryResponse>> GetAllIzinTalepler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new IzinTalepGetAllQuery(), cancellationToken);
        return response;
    }
    [HttpGet("izin-talepler-onay-bekleyenler")]
    [Authorize()]
    public async Task<IQueryable<IzinTalepGetOnayBekleyenQueryResponse>> GetAllOnayBekleyenIzinTalepler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new IzinTalepGetOnayBekleyenQuery(), cancellationToken);
        return response;
    }

    [HttpGet("personel-izin-talepler")]
    [Authorize]
    public async Task<IQueryable<IzinTalepGetQueryResponse>> PersonelGetIzinTalepler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new IzinTalepGetQuery(), cancellationToken);
        return response;
    }

    [HttpGet("calisma-takvim")]
    [Authorize]
    public async Task<IQueryable<CalismaTakvimiGetQueryResponse>> GetCalismaTakvim(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new CalismaTakvimiGetQuery(), cancellationToken);
        return response;
    }
    [HttpGet("takvim-etkinlikler")]
    [Authorize]
    public async Task<IQueryable<TakvimEtkinlikGetAllQueryResponse>> GetTakvimEtkinlikler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new TakvimEtkinlikGetAllQuery(), cancellationToken);
        return response;
    }

    //[HttpGet("personel-atamalar")]
    //[Authorize]
    //public async Task<IQueryable<PersonelAtamaGetQueryResponse>> GetPersonelAtamalar(CancellationToken cancellationToken)
    //{
    //    var response = await sender.Send(new PersonelAtamaGetQuery(), cancellationToken);
    //    return response;
    //}
    [HttpGet("bildirimler")]
    [Authorize]
    public async Task<IQueryable<BildirimlerGetQueryResponse>> GetBildirimler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new BildirimlerGetQuery(), cancellationToken);
        return response;
    }
    [HttpGet("getkalanizinler")]
    [Authorize]
    public async Task<IzinlerGetKalanQueryResponse> GetKalanIzinler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new IzinlerGetKalanQuery(), cancellationToken);
        return response;
    }

    [HttpGet("duyurular")]
    [Authorize]
    public async Task<IQueryable<DuyuruGetAllQueryResponse>> GetDuyurular(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DuyuruGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("calisma-cizelgeler")]
    [Authorize]
    public async Task<List<CalismaCizelgeleriGetAllQueryResponse>> GetCalismaCizelgeler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new CalismaCizelgeleriGetAllQuery(), cancellationToken);
        return response;
    }
    [HttpGet("calisma-cizelgeler-new")]
    [Authorize]
    public async Task<IQueryable<CalismaCizelgeGetAllQueryResponse>> GetCalismaCizelgelernew(int yil, int ay,CancellationToken cancellationToken)
    {
        var response = await sender.Send(new CalismaCizelgelerGetAllQuery(yil,ay), cancellationToken);
        return response;
    }
    [HttpGet("kurumsal-birim-tipleri")]
    [Authorize]
    public async Task<IQueryable<KurumsalBirimTipiGetAllQueryResponse>> GetKurumsalBirimTipleri(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new KurumsalBirimTipiGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("kurumsal-birimler")]
    [Authorize]
    public async Task<IQueryable<KurumsalBirimGetAllQueryResponse>> GetKurumsalBirimler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new KurumsalBirimGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("bordro")]
    [Authorize()]
    public async Task<IQueryable<BordroGetAllQueryResponse>> GetBordro(int yil, int ay, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new BordroGetAllQuery(yil, ay), cancellationToken);
        return response;
    }
    [HttpGet("bordro-calisanlar")]
    [Authorize()]
    public async Task<IQueryable<BordroGetCalisanlarQueryResponse>> GetBordroCalisanlar(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new BordroGetCalisanlarQuery(), cancellationToken);
        return response;
    }

}

