using MediatR;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using TS.Result;

namespace PersonelYonetim.Server.Application.Bordro;
public sealed record BordroCalisanlarUpdateCommand(
    List<BordroCalisanlarUpdateCommandDto> BordroCalisanlarUpdateCommands
    ) : IRequest<Result<string>>;

public sealed class BordroCalisanlarUpdateCommandDto
{
    public Guid Id;
    public string? TCKN;
    public DateTimeOffset? IseBaslangicTarihi;
    public DateTimeOffset? IstenCikisTarihi;
    public int EngelDerecesi;
    public string? TabiOlduguKanun;
    public string? SGKIsyeri;
    public string? VergiDairesiAdi;
    public decimal KumulatifVergiMatrahi;
    public string? MeslekKodu;
}
    internal sealed class BordroCalisanlarUpdateCommandHandler(
        IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
        IPersonelDetayRepository personelDetayRepository
        ) : IRequestHandler<BordroCalisanlarUpdateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(BordroCalisanlarUpdateCommand request, CancellationToken cancellationToken)
        {
            foreach (var updateCommand in request.BordroCalisanlarUpdateCommands)
            {
                var personelGorevlendirme = await personelGorevlendirmeRepository.FirstOrDefaultAsync(p => p.Id == updateCommand.Id && !p.IsDeleted);
                if (personelGorevlendirme is null)
                    return Result<string>.Failure("Personel gorevlendirme bulunamamdı");

                var personelDetay = await personelDetayRepository.FirstOrDefaultAsync(p => p.PersonelId == personelGorevlendirme.PersonelId && !p.IsDeleted);
                if (personelDetay is null)
                    return Result<string>.Failure("Personel detay bulunamadı");

                personelGorevlendirme.TabiOlduguKanun = updateCommand.TabiOlduguKanun;
                personelGorevlendirme.SGKIsyeri = updateCommand.SGKIsyeri;
                personelGorevlendirme.VergiDairesiAdi = updateCommand.VergiDairesiAdi;
                personelGorevlendirme.MeslekKodu = updateCommand.MeslekKodu;

                personelDetay.TCKN = updateCommand.TCKN;
            }


            return Result<string>.Succeed("Bilgiler başarıyla güncellendi");

        }
    
}
