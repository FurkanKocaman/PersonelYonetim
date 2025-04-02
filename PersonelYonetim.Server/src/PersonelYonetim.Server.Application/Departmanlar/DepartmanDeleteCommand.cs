using MediatR;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Departmanlar;
public sealed record DepartmanDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class DepartmanDeleteCommandHandler(
    IDepartmanRepository departmanRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DepartmanDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DepartmanDeleteCommand request, CancellationToken cancellationToken)
    {
        Departman departman = await departmanRepository.FirstOrDefaultAsync(p => p.Id == request.Id);
        if (departman == null)
            return Result<string>.Failure("Departman bulunamadı");

        departman.IsDeleted = true;
       
        departmanRepository.Update(departman);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Departman başarıyla silindi");
    }
}
