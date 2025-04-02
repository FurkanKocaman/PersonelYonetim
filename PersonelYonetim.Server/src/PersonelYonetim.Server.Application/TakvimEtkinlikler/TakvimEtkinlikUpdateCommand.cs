
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.TakvimEtkinlikler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.TakvimEtkinlikler;
public sealed record TakvimEtkinlikUpdateCommand(
    Guid EtkinlikId,
    string Baslik,
    string? Aciklama,
    DateTimeOffset BaslangicTarihi,
    DateTimeOffset? BitisTarihi,
    bool IsPublic,
    IEnumerable<Guid>? PersonelIdler) : IRequest<Result<string>>;

internal sealed class TakvimEtkinlikUpdateCommandHandler(
    ITakvimEtkinlikRepository takvimEtkinlikRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<TakvimEtkinlikUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(TakvimEtkinlikUpdateCommand request, CancellationToken cancellationToken)
    {
        TakvimEtkinlik? takvimEtkinlik = await takvimEtkinlikRepository
            .WhereWithTracking(p => p.Id == request.EtkinlikId && !p.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
        if (takvimEtkinlik is null)
        {
            return Result<string>.Failure("Etkinlik bulunamadı.");
        }
        takvimEtkinlik.Baslik = request.Baslik;
        takvimEtkinlik.Aciklama = request.Aciklama;
        takvimEtkinlik.BaslangicTarihi = request.BaslangicTarihi;
        takvimEtkinlik.BitisTarihi = request.BitisTarihi;
        takvimEtkinlik.IsPublic = request.IsPublic;
        takvimEtkinlik.PersonelIdler = request.PersonelIdler;   

        takvimEtkinlikRepository.Update(takvimEtkinlik);
        var affectedRows = await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed(takvimEtkinlik.Id.ToString());
    }
}
