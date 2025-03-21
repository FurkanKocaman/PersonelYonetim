﻿using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.PersonelAtamalar;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.Sirketler;

public sealed record SirketCreateCommand(
    string Ad,
    string? Aciklama,
    string? LogoUrl,
    Adres Adres,
    Iletisim Iletisim) : IRequest<Result<string>>;

internal sealed class SirketCreateCommandHandler(
    ISirketRepository sirketRepository,
    IPersonelRepository personelRepository,
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    ISender sender) : IRequestHandler<SirketCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SirketCreateCommand request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }
        var personel = personelRepository.GetAll().AsNoTracking().FirstOrDefault(p => p.UserId == Guid.Parse(userIdString));
        if (personel == null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı");
        }

        var sirketVarMi = await sirketRepository.AnyAsync(p => p.Ad == request.Ad);
        if (sirketVarMi)
            return Result<string>.Failure("Bu isme sahip şirket zaten mevcut.");

        Sirket sirket = request.Adapt<Sirket>();
        sirketRepository.Add(sirket);
        await unitOfWork.SaveChangesAsync();

        PersonelAtamaCreateCommand createCommand = new(personel, sirket.Id, null, null, null, 2, 0, 1, null);
        var result = await sender.Send(createCommand);
        if(!result.IsSuccessful)
            return Result<string>.Failure("Şirket oluşturma başarısız.");


        return Result<string>.Succeed(sirket.Id.ToString());
    }
}
