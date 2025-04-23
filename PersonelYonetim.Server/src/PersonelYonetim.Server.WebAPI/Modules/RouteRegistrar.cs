namespace PersonelYonetim.Server.WebAPI.Modules;

public static class RouteRegistrar
{
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterPersonelRoutes();
        app.RegisterAuthRoutes();
        app.RegisterUserRoutes();        
        app.RegisterPozisyonRoutes();
        app.RegisterIzinTalepRoutes();
        app.RegisterIzinRoutes();
        app.RegisterCalismaTakvimRoutes();
        app.RegisterTakvimEtkinlikRoutes();
        app.RegisterFileRoutes();
        app.RegisterBildirimRoutes();
        app.RegisterDuyuruRoutes();
        app.RegisterCalismaCizelgeRoutes();
        app.RegisterMesaiRoutes();
        app.RegisterKurumsalBirimRoutes();
        app.RegisterBordroRoutes();
        app.RegisterpersonelDetayRoutes();
        app.RegisterMaasPusulaRoutes();
        app.RegisterKurumsalBirimTipiRoutes();
        app.RegisterTenantRoutes();
    }
}
