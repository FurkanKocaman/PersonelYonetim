using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using PersonelYonetim.Server.Application.Behaviors;
using PersonelYonetim.Server.Application.MaasPusulalar;
using PersonelYonetim.Server.Domain.Bordro;

namespace PersonelYonetim.Server.Application;

public static class ApplicationRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssembly(typeof(ApplicationRegistrar).Assembly);
            conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationRegistrar).Assembly);

        TypeAdapterConfig<MaasPusulaUpdateCommand, MaasPusula>
            .NewConfig()
            .IgnoreNullValues(true);

        return services;
    }

}

