using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Personeller;
using PersonelYonetim.Server.Application.Sirketler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Auth;

public sealed record RegisterCommand (
    string Ad,
    string Soyad,
    DateTimeOffset DogumTarihi,
    bool? Cinsiyet,
    Iletisim PersonelIletisim,
    Adres PersonelAdres,
    string SirketAd,
     DateTimeOffset SirketKurulusTarihi,
    Iletisim SirketIletisim,
    Adres SirketAdres,
    Guid? SirketId) : IRequest<Result<LoginCommandResponse>>;

internal sealed class RegisterCommandHandler(
    ISirketRepository sirketRepository,
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork,
    ISender sender) : IRequestHandler<RegisterCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        SirketCreateCommand sirketCreateCommand = new(request.SirketAd,null,null,request.SirketAdres,request.SirketIletisim);
        var result = await sender.Send(sirketCreateCommand);
        if (result.IsSuccessful)
        {
            PersonelCreateCommand personelCreateCommand = new(
                request.Ad,
                request.Soyad,
                request.DogumTarihi,
                request.Cinsiyet,
                null,
                request.PersonelIletisim,
                request.PersonelAdres,
                DateTimeOffset.Now,
                null,
                Guid.Parse(result.Data!),
                null, null, null,2);

            var response = await sender.Send(personelCreateCommand);
            if (response.IsSuccessful)
            {
                var sirket = await sirketRepository.FirstOrDefaultAsync(p => p.Id == Guid.Parse(result.Data!));
                sirket.CreateUserId = Guid.Parse(response.Data!);
                await unitOfWork.SaveChangesAsync();

                var user = await userManager.FindByIdAsync(response.Data!);
                user!.CreateUserId = user.Id;
                await unitOfWork.SaveChangesAsync();

                LoginCommand login = new(request.PersonelIletisim.Eposta, request.Ad);
                var loginRes = await sender.Send(login);
                return loginRes;
            }
        }
        return Result<LoginCommandResponse>.Failure("Kayıt oluşturulamadı");
    }
}
