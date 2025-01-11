using System.Globalization;
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

            var exchangeDetails = commandLineArgumentsParser.Parse(args);
            var convertedAmount = await currencyConverter.ConvertAsync(exchangeDetails.MainCurrency, exchangeDetails.TargetCurrency, exchangeDetails.AmountToExchange);
            Console.WriteLine(convertedAmount.ToString("F5", CultureInfo.InvariantCulture));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return -1;
        }

        return 0;
    }
}