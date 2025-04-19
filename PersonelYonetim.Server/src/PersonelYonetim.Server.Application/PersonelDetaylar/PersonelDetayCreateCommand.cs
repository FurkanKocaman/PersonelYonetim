using Mapster;
using MediatR;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.PersonelDetaylar;
public sealed record PersonelDetayCreateCommand(
    Guid? PersonelId,

    // Kimlik Bilgileri
    string? TCKN,
    string? NufusIl,
    string? NufusIlce,
    string? AnaAdi,
    string? BabaAdi,
    string? DogumYeri,
    DateTime? DogumTarihi,
    string? MedeniHali,
    string? Cinsiyet,
    string? Uyruk,

    // İletişim Bilgileri
    string? CepTelefonu,
    string? IsTelefonu,
    string? Eposta,
    string? EpostaIs,
    string? Adres,
    string? IkametIl,
    string? IkametIlce,
    string? PostaKodu,

    // Eğitim Bilgileri
    string? EgitimDurumu,
    string? MezuniyetOkulu,
    string? MezuniyetBolumu,
    DateTime? MezuniyetTarihi,

    // Askerlik Bilgileri
    string? AskerlikDurumu,
    DateTime? AskerlikTarihi,

    // Ehliyet Bilgileri
    string? EhliyetSinifi,
    DateTime? EhliyetVerilisTarihi,

    // Sağlık Bilgileri
    bool? EngelliMi,
    int? EngelOrani,
    string? SaglikDurumu,
    string? KanGrubu,

    // Acil Durum Bilgileri
    string? AcilDurumKisiAdi,
    string? AcilDurumKisiTelefon,
    string? AcilDurumKisiYakinlik,

    // Aile Bilgileri
    int? CocukSayisi,
    bool? EsCalisiyorMu,

    // Banka Bilgileri
    string? BankaAdi,
    string? IBAN,

    // Diğer
    string? Notlar,
    Guid? TenantId
) : IRequest<Result<string>>;

internal sealed class PersonelDetayCreateCommandHandler(
    IPersonelDetayRepository personelDetayRepository,
    ICurrentUserService currentUserService ,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<PersonelDetayCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelDetayCreateCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        var tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
            return Result<string>.Failure("Tenant bulunamamdı");

        var isPersonelDetayExist = await personelDetayRepository.AnyAsync(p => p.TenantId == tenantId && p.PersonelId == request.PersonelId);

        if (isPersonelDetayExist)
            return Result<string>.Failure("Zaten personelDetay oluşturulmuş");

        PersonelDetay personelDetay = request.Adapt<PersonelDetay>();

        personelDetayRepository.Add(personelDetay);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Başarıyla oluşturuldu");
    }
}
