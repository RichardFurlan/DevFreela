using DevFreela.Domain.Respositories;
using DevFreela.Domain.TransferObjects;
using DevFreela.Infrastructure.CacheStorage;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Infrastructure.Persistence.Repositories;
using DevFreela.Infrastructure.Services.AuthService;
using DevFreela.Infrastructure.Services.MessageBus;
using DevFreela.Infrastructure.Services.MessageBus.Consumer;
using DevFreela.Infrastructure.Services.MessageBus.Publisher;
using DevFreela.Infrastructure.Services.PaymentService;
using MassTransit;
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
            .AddPaymentService()
            .AddConsumers();
        
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
        var rabbitMqOptions = new RabbitMQOptions(); 
        rabbitMqSection.Bind(rabbitMqOptions); 

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqOptions.Host, h =>
                {
                    h.Username(rabbitMqOptions.Username);
                    h.Password(rabbitMqOptions.Password);
                });
            });
        });
        
        services.AddMassTransitHostedService();
        services.AddScoped<IMessagePublisher, RabbitMQPublisher>();
        
        return services;
    }
    
    public static IServiceCollection AddConsumers(this IServiceCollection services)
    {
        services.AddScoped<IConsumer<PaymentApprovedIntegrationEvent>, PaymentApprovedConsumer>();
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }

    #region Options
    public class RabbitMQOptions
    {
        public string Host { get; set; } = "localhost";
        public string Username { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
    

    #endregion
}