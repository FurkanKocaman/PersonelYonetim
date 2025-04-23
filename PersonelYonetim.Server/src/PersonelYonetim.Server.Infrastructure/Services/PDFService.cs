using Microsoft.AspNetCore.Hosting;
using Microsoft.Playwright;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using System.Globalization;

namespace PersonelYonetim.Server.Infrastructure.Services;
internal sealed class PDFService(
    IWebHostEnvironment env
    ) : IPDFService
{
    public async Task<byte[]> CreateBordroPdf(PersonelGorevlendirme personelGorevlendirme, PersonelDetay personelDetay, Tenant tenant, MaasPusula maasPusula)
    {

        var htmlPath = Path.Combine(env.ContentRootPath, "Templates", "Bordro.html");
        string htmlTemplate = File.ReadAllText(htmlPath);

        string htmlContent = htmlTemplate
            .Replace("{{YIL_AY}}", maasPusula.Yil + " " + CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetMonthName(maasPusula.Ay))
            .Replace("{{TENANT_LOGO}}", tenant.LogoUrl)
            .Replace("{{ADSOYAD}}", personelGorevlendirme.Personel.FullName)
            .Replace("{{TCKN}}", personelDetay.TCKN)
            .Replace("{{ISE_GIRIS_TARIHI}}", personelGorevlendirme.IseGirisTarihi.ToString("d MMMM yyyy", new CultureInfo("tr-TR")))
            .Replace("{{ISTEN_CIKIS_TARIHI}}", personelGorevlendirme.IstenCikisTarihi != null ?  personelGorevlendirme.IstenCikisTarihi.Value.ToString("d MMMM yyyy", new CultureInfo("tr-TR")) : "-")
            .Replace("{{BRUT_UCRET}}", personelGorevlendirme.BrutUcret.ToString())

            .Replace("{{ISYERI_ADI}}", tenant.Name)
            .Replace("{{SGK_ISYERI_ADI}}", personelGorevlendirme.SGKIsyeri ?? tenant.SGKIsyeri)
            .Replace("{{SGK_NUMARASI}}", personelGorevlendirme.SGKNumarasi ?? tenant.SGKNumarasi)
            .Replace("{{VERGI_DAIRESI}}", personelGorevlendirme.VergiDairesiAdi ?? tenant.VergiDairesiAdi)
            .Replace("{{VERGI_NUMARASI}}", personelGorevlendirme.VergiNumarasi ?? tenant.VergiNumarasi)
            .Replace("{{TABI_OLDUGU_KANUN}}", personelGorevlendirme.TabiOlduguKanun ?? tenant.TabiOlduguKanun)

            .Replace("{{NORMAL_KAZANC}}", maasPusula.BrutUcret.ToString())
            .Replace("{{FAZLA_CALISMA}}", maasPusula.EkKazancToplam.ToString())
            .Replace("{{EK_ODEMELER}}", maasPusula.EkKazancToplam.ToString())
            .Replace("{{TUM_GELIRLER}}", maasPusula.ToplamBrutKazanc.ToString())

            .Replace("{{YASAL_KESINTILER}}", maasPusula.ToplamKesinti.ToString())
            .Replace("{{AYNI_YARDIM_KESINTILER}}", "0")
            .Replace("{{OZEL_KESINTILER}}", maasPusula.DigerKesintilerToplam.ToString())
            .Replace("{{KESINTILER_TOPLAM}}", maasPusula.ToplamKesinti.ToString())

            .Replace("{{FIILI_CALISMA_GUN}}", maasPusula.FiiliCalismaGunu.ToString())
            .Replace("{{UCRETLI_IZIN_GUN}}", "0")
            .Replace("{{RAPORLU_IZIN_GUN}}", "0")
            .Replace("{{HAFTA_TATILI_GUN}}", "0")
            .Replace("{{RESMI_TATIL_GUN}}", "0")
            .Replace("{{SGK_GUN}}", maasPusula.SGKGunSayisi.ToString())

            .Replace("{{UCRETSIZ_IZIN_GUN}}", "0")
            .Replace("{{DIGER_EKSIK_GUN}}", maasPusula.EksikGunler.Count.ToString())
            .Replace("{{SGK_AYLIK_MATRAH}}", maasPusula.SGKMatrahi.ToString())
            .Replace("{{SGK_PRIMI}}", maasPusula.SGKPrimiIsci.ToString())
            .Replace("{{SGK_ISSIZLIK_PRIMI}}", maasPusula.IssizlikPrimiIsci.ToString())
            .Replace("{{SGK_PRIM_TOPLAM}}", (maasPusula.SGKPrimiIsci + maasPusula.IssizlikPrimiIsci).ToString())

            .Replace("{{KUMULATIF_GELIR_VERGISI_MATRAHI}}", maasPusula.KumulatifGelirVergisiMatrahiDonemSonu.ToString())
            .Replace("{{AYLIK_GELIR_VERGISI_MATRAHI}}", maasPusula.GelirVergisiMatrahi.ToString())
            .Replace("{{GELIR_VERGISI}}", maasPusula.OdenecekGelirVergisi.ToString())

            .Replace("{{DAMGA_VERGISI_MATRAHI}}", maasPusula.ToplamBrutKazanc.ToString())
            .Replace("{{DAMGA_VERGISI}}", maasPusula.OdenecekDamgaVergisi.ToString())

            .Replace("{{ELE_GECEN_UCRET}}",maasPusula.NetMaas.ToString())
            ;

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();

        await page.SetContentAsync(htmlContent, new() { WaitUntil = WaitUntilState.NetworkIdle });

        var pdfBytes = await page.PdfAsync(new()
        {
            Format="A4",
            PrintBackground=true,
            //Margin = new() { Top = "20px", Bottom = "20px", Left = "20px", Right = "20px" }
        });

        return pdfBytes;
    }
}
