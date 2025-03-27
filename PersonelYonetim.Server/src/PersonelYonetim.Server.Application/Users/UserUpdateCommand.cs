using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Users;

public sealed record UserUpdateCommand(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? Email
) : IRequest<Result<string>>;

public sealed class UserUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
{
    public UserUpdateCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id değeri boş olamaz");
    }
}

internal sealed class UserUpdateCommanHandler(
    UserManager<AppUser> userManager) : IRequestHandler<UserUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
            return Result<string>.Failure("Kullanıcı bulunamadı");

        bool nameChanged = false;
        if (!string.IsNullOrWhiteSpace(request?.FirstName) && user.FirstName != request.FirstName)
        {
            user.FirstName = request.FirstName;
            nameChanged = true;
        }
        if (!string.IsNullOrWhiteSpace(request?.LastName) && user.LastName != request.LastName)
        {
            user.LastName = request.LastName;
            nameChanged = true;
        }

        if (nameChanged)
        {
            string baseUserName = $"{request!.FirstName}.{request.LastName}".ToLower().Replace(" ", "");
            string userName = baseUserName;
            int counter = 1;

            while (await userManager.FindByNameAsync(userName) != null && user.UserName != userName)
            {
                userName = $"{baseUserName}{counter}";
                counter++;
            }
            user.UserName = userName;
        }

        if (!string.IsNullOrWhiteSpace(request?.Email) && user.Email != request.Email)
        {
            user.Email = request.Email;
            user.EmailConfirmed = false;
        }

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return Result<string>.Failure("Kullanıcı güncellenirken hata oluştu");

        return Result<string>.Succeed("Kullanıcı başarıyla güncellendi");
    }
}
