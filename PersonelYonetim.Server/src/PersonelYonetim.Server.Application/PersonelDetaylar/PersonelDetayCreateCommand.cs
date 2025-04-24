using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.PersonelDetaylar;
public sealed record PersonelDetayCreateCommand : IRequest<Result<string>> 
{
    public Guid? PersonelId { get; init; }

    // Kimlik Bilgileri
    public string? TCKN { get; init; }
    public string? NufusIl { get; init; }
    public string? NufusIlce { get; init; }
    public string? AnaAdi { get; init; }
    public string? BabaAdi { get; init; }
    public string? DogumYeri { get; init; }
    public DateTime? DogumTarihi { get; init; }
    public string? MedeniHali { get; init; }
    public string? Cinsiyet { get; init; }
    public string? Uyruk { get; init; }

    // İletişim Bilgileri
    public string? CepTelefonu { get; init; }
    public string? IsTelefonu { get; init; }
    public string? Eposta { get; init; }
    public string? EpostaIs { get; init; }
    public string? Adres { get; init; }
    public string? IkametIl { get; init; }
    public string? IkametIlce { get; init; }
    public string? PostaKodu { get; init; }

    // Eğitim Bilgileri
    public string? EgitimDurumu { get; init; }
    public string? MezuniyetOkulu { get; init; }
    public string? MezuniyetBolumu { get; init; }
    public DateTime? MezuniyetTarihi { get; init; }

    // Askerlik Bilgileri
    public string? AskerlikDurumu { get; init; }
    public DateTime? AskerlikTarihi { get; init; }

    // Ehliyet Bilgileri
    public string? EhliyetSinifi { get; init; }
    public DateTime? EhliyetVerilisTarihi { get; init; }

    // Sağlık Bilgileri
    public bool EngelliMi { get; init; }
    public int? EngelOrani { get; init; }
    public string? SaglikDurumu { get; init; }
    public string? KanGrubu { get; init; }

    // Acil Durum Bilgileri
    public string? AcilDurumKisiAdi { get; init; }
    public string? AcilDurumKisiTelefon { get; init; }
    public string? AcilDurumKisiYakinlik { get; init; }

    // Aile Bilgileri
    public int? CocukSayisi { get; init; }
    public bool? EsCalisiyorMu { get; init; }

    // Banka Bilgileri
    public string? BankaAdi { get; init; }
    public string? IBAN { get; init; }

    // Diğer
    public string? Notlar { get; init; }
    public Guid? TenantId { get; set; }
}


internal sealed class PersonelDetayCreateCommandHandler(
    ICurrentUserService currentUserService,
    IPersonelRepository personelRepository,
    IPersonelDetayRepository personelDetayRepository,
     IUnitOfWork unitOfWork
    ) : IRequestHandler<PersonelDetayCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelDetayCreateCommand request, CancellationToken cancellationToken)
    {
        var personelId = Guid.Empty;
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant bulunamamdı");

        if (request.PersonelId is null)
        {
            Guid? userId = currentUserService.UserId;
            if (!userId.HasValue)
                return Result<string>.Failure("Personel bulunamadı");

            var personel = await personelRepository.Where(p => p.UserId == userId).Select(p => new { p.Id }).FirstOrDefaultAsync();
            if (personel is null)
                return Result<string>.Failure("Personel bulunamadı");

            personelId = personel.Id;
        }
        else
        {
            personelId = request.PersonelId.Value;
        }

        if (personelId == Guid.Empty)
            return Result<string>.Failure("Personel bulunammadı");

        var isExist = await personelDetayRepository.AnyAsync(p => p.PersonelId == personelId);
        if (isExist)
            return Result<string>.Failure("Bu personel için zaten detay oluşturulmuş");

        PersonelDetay personelDetay = request.Adapt<PersonelDetay>();
        personelDetay.TenantId = tenantId;
        personelDetayRepository.Add(personelDetay);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Personel detay başarıyla oluşturuldu");
    }
}
