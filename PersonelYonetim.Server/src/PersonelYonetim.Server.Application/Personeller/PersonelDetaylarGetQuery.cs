using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Application.Personeller;
public sealed record PersonelDetaylarGetQuery(
    Guid? PersonelId
    ) : IRequest<IQueryable<PersonelDetaylarGetQueryResponse>>;

public sealed class PersonelDetaylarGetQueryResponse : EntityDto
{
    public Guid PersonelId { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public Iletisim Iletisim { get; set; } = default!;
    public Adres Adres { get; set; } = default!;
    public string KurumsalBirimAd { get; set; } = string.Empty;
    public string PozisyonAd { get; set; } = string.Empty;
    public string GorevlendirmeTipi { get; set; } = string.Empty;
    public string CalismaSekli { get; set; } = string.Empty;
    public string? YoneticiAd { get; set; }
    public string? YoneticiPozisyon { get; set; }
    public DateTimeOffset? BaslangicTarih { get; set; }
    public DateTimeOffset? BitisTarih { get; set; }

    // Kimlik Bilgileri
    public string? TCKN { get; set; }
    public string? AnaAdi { get; set; }
    public string? BabaAdi { get; set; }
    public string? DogumYeri { get; set; }
    public DateTimeOffset? DogumTarihi { get; set; }
    public string? MedeniHali { get; set; }
    public string? Cinsiyet { get; set; }
    public string? Uyruk { get; set; }

    // İletişim Bilgileri
    public string? IsTelefonu { get; set; }
    public string? EpostaIs { get; set; }
    public string? PostaKodu { get; set; }

    // Eğitim Bilgileri
    public string? EgitimDurumu { get; set; }
    public string? MezuniyetOkulu { get; set; }
    public string? MezuniyetBolumu { get; set; }
    public DateTime? MezuniyetTarihi { get; set; }

    // Askerlik Bilgileri
    public string? AskerlikDurumu { get; set; }
    public DateTime? AskerlikTarihi { get; set; }

    // Ehliyet Bilgileri
    public string? EhliyetSinifi { get; set; }
    public DateTime? EhliyetVerilisTarihi { get; set; }

    // Sağlık Bilgileri
    public bool EngelliMi { get; set; } = false;
    public int? EngelOrani { get; set; }
    public string? SaglikDurumu { get; set; }
    public string? KanGrubu { get; set; }

    // Acil Durum Bilgileri
    public string? AcilDurumKisiAdi { get; set; }
    public string? AcilDurumKisiTelefon { get; set; }
    public string? AcilDurumKisiYakinlik { get; set; }

    // Aile Bilgileri
    public int? CocukSayisi { get; set; }
    public bool? EsCalisiyorMu { get; set; }

    // Banka Bilgileri
    public string? BankaAdi { get; set; }
    public string? IBAN { get; set; }

    // Diğer
    public string? Notlar { get; set; }
}

internal sealed class PersonelDetaylarGetQueryHandler(
    IPersonelRepository personelRepository,
    ICurrentUserService currentUserService
    ) : IRequestHandler<PersonelDetaylarGetQuery, IQueryable<PersonelDetaylarGetQueryResponse>>
{
    public Task<IQueryable<PersonelDetaylarGetQueryResponse>> Handle(PersonelDetaylarGetQuery request, CancellationToken cancellationToken)
    {
        Guid personelId = Guid.Empty;

        if(request.PersonelId != Guid.Empty && request.PersonelId is not null)
        {
            personelId = request.PersonelId.Value;
        }
        else
        {
            Guid? userId = currentUserService.UserId;

            personelId = personelRepository.Where(p => p.UserId == userId).Select(p => p.Id).FirstOrDefault();
        }

        var personel = personelRepository.Where(p => p.Id == personelId)
            .Include(p => p.PersonelDetay)
            .Include(p => p.PersonelGorevlendirmeler)
                .ThenInclude(p => p.KurumsalBirim)
            .Include(p => p.PersonelGorevlendirmeler)
                .ThenInclude(p => p.KurumsalBirim)
            .Include(p => p.PersonelGorevlendirmeler)
                .ThenInclude(p => p.Pozisyon).ToList();

        var response = personel.Select(p => new PersonelDetaylarGetQueryResponse
        {
            Id = p.PersonelDetay.Id,
            PersonelId = p.Id,
            FullName = p.FullName,
            AvatarUrl = p.AvatarUrl,
            Iletisim = p.Iletisim,
            Adres = p.Adres,
            KurumsalBirimAd = p.PersonelGorevlendirmeler.Where(p => !p.IsDeleted).Select(p => p.KurumsalBirim?.Ad).FirstOrDefault() ?? "Bulunamamdı",
            PozisyonAd = p.PersonelGorevlendirmeler.Where(p => !p.IsDeleted).Select(p => p.Pozisyon?.Ad).FirstOrDefault() ?? "Bulunamamdı",
            GorevlendirmeTipi = p.PersonelGorevlendirmeler.Where(p => !p.IsDeleted).Select(p => p.GorevlendirmeTipi).FirstOrDefault()?.Name ?? "Bulunamamdı",
            CalismaSekli = p.PersonelGorevlendirmeler.Where(p => !p.IsDeleted).Select(p => p.CalismaSekli).FirstOrDefault()?.Name ?? "Bulunamamdı",
            BaslangicTarih = p.PersonelGorevlendirmeler.Where(p => !p.IsDeleted).Select(p => p.BaslangicTarihi).FirstOrDefault(),
            BitisTarih = p.PersonelGorevlendirmeler.Where(p => !p.IsDeleted).Select(p => p.BitisTarihi).FirstOrDefault(),

            // Kimlik Bilgileri
            TCKN = p.PersonelDetay.TCKN,
            AnaAdi = p.PersonelDetay.AnaAdi,
            BabaAdi = p.PersonelDetay.BabaAdi,
            DogumYeri = p.PersonelDetay.DogumYeri,
            DogumTarihi = p.DogumTarihi,
            MedeniHali = p.PersonelDetay?.MedeniHali,
            Cinsiyet = p.Cinsiyet != null ? p.Cinsiyet.Value ? "Erkek" : "Kadın" : "Bilinmiyor",
            Uyruk = p.PersonelDetay?.Uyruk,

            // İletişim Bilgileri
            
            IsTelefonu = p.PersonelDetay?.IsTelefonu,
            EpostaIs = p.PersonelDetay?.EpostaIs,
            PostaKodu = p.PersonelDetay?.PostaKodu,

            EgitimDurumu = p.PersonelDetay?.EgitimDurumu,
            MezuniyetBolumu = p.PersonelDetay?.MezuniyetBolumu,
            MezuniyetOkulu = p.PersonelDetay?.MezuniyetOkulu,
            MezuniyetTarihi = p.PersonelDetay?.MezuniyetTarihi,

            AskerlikDurumu = p.PersonelDetay?.AskerlikDurumu,
            AskerlikTarihi = p.PersonelDetay?.AskerlikTarihi,

            EhliyetSinifi = p.PersonelDetay?.EhliyetSinifi,
            EhliyetVerilisTarihi = p.PersonelDetay?.EhliyetVerilisTarihi,

            EngelliMi = p.PersonelDetay!.EngelliMi,
            EngelOrani = p.PersonelDetay?.EngelOrani,
            SaglikDurumu = p.PersonelDetay?.SaglikDurumu,
            KanGrubu = p.PersonelDetay?.KanGrubu,

            AcilDurumKisiAdi = p.PersonelDetay?.AcilDurumKisiAdi,
            AcilDurumKisiTelefon = p.PersonelDetay?.AcilDurumKisiTelefon,
            AcilDurumKisiYakinlik = p.PersonelDetay?.AcilDurumKisiYakinlik,

            CocukSayisi = p.PersonelDetay?.CocukSayisi,
            EsCalisiyorMu = p.PersonelDetay?.EsCalisiyorMu,

            BankaAdi = p.PersonelDetay?.BankaAdi,
            IBAN = p.PersonelDetay?.IBAN,
            
            Notlar = p.PersonelDetay?.Notlar,
            
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            CreateUserId = p.CreateUserId,
            CreateUserName = p.CreateuserName,
            UpdateAt = p.UpdateAt,
            UpdateUserId = p.UpdateUserId,
            UpdateUserName = p.UpdateuserName,
            IsDeleted = p.IsDeleted,
            DeleteAt = p.DeleteAt,
        });

        return Task.FromResult(response.AsQueryable());
    }
}
