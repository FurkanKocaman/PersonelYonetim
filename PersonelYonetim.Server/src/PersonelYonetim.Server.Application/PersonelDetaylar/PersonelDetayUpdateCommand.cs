using Mapster;
using MediatR;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.PersonelDetaylar;
public sealed record PersonelDetayUpdateCommand(
    Guid Id, // Güncellenecek detay kaydının Id'si

    Guid? PersonelId,

    // Kimlik Bilgileri
    string? TCKN,
    string? AnaAdi,
    string? BabaAdi,
    string? DogumYeri,
    string? MedeniHali,
    string? Uyruk,

    // İletişim Bilgileri
    string? IsTelefonu,
    string? EpostaIs,
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
    string? Notlar
    ) : IRequest<Result<string>>;

internal sealed class PersonelDetayUpdateCommandHandler(
    IPersonelDetayRepository personelDetayRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<PersonelDetayUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelDetayUpdateCommand request, CancellationToken cancellationToken)
    {
        var tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant bulunamadı");

        var existingDetay = await personelDetayRepository.FirstOrDefaultAsync(p => p.Id == request.Id && p.TenantId == tenantId);

        if (existingDetay is null)
            return Result<string>.Failure("PersonelDetay bulunamadı");

        request.Adapt(existingDetay);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Başarıyla güncellendi");
    }
}