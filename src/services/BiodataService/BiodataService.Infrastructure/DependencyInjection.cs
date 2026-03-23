using BiodataService.Application.Contracts;
using BiodataService.Infrastructure.Persistence;
using BiodataService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BiodataService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=biodata_db;Username=postgres;Password=postgres";

        services.AddDbContext<BiodataDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IBiodataRepository, BiodataRepository>();
        return services;
    }
}
