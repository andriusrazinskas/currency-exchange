using Microsoft.Extensions.DependencyInjection;

namespace FXExchange.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ICurrencyConverter, CurrencyConverter>();

        return services;
    }
}