using FluentValidation;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Application.PersonelAtamalar;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Application.TakvimEtkinlikler;
using PersonelYonetim.Server.Application.Users;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
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
    Guid? YoneticiId,
    Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId,
    Guid? CalismaTavimiId,
    Guid? IzinKuralId,
    Guid? MesaiOnaySurecId,
    Guid? IzinOnaySurecId,
    int SozlesmeTuruValue,
    int CalismaSekliValue,
    DateTimeOffset? PozisyonBaslangicTarih,
    DateTimeOffset? SozlesmeBitisTarihi,
    int RolValue = 0
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

        UserCreateCommand userCreateCommand = new(personel.Ad, personel.Soyad, personel.Iletisim.Eposta);
        var userResult = await sender.Send(userCreateCommand, cancellationToken);
        if (!userResult.IsSuccessful)
        {
            return Result<string>.Failure("Kullanıcı oluşturulurken hata oluştu");
        } 

        personel.UserId = userResult.Data;
        personel.CreateUserId = userResult.Data;

        personelRepository.Add(personel);
        await unitOfWork.SaveChangesAsync(cancellationToken);
         
        PersonelAtamaCreateCommand personelAtamaCreateCommand =
            new(personel.Id, request.SirketId, request.SubeId, request.DepartmanId,
            request.PozisyonId, request.YoneticiId, request.RolValue, request.CalismaTavimiId, request.IzinKuralId,
            request.MesaiOnaySurecId, request.IzinOnaySurecId,request.SozlesmeTuruValue, request.CalismaSekliValue, request.SozlesmeBitisTarihi, request.PozisyonBaslangicTarih ?? DateTimeOffset.Now);

        var personelAtamaResult = await sender.Send(personelAtamaCreateCommand, cancellationToken);

        if (!personelAtamaResult.IsSuccessful)
            return Result<string>.Failure("Personel atama oluşturulamadı");

        var currentDate = DateTimeOffset.Now;

        var cizelge = new CalismaCizelge
        {
            PersonelId = personel.Id,
            Yil = currentDate.Year,
            Ay = currentDate.Month,
        };

        calismaCizelgeRepository.Add(cizelge);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        List<GunlukCalisma> GunlukCalismalar = new();
        int toplamGun = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
        for (int gun = 1; gun <= toplamGun; gun++)
        {
            var tarih = new DateTimeOffset(new DateTime(currentDate.Year, currentDate.Month, gun));
            var gunlukCalisma = new GunlukCalisma
            {
                CalismaCizelgesiId = cizelge.Id,
                Tarih = tarih,
                PersonelId = personel.Id,
            };

            GunlukCalismalar.Add(gunlukCalisma);
        }
        gunlukCalismaRepository.AddRange(GunlukCalismalar);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var currentYear = DateTimeOffset.Now.Year;
        var dogumGunEtkinlikTarihi = new DateTimeOffset(
            new DateTime(currentYear, personel.DogumTarihi.Month, personel.DogumTarihi.Day),
            personel.DogumTarihi.Offset);

        TakvimEtkinlikCreateCommand command = new($"{personel.FullName} doğum günü", null,dogumGunEtkinlikTarihi, dogumGunEtkinlikTarihi.AddHours(23), true,request.SirketId, null);
        await sender.Send(command, cancellationToken);

        emailService.SendAsync(personel.Iletisim.Eposta, $"Personel Yönetim Sistemi", $"<p>Personel Yönetim sistemi hesabınız oluşturulmuştur. Giriş Bilgileri: E-posta:{personel.Iletisim.Eposta}</br> Şifre:{request.Ad}</p>Giriş sayfasına gitmek için<a href='http://localhost:5173/login'>Giriş sayfası</a>", cancellationToken).Wait();

        return Result<string>.Succeed(userResult.Data.ToString());
    }
}

