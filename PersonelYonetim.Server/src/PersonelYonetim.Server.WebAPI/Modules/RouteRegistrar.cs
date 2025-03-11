namespace PersonelYonetim.Server.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterPersonelRoutes();
        app.RegisterAuthRoutes();
        app.RegisterUserRoutes();
        app.RegisterDepartmanRoutes();
        app.RegisterPozisyonRoutes();
    }
}
