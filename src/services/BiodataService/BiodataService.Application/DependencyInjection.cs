using BiodataService.Application.Contracts;
using BiodataService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BiodataService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBiodataService, BiodataAppService>();
        return services;
    }
}
