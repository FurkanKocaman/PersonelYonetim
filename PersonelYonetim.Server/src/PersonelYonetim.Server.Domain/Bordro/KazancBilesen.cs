﻿using PersonelYonetim.Server.Domain.Abstractions;

namespace PersonelYonetim.Server.Domain.Bordro;
public class KazancBilesen : Entity
{
    public Guid MaasPusulaId { get; set; }
    public MaasPusula? MaasPusula { get; set; }
    public string KazancTuru { get; set; } = default!;
    public string? Aciklama { get; set; }
    public decimal Tutar { get; set; }
    public bool SGKMatrahinaDahil { get; set; }
    public bool GelirVergisiMatrahinaDahil { get; set; }
    public bool DamgaVergisiMatrahinaDahil { get; set; }
    public Guid TenantId { get; set; }
}
