using DevFreela.Application.Commands.Project.InsertProject;
using DevFreela.Application.Consumers;
using DevFreela.Application.DTOs;
using DevFreela.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DevFreela.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services
            .AddHandlers()
            .AddValidation()
            .AddConsumer();
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

    private static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<InsertProjectCommand>();

        return services;
    }

    private static IServiceCollection AddConsumer(this IServiceCollection services)
    {
        services.AddHostedService<PaymentApprovedConsumer>();

        return services;
    }
}