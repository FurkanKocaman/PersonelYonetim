﻿using MediatR;
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
using PersonelYonetim.Server.Application.Sirketler;
using PersonelYonetim.Server.Application.Subeler;
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
        builder.EntitySet<SirketlerGetCommandResponse>("sirketler");
        builder.EntitySet<SubelerGetQueryResponse>("subeler");
        return builder.GetEdmModel();
    }

    [HttpGet("personeller")]
    [Authorize(Permissions.ViewPersonel)]
    public async Task<IQueryable<PersonelGetAllQueryResponse>> GetAllPersoneller(Guid SirketId, Guid? SubeId, Guid? DepartmanId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PersonelGetAllQuery(SirketId, SubeId, DepartmanId), cancellationToken);

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
    [Authorize]
    public async Task<IQueryable<SirketlerGetCommandResponse>> GetAllSirketler(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SirketlerGetQuery(), cancellationToken);
        return response;
    }

    [HttpGet("subeler/{SirketId}")]
    [Authorize]
    public async Task<IQueryable<SubelerGetQueryResponse>> GetAllSubeler(Guid SirketId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new SubelerGetQuery(SirketId), cancellationToken);
        return response;
    }

    [HttpGet("departmanlar/{SubeId}")]
    [Authorize]
    public async Task<IQueryable<DepartmanGetAllQueryResponse>> GetAllDepartmanlar(Guid SubeId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new DepartmanGetAllQuery(SubeId), cancellationToken);
        return response;
    }

    [HttpGet("pozisyonlar/{DepartmanId}")]
    [Authorize]
    public async Task<IQueryable<PozisyonGetAllQueryResponse>> GetAllPozisyonlar(Guid DepartmanId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new PozisyonGetAllQuery(DepartmanId), cancellationToken);
        return response;
    }

    [HttpGet("IzinTalepler/{DepartmanId}")]
    [Authorize(Permissions.ViewIzinler)]
    public async Task<IQueryable<IzinTalepGetAllQueryResponse>> GetAllIzinTalepler(Guid DepartmanId, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new IzinTalepGetAllQuery(DepartmanId), cancellationToken);
        return response;
    }
}

