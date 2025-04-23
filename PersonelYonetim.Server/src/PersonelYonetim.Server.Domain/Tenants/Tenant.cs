using PersonelYonetim.Server.Domain.Abstractions;

public class Tenant : Entity
{
    // Temel Bilgiler
    public string Name { get; set; } = default!;       
    public string DisplayName { get; set; } = default!;    
    public string? LogoUrl { get; set; }


    public string? SGKIsyeri { get; set; }
    public string? SGKNumarasi { get; set; }
    public string? VergiDairesiAdi { get; set; }
    public string? VergiNumarasi { get; set; }
    public string? TabiOlduguKanun { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone { get; set; } 
    public string? Email { get; set; }

    //Bordro için gerekli değişkenler
    public decimal AsgariUcret { get; set; }
    public decimal SGKPrimIsciKesintiOrani { get; set; }
    public decimal SGKIssizlikPrimIsciKesintiOrani { get; set; }
    public decimal SGKPrimIsverenKesintiOrani { get; set; }
    public decimal SGKIssizlikPrimIsverenKesintiOrani { get; set; }
    public decimal DamgaVergisiOrani { get; set; }

    public decimal FazlaMesaiKatsayi { get;set; }
    public decimal HaftasonuFazlaMesaiKatsayi { get; set; }
    public decimal ResmiTatilFazlaMesaiKatsayi { get; set; }
}