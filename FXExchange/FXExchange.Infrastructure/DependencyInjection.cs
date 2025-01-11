using FXExchange.Core;
using Microsoft.Extensions.DependencyInjection;

namespace FXExchange.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ICurrencyExchangeRateProvider, FakeCurrencyExchangeRateProvider>();

        return services;
    }
}