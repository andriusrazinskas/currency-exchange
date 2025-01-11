using Microsoft.Extensions.DependencyInjection;

namespace FXExchange.CLI;

internal static class DependencyInjection
{
    public static IServiceCollection AddCliDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ICommandLineArgumentsParser, CommandLineArgumentsParser>();

        return services;
    }
}