using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Playwright;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using System.Threading.Tasks;

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
            .Replace("{{ADSOYAD}}", personelGorevlendirme.Personel.FullName);

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();

        await page.GotoAsync($"{htmlPath}", new() { WaitUntil = WaitUntilState.NetworkIdle });

        var pdfBytes = await page.PdfAsync(new()
        {
            Format="A4",
            PrintBackground=true,
            //Margin = new() { Top = "20px", Bottom = "20px", Left = "20px", Right = "20px" }
        });

        return pdfBytes;
    }
}
