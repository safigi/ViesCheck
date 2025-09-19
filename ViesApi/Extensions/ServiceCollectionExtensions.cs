using Microsoft.Extensions.DependencyInjection;
using ViesApi.Configuration;
using ViesApi.Interfaces;
using ViesApi.Services;

namespace ViesApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViesApiServices(this IServiceCollection services, Action<ViesApiConfiguration>? configure = null)
    {
        var config = new ViesApiConfiguration();
        configure?.Invoke(config);
        
        services.AddSingleton(config);
        services.AddHttpClient();
        services.AddTransient<IViesApiService, ViesApiService>();
        services.AddSingleton<ViesVatFormatService>();
        
        return services;
    }
}
