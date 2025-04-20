using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;

namespace PersonelYonetim.Server.Infrastructure.Services;
internal sealed class PDFService(
    IWebHostEnvironment env,
    IConverter converter
    ) : IPDFService
{
    public byte[] CreateBordroPdf(PersonelGorevlendirme personelGorevlendirme, PersonelDetay personelDetay, Tenant tenant, MaasPusula maasPusula)
    {

        var htmlPath = Path.Combine(env.ContentRootPath, "Templates", "Bordro.html");
        string htmlTemplate = File.ReadAllText(htmlPath);

        string htmlContent = htmlTemplate
            .Replace("{{ADSOYAD}}", personelGorevlendirme.Personel.FullName);

        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings =
            {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
            },
            Objects =
            {
                new ObjectSettings()
                {
                    HtmlContent = htmlContent,
                }
            }
        };

        return converter.Convert(doc);
    }
}
