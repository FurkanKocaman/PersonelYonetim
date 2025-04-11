using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Personeller;
using PersonelYonetim.Server.Application.Sirketler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Auth;

public sealed record RegisterCommand(
   PersonelCreateCommand PersonelCreateCommand,
   SirketCreateCommand SirketCreateCommand
   ) : IRequest<Result<LoginCommandResponse>>;
//{
//    public RegisterCommand WithUpdatedPersonelCreateCommand(PersonelCreateCommand updatedPersonelCreateCommand)
//    {
//        return this with { PersonelCreateCommand = updatedPersonelCreateCommand };
//    }
//}

internal sealed class RegisterCommandHandler(
   ISirketRepository sirketRepository,
   IPersonelRepository personelRepository,
   UserManager<AppUser> userManager,
   IUnitOfWork unitOfWork,
   ISender sender) : IRequestHandler<RegisterCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        //using (var transaction = unitOfWork.BeginTransaction())
        //{
            try
            {
                var result = await sender.Send(request.SirketCreateCommand);
                if (result.IsSuccessful)
                {
                    var updatedPersonelCreateCommand = request.PersonelCreateCommand with
                    {
                        SirketId = Guid.Parse(result.Data!),
                        RolValue = RolTipiEnum.SirketYonetici.Value,
                    };

                    //request = request.WithUpdatedPersonelCreateCommand(updatedPersonelCreateCommand);

                    var response = await sender.Send(updatedPersonelCreateCommand);
                    if (response.IsSuccessful)
                    {
                        var sirket = await sirketRepository.FirstOrDefaultAsync(p => p.Id == Guid.Parse(result.Data!));
                        sirket.CreateUserId = Guid.Parse(response.Data!);
                        await unitOfWork.SaveChangesAsync();

                        var personel = personelRepository.GetAll().AsNoTracking().FirstOrDefault(p => p.UserId == Guid.Parse(response.Data!));
                        if (personel == null)
                        {
                            return Result<LoginCommandResponse>.Failure("Kullanıcı bulunamadı");
                        }

                        var user = await userManager.FindByIdAsync(response.Data!);
                        user!.CreateUserId = user.Id;
                        await unitOfWork.SaveChangesAsync();
                        //await unitOfWork.CommitTransactionAsync(transaction);

                        LoginCommand login = new(request.PersonelCreateCommand.Iletisim.Eposta, request.PersonelCreateCommand.Ad);
                        var loginRes = await sender.Send(login);
                        if (loginRes.IsSuccessful)
                        {
                            return loginRes;
                        }
                    }
                }
                else
                {
                    //await unitOfWork.RollbackTransactionAsync(transaction);
                    return Result<LoginCommandResponse>.Failure("Kayıt oluşturulamadı");
                }
                //await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<LoginCommandResponse>.Failure("Kayıt oluşturulamadı");
            }
            catch (Exception ex)
            {
                //await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<LoginCommandResponse>.Failure("Hata oluştu: " + ex.Message);
            }
        //}
    }
}
