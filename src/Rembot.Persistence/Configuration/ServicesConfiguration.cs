using Microsoft.Extensions.DependencyInjection;
using Rembot.Core.Interfaces;
using Rembot.Persistence.Services;

namespace Rembot.Persistence.Configuration;

public static class ServicesConfiguration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICashbackService, CashbackService>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<IOrdersService, OrdersService>();
        services.AddScoped<IReferalService, ReferalService>();

        return services;
    }
}