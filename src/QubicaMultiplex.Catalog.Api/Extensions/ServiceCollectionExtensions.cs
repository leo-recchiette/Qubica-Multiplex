using QubicaMultiplex.Catalog.Api.Repositories;
using QubicaMultiplex.Catalog.Api.Repositories.Interfaces;
using QubicaMultiplex.Catalog.Api.Services;
using QubicaMultiplex.Catalog.Api.Services.Interfaces;

namespace QubicaMultiplex.Catalog.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogServices(this IServiceCollection services)
    {
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();

        return services;
    }
}
