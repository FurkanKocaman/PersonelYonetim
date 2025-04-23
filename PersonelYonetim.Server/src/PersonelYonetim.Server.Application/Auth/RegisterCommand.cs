using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.KurumsalBirimler;
using PersonelYonetim.Server.Application.Personeller;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Tenants;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Auth;

public sealed record RegisterCommand(
   KurumsalBirimCreateCommand KurumsalBirimCreateCommand,
   PersonelCreateCommand PersonelCreateCommand
   ) : IRequest<Result<LoginCommandResponse>>;

internal sealed class RegisterCommandHandler(
   IKurumsalBirimRepository kurumsalBirimRepository,
   IPersonelRepository personelRepository,
   UserManager<AppUser> userManager,
   IIzinKuralRepository izinKuralRepository,
   IIzinTurRepository izinTurRepository,
   IOnaySurecRepository onaySurecRepository,
   IOnaySurecAdimRepository onaySurecAdimRepository,
   ICalismaTakvimRepository calismaTakvimRepository,
   ICalismaGunRepository calismaGunRepository,
   IKurumsalBirimTipiRepository kurumsalBirimTipiRepository,
   ITenantRepository tenantRepository,
   IUnitOfWork unitOfWork,
   IDataSeeder dataSeeder,
   ISender sender) : IRequestHandler<RegisterCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Tenant tenant = new()
            {
                DisplayName = request.KurumsalBirimCreateCommand.Ad,
                Name = request.KurumsalBirimCreateCommand.Ad,
                AsgariUcret = 22104,

                SGKPrimIsciKesintiOrani = (decimal)0.14,
                SGKIssizlikPrimIsciKesintiOrani = (decimal)0.01,
                SGKPrimIsverenKesintiOrani = (decimal)0.2075,
                SGKIssizlikPrimIsverenKesintiOrani =(decimal)0.02,
                DamgaVergisiOrani = (decimal)0.00759,

                FazlaMesaiKatsayi = (decimal)1.5,
                HaftasonuFazlaMesaiKatsayi = (decimal)1.5,
                ResmiTatilFazlaMesaiKatsayi = (decimal)2,
            };

            Guid tenantId = tenant.Id;

            var (role,pozisyon) = await dataSeeder.SeedAsync(tenantId);
            List<KurumsalBirimTipi> birimTipleri = KurumsalYapiSeedData.OlusturDefaultYapi(tenantId);
            kurumsalBirimTipiRepository.AddRange(birimTipleri);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            KurumsalBirimTipi sirket = birimTipleri.FirstOrDefault(p => p.HiyerarsiSeviyesi == 1)!;

           var updatedKurumsalBirimCreateCommand = request.KurumsalBirimCreateCommand with
            {
               BirimTipiId = sirket.Id,
               TenantId = tenantId,
            };
            var result = await sender.Send(updatedKurumsalBirimCreateCommand);
            if (result.IsSuccessful)
            {

                IzinKural defaultIzinKural = new()
                {
                    Ad = "Default İzin Kural",
                    Aciklama = "Şirket oluşturulduğunda otomatik oluşan default izin kuralı",
                    TenantId = tenantId,
                };

                izinKuralRepository.Add(defaultIzinKural);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                var defaultIzinTurler = DefaultIzinTurler.GetDefaultIzinTurler(tenantId, defaultIzinKural.Id);
                izinTurRepository.AddRange(defaultIzinTurler);

                var defaultOnaySurecler = DefaultOnaySurec.GetDefaultOnaySurecleri(tenantId);

                onaySurecRepository.AddRange(defaultOnaySurecler);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                List<OnaySureciAdimi> defaultOnayAdimlari = new();

                foreach (var onaySurec in defaultOnaySurecler)
                {
                    defaultOnayAdimlari.Add(DefaultOnaySurec.GetDefaultOnayAdim(tenantId, onaySurec.Id, role.Id));
                }
                onaySurecAdimRepository.AddRange(defaultOnayAdimlari);

                CalismaTakvimi tamCalismaTakvim = DefaultCalismaTakvim.GetDefaultTamCalismaTakvim(tenantId);
                CalismaTakvimi yariCalismaTakvim = DefaultCalismaTakvim.GetDefaultYarimCalismaTakvim(tenantId);

                List<CalismaGun> tamCalismaGunler = DefaultCalismaTakvim.GetDefaultTamCalismaGunler(tamCalismaTakvim.Id, tenantId);
                List<CalismaGun> yariCalismaGunler = DefaultCalismaTakvim.GetDefaultYariCalismaGunler(yariCalismaTakvim.Id, tenantId);

                calismaTakvimRepository.AddRange([tamCalismaTakvim, yariCalismaTakvim]);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                calismaGunRepository.AddRange(tamCalismaGunler);
                calismaGunRepository.AddRange(yariCalismaGunler);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                var updatedPersonelCreateCommand = request.PersonelCreateCommand with
                {
                    TenantId = tenantId,
                    KurumsalBirimId = result.Data,
                    RoleId = [role.Id],
                    PozisyonId = pozisyon.Id,
                };

                var response = await sender.Send(updatedPersonelCreateCommand);
                if (response.IsSuccessful)
                {
                    var kurumsalBirim = await kurumsalBirimRepository.FirstOrDefaultAsync(p => p.Id == result.Data);
                    kurumsalBirim.CreateUserId = Guid.Parse(response.Data!);
                    await unitOfWork.SaveChangesAsync();

                    var personel = personelRepository.GetAll().AsNoTracking().FirstOrDefault(p => p.UserId == Guid.Parse(response.Data!));
                    if (personel == null)
                    {
                        return Result<LoginCommandResponse>.Failure("Kullanıcı bulunamadı");
                    }

                    var user = await userManager.FindByIdAsync(response.Data!);
                    user!.CreateUserId = user.Id;
                    tenant.CreateUserId = user.Id;
                    await tenantRepository.AddAsync(tenant,cancellationToken);
                    await unitOfWork.SaveChangesAsync();

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
                return Result<LoginCommandResponse>.Failure("Kayıt oluşturulamadı");
            }
            return Result<LoginCommandResponse>.Failure("Kayıt oluşturulamadı");
        }
        catch (Exception ex)
        {
            return Result<LoginCommandResponse>.Failure("Hata oluştu: " + ex.Message);
        }
    }
}
