﻿using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;

namespace PersonelYonetim.Server.Domain.Izinler;

public sealed class IzinKural : Entity
{
    public string Ad {  get; set; } = default!; 
    public string? Aciklama { get; set; }
    public ICollection<GorevlendirmeIzinKurali> GorevlendirmeIzinKurallar { get; set; } = new List<GorevlendirmeIzinKurali>();
    public ICollection<IzinTur> IzinTurler { get; set; } = new List<IzinTur>();
    public Guid? VarsayilanOnaySurecId { get; set; }
    public OnaySurec? VarsayilanOnaySurec { get; set; }
    public Guid TenantId { get; set; }
}
