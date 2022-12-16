using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using System.Reflection;
using Rembot.StateMachines.Users;
using Microsoft.EntityFrameworkCore;
using Rembot.StateMachines.Orders;

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
            config.AddSagaStateMachine<UserStateMachine, UserState>()
                .EntityFrameworkRepository(repo => 
                {
                    repo.ConcurrencyMode = ConcurrencyMode.Optimistic;
                    repo.ExistingDbContext<DbContext>();
                });
            config.AddSagaStateMachine<OrderStateMachine, OrderState>()
                .EntityFrameworkRepository(repo => 
                {
                    repo.ConcurrencyMode = ConcurrencyMode.Optimistic;
                    repo.ExistingDbContext<DbContext>();
                });
            config.UsingInMemory();
        });
        return services;
    }
}