using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Users;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelUpdateCommand(
    Guid Id,
    string Ad,
    string Soyad,
    DateTimeOffset DogumTarihi,
    bool Cinsiyet,
    string? ProfilResimUrl,
    Iletisim Iletisim,
    Adres Adres,
    Guid? YoneticiId,
    Guid? PozisyonId,
    Guid? CalismaTakvimiId,
    int SozlesmeTuruValue,
    DateTime PozisyonBaslangicTarih,
    DateTime? SozlesmeBitisTarihi,
    Guid? IzinKuralId,
    int RolValue) : IRequest<Result<string>>;

public sealed class PersonelUpdateCommandValidator : AbstractValidator<PersonelUpdateCommand>
{
    public PersonelUpdateCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");
        RuleFor(x => x.Ad).NotEmpty().WithMessage("Ad boş olamaz");
        RuleFor(x => x.Soyad).NotEmpty().WithMessage("Soyad boş olamaz");
        RuleFor(x => x.Iletisim.Eposta).NotEmpty().EmailAddress().WithMessage("Geçerli bir e-posta giriniz");
    }
}

internal sealed class PersonelUpdateCommandHandler(
    IPersonelRepository personelRepository,
    IPozisyonRepository pozisyonRepository,
    UserManager<AppUser> userManager,
    IWebHostEnvironment env,
    ISender sender) : IRequestHandler<PersonelUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelUpdateCommand request, CancellationToken cancellationToken)
    {
        var personel = await personelRepository.FirstOrDefaultAsync(p => p.Id == request.Id);
        if (personel is null || personel!.IsDeleted)
            return Result<string>.Failure("Personel bulunamadı");

        var user = await userManager.FindByEmailAsync(personel.Iletisim.Eposta);
        if (user is null || user!.IsDeleted)
            return Result<string>.Failure("Kullanıcı bulunamadı");

        var userUpdateCommand = new UserUpdateCommand(user.Id, request.Ad, request.Soyad, request.Iletisim.Eposta);
        var userResult = await sender.Send(userUpdateCommand, cancellationToken);
        if (!userResult.IsSuccessful)
            return Result<string>.Failure("Kullanıcı güncellenirken hata oluştu");

        personel.Ad = request.Ad;
        personel.Soyad = request.Soyad;
        personel.DogumTarihi = request.DogumTarihi;
        personel.Cinsiyet = request.Cinsiyet;
        if (request.ProfilResimUrl != null && request.ProfilResimUrl != "")
        {
            if (personel.AvatarUrl != null && request.ProfilResimUrl != null)
            {
                var filePath = Path.Combine(env.WebRootPath, "profile_images", Path.GetFileName(personel.AvatarUrl));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

            }

            personel.AvatarUrl = request.ProfilResimUrl;
        }
        personel.Iletisim = request.Iletisim;
        personel.Adres = request.Adres;

        if (request.PozisyonId.HasValue)
        {
            var pozisyon = await pozisyonRepository.FirstOrDefaultAsync(p => p.Id == request.PozisyonId);
            if (pozisyon is null)
                return Result<string>.Failure("Pozisyon bulunamadı");
        }

        //var personelAtama = await personelAtamaRepository.FirstOrDefaultAsync(pd => pd.PersonelId == personel.Id && pd.SirketId == request.SirketId && pd.IsDeleted == false);
        //if (request.SirketId != personelAtama.SirketId || request.SubeId != personelAtama.SubeId || request.DepartmanId != personelAtama.DepartmanId || request.PozisyonId != personelAtama.PozisyonId || request.RolValue != personelAtama.RolTipi.Value || request.YoneticiId != personelAtama.YoneticiId)
        //{
        //    personelAtama.IsDeleted = true;

        //    //PersonelAtamaCreateCommand personelAtamaCreateCommand = new(personel.Id, request.SirketId, request.SubeId, request.DepartmanId, request.PozisyonId, request.YoneticiId, request.RolValue, request.CalismaTakvimiId, request.SozlesmeTuruValue, request.SozlesmeBitisTarihi, request.PozisyonBaslangicTarih, request.IzinKuralId );
        //    //var result = await sender.Send(personelAtamaCreateCommand);
        //}
        //else
        //{
        //    if (request.IzinKuralId is not null)
        //        //personelAtama.IzinKuralId = request.IzinKuralId;
        //    if (request.CalismaTakvimiId is not null)
        //        personelAtama.CalismaTakvimId = request.CalismaTakvimiId;
        //}


        //personelRepository.Update(personel);
        //personelAtamaRepository.Update(personelAtama);
        //await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Personel başarıyla güncellendi");
    }
}
