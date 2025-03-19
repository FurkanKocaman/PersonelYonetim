﻿using PersonelYonetim.Server.Domain.Users;
using PersonelYonetim.Server.Infrastructure.Context;
using PersonelYonetim.Server.Infrastructure.Options;
using GenericRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Roller;
using PersonelYonetim.Server.Domain.RoleClaim;

namespace PersonelYonetim.Server.Infrastructure;

public static class InfrastructureRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string connectionString = configuration.GetConnectionString("SqlServer")!;
            opt.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

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
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtOptionsSetup>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy(Permissions.ViewSirket, policy => policy.RequireClaim("permission", Permissions.ViewSirket));
            opt.AddPolicy(Permissions.CreateSirket, policy => policy.RequireClaim("permission", Permissions.CreateSirket));
            opt.AddPolicy(Permissions.EditSirket, policy => policy.RequireClaim("permission", Permissions.EditSirket));
            opt.AddPolicy(Permissions.DeleteSirket, policy => policy.RequireClaim("permission", Permissions.DeleteSirket));

            opt.AddPolicy(Permissions.ViewSube, policy => policy.RequireClaim("permission", Permissions.ViewSube));
            opt.AddPolicy(Permissions.CreateSube, policy => policy.RequireClaim("permission", Permissions.CreateSube));
            opt.AddPolicy(Permissions.EditSube, policy => policy.RequireClaim("permission", Permissions.EditSube));
            opt.AddPolicy(Permissions.DeleteSube, policy => policy.RequireClaim("permission", Permissions.DeleteSube));

            opt.AddPolicy(Permissions.ViewDepartman, policy => policy.RequireClaim("permission", Permissions.ViewDepartman));
            opt.AddPolicy(Permissions.CreateDepartman, policy => policy.RequireClaim("permission", Permissions.CreateDepartman));
            opt.AddPolicy(Permissions.EditDepartman, policy => policy.RequireClaim("permission", Permissions.EditDepartman));
            opt.AddPolicy(Permissions.DeleteDepartman, policy => policy.RequireClaim("permission", Permissions.DeleteDepartman));

            opt.AddPolicy(Permissions.ViewPozisyon, policy => policy.RequireClaim("permission", Permissions.ViewPozisyon));
            opt.AddPolicy(Permissions.CreatePozisyon, policy => policy.RequireClaim("permission", Permissions.CreatePozisyon));
            opt.AddPolicy(Permissions.EditPozisyon, policy => policy.RequireClaim("permission", Permissions.EditPozisyon));
            opt.AddPolicy(Permissions.DeletePozisyon, policy => policy.RequireClaim("permission", Permissions.DeletePozisyon));

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

