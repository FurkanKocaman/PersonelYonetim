//using Microsoft.Extensions.Logging;
//using PersonelYonetim.Server.Application.Services;
//using PersonelYonetim.Server.Domain.OnaySurecleri;
//using PersonelYonetim.Server.Domain.Personeller;
//using PersonelYonetim.Server.Domain.Rols;
//using TS.Result;

//namespace PersonelYonetim.Server.Infrastructure.Services;
//internal sealed class OnaylayiciResolverService(
//    IPersonelRepository personelRepository,
//    ILogger<OnaylayiciResolverService> logger) : IOnaylayiciResolverService
//{
//    public async Task<Result<Guid?>> OnaylayiciGetirAsync(OnaySureciAdimi adim, Personel requester, CancellationToken cancellationToken)
//    {
//        try
//        {
//            if (adim.PersonelId.HasValue)
//            {
//                var personelExist = await personelRepository.AnyAsync(p => p.Id == adim.PersonelId, cancellationToken);
//                if (personelExist)
//                {
//                    logger.LogInformation("Onaylayıcı, adım [{AdimId}] için doğrudan atanan personel [{PersonelId}] olarak çözümlendi.", adim.Id, adim.PersonelId.Value);
//                    return Result<Guid?>.Succeed(adim.PersonelId);
//                }
//                else
//                {
//                    logger.LogWarning("Doğrudan atanan onaylayıcı personel [{PersonelId}] bulunamadı veya geçerli değil.", adim.PersonelId.Value);
//                    return Result<Guid?>.Failure($"Atanan onaylayıcı ({adim.PersonelId.Value}) bulunamadı.");
//                }
//            }

//            if(adim.Rol is not null)
//            {
//                logger.LogInformation("Onaylayıcı, adım [{AdimId}] için rol [{Rol}] bazında çözümleniyor.", adim.Id, adim.Rol.Name);

//                if(adim.Rol == RolTipiEnum.DepartmanYonetici)
//                {

//                }

//            }


//        }catch(Exception ex)
//        {
//            logger.LogError(ex, "Onaylayıcı çözümlenirken beklenmedik bir hata oluştu. Adım ID: {AdimId}", adim.Id);
//            return Result<Guid?>.Failure($"Onaylayıcı çözümlenirken hata: {ex.Message}");
//        }
//    }
//}
