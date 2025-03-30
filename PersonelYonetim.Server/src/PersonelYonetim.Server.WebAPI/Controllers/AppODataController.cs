using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PersonelYonetim.Server.Application.CalismaTakvimleri;
using PersonelYonetim.Server.Application.Departmanlar;
using PersonelYonetim.Server.Application.IzinKurallar;
using PersonelYonetim.Server.Application.IzinTalepler;
using PersonelYonetim.Server.Application.IzinTurler;
using PersonelYonetim.Server.Application.Personeller;
using PersonelYonetim.Server.Application.Pozisyonlar;
using PersonelYonetim.Server.Application.Sirketler;
using PersonelYonetim.Server.Application.Subeler;
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
        builder.EntitySet<DepartmanGetAllQueryResponse>("departmanlar");
        builder.EntitySet<PozisyonGetAllQueryResponse>("pozisyonlar");
        builder.EntitySet<SirketlerGetQueryResponse>("sirketler");
        builder.EntitySet<SubelerGetQueryResponse>("subeler");
        builder.EntitySet<IzinKuralGetAllResponse>("izin-kurallar");
        builder.EntitySet<IzinTurGetAllQueryResponse>("izin-turler");
        builder.EntitySet<CalismaTakvimiGetQueryResponse>("calisma-takvim");
        builder.EntitySet<TakvimEtkinlikGetAllQueryResponse>("takvim-etkinlikler");
        return builder.GetEdmModel();
    }

    [HttpGet("personeller")]
    [Authorize(Permissions.ViewPersonel)]
    public async Task<IQueryable<PersonelGetAllQueryResponse>> GetAllPersoneller(Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PersonelGetAllQuery(SirketId,SubeId,DepartmanId,PozisyonId), cancellationToken);

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
    [HttpGet("sirketler")]
    [Authorize(Permissions.ViewSirket)]
    public async Task<IQueryable<SirketlerGetQueryResponse>> GetAllSirketler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SirketlerGetQuery(), cancellationToken);
        return response;
    }

    [HttpGet("subeler")]
    [Authorize(Permissions.ViewSube)]
    public async Task<IQueryable<SubelerGetQueryResponse>> GetAllSubeler(Guid? SirketId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SubelerGetQuery(SirketId), cancellationToken);
        return response;
    }

    [HttpGet("departmanlar")]
    [Authorize(Permissions.ViewDepartman)]
    public async Task<IQueryable<DepartmanGetAllQueryResponse>> GetAllDepartmanlar(Guid? SubeId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DepartmanGetAllQuery(SubeId), cancellationToken);
        return response;
    }

    [HttpGet("pozisyonlar")]
    [Authorize(Permissions.ViewPozisyon)]
    public async Task<IQueryable<PozisyonGetAllQueryResponse>> GetAllPozisyonlar(Guid? SirketId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PozisyonGetAllQuery(SirketId), cancellationToken);
        return response;
    }
    [HttpGet("izin-kurallar")]
    [Authorize]
    public async Task<IQueryable<IzinKuralGetAllResponse>> GetAllIzinKurallar(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new IzinKuralGetAllQuery(), cancellationToken);
        return response;
    }
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

}

