using GenericRepository;
using MediatR;
using PersonelYonetim.Server.Application.Users;
using PersonelYonetim.Server.Domain.Personeller;
using TS.Result;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class PersonelDeleteCommandHandler(
    IPersonelRepository personelRepository,
    IUnitOfWork unitOfWork,
    ISender sender) : IRequestHandler<PersonelDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelDeleteCommand request, CancellationToken cancellationToken)
    {
        Personel personel = personelRepository.FirstAsync(p => p.Id == request.Id).Result;
        if (personel is null)
            return Result<string>.Failure("Personel bulunamadı");
        personel.IsDeleted = true;
        personel.IsActive = false;

        var userResult = await sender.Send(new UserDeleteCommand(personel.Iletisim.Eposta), cancellationToken);
        if (!userResult.IsSuccessful){
            return Result<string>.Failure("Kullanıcı silinirken hata oluştu");
        }

        unitOfWork.SaveChanges();

        return Result<string>.Succeed($"{personel.FullName} isimli kişinin kaydı silindi");
    }
}