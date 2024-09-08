using DevFreela.Domain.Respositories;
using DevFreela.Infrastructure.CacheStorage;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Infrastructure.Persistence.Repositories;
using DevFreela.Infrastructure.Services.AuthService;
using DevFreela.Infrastructure.Services.MessageBus;
using DevFreela.Infrastructure.Services.PaymentService;
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
            .AddData(configuration)
            .AddRabbitMQ(configuration)
            .AddPaymentService();;
        
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

    private static IServiceCollection AddPaymentService(this IServiceCollection services)
    {
        services.AddScoped<IPaymentService, PaymentService>();
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
    
    private static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSection = configuration.GetSection("RabbitMQ");
        
        var rabbitMqHost = rabbitMqSection.GetConnectionString("RabbitMQ__UserName") ?? "localhost";
        var rabbitMqUser = rabbitMqSection.GetConnectionString("RabbitMQ__UserName") ?? "guest";
        var rabbitMqPassword = rabbitMqSection.GetConnectionString("RabbitMQ__Password") ?? "guest";


        services.AddScoped<IMessagePublisher>(provider =>
            new RabbitMQPublisher(rabbitMqHost, rabbitMqUser, rabbitMqPassword));
        
        services.AddScoped<IMessageConsumer>(provider =>
            new RabbitMQConsumer(rabbitMqHost, rabbitMqUser, rabbitMqPassword, provider));
        
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }
}