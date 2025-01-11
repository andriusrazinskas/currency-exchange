using FXExchange.Core;
using FXExchange.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FXExchange.CLI;

internal class Program
{
    public static async Task<int> Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddCliDependencies()
            .AddCoreDependencies()
            .AddInfrastructureDependencies()
            .BuildServiceProvider();

        try
        {
            var commandLineArgumentsParser = serviceProvider.GetRequiredService<ICommandLineArgumentsParser>();
            var currencyConverter = serviceProvider.GetRequiredService<ICurrencyConverter>();

            var request = commandLineArgumentsParser.Parse(args);
            var convertedAmount = await currencyConverter.ConvertAsync(request.MainCurrency, request.TargetCurrency, request.AmountToExchange);
            Console.WriteLine(convertedAmount);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return -1;
        }

        return 0;
    }
}