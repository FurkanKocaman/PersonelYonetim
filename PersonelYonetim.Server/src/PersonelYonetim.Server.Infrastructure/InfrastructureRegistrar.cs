using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.RoleClaim;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Users;
using PersonelYonetim.Server.Infrastructure.Context;
using PersonelYonetim.Server.Infrastructure.Options;
using PersonelYonetim.Server.Infrastructure.Repositories;
using PersonelYonetim.Server.Infrastructure.Services;
using Scrutor;

namespace PersonelYonetim.Server.Infrastructure;

public static class InfrastructureRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string connectionString = configuration.GetConnectionString("SqlServer")!;
            opt.UseSqlServer(connectionString);
            opt.EnableSensitiveDataLogging();
        });

        services.AddScoped<Domain.UnitOfWork.IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBildirimService, BildirimService>();

        services
            .AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequiredLength = 1;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.SignIn.RequireConfirmedEmail = true;

                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ğüşöçıİĞÜŞÖÇ";
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(2);
        });

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtOptionsSetup>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy(Permissions.ViewKurumsalYapi, policy => policy.RequireClaim("permission", Permissions.ViewKurumsalYapi));
            opt.AddPolicy(Permissions.CreateKurumsalYapi, policy => policy.RequireClaim("permission", Permissions.CreateKurumsalYapi));
            opt.AddPolicy(Permissions.EditKurumsalYapi, policy => policy.RequireClaim("permission", Permissions.EditKurumsalYapi));
            opt.AddPolicy(Permissions.DeleteKurumsalYapi, policy => policy.RequireClaim("permission", Permissions.DeleteKurumsalYapi));

            opt.AddPolicy(Permissions.ViewPersonel, policy => policy.RequireClaim("permission", Permissions.ViewPersonel));
            opt.AddPolicy(Permissions.CreatePersonel, policy => policy.RequireClaim("permission", Permissions.CreatePersonel));
            opt.AddPolicy(Permissions.EditPersonel, policy => policy.RequireClaim("permission", Permissions.EditPersonel));
            opt.AddPolicy(Permissions.DeletePersonel, policy => policy.RequireClaim("permission", Permissions.DeletePersonel));

            opt.AddPolicy(Permissions.ViewIzinler, policy => policy.RequireClaim("permission", Permissions.ViewIzinler));
            opt.AddPolicy(Permissions.CreateIzinler, policy => policy.RequireClaim("permission", Permissions.CreateIzinler));
            opt.AddPolicy(Permissions.ApproveIzinler, policy => policy.RequireClaim("permission", Permissions.ApproveIzinler));

            opt.AddPolicy(Permissions.ViewRaporlar, policy => policy.RequireClaim("permission", Permissions.ViewRaporlar));
        });
        
        services.Scan(opt => opt
        .FromAssemblies(typeof(InfrastructureRegistrar).Assembly)
        .AddClasses(publicOnly:false)
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsImplementedInterfaces()
        .WithScopedLifetime()
        );
        return services;
    }
}

