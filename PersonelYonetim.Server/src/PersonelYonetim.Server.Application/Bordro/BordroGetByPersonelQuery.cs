using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;

namespace PersonelYonetim.Server.Application.Bordro;
public sealed record BordroGetByPersonelQuery(
    int? yil 
    ) : IRequest<IQueryable<BordroGetByPersonelQueryResponse>>;

public class BordroGetByPersonelQueryResponse
{
    public Guid Id { get; set; }
    public Guid PersonelId { get; set; }
    public string FullName { get; set; } = default!;
    public int Yil { get; set; }
    public int Ay { get; set; }
    public string Durum { get; set; } = default!;

    //Girdiler
    public decimal BrutUcret { get; set; }
    public int SGKGun { get; set; }
    public decimal EkOdemelerToplam { get; set; }
    public decimal KesintilerToplam { get; set; }

    //Kazançlar
    public decimal GunlukUcret { get; set; }
    public int OdemeyeEsasGunSayisi { get; set; }
    public decimal FazlaCalismaUcretToplam { get; set; }

    //SGK
    public int FiiliCalisma { get; set; }
    public decimal UcretliIzin { get; set; }
    public decimal Raporlu { get; set; }
    public decimal UcretsizIzin { get; set; }
    public decimal DigerEksikGun { get; set; }

    //Gelir Vergisi
    public decimal KumulatifToplam { get; set; }
    public decimal EkOdemeIstisnaToplam { get; set; }
    public decimal GVAylikMatrah { get; set; }
    public decimal GVOdemesi { get; set; }

    //Damga Vergisi
    public decimal EkOdemeIstisna { get; set; }
    public decimal DVAylikMatrah { get; set; }
    public decimal DVOdemesi { get; set; }

    //Kesintiler
    public decimal YasalKesintiler { get; set; } = 0;
    public decimal OzelKesintiler { get; set; } = 0;
    public decimal TumKesintiler { get; set; }

    //Maliyet
    public decimal EleGecenUcret { get; set; }
    public decimal Tesvikler { get; set; }
    public decimal TesvikliMaliyet { get; set; }
}

internal sealed class BordroGetByPersonelQueryHandler(
    IBordroDonemRepository bordroDonemRepository,
    ICurrentUserService currentUserService
    ) : IRequestHandler<BordroGetByPersonelQuery, IQueryable<BordroGetByPersonelQueryResponse>>
{
    public Task<IQueryable<BordroGetByPersonelQueryResponse>> Handle(BordroGetByPersonelQuery request, CancellationToken cancellationToken)
    {

        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
            throw new Exception("Personel bulunamamdı");

        var bordroDonem = bordroDonemRepository.Where(p => p.TenantId == tenantId && request.yil != null ? p.Yil == request.yil : true).Include(p => p.MaasPusulalar).ThenInclude(p => p.Personel).FirstOrDefault();
        if (bordroDonem is null)
            return Task.FromResult(Enumerable.Empty<BordroGetByPersonelQueryResponse>().AsQueryable());

        var response = bordroDonem.MaasPusulalar
            .Where(p => p.Personel!.UserId == userId && !p.IsDeleted)
                .Select(p => new BordroGetByPersonelQueryResponse
                {
                    Id = p.Id,
                    PersonelId = p.PersonelId,
                    FullName = p.Personel!.Ad + " " + p.Personel.Soyad,
                    Durum = p.Durum.Name,
                    Yil = p.BordroDonem?.Yil ?? 0,
                    Ay = p.BordroDonem?.Ay ?? 0,

                    BrutUcret = p.BrutUcret,
                    SGKGun = 30,
                    EkOdemelerToplam = 0,
                    KesintilerToplam = 0,

                    GunlukUcret = p.BrutUcret / 30,
                    OdemeyeEsasGunSayisi = 30,
                    FazlaCalismaUcretToplam = 0,

                    FiiliCalisma = 22,
                    UcretliIzin = 0,
                    Raporlu = 0,
                    UcretsizIzin = 0,
                    DigerEksikGun = 0,

                    KumulatifToplam = p.KumulatifGelirVergisiMatrahiDonemSonu,
                    EkOdemeIstisnaToplam = p.GelirVergisiIstisnasiUygulanan,
                    GVAylikMatrah = p.GelirVergisiMatrahi,
                    GVOdemesi = p.OdenecekGelirVergisi,

                    EkOdemeIstisna = p.DamgaVergisiIstisnasiUygulanan,
                    DVAylikMatrah = p.BrutUcret - 22003,//Toplam ucret - asgari ücret
                    DVOdemesi = p.OdenecekDamgaVergisi,

                    YasalKesintiler = p.OdenecekGelirVergisi + p.OdenecekDamgaVergisi + p.IssizlikPrimiIsci,
                    OzelKesintiler = 0,
                    TumKesintiler = p.ToplamKesinti,

                    EleGecenUcret = p.NetMaas,
                    Tesvikler = 0,
                    TesvikliMaliyet = p.ToplamIsverenMaliyeti,
                }).AsQueryable();

        return Task.FromResult(response.AsQueryable());
    }
}
