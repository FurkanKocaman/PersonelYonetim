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

    // Teknik Özelleştirme
    public string? BrandingSettingsJson { get; set; }   
    public string? FeatureTogglesJson { get; set; } 
    public string? CustomFieldsJson { get; set; }

    // Güvenlik & Rol
    public int UserLimit { get; set; }   
    public long StorageQuotaMb { get; set; } 

    // Abonelik & Faturalama
    public Guid? SubscriptionPlanId { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public string? PaymentMethodJson { get; set; }

    // İzleme & Metrikler
    //public ICollection<TenantUsageMetric> UsageMetrics { get; set; } = new List<TenantUsageMetric>();
}