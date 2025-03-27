using GenericRepository;
using MediatR;
using PersonelYonetim.Server.Domain.Izinler;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class IzinTalepDeleteCommandHandler(
    IIzinTalepRepository izinTalepRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<IzinTalepDeleteCommand, Result<string>>
{
    public Task<Result<string>> Handle(IzinTalepDeleteCommand request, CancellationToken cancellationToken)
    {
        var izinTalep = izinTalepRepository.FirstOrDefault(p => p.Id == request.Id);
        if (izinTalep is null)
            return Task.FromResult(Result<string>.Failure("İzin talebi bulunamadı"));

        izinTalepRepository.Delete(izinTalep);
        unitOfWork.SaveChangesAsync();

        return Task.FromResult(Result<string>.Succeed("İzin talebi silindi"));
    }
}