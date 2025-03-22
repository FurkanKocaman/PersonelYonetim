using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Application.PersonelAtamalar;
using PersonelYonetim.Server.Application.Users;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelCreateCommand(
    string Ad,
    string Soyad,
    DateTimeOffset DogumTarihi,
    bool? Cinsiyet,
    string? ProfilResimUrl,
    Iletisim Iletisim,
    Adres Adres,
    DateTimeOffset IseGirisTarihi,
    Guid? YoneticiId,
    Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId,
    int CalismaSekliValue,
    int SozlesmeTuruValue,
    DateTimeOffset? SozlesmeBitisTarihi,
    int YoneticiTipiValue = 0
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
    IUnitOfWork unitOfWork,
    ISender sender) : IRequestHandler<PersonelCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelCreateCommand request, CancellationToken cancellationToken)
    {
        var personelVarMi = await personelRepository.AnyAsync(p => p.Iletisim.Eposta == request.Iletisim.Eposta);
        if (personelVarMi)
            return Result<string>.Failure("Personel zaten mevcut");

        Personel personel = request.Adapt<Personel>();

        IEnumerable<string> Rol = [RoleClaims.Calisan];

        if (request.YoneticiTipiValue == 2)
            Rol = [RoleClaims.SirketSahibi];

        if (request.YoneticiTipiValue == 1 || request.YoneticiTipiValue == 0)
            Rol = [RoleClaims.Yonetici];

        UserCreateCommand userCreateCommand = new(personel.Ad, personel.Soyad, personel.Iletisim.Eposta, request.SirketId, Rol);
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
            new(personel, request.SirketId,request.SubeId,request.DepartmanId,
            request.PozisyonId,request.YoneticiTipiValue,request.CalismaSekliValue,
            request.SozlesmeTuruValue,request.SozlesmeBitisTarihi);

        var personelAtamaResult = await sender.Send(personelAtamaCreateCommand, cancellationToken);

        if (!personelAtamaResult.IsSuccessful)
            Result<string>.Failure("Personel atama oluşturulamadı");

        return userResult.Data.ToString();
    }
}

