using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Users; 

public sealed record UserCreateCommand(
    string FirstName,
    string LastName,
    string Email
    ) : IRequest<Result<Guid>>;

public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad boş olamaz").MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad boş olamaz").MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().WithMessage("Eposta boş olamaz").EmailAddress();
    }
}
internal sealed class UserCreateCommandHadler(
    UserManager<AppUser> usermanager) : IRequestHandler<UserCreateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        AppUser user = request.Adapt<AppUser>();
        string baseUserName = $"{request.FirstName}.{request.LastName}".ToLower().Replace(" ", "");
        string userName = baseUserName;
        int counter = 1;

        while (await usermanager.FindByNameAsync(userName) != null)
        {
            userName = $"{baseUserName}{counter}";
            counter++;
        }
        user.UserName = userName;
        user.EmailConfirmed = true;
        IdentityResult result = await usermanager.CreateAsync(user, request.FirstName);

        if (!result.Succeeded)
        {
            return Result<Guid>.Failure("Kullanıcı oluşturulurken hata oluştu");
        }
        return Result<Guid>.Succeed(user.Id);
    }
}
