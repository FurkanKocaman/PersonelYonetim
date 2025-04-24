using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Application.Users;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelUpdateCommand(
    Guid Id,
    string Ad,
    string Soyad,
    DateTimeOffset DogumTarihi,
    bool? Cinsiyet,
    string? AvatarUrl,
    Iletisim Iletisim,
    Adres Adres,

    Guid? KurumsalBirimId,
    Guid? PozisyonId,
    List<Guid>? RoleIdler,
    DateTimeOffset IseGirisTarihi,
    DateTimeOffset? IstenCikisTarihi,
    DateTimeOffset PozisyonBaslangicTarihi,
    DateTimeOffset? PozisyonBitisTarihi,
    bool BirincilGorevMi,
    int GorevlendirmeTipiValue,
    int CalismaSekliValue,
    Guid? RaporlananPersonelId,
    Guid? IzinKuralId,
    Guid? CalismaTakvimId,
    decimal BrutUcret,
    string? TabiOlduguKanun,
    string? SGKIsYeri,
    string? VergiDairesiAdi,
    string? MeslekKodu
    ) : IRequest<Result<string>>;

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
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IGorevlendirmeRoluRepository gorevlendirmeRoluRepository,
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
        if (request.AvatarUrl != null && request.AvatarUrl != "")
        {
            if (personel.AvatarUrl != null && request.AvatarUrl != null)
            {
                var filePath = Path.Combine(env.WebRootPath, "profile_images", Path.GetFileName(personel.AvatarUrl));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

            }

            personel.AvatarUrl = request.AvatarUrl;
        }
        personel.Iletisim = request.Iletisim;
        personel.Adres = request.Adres;

        if (request.PozisyonId.HasValue)
        {
            var pozisyon = await pozisyonRepository.FirstOrDefaultAsync(p => p.Id == request.PozisyonId);
            if (pozisyon is null)
                return Result<string>.Failure("Pozisyon bulunamadı");
        }

        var personelGorevlendirme = await personelGorevlendirmeRepository.WhereWithTracking(p => p.PersonelId == personel.Id && !p.IsDeleted && p.TenantId == personel.TenantId).Include(p => p.GorevlendirmeIzinKurali).FirstOrDefaultAsync();

        if (personelGorevlendirme is null)
            return Result<string>.Failure("Mevcut gorevlendirme bulunamamdı");

        if((request.KurumsalBirimId != null && request.KurumsalBirimId != personelGorevlendirme.KurumsalBirimId) || (request.PozisyonId != null && request.PozisyonId != personelGorevlendirme.PozisyonId))
        {
            personelGorevlendirme.IsActive = false;
            personelGorevlendirme.IsDeleted = true;
            personelGorevlendirme.PozisyonBitisTarihi = DateTimeOffset.Now;

            PersonelGorevlendirmeCreateCommand personelGorevlendirmeCreateCommand = new(personel.Id,request.KurumsalBirimId,request.PozisyonId,request.RoleIdler,request.IseGirisTarihi,request.IstenCikisTarihi,request.PozisyonBaslangicTarihi,request.PozisyonBitisTarihi,request.BirincilGorevMi,request.GorevlendirmeTipiValue,request.CalismaSekliValue,request.RaporlananPersonelId,request.IzinKuralId,request.CalismaTakvimId,request.BrutUcret,request.TabiOlduguKanun,request.SGKIsYeri,request.VergiDairesiAdi,request.MeslekKodu,personel.TenantId);
            var result = await sender.Send(personelGorevlendirmeCreateCommand);

            if (!result.IsSuccessful)
                return Result<string>.Failure(result.ErrorMessages![0]);
        }
        else
        {
            if(request.RoleIdler is not null)
            {
                var gorevlendirmeRoller = await gorevlendirmeRoluRepository
               .Where(p => p.PersonelGorevlendirmeId == personelGorevlendirme.Id)
               .ToListAsync(cancellationToken);

                var mevcutRolIdler = gorevlendirmeRoller.Select(r => r.RolId).ToList();
                var gelenRolIdler = request.RoleIdler;

                var ortakRoller = mevcutRolIdler.Intersect(gelenRolIdler).ToList();
                var eklenecekRoller = gelenRolIdler.Except(ortakRoller).ToList();
                var silinecekRoller = mevcutRolIdler.Except(ortakRoller).ToList();

                foreach (var roleId in eklenecekRoller)
                {
                    var yeniRol = new GorevlendirmeRolu
                    {
                        Id = Guid.NewGuid(),
                        PersonelGorevlendirmeId = personelGorevlendirme.Id,
                        RolId = roleId,
                    };

                    await gorevlendirmeRoluRepository.AddAsync(yeniRol);
                }

                if (silinecekRoller.Count < mevcutRolIdler.Count && silinecekRoller.Any())
                {
                    var silinecekEntities = gorevlendirmeRoller
                        .Where(r => silinecekRoller.Contains(r.RolId))
                        .ToList();

                    gorevlendirmeRoluRepository.DeleteRange(silinecekEntities);
                }
            }

            personelGorevlendirme.BirincilGorevMi = request.BirincilGorevMi;
            personelGorevlendirme.GorevlendirmeTipi = GorevlendirmeTipiEnum.FromValue(request.GorevlendirmeTipiValue);
            personelGorevlendirme.CalismaSekli = CalismaSekliEnum.FromValue(request.CalismaSekliValue);


            if (request.RaporlananPersonelId.HasValue)
                personelGorevlendirme.RaporlananGorevlendirmeId = request.RaporlananPersonelId;
            if(request.IzinKuralId.HasValue)
                personelGorevlendirme.GorevlendirmeIzinKurali!.IzinKuralId = request.IzinKuralId!.Value;
            if (request.CalismaTakvimId.HasValue)
                personelGorevlendirme.CalismaTakvimId = request.CalismaTakvimId!.Value;

            personelGorevlendirme.BrutUcret = request.BrutUcret;

            personelGorevlendirme.TabiOlduguKanun = request.TabiOlduguKanun;
            personelGorevlendirme.SGKIsyeri = request.SGKIsYeri;
            personelGorevlendirme.VergiDairesiAdi = request.VergiDairesiAdi;
            personelGorevlendirme.MeslekKodu = request.MeslekKodu;
        }
            return Result<string>.Succeed("Personel başarıyla güncellendi");
    }
}
