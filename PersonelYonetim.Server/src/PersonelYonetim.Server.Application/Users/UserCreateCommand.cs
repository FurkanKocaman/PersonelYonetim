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
    string Email,
    string Password
    ) : IRequest<Result<string>>;

public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad boş olamaz").MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad boş olamaz").MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().WithMessage("Eposta boş olamaz").EmailAddress();
        RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre boş olamaz");
    }
}
internal sealed class UserCreateCommandHadler(
    UserManager<AppUser> usermanager) : IRequestHandler<UserCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        AppUser user = request.Adapt<AppUser>();
        user.UserName = user.Email;
        user.EmailConfirmed = true;
        IdentityResult result = await usermanager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return "Kullanıcı oluşturulurken hata oluştu";
        }
        return "Kullanıcı başarıyla oluşturuldu";
    }
}
