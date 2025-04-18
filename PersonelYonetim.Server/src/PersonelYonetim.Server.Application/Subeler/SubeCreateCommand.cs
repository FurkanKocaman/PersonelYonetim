﻿
using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Subeler;

public sealed record SubeCreateCommand(
    string Ad,
    string? Aciklama,
    Guid SirketId,
    Adres Adres,
    Iletisim Iletisim) : IRequest<Result<string>>;

internal sealed class SubeCreateCommandHandler(
    ISubeRepository subeRepository,
    ISirketRepository sirketRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SubeCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SubeCreateCommand request, CancellationToken cancellationToken)
    {
        var subeVarMi = await subeRepository.FirstOrDefaultAsync(p => (p.Ad == request.Ad || p.Iletisim.Eposta == request.Iletisim.Eposta || p.Iletisim.Telefon == request.Iletisim.Telefon) && p.IsDeleted == false);
        if (subeVarMi is not null)
            return Result<string>.Failure("Bu isme sahip şube zaten var");

        var sirketVarMi = await sirketRepository.AnyAsync(p => p.Id == request.SirketId && !p.IsDeleted);
        if (!sirketVarMi)
            return Result<string>.Failure("Şirket bulunamadı");

        Sube sube = request.Adapt<Sube>();
        subeRepository.Add(sube);
        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("Şube oluşturuldu");
    }
}
