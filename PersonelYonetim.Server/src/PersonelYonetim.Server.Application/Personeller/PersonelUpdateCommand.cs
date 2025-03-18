using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Users;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
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
    bool Cinsiyet,
    string Eposta,
    string Telefon,
    Adres Adres,
    Guid DepartmanId,
    Guid PozisyonId) :IRequest<Result<string>> ;

public sealed class PersonelUpdateCommandValidator : AbstractValidator<PersonelUpdateCommand>
{
    public PersonelUpdateCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");
    }
}
internal sealed class PersonelUpdateCommandHandler(
    IPersonelRepository personelRepository,
    IDepartmanRepository departmanRepository,
    IPozisyonRepository pozisyonRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork,
    ISender sender) : IRequestHandler<PersonelUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelUpdateCommand request, CancellationToken cancellationToken)
    {
        var personel = await personelRepository.FirstOrDefaultAsync(p => p.Id == request.Id);
        if (personel is null)
            return Result<string>.Failure("Personel bulunamadı");

        var user = await userManager.FindByEmailAsync(personel.Iletisim.Eposta);
        if (user is null)
            return Result<string>.Failure("Kullanıcı bulunamadı");

        var userUpdateCommand = new UserUpdateCommand(user.Id, request.Ad, request.Soyad, request?.Eposta);
        var userResult = await sender.Send(userUpdateCommand, cancellationToken);
        if (!userResult.IsSuccessful)
            return Result<string>.Failure("Kullanıcı güncellenirken hata oluştu");
        personel.Ad = request!.Ad;
        personel.Soyad = request.Soyad;
        personel.Iletisim.Eposta = request.Eposta;
        personel.Iletisim.Telefon = request.Telefon;
        personel.Adres = request.Adres;
        
        if (request?.DepartmanId is not null)
        {
            var departman = await departmanRepository.FirstOrDefaultAsync(d => d.Id == request.DepartmanId);
            if (departman is null)
                return Result<string>.Failure("Departman bulunamadı");
        }

        if (request?.PozisyonId is not null)
        {
            var pozisyon = await pozisyonRepository.FirstOrDefaultAsync(p => p.Id == request.PozisyonId);
            if (pozisyon is null)
                return Result<string>.Failure("Pozisyon bulunamadı");
        }

        var personelAtama = await personelAtamaRepository.FirstOrDefaultAsync(pd => pd.PersonelId == personel.Id);
        if (personelAtama is null)
            return Result<string>.Failure("Personel departman bilgisi bulunamadı");
        personelAtama.DepartmanId = request!.DepartmanId;
        personelAtama.PozisyonId = request.PozisyonId;

            

        personelRepository.Update(personel);
        personelAtamaRepository.Update(personelAtama);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Personel başarıyla güncellendi");
    }
}