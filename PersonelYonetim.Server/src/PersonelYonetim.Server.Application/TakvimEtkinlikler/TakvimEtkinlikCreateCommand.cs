using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.TakvimEtkinlikler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.TakvimEtkinlikler;
public sealed record TakvimEtkinlikCreateCommand(
    string Baslik,
    string? Aciklama,
    DateTimeOffset BaslangicTarihi,
    DateTimeOffset? BitisTarihi,
    bool IsPublic,
    IEnumerable<Guid>? PersonelIdler) : IRequest<Result<string>>;

public sealed class TakvimEtkinlikCreateCommandValidator : AbstractValidator<TakvimEtkinlikCreateCommand>
{
    public TakvimEtkinlikCreateCommandValidator()
    {
        RuleFor(x => x.Baslik).NotEmpty().WithMessage("Baslik boş olamaz");
        RuleFor(x => x.BaslangicTarihi).NotEmpty().WithMessage("Başlangıç tarihi boş olamaz");
    }
}

internal sealed class TakvimEtkinlikCreateCommandHandler(
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    ITakvimEtkinlikRepository takvimEtkinlikRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<TakvimEtkinlikCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(TakvimEtkinlikCreateCommand request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString))
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }
        var sirket = personelAtamaRepository
            .Where(p => p.PersonelId == personel.Id)
            .Select(p => new { p.SirketId })
            .FirstOrDefault();

        TakvimEtkinlik takvimEtkinlik = request.Adapt<TakvimEtkinlik>();
        takvimEtkinlik.SirketId = sirket!.SirketId;

        takvimEtkinlikRepository.Add(takvimEtkinlik);
        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed(takvimEtkinlik.Id.ToString());
    }
}
