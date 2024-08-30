using DevFreela.Application.Commands.InsertProject;
using DevFreela.Application.DTOs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DevFreela.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services
            .AddHandlers();
        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<InsertProjectCommand>());
        services
            .AddTransient<IPipelineBehavior<InsertProjectCommand, ResultViewModel<int>>,
                ValidateInsertProjectCommandBehavior>();
        return services;
    }
}