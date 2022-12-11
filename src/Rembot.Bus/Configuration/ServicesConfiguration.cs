using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Rembot.Bus.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddBusServices(this IServiceCollection services)
    {
        services.AddScoped<AuthenticationStateHandler>();
        services.AddScoped<MenuStateHandler>();
        services.AddScoped<PlacingOrderStateHandler>();
        services.AddMediatR(typeof(RegisterRequest).Assembly);
        return services;
    }
}