using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.IzinTalepler;
using PersonelYonetim.Server.Domain.PersonelDepartmanlar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepCreateCommand(
    DateTimeOffset BaslangicTarihi,
    DateTimeOffset BitisTarihi,
    int IzinTipiValue,
    string? Aciklama) : IRequest<Result<string>>;

public sealed class IzinTalepCreateCommandValidator : AbstractValidator<IzinTalepCreateCommand>
{
    public IzinTalepCreateCommandValidator()
    {
        RuleFor(x => x.BaslangicTarihi).NotEmpty().WithMessage("Başlangıç tarihi boş olamaz");
        RuleFor(x => x.BitisTarihi).NotEmpty().WithMessage("Bitiş tarihi boş olamaz");
    }
}

internal sealed class IzinTalepCreateCommandHandler(
    IIzinTalepRepository izinTalepRepository,
    IPersonelDepartmanRepository personelDepartmanRepository,
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<IzinTalepCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinTalepCreateCommand request, CancellationToken cancellationToken)
    {
        HttpContextAccessor httpContextAccessor = new();
        string? userIdString = httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
        if(userIdString == null)
            return Result<string>.Failure("Kullanıcı bulunamadı");
        var user = await userManager.FindByIdAsync(userIdString);
        var personel = await personelRepository.FirstOrDefaultAsync(p => p.Iletisim.Eposta == user!.Email);

        var personelDepartman = await personelDepartmanRepository.FirstOrDefaultAsync(p => p.PersonelId ==personel.Id);
        if (personelDepartman is null)
            return Result<string>.Failure("Personel departmanı bulunamadı");

        IzinTalep izinTalep = request.Adapt<IzinTalep>();
        izinTalep.PersonelId = Guid.Parse(userIdString);
        izinTalep.DepartmanId = personelDepartman.DepartmanId;
        izinTalep.IzinTipi = IzinTipiEnum.FromValue(request.IzinTipiValue);
        izinTalep.OnayDurumu = OnayDurumEnum.FromValue(2);

        izinTalepRepository.Add(izinTalep);
        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("İzin talebi oluşturuldu");
    }
}

