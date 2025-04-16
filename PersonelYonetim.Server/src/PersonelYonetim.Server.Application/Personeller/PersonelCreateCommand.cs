using FluentValidation;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Application.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Application.Users;
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

    Guid KurumsalBirimId,
    Guid PozisyonId,
    Guid RoleId,
    DateTimeOffset BaslangicTarihi,
    DateTimeOffset? BitisTarihi,
    bool BirincilGorevMi,
    int GorevlendirmeTipiValue,
    int CalismaSekliValue,
    Guid? RaporlananGorevlendirmeId,
    Guid? IzinKuralId,
    Guid? CalismaTakvimId,
    decimal BrutUcret,
    Guid TenantId
    ) : IRequest<Result<string>>;

public sealed class PersonelCreateCommandValidator : AbstractValidator<PersonelCreateCommand>
{
    public PersonelCreateCommandValidator()
    {
        RuleFor(x => x.Ad).NotEmpty().WithMessage("Ad boş olamaz").MaximumLength(50);
        RuleFor(x => x.Soyad).NotEmpty().WithMessage("Soyad boş olamaz").MaximumLength(50);
        RuleFor(x => x.DogumTarihi).NotEmpty().WithMessage("Doğum tarihi boş olamaz");
        RuleFor(x => x.Iletisim.Eposta).NotEmpty().WithMessage("Eposta boş olamaz").EmailAddress();
        RuleFor(x => x.Iletisim.Telefon).NotEmpty().WithMessage("Telefon boş olamaz");
        RuleFor(x => x.Adres.Ulke).NotEmpty().WithMessage("Ülke boş olamaz");
        RuleFor(x => x.Adres.Sehir).NotEmpty().WithMessage("Şehir boş olamaz");
        RuleFor(x => x.Adres.Ilce).NotEmpty().WithMessage("İlçe boş olamaz");
        RuleFor(x => x.Adres.TamAdres).NotEmpty().WithMessage("Tam adres boş olamaz");
    }
}
internal sealed class PersonelCreateCommandHandler(
    IPersonelRepository personelRepository, 
    ICalismaCizelgeRepository calismaCizelgeRepository,
    IGunlukCalismaRepository gunlukCalismaRepository,
    IEmailService emailService,
    IUnitOfWork unitOfWork,
    ISender sender) : IRequestHandler<PersonelCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelCreateCommand request, CancellationToken cancellationToken)
    {

        var personelVarMi = await personelRepository.AnyAsync(p => p.Iletisim.Eposta == request.Iletisim.Eposta);
        if (personelVarMi)
            return Result<string>.Failure("Personel zaten mevcut");

        Personel personel = request.Adapt<Personel>();

        UserCreateCommand userCreateCommand = new(personel.Ad, personel.Soyad, personel.Iletisim.Eposta,request.TenantId);
        var userResult = await sender.Send(userCreateCommand, cancellationToken);
        if (!userResult.IsSuccessful)
        {
            return Result<string>.Failure("Kullanıcı oluşturulurken hata oluştu");
        } 

        personel.UserId = userResult.Data;
        personel.CreateUserId = userResult.Data;

        personelRepository.Add(personel);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        PersonelGorevlendirmeCreateCommand personelGorevlendirmeCreateCommand = new(personel.Id,request.KurumsalBirimId,request.PozisyonId,request.RoleId,request.BaslangicTarihi,request.BitisTarihi,request.BirincilGorevMi,request.GorevlendirmeTipiValue,request.CalismaSekliValue,request.RaporlananGorevlendirmeId,request.IzinKuralId,request.CalismaTakvimId,request.BrutUcret,request.TenantId);

        var gorevlendirmeCreateRes = await sender.Send(personelGorevlendirmeCreateCommand);
        if (!gorevlendirmeCreateRes.IsSuccessful)
            return Result<string>.Failure("Görevlendirilme oluşturulurken hata oluştu");

        var currentDate = DateTimeOffset.Now;

        var cizelge = new CalismaCizelge
        {
            PersonelId = personel.Id,
            Yil = currentDate.Year,
            Ay = currentDate.Month,
            TenantId = request.TenantId,
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
                TenantId = request.TenantId,
            };

            GunlukCalismalar.Add(gunlukCalisma);
        }
        gunlukCalismaRepository.AddRange(GunlukCalismalar);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var currentYear = DateTimeOffset.Now.Year;
        var dogumGunEtkinlikTarihi = new DateTimeOffset(
            new DateTime(currentYear, personel.DogumTarihi.Month, personel.DogumTarihi.Day),
            personel.DogumTarihi.Offset);

        //TakvimEtkinlikCreateCommand command = new($"{personel.FullName} doğum günü", null,dogumGunEtkinlikTarihi, dogumGunEtkinlikTarihi.AddHours(23), true,request.SirketId, null);
        //await sender.Send(command, cancellationToken);

        emailService.SendAsync(personel.Iletisim.Eposta, $"Personel Yönetim Sistemi", $"<p>Personel Yönetim sistemi hesabınız oluşturulmuştur. Giriş Bilgileri: E-posta:{personel.Iletisim.Eposta}</br> Şifre:{request.Ad}</p>Giriş sayfasına gitmek için<a href='http://localhost:5173/login'>Giriş sayfası</a>", cancellationToken).Wait();

        return Result<string>.Succeed(userResult.Data.ToString());
    }
}

