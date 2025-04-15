using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Dtos;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.UnitOfWork;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.CalismaTakvimleri;

public sealed record CalismaTakvimiCreateCommand(
    string Ad,
    string? Aciklama,
    IEnumerable<CalismaGunDto> CalismaGunlerModel) : IRequest<Result<string>>;


internal sealed class CalismaTakvimiCreateCommandHandler(
    ICalismaTakvimRepository calismaTakvimRepository,
    ICalismaGunRepository calismaGunRepository,
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<CalismaTakvimiCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CalismaTakvimiCreateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdString))
                {
                    throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
                }

                var personel = personelRepository.GetAll()
                    .Where(p => p.UserId == Guid.Parse(userIdString))
                    .Select(p => new { p.Id, })
                    .FirstOrDefault();

                var personelAtama = personelAtamaRepository.GetAll()
                    .Where(p => p.PersonelId == personel!.Id)
                    .Select(p => new { p.SirketId, }) .FirstOrDefault();

                var calismaTakvimDb = await calismaTakvimRepository.FirstOrDefaultAsync(p => p.Ad == request.Ad );
                    
                if (calismaTakvimDb == null)
                {
                    CalismaTakvimi calismaTakvim = request.Adapt<CalismaTakvimi>();
                    //calismaTakvim.SirketId = personelAtama!.SirketId;
                    calismaTakvimRepository.Add(calismaTakvim);
                    calismaTakvimDb = calismaTakvim;
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }

                foreach(var calismaGun in request.CalismaGunlerModel)
                {
                    var isGunExist = await calismaGunRepository.AnyAsync(p => p.Gun == calismaGun.Gun && p.CalismaTakvimId == calismaTakvimDb.Id);
                    if (!isGunExist)
                    {
                        CalismaGun gun = calismaGun.Adapt<CalismaGun>();
                        gun.CalismaTakvimId = calismaTakvimDb.Id;
                        calismaGunRepository.Add(gun);
                    }
                }
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Takvim oluşturuldu");


            }catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Hata oluştu: " + ex.Message);
            }
        }
    }
}
