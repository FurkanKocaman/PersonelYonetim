using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PersonelYonetim.Server.Application.Departmanlar;
using PersonelYonetim.Server.Application.IzinTalepler;
using PersonelYonetim.Server.Application.Personeller;
using PersonelYonetim.Server.Application.Pozisyonlar;

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
        return builder.GetEdmModel();
    }

    [HttpGet("personeller")]
    public async Task<IQueryable<PersonelGetAllQueryResponse>> GetAllPersoneller(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PersonelGetAllQuery(), cancellationToken);

        return response;
    }
    [HttpGet("personeller/{Id}")]
    public async Task<IQueryable<PersonelGetQueryResponse>> GetPersonel(Guid Id, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PersonelGetQuery(Id), cancellationToken);
        return response;
    }

    [HttpGet("departmanlar")]
    public async Task<IQueryable<DepartmanGetAllQueryResponse>> GetAllDepartmanlar(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DepartmanGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("pozisyonlar/{Id}")]
    public async Task<IQueryable<PozisyonGetAllQueryResponse>> GetAllPozisyonlar(Guid Id,CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PozisyonGetAllQuery(Id), cancellationToken);
        return response;
    }
    [HttpGet("IzinTalepler/{Id}")]
    [Authorize("manager")]
    public async Task<IQueryable<IzinTalepGetAllQueryResponse>> GetAllIzinTalepler(Guid Id, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new IzinTalepGetAllQuery(Id), cancellationToken);
        return response;
    }
}

