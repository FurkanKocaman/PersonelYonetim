using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;

namespace PersonelYonetim.Server.Application.Bordro;
public sealed record BordroGetAllQuery(
    int Yil,
    int Ay
    ) : IRequest<IQueryable<BordroGetAllQueryResponse>>;

public sealed class BordroGetAllQueryResponse
{
    public Guid Id { get; set; }
    public Guid PersonelId { get; set; }
    public string FullName { get; set; } = default!;
    public string Durum { get; set; } = default!;
    public string? AvatarUrl { get; set; }

    public int Yil { get; set; }
    public int Ay { get; set; }

    //Girdiler
    public decimal BrutUcret { get; set; }
    public int SGKGun { get; set; }
    public decimal EkOdemelerToplam { get; set; }
    public decimal KesintilerToplam { get; set; }

    //Kazançlar
    public decimal GunlukUcret { get; set; }
    public int OdemeyeEsasGunSayisi {get;set;}
    public decimal FazlaCalismaUcretToplam { get; set; }

    //SGK
    public int FiiliCalisma { get;set; }
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
    public decimal TesvikliMaliyet {get; set; }
}

internal sealed class BordroGetAllQueryHandler(
    IBordroDonemRepository bordroDonemRepository,
    ICurrentUserService currentUserService
    ) : IRequestHandler<BordroGetAllQuery, IQueryable<BordroGetAllQueryResponse>>
{
    public Task<IQueryable<BordroGetAllQueryResponse>> Handle(BordroGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
            throw new Exception("Personel bulunamamdı");

        var bordroDonem = bordroDonemRepository.Where(p => p.TenantId == tenantId && p.Yil == request.Yil && p.Ay == request.Ay).Include(p => p.MaasPusulalar).ThenInclude(p => p.Personel).FirstOrDefault();
        if (bordroDonem is null)
            return Task.FromResult(Enumerable.Empty<BordroGetAllQueryResponse>().AsQueryable());

        var response = bordroDonem.MaasPusulalar
                .Where(p => !p.IsDeleted)
                .Select(p => new BordroGetAllQueryResponse
                {
                    Id = p.Id,
                    PersonelId = p.PersonelId,
                    FullName = p.Personel!.Ad + " " + p.Personel.Soyad,
                    Durum = p.Durum.Name,
                    AvatarUrl = p.Personel.AvatarUrl,

                    Yil = p.Yil,
                    Ay = p.Ay,

                    BrutUcret = p.BrutUcret,
                    SGKGun = p.SGKGunSayisi,
                    EkOdemelerToplam = 0,
                    KesintilerToplam = 0,

                    GunlukUcret = p.BrutUcret/p.SGKGunSayisi,
                    OdemeyeEsasGunSayisi = p.SGKGunSayisi,
                    FazlaCalismaUcretToplam = 0,

                    FiiliCalisma = p.FiiliCalismaGunu,
                    UcretliIzin = 0,
                    Raporlu = 0,
                    UcretsizIzin = 0,
                    DigerEksikGun = 0,

                    KumulatifToplam = p.KumulatifGelirVergisiMatrahiDonemSonu,
                    EkOdemeIstisnaToplam = p.GelirVergisiIstisnasiUygulanan,
                    GVAylikMatrah = p.GelirVergisiMatrahi,
                    GVOdemesi = p.OdenecekGelirVergisi,
                    
                    EkOdemeIstisna = p.DamgaVergisiIstisnasiUygulanan,
                    DVAylikMatrah = p.ToplamBrutKazanc,//Toplam ucret - asgari ücret
                    DVOdemesi = p.OdenecekDamgaVergisi,

                    YasalKesintiler = p.OdenecekGelirVergisi + p.OdenecekDamgaVergisi + p.IssizlikPrimiIsci+ p.SGKPrimiIsci,
                    OzelKesintiler = 0,
                    TumKesintiler = p.ToplamKesinti,
                    
                    EleGecenUcret = p.NetMaas,
                    Tesvikler = 0,
                    TesvikliMaliyet = p.ToplamIsverenMaliyeti,
                }).AsQueryable();

        return Task.FromResult(response);
    }
}
