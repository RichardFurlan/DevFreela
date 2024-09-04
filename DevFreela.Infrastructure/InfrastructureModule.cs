using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.CacheStorage;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Infrastructure.Persistence.Repositories;
using DevFreela.Infrastructure.Services.AuthService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevFreela.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuth()
            .AddRedisCache()
            .AddRepository()
            .AddData(configuration);
        
        return services;
    }

    private static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DevFreelaDb");

        services.AddDbContext<DevFreelaDbContext>(o => o.UseNpgsql(connectionString));

        return services;
    }

    private static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();        
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static IServiceCollection AddRedisCache(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.InstanceName = "DevFreelaCache";
            options.Configuration = "localhost:6379";
        });

        services.AddTransient<ICacheService, CacheService>();
        
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }
}