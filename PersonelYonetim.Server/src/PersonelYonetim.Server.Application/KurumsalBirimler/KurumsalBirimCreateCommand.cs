using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.UnitOfWork;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.KurumsalBirimler;
public sealed record KurumsalBirimCreateCommand(
    string Ad,
    string? Kod,
    Guid BirimTipiId,
    Guid? UstBirimId,
    Guid? TenantId
    ) : IRequest<Result<Guid>>;

internal sealed class KurumslBirimCreateCommandHandler(
    IKurumsalBirimRepository kurumsalBirimRepository,
    IKurumsalBirimTipiRepository kurumsalBirimTipiRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IPersonelRepository personelRepository
    ) : IRequestHandler<KurumsalBirimCreateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(KurumsalBirimCreateCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
               var tenantId = Guid.Empty;

               if(request.TenantId is null)
               {
                    var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdString))
                    {
                        throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
                    }

                    var personel = personelRepository
                        .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted).Select(p => new { p.TenantId }).FirstOrDefault();

                    if (personel == null)
                    {
                        throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
                    }
                    tenantId = personel.TenantId;
                }
                else
                {
                    tenantId = request.TenantId.Value;
                }

                var isKurumsalBirimExist = await kurumsalBirimRepository.AnyAsync(p => p.Ad == request.Ad && p.TenantId == tenantId);
                if (isKurumsalBirimExist)
                {
                    await unitOfWork.RollbackTransactionAsync(transaction);
                    return Result<Guid>.Failure("Bu isimde bir birim zaten mevcut");
                }

      


                var isBirimTipiExist = await kurumsalBirimTipiRepository.AnyAsync(p => p.Id == request.BirimTipiId && p.TenantId == tenantId);
                if (!isBirimTipiExist)
                {
                    await unitOfWork.RollbackTransactionAsync(transaction);
                    return Result<Guid>.Failure("Birim tipi bulunamadı");
                }

                var kurumsalBirim = request.Adapt<KurumsalBirim>();

                kurumsalBirim.TenantId = tenantId;

                kurumsalBirimRepository.Add(kurumsalBirim);

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<Guid>.Succeed(kurumsalBirim.Id);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<Guid>.Failure(ex.Message);
            }
        }
    }
}

