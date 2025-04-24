using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Users;
using PersonelYonetim.Server.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using Microsoft.EntityFrameworkCore;

namespace PersonelYonetim.Server.Infrastructure.Service;

internal sealed class JwtProvider(
    IOptions<JwtOptions> options,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    UserManager<AppUser> userManager) : IJwtProvider
{
    public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        var gorevlendirme = personelGorevlendirmeRepository.Where(p => p.Personel.UserId == user.Id && !p.IsDeleted).Include(p => p.Personel).Include(p => p.GorevlendirmeRolleri).FirstOrDefault();
        var roller = gorevlendirme!.GorevlendirmeRolleri;

        var roles =  userManager.GetRolesAsync(user).Result;
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim("tenant_id", user.TenantId.ToString()),
        };
        foreach(var role in roller)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.RolId.ToString()));
        }

        var expires = DateTime.Now.AddDays(1);

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        JwtSecurityToken securityToken = new(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims:claims,
            notBefore: DateTime.Now,
            expires:expires,
            signingCredentials:signingCredentials);
        
        JwtSecurityTokenHandler handler = new();

        string token = handler.WriteToken(securityToken);

        return Task.FromResult(token);
    }
}
