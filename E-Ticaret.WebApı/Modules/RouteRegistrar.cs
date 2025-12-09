namespace E_Ticaret.WebApı.Modules;
    public static class RouteRegistrar
    {
    public static void RegisterRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterCategoryRoutes();
        app.RegisterCartRoutes();
        app.RegisterProductRoutes();
    }


    }
