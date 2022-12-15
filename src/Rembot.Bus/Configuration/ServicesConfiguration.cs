using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using System.Reflection;

namespace Rembot.Bus.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddBusServices(this IServiceCollection services)
    {
        services.AddMassTransit(config =>
        {
            var assembly = Assembly.GetExecutingAssembly();
            config.AddActivities(assembly);
            config.AddConsumers(assembly);
            config.AddSagaStateMachines(assembly);
            config.AddSagas(assembly);
            config.SetInMemorySagaRepositoryProvider();
            config.UsingInMemory();
        });
        return services;
    }
}