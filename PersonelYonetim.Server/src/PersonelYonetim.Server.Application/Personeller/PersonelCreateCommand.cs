using FluentValidation;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Application.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Application.TakvimEtkinlikler;
using PersonelYonetim.Server.Application.Users;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using TS.Result;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelCreateCommand(
    string Ad,
    string Soyad,
    DateTimeOffset DogumTarihi,
    bool? Cinsiyet,
    string? AvatarUrl,
    Iletisim Iletisim,
    Adres Adres,

    Guid? KurumsalBirimId,
    Guid? PozisyonId,
    List<Guid>? RoleId,
    DateTimeOffset IseGirisTarihi,
    DateTimeOffset? IstenCikisTarihi,
    DateTimeOffset PozisyonBaslangicTarihi,
    DateTimeOffset? PozisyonBitisTarihi,

    bool BirincilGorevMi,
    int GorevlendirmeTipiValue,
    int CalismaSekliValue,
    Guid? RaporlananGorevlendirmeId,
    Guid? IzinKuralId,
    Guid? CalismaTakvimiId,
    decimal BrutUcret,
    Guid? TenantId
    ) : IRequest<Result<string>>;

public sealed class PersonelCreateCommandValidator : AbstractValidator<PersonelCreateCommand>
{
    public PersonelCreateCommandValidator()
    {
        RuleFor(x => x.Ad).NotEmpty().WithMessage("Ad boş olamaz").MaximumLength(50);
        RuleFor(x => x.Soyad).NotEmpty().WithMessage("Soyad boş olamaz").MaximumLength(50);
        RuleFor(x => x.DogumTarihi).NotEmpty().WithMessage("Doğum tarihi boş olamaz");
        RuleFor(x => x.Iletisim.Eposta).NotEmpty().WithMessage("Eposta boş olamaz").EmailAddress();
    }
}
internal sealed class PersonelCreateCommandHandler(
    IPersonelRepository personelRepository, 
    IPersonelDetayRepository personelDetayRepository,
    ICalismaCizelgeRepository calismaCizelgeRepository,
    IGunlukCalismaRepository gunlukCalismaRepository,
    IEmailService emailService,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ISender sender
    ) : IRequestHandler<PersonelCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelCreateCommand request, CancellationToken cancellationToken)
    {
        //using (var transaction = unitOfWork.BeginTransaction())
        //{
            try
            {

                Guid? tenantId = null;

                if (request.TenantId is null)
                    tenantId = currentUserService.TenantId;
                else
                    tenantId = request.TenantId;

                if (tenantId is null)
                    return Result<string>.Failure("TenantId bulunamadı");

                var personelVarMi = await personelRepository.AnyAsync(p => p.Iletisim.Eposta == request.Iletisim.Eposta);
                if (personelVarMi)
                    return Result<string>.Failure("Personel zaten mevcut");

                Personel personel = request.Adapt<Personel>();
                personel.TenantId = tenantId.Value;

                UserCreateCommand userCreateCommand = new(personel.Ad, personel.Soyad, personel.Iletisim.Eposta, tenantId.Value);
                var userResult = await sender.Send(userCreateCommand, cancellationToken);
                if (!userResult.IsSuccessful)
                {
                    return Result<string>.Failure("Kullanıcı oluşturulurken hata oluştu");
                }

                personel.UserId = userResult.Data;
                personel.CreateUserId = userResult.Data;

                personelRepository.Add(personel);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                PersonelDetay personelDetay = new()
                {
                    PersonelId = personel.Id,
                    TenantId = tenantId.Value,
                };

                personelDetayRepository.Add(personelDetay);

                PersonelGorevlendirmeCreateCommand personelGorevlendirmeCreateCommand = new(personel.Id, request.KurumsalBirimId, request.PozisyonId, request.RoleId, request.IseGirisTarihi, request.IstenCikisTarihi,request.PozisyonBaslangicTarihi,request.PozisyonBitisTarihi, request.BirincilGorevMi, request.GorevlendirmeTipiValue, request.CalismaSekliValue, request.RaporlananGorevlendirmeId, request.IzinKuralId, request.CalismaTakvimiId, request.BrutUcret, null, null, null, null, tenantId.Value);

                var gorevlendirmeCreateRes = await sender.Send(personelGorevlendirmeCreateCommand);
                if (!gorevlendirmeCreateRes.IsSuccessful)
                {
                    //await unitOfWork.RollbackTransactionAsync(transaction);
                    return Result<string>.Failure("Görevlendirilme oluşturulurken hata oluştu");
                }
                   

                var currentDate = DateTimeOffset.Now;

                var cizelge = new CalismaCizelge
                {
                    PersonelId = personel.Id,
                    Yil = currentDate.Year,
                    Ay = currentDate.Month,
                    TenantId = tenantId.Value,
                };

                calismaCizelgeRepository.Add(cizelge);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                List<GunlukCalisma> GunlukCalismalar = new();
                int toplamGun = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                for (int gun = currentDate.Day; gun <= toplamGun; gun++)
                {
                    var tarih = new DateTimeOffset(new DateTime(currentDate.Year, currentDate.Month, gun));
                    var gunlukCalisma = new GunlukCalisma
                    {
                        CalismaCizelgesiId = cizelge.Id,
                        Tarih = tarih,
                        PersonelId = personel.Id,
                        TenantId = tenantId.Value,
                    };

                    GunlukCalismalar.Add(gunlukCalisma);
                }
                gunlukCalismaRepository.AddRange(GunlukCalismalar);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                var currentYear = DateTimeOffset.Now.Year;
                var dogumGunEtkinlikTarihi = new DateTimeOffset(
                    new DateTime(currentYear, personel.DogumTarihi.Month, personel.DogumTarihi.Day),
                    personel.DogumTarihi.Offset);

                TakvimEtkinlikCreateCommand command = new($"{personel.FullName} doğum günü", null, dogumGunEtkinlikTarihi, dogumGunEtkinlikTarihi.AddHours(23), true, tenantId.Value, null);
                var res = await sender.Send(command, cancellationToken);
                if (!res.IsSuccessful)
                {
                    //await unitOfWork.RollbackTransactionAsync(transaction);
                    return Result<string>.Failure("Takvim etkinliği oluşturulurken hata oluştu");
                }

                emailService.SendAsync(personel.Iletisim.Eposta, $"Personel Yönetim Sistemi", $"<p>Personel Yönetim sistemi hesabınız oluşturulmuştur. Giriş Bilgileri: E-posta:{personel.Iletisim.Eposta}</br> Şifre:{request.Ad}</p>Giriş sayfasına gitmek için<a href='http://localhost:5173/login'>Giriş sayfası</a>", cancellationToken).Wait();

                //await unitOfWork.CommitTransactionAsync(transaction);
                return Result<string>.Succeed(userResult.Data.ToString());
            }catch(Exception ex)
            {
                //await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Succeed("Personel oluşturulurken hata oluştu: "+ex);
            }
        }
    //}
}

